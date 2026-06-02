using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICoinProvider
{
    [SerializeField] private Sprite openedSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;

    private int coinAmount;
    public event Action<int> OnActivated;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        System.Random random = new();
        coinAmount = random.Next(1, 6);

        if (spriteRenderer == null) throw new Exception($"SpriteRenderer in {this} not found");
        if (openedSprite == null) throw new Exception($"Attribute openedSprite in {this} cannot be null");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.CompareTag("Player")) {
            OpenChest();
            OnActivated?.Invoke(coinAmount);
        }
    }

    public int CoinAmount() => coinAmount;

    private void OpenChest()
    {
        isOpened = true;
        spriteRenderer.sprite = openedSprite;
    }
}
