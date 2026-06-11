using System;
using UnityEditor.PackageManager;
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
        InitCoinUI(island.CoinsRequired);
        island.OnGatheredCoins += UpdateCoinUI;
    }

    public void RemoveIslandListener(Island island)
    {
        island.OnGatheredCoins -= UpdateCoinUI;
    }

    private void InitCoinUI(int maxCoins)
    {
        coinUI.InitText(maxCoins);
    }

    private void UpdateCoinUI(int coins)
    {
        coinUI.StartAnimateCoins(coins);
    }
}