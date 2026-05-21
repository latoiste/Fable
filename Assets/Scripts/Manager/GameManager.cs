using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private int islandCount = 2;

    private string nextIsland;
    private AsyncOperation preloadOp;
    private bool isPreloaded;
    private bool preloadLock = true;

    public static GameManager instance;

    void Awake()
    {
        if (player == null) throw new Exception($"Attribute player in {this} cannot be null");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }

        nextIsland = NextIslandName();
    }

    // emitted from altar?
    public async Task SwitchIslands()
    {
        await SceneTransition.instance.FadeInAsync();

        await LoadNewIsland();
        
        await SceneTransition.instance.FadeOutAsync();
    }

    // Event when player gathers enough coins
    public void StartPreloadIsland()
    {
        StartCoroutine(PreloadIsland());
    }

    private IEnumerator PreloadIsland()
    {
        if (isPreloaded) yield return null;

        isPreloaded = true;
        preloadLock = true;

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
            if (string.IsNullOrEmpty(nextIsland)) {
                Debug.LogWarning("nextIsland is not set, cannot preload next island scene");
                return;
            }
            preloadOp = SceneManager.LoadSceneAsync(nextIsland, LoadSceneMode.Additive);
        }

        preloadOp.allowSceneActivation = true;
        while (!preloadOp.isDone) await Task.Yield();

        Scene oldIsland = GetCurrentIslandScene();
        if (oldIsland.IsValid())
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(oldIsland); 
            while (!unloadOp.isDone) await Task.Yield();
        }

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

    private string NextIslandName()
    {
        System.Random random = new();
        int nextIslandIndex = random.Next(0, islandCount);

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
