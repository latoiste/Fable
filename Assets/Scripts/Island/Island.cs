using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Island : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Transform coinProviders;

    private int totalCoins;
    public int CoinsRequired { get; private set; }
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
                totalCoins += provider.CoinAmount();
                coinProviderRefs.Add(provider);
            } else
            {
                Debug.LogWarning($"{child.name} in {this} is not a CoinProvider");
            }
        }

        CoinsRequired = (int)(totalCoins * 0.5) + 1;

        UIManager.instance.AddIslandListener(this);
        OnEnoughCoins.AddListener(GameManager.instance.StartPreloadIsland);
        altar.OnAltarActivated.AddListener(TrySwitchIslands);
    }

    public void AddCoinsGathered(int amount)
    {
        coinsGathered += amount;
        OnGatheredCoins.Invoke(coinsGathered);

        if (coinsGathered >= CoinsRequired)
        {
            if (!onEnoughCoinsCalled) {
                // Debug.Log("OnEnoughCoins invoked");
                onEnoughCoinsCalled = true;
                OnEnoughCoins.Invoke();
            }
        }
    }

    public List<ICoinProvider> ActiveCoinProviders() => coinProviderRefs.Where(c => c.IsActive).ToList();

    private void TrySwitchIslands()
    {
        // Debug.Log("TrySwitchIslands called");
        // Debug.Log($"ActivateAltarCalled: {onActivateAltarCalled}");
        if (coinsGathered >= CoinsRequired)
        {
            if (!onActivateAltarCalled) {
                onActivateAltarCalled = true;
                Debug.Log("SwitchIslands called");
                AudioManager.instance.PlaySfx(AudioClips.IslandComplete);
                _ = GameManager.instance.SwitchIslands();

                int bonusTime = (coinsGathered - CoinsRequired) * 2;
                GameManager.instance.AddTime(bonusTime);
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