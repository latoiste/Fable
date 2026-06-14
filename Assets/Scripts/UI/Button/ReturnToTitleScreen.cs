using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToTitleScreenButton : MonoBehaviour
{
    private Button button;
    private bool isPressed; 

    void Awake()
    {
        isPressed = false;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isPressed) return;
        isPressed = true;
        
        _ = ReturnToTitleScreen();
    }

    private async Task ReturnToTitleScreen()
    {
        await SceneManager.LoadSceneAsync("TitleScreen");
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }
}
