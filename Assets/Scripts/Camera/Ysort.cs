using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private int baseOrder = 1000;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(baseOrder - transform.position.y * 100);
    }
}
