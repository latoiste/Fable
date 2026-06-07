using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public UnityEvent OnActivateAltar;
    public bool onActivateAltarCalled;
    public UnityEvent OnEnoughCoins;
    public bool onEnoughCoinsCalled;

    // public Vector3 SpawnPoint => altar.transform.position;
    public Vector3 SpawnPoint => altar.transform.position - new Vector3(0, -1, 0);

    void Awake()
    {
        if (coinProviders == null) throw new Exception($"Attribute coinProviders in {this} cannot be null");
        if (altar == null) throw new Exception($"Attribute altar in {this} cannot be null");
        
    }

    public void Start()
    {
        onActivateAltarCalled = false;
        onEnoughCoinsCalled = false;
        
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

        coinsRequired = (int)(TotalCoins * 0.5) + 1;

        UIManager.instance.AddIslandListener(this);
        OnEnoughCoins.AddListener(GameManager.instance.StartPreloadIsland);
        altar.OnAltarActivated.AddListener(TrySwitchIslands);
    }

    public void AddCoinsGathered(int amount)
    {
        coinsGathered += amount;
        OnGatheredCoins.Invoke(coinsGathered);

        if (coinsGathered >= coinsRequired)
        {
            if (!onEnoughCoinsCalled) {
                // Debug.Log("OnEnoughCoins invoked");
                onEnoughCoinsCalled = true;
                OnEnoughCoins.Invoke();
            }
        }
    }

    private void TrySwitchIslands()
    {
        // Debug.Log("TrySwitchIslands called");
        // Debug.Log($"ActivateAltarCalled: {onActivateAltarCalled}");
        if (coinsGathered >= coinsRequired)
        {
            if (!onActivateAltarCalled) {
                onActivateAltarCalled = true;
                Debug.Log("SwitchIslands called");
                _ = GameManager.instance.SwitchIslands();
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
        UIManager.instance.RemoveIslandListener(this);
        altar.OnAltarActivated.RemoveListener(TrySwitchIslands);
    }
}