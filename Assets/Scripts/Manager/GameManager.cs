using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Timer timer;

    private int islandCount = 3;
    private string nextIsland;
    private AsyncOperation preloadOp;
    private bool isPreloaded;
    private bool preloadLock = true;
    private bool isSwitching = false;
    private System.Random random;

    public static GameManager instance;

    void Awake()
    {
        random = new System.Random();
        if (player == null) throw new Exception($"Attribute player in {this} cannot be null");
        if (timer == null) throw new Exception($"Attribute timer in {this} cannot be null");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        await SceneTransition.instance.FadeInAsync();

        player.Freeze();
        timer.Pause();

        await LoadNewIsland();
        
        Island newIsland = GetIslandObject();
        player.SetSpawnPoint(newIsland.SpawnPoint);

        player.Unfreeze();
        timer.Resume();
        
        await SceneTransition.instance.FadeOutAsync();
    }

    public void AddTime(int seconds) => timer.AddTime(seconds);

    public async Task SwitchIslands()
    {
        if (isSwitching) return;

        isSwitching = true;
        player.Freeze();
        timer.Pause();
        await SceneTransition.instance.FadeInAsync();

        Scene oldIsland = GetCurrentIslandScene();

        await LoadNewIsland();

        if (oldIsland.IsValid())
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(oldIsland); 
            while (!unloadOp.isDone) await Task.Yield();
        }

        Island newIsland = GetIslandObject();
        player.SetSpawnPoint(newIsland.SpawnPoint);
    
        player.Unfreeze();
        timer.Resume();
        await SceneTransition.instance.FadeOutAsync();
        
        isSwitching = false;
    }

    // Event when player gathers enough coins
    public void StartPreloadIsland()
    {
        StartCoroutine(PreloadIsland());
    }

    private IEnumerator PreloadIsland()
    {
        if (isPreloaded) yield return null;

        Debug.Log("Preloading");
        isPreloaded = true;
        preloadLock = true;
        nextIsland = NextIslandName();

        if (string.IsNullOrEmpty(nextIsland))
        {
            Debug.LogWarning("nextIsland is not set, cannot preload next island scene");
            preloadLock = false;
            isPreloaded = false;
            yield return null;
        }

        preloadOp = SceneManager.LoadSceneAsync(nextIsland, LoadSceneMode.Additive);
        preloadOp.allowSceneActivation = false;

        while (preloadOp.progress < 0.9f) yield return null;

        preloadLock = false;
    }

    private async Task LoadNewIsland()
    {
        if (isPreloaded)
        {
            while (preloadLock) await Task.Yield();
        } else
        {
            nextIsland = NextIslandName();
            if (string.IsNullOrEmpty(nextIsland)) {
                Debug.LogWarning("nextIsland is not set, cannot preload next island scene");
                return;
            }
            preloadOp = SceneManager.LoadSceneAsync(nextIsland, LoadSceneMode.Additive);
        }

        preloadOp.allowSceneActivation = true;
        while (!preloadOp.isDone) await Task.Yield();

        preloadOp = null;
        isPreloaded = false;
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
}
