using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Timer timer;
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (timer == null) throw new Exception($"Attribute timer in {this} cannot be null");
        if (canvas == null) throw new Exception($"Attribute canvas in {this} cannot be null");
    }

    void Start()
    {
        canvas.enabled = false;
    }

    public void Test()
    {
        Debug.Log("AAAAAAAAAa");
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool visible = canvas.enabled;
            if (visible)
            {
                Hide();
            } else
            {
                Show();
            }

            Debug.Log("hello");
        }
    }

    public void Show()
    {
        timer.Pause();
        canvas.enabled = true;
    }

    public void Hide()
    {
        timer.Resume();
        canvas.enabled = false;
    }
}
