using UnityEngine;
using UnityEngine.UI;

public class QuitButton : BaseButton
{
    protected override void OnClick()
    {
        if (isPressed) return;
        isPressed = true;
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
