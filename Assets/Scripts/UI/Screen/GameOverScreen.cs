using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class GameOverScreen : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private CanvasGroup buttons;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (canvas == null) throw new Exception($"Attribute canvas in {this} cannot be null");
        if (text == null) throw new Exception($"Attribute text in {this} cannot be null");
        if (scoreText == null) throw new Exception($"Attribute scoreText in {this} cannot be null");
        if (buttons == null) throw new Exception($"Attribute buttons in {this} cannot be null");
    }

    async void Start()
    {
        scoreText.enabled = false;
        buttons.blocksRaycasts = false;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(text.transform.DOLocalMoveY(300, 4f));
        sequence.Join(text.DOFade(1f, 3.5f));

        sequence.Join(buttons.transform.DOLocalMoveY(-160f, 4f));
        sequence.Join(buttons.DOFade(1f, 3.5f));

        await sequence.AsyncWaitForCompletion();
        
        await Task.Delay(500);
        scoreText.text = $"SCORE: {SaveManager.CurrentRunScore}";
        scoreText.enabled = true;
        if (SaveManager.CurrentRunScore == SaveManager.Highscore)
        {
            await Task.Delay(1000);
            AudioManager.instance.PlaySfx(AudioClips.NewHighscore);
            scoreText.text += "\n<size=80>NEW HIGHSCORE!</size>";
        }
        buttons.blocksRaycasts = true;
    }
}