using System;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CoinUI coinUI;

    public static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }

        if (coinUI == null) throw new Exception("CoinUI in UIManager cannot be null");
    }

    public void AddIslandListener(Island island)
    {
        coinUI.InitText(island.CoinsRequired);
        island.OnGatheredCoins += coinUI.StartAnimateCoins;
    }

    public void RemoveIslandListener(Island island)
    {
        island.OnGatheredCoins -= coinUI.StartAnimateCoins;
    }
}