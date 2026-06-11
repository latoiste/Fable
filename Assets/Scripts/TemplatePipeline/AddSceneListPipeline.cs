#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine.SceneManagement;


public class AddSceneListPipeline : ISceneTemplatePipeline
{
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
    {
        return true;
    }

    public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
    {
        
    }

    public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
    {
        List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
        int islandCount = 0;

        foreach (var buildScene in scenes)
        {
            if (buildScene.path.Contains("Island") && buildScene.enabled) islandCount++;
        }

        string newSceneName = $"Island_{islandCount}";
        string newScenePath = $"Assets/Scenes/Islands/{newSceneName}.unity";

        Debug.Log(newScenePath);

        EditorSceneManager.SaveScene(scene, newScenePath);

        EditorBuildSettingsScene newBuildScene = new(newScenePath, true);
        scenes.Add(newBuildScene);

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
#endif
