using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToTitleScreenButton : BaseButton
{
    protected override void OnClick()
    {
        if (isPressed) return;
        isPressed = true;
        
        _ = ReturnToTitleScreen();
    }

    private async Task ReturnToTitleScreen()
    {
        await SceneManager.LoadSceneAsync("TitleScreen");
    }
}
