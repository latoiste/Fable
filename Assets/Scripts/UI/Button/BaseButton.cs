using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class BaseButton : MonoBehaviour
{
    private Button button;
    protected bool isPressed; 

    void Awake()
    {
        isPressed = false;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        button.onClick.AddListener(PlaySfx);
    }

    protected abstract void OnClick();

    private void PlaySfx() => AudioManager.instance.PlaySfx(AudioClips.ButtonClick);

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
        button.onClick.RemoveListener(PlaySfx);
    }
}