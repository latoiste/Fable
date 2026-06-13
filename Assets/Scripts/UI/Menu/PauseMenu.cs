using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Timer timer;
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        PauseKeybind.OnPressed += ToggleScreen;

        if (timer == null) throw new Exception($"Attribute timer in {this} cannot be null");
        if (canvas == null) throw new Exception($"Attribute canvas in {this} cannot be null");
    }

    void Start()
    {
        canvas.enabled = false;
    }

    private void ToggleScreen()
    {
            bool visible = canvas.enabled;

            if (visible) Hide(); else Show();
    }

    private void Show()
    {
        timer.Pause();
        canvas.enabled = true;
    }

    private void Hide()
    {
        timer.Resume();
        canvas.enabled = false;
    }

    void OnDestroy()
    {
        PauseKeybind.OnPressed -= ToggleScreen;
    }
}
