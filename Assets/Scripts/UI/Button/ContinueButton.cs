using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : BaseButton
{
    protected override void OnClick()
    {
        if (isPressed) return;
        isPressed = true;
        
        _ = Restart();
    }

    private async Task Restart()
    {
        await SceneTransition.instance.FadeInAsync();
        await SceneManager.LoadSceneAsync("Gameplay");
    }
}
