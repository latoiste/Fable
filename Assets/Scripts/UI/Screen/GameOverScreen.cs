using UnityEngine;
using System;
using UnityEditor.Search;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class GameOverScreen : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup buttons;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (canvas == null) throw new Exception($"Attribute canvas in {this} cannot be null");
        if (text == null) throw new Exception($"Attribute text in {this} cannot be null");
        if (buttons == null) throw new Exception($"Attribute buttons in {this} cannot be null");
    }

    void Start()
    {
        _ = Test();
        // canvas.enabled = false;
    }

    [ContextMenu("Test")]
    private async Task Test()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(text.transform.DOLocalMoveY(83, 5f));
        sequence.Join(text.DOFade(1f, 4.5f));

        sequence.Join(buttons.transform.DOLocalMoveY(-18f, 5f));
        sequence.Join(buttons.DOFade(1f, 4.5f));

        await sequence.AsyncWaitForCompletion();
    }
}