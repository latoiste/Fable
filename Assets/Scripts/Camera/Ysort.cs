using System;
using System.Collections.Generic;
using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField, Range(1000, 5000)] private int baseOrder = 1000;
    private List<SpriteRenderer> spriteRenderers = new();

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null) spriteRenderers.Add(spriteRenderer);
        
        foreach (Transform child in transform)
        {
            GameObject gameObject = child.gameObject;
            gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer childSpriteRenderer);
            if (childSpriteRenderer != null) spriteRenderers.Add(childSpriteRenderer);
        }
    }

    void LateUpdate()
    {
        foreach (var s in spriteRenderers) {
            s.sortingOrder = (int)(baseOrder - transform.position.y * 100) % 10000;
        }
    }
}
