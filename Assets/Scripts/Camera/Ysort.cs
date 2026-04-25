using System;
using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField, Range(1000, 5000)] private int baseOrder = 5000;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) throw new Exception($"Attribute {this} cannot be null");
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(baseOrder - transform.position.y * 100) % 1000;
    }
}
