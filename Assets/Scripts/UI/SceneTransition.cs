using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private CanvasGroup overlay;

    public static SceneTransition instance;
    
    void Awake()
    {
        overlay = GetComponent<CanvasGroup>();
        if (overlay == null) throw new Exception($"{this} must have a {overlay.GetType()} component");
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }
    
    public async Task FadeInAsync()
    {
        await overlay.DOFade(1, 0.5f).AsyncWaitForCompletion();
    }

    public async Task FadeOutAsync()
    {
        await overlay.DOFade(0, 0.5f).AsyncWaitForCompletion();
    }
}