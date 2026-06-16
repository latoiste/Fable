using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Timer timer;
    [SerializeField] private Hud hud;

    private int islandCount = 3;
    private string nextIsland;
    private bool isSwitching = false;
    private System.Random random;
    private bool paused = false;
    private bool canPause = false;
    private int score;

    void Awake()
    {
        random = new System.Random();
        score = 0;

        if (player == null) throw new Exception($"Attribute player in {this} cannot be null");
        if (timer == null) throw new Exception($"Attribute timer in {this} cannot be null");

        PauseKeybind.OnPressed += TogglePause;
        timer.OnTimerEnd += OnGameOver;
    }

    async void Start()
    {
        await SceneTransition.instance.FadeInAsync();

        canPause = false;
        player.Freeze();
        timer.Pause();

        await LoadNewIsland();
        
        Island newIsland = GetIslandObject();
        AddIslandListener(newIsland);
        OnNewIslandLoaded(newIsland);

        player.Unfreeze();
        timer.Resume();
        canPause = true;
        
        await SceneTransition.instance.FadeOutAsync();
    }

    private void OnNewIslandLoaded(Island island)
    {
        player.SetSpawnPoint(island.SpawnPoint);
        player.SetLayer(island.startingLayer);
        player.SetSortingLayer(island.startingSortingLayer);
    }

    public void AddTime(int seconds) => timer.AddTime(seconds);

    private void TogglePause()
    {
        if (!canPause) return;

        if (paused) {
            player.Unfreeze();
            Time.timeScale = 1;
        } else {
            player.Freeze();
            Time.timeScale = 0;
        }
        
        paused = !paused;
    }

    private void OnGameOver()
    {
        SaveManager.CurrentRunScore = score;
        SaveManager.Highscore = math.max(score, SaveManager.Highscore);
        Debug.Log("Game over");
        _ = GameOver();
    }

    private async Task GameOver()
    {
        Debug.Log("Loading Game Over Screen");
        
        await SceneManager.LoadSceneAsync("GameOverScreen");
        Scene currentIsland = GetCurrentIslandScene();
        _ = UnloadScene(currentIsland);
    }

    public void StartSwitchIslands()
    {
        score++;
        _ = SwitchIslands();
    }

    private async Task SwitchIslands()
    {
        if (isSwitching) return;

        isSwitching = true;
        await SceneTransition.instance.FadeInAsync();

        canPause = false;
        player.Freeze();
        timer.Pause();

        Scene oldIslandScene = GetCurrentIslandScene();
        if (oldIslandScene.IsValid())
        {
            Island oldIsland = GetIslandObject();
            RemoveIslandListener(oldIsland);  
        }

        await LoadNewIsland();

        await UnloadScene(oldIslandScene);

        Island newIsland = GetIslandObject();
        AddIslandListener(newIsland);
        OnNewIslandLoaded(newIsland);
    
        player.Unfreeze();
        timer.Resume();
        canPause = true;

        if (paused) await Task.Yield();

        await SceneTransition.instance.FadeOutAsync();
        
        isSwitching = false;
    }

    private void AddIslandListener(Island island)
    {
        island.OnIslandCompleted += StartSwitchIslands;
        island.OnAddBonusTime += AddTime;

        hud.AddIslandListener(island);
    }

    private void RemoveIslandListener(Island island)
    {
        island.OnIslandCompleted -= StartSwitchIslands;
        island.OnAddBonusTime -= AddTime;
        
        hud.RemoveIslandListener(island);
    }

    private async Task LoadNewIsland()
    {
        nextIsland = NextIslandName();
        if (string.IsNullOrEmpty(nextIsland)) {
            Debug.LogWarning("nextIsland is not set, cannot preload next island scene");
            return;
        }
        await SceneManager.LoadSceneAsync(nextIsland, LoadSceneMode.Additive);
    }

    private Scene GetCurrentIslandScene()
    {
        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.StartsWith("Island"))
            {
                return scene;
            }
        }
        return default;
    }

    private Island GetIslandObject()
    {
        Scene scene = GetCurrentIslandScene();
        if (!scene.IsValid()) throw new Exception("No island scene found");

        GameObject islandObject = scene
            .GetRootGameObjects()
            .FirstOrDefault(obj => obj.CompareTag("Island"));

        bool found = islandObject.TryGetComponent<Island>(out Island island);
        if (!found) throw new Exception($"Island object missing in {scene.path}");

        return island;
    }

    private string NextIslandName()
    {
        int nextIslandIndex = random.Next(0, islandCount);
        // Debug.Log(nextIslandIndex);

        Scene currentIsland = GetCurrentIslandScene();
        if (!currentIsland.IsValid()) return $"Island_{nextIslandIndex}";

        string currentIslandIndex = currentIsland.name.Split('_')[1];
        if (string.Equals(currentIslandIndex, nextIslandIndex.ToString()))
        {
            nextIslandIndex = (nextIslandIndex + 1) % islandCount;
        }

        return $"Island_{nextIslandIndex}";
    }

    private async Task UnloadScene(Scene scene)
    {
        if (scene.IsValid())
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene); 
            while (!unloadOp.isDone) await Task.Yield();   
        }
    }

    void OnDestroy()
    {
        PauseKeybind.OnPressed -= TogglePause;
        timer.OnTimerEnd -= OnGameOver;
    }
}
