using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
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
        
        _ = StartGame();
    }

    private async Task StartGame()
    {
        await SceneTransition.instance.FadeInAsync();
        await SceneManager.LoadSceneAsync("Gameplay");
        // await Task.Delay(1000);
        await SceneTransition.instance.FadeOutAsync();
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }
}
