using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    private string nextIslandName;
    private AsyncOperation preloadOp;
    public bool IsPreloading => preloadOp != null && (preloadOp.progress < 0.9f);

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
    }

    // emitted from altar?
    public async Task SwitchIslands()
    {
        await SceneTransition.instance.FadeInAsync();

        // await LoadNewIsland();
        
        await SceneTransition.instance.FadeOutAsync();
    }

    // Event when player gathers enough coins
    public void StartPreloadIsland()
    {
        StartCoroutine(PreloadIsland());
    }

    private IEnumerator PreloadIsland()
    {
        preloadOp.allowSceneActivation = false;
        preloadOp = SceneManager.LoadSceneAsync(nextIslandName, LoadSceneMode.Additive);

        while (preloadOp.progress < 0.9f) yield return null;
    }

    // private async Task LoadNewIsland()
    // {
    //     while (!IsPreloading) await Task.Yield();

    //     preloadOp.allowSceneActivation = true;
    //     while (!preloadOp.isDone) await Task.Yield();

    //     Scene? oldIsland = GetCurrentIslandScene();
    //     if (oldIsland != null)
    //     {
    //         AsyncOperation unloadOp = SceneManager.UnloadSceneAsync((Scene)oldIsland); // tak tau pake oldIsland! gbs 
    //         while (!unloadOp.isDone) await Task.Yield();
    //     }

    //     preloadOp = null;
    // }

    // private Scene? GetCurrentIslandScene()
    // {
    //     int count = SceneManager.sceneCount;
    //     for (int i = 0; i < count; i++)
    //     {
    //         Scene scene = SceneManager.GetSceneAt(i);
    //         if (scene.name.StartsWith("Island"))
    //         {
    //             return scene;
    //         }
    //     }
    //     return null;
    // }

}
