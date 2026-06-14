using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    [SerializeField] private Direction stairDirection;

    [SerializeField] private string upperLayer;
    [SerializeField] private string upperSortingLayer;

    [SerializeField] private string lowerLayer;
    [SerializeField] private string lowerSortingLayer;
    private enum Direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            other.TryGetComponent(out Rigidbody2D rb);
            if (stairDirection == Direction.EAST && rb.linearVelocityX < -0.1f) SwitchLayers(player, upperLayer, upperSortingLayer);
            else if (stairDirection == Direction.WEST && rb.linearVelocityX > 0.1f) SwitchLayers(player, upperLayer, upperSortingLayer);
            else if (stairDirection == Direction.SOUTH && rb.linearVelocityY > 0.1f) SwitchLayers(player, upperLayer, upperSortingLayer);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            other.TryGetComponent(out Rigidbody2D rb);
            
            if (stairDirection == Direction.EAST && rb.linearVelocityX > 0.1f) {
                SwitchLayers(player, lowerLayer, lowerSortingLayer);
                Debug.Log("Switching to lower layer");    
            }
            else if (stairDirection == Direction.WEST && rb.linearVelocityX < -0.1f) SwitchLayers(player, lowerLayer, lowerSortingLayer);
            else if (stairDirection == Direction.SOUTH && rb.linearVelocityY < -0.1f) SwitchLayers(player, lowerLayer, lowerSortingLayer);
        }
    }


    private void SwitchLayers(GameObject other, string targetLayer, string targetSortingLayer)
    {
        other.layer = LayerMask.NameToLayer(targetLayer);

        SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = targetSortingLayer;

        // SpriteRenderer[] srs = other.GetComponentsInChildren<SpriteRenderer>();
        // foreach (var s in srs)
        // {
        //     s.sortingLayerName = targetSortingLayer;
        // }
    }
}
