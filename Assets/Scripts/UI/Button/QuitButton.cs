using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    private Button button;
    private bool isPressed; 

    void Awake()
    {
        isPressed = false;
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        if (isPressed) return;
        isPressed = true;
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(QuitGame);
    }
}
