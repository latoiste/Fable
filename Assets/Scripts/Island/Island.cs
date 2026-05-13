using System;
using UnityEngine;
using UnityEngine.Events;

public class Island : MonoBehaviour
{
    [SerializeField]
    private Altar altar;

    public Vector3 SpawnPoint;
    
    public UnityEvent OnEnoughCoins;

    void Awake()
    {
        if (altar == null) throw new Exception($"Attribute altar in {this} cannot be null");
        SpawnPoint = altar.transform.position;
        
        OnEnoughCoins.AddListener(GameManager.instance.StartPreloadIsland);
    }

    void OnDestroy()
    {
        OnEnoughCoins.RemoveListener(GameManager.instance.StartPreloadIsland);
    }
}