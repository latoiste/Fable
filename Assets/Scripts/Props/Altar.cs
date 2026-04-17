using System;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> runes;
    private float targetAlpha = 0;

    void Awake()
    {
        if (runes.Count != 4) throw new Exception($"Attribute runes in {this} cannot be null");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) targetAlpha = 1;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) targetAlpha = 0;
    }

    void Update()
    {
        float currentAlpha = runes[0].color.a;
        float newAlpha= Mathf.Lerp(currentAlpha, targetAlpha, 2 * Time.deltaTime);

        Color newColor = runes[0].color;
        newColor.a = newAlpha;

        foreach (SpriteRenderer r in runes)
        {
            r.color = newColor;
        }
    }
}
