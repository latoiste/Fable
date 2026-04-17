using System;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private Sprite openedSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) throw new Exception($"SpriteRenderer in {this} not found");
        if (openedSprite == null) throw new Exception($"Attribute openedSprite in {this} cannot be null");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.CompareTag("Player")) OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        spriteRenderer.sprite = openedSprite;
    }
}
