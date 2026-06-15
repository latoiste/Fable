using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private CoinUI coinUI;

    void Awake()
    {
        if (coinUI == null) throw new Exception("CoinUI in UIManager cannot be null");
    }

    public void AddIslandListener(Island island)
    {
        Debug.Log($"set coin ui to {island.CoinsRequired}");
        coinUI.InitText(island.CoinsRequired);
        island.OnGatheredCoins += coinUI.StartAnimateCoins;
    }

    public void RemoveIslandListener(Island island)
    {
        island.OnGatheredCoins -= coinUI.StartAnimateCoins;
    }
}