using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Island : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Transform coinProviders;

    public int TotalCoins { get; private set; }
    private int coinsRequired;
    private int coinsGathered = 0;
    private List<ICoinProvider> coinProviderRefs = new();

    public Action<int> OnGatheredCoins;

    public UnityEvent OnEnoughCoins;
    public bool wasCalled = false;

    void Awake()
    {
        if (coinProviders == null) throw new Exception($"Attribute coinProviders in {this} cannot be null");
        if (altar == null) throw new Exception($"Attribute altar in {this} cannot be null");
        
        foreach (Transform child in coinProviders)
        {
            bool isCoinProvider = child.TryGetComponent<ICoinProvider>(out ICoinProvider provider);

            if (isCoinProvider)
            {
                provider.OnActivated += AddCoinsGathered;
                TotalCoins += provider.CoinAmount();
                coinProviderRefs.Add(provider);
            } else
            {
                Debug.LogWarning($"{child.name} in {this} is not a CoinProvider");
            }
        }

        coinsRequired = (int)(TotalCoins * 0.5);

        UIManager.instance.AddIslandListener(this);
        OnEnoughCoins.AddListener(GameManager.instance.StartPreloadIsland);
    }

    public Vector3 GetSpawnPoint() => altar.transform.position;

    public void AddCoinsGathered(int amount)
    {
        coinsGathered += amount;
        OnGatheredCoins.Invoke(coinsGathered * 10);

        if (coinsGathered >= coinsRequired)
        {
            if (!wasCalled) {
                wasCalled = true;
                OnEnoughCoins.Invoke();
            }
        }
    }

    void OnDestroy()
    {
        foreach (ICoinProvider provider in coinProviderRefs)
        {
            if (provider != null) provider.OnActivated -= AddCoinsGathered;
        }

        OnEnoughCoins.RemoveListener(GameManager.instance.StartPreloadIsland);
    }
}