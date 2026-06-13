using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseKeybind : MonoBehaviour
{
    public static event Action OnPressed;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnPressed.Invoke();
        }
    }
}