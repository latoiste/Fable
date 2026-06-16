using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Transform coinProviders;
    [SerializeField] public string startingLayer = "Layer 1";
    [SerializeField] public string startingSortingLayer = "World - Layer 1";

    private int totalCoins;
    public int CoinsRequired { get; private set; }
    private int coinsGathered = 0;
    private List<ICoinProvider> coinProviderRefs = new();

    public event Action<int> OnGatheredCoins;

    public event Action OnIslandCompleted;
    public event Action<int> OnAddBonusTime;
    private bool onIslandCompletedCalled;

    public Vector3 SpawnPoint => altar.transform.position - new Vector3(0, -1, 0);

    void Awake()
    {
        if (coinProviders == null) throw new Exception($"Attribute coinProviders in {this} cannot be null");
        if (altar == null) throw new Exception($"Attribute altar in {this} cannot be null");
        
        onIslandCompletedCalled = false;
        
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

        float multiplier = (float)math.min(0.7, 0.3f + SaveManager.CurrentRunScore/5f * 0.2f);
        CoinsRequired = math.max((int)(totalCoins * multiplier), 1);

        altar.OnAltarActivated.AddListener(TrySwitchIslands);
    }

    public void AddCoinsGathered(int amount)
    {
        coinsGathered += amount;
        OnGatheredCoins.Invoke(coinsGathered);
    }

    public List<ICoinProvider> ActiveCoinProviders() => coinProviderRefs.Where(c => c.IsActive).ToList();

    private void TrySwitchIslands()
    {
        // Debug.Log("TrySwitchIslands called");
        // Debug.Log($"ActivateAltarCalled: {onActivateAltarCalled}");
        if (coinsGathered >= CoinsRequired)
        {
            if (!onIslandCompletedCalled) {
                onIslandCompletedCalled = true;
                OnIslandCompleted.Invoke();
                AudioManager.instance.PlaySfx(AudioClips.IslandComplete);

                int bonusTime = (coinsGathered - CoinsRequired) * 2;
                OnAddBonusTime.Invoke(bonusTime);
            }
        }
    }

    void OnDestroy()
    {
        foreach (ICoinProvider provider in coinProviderRefs)
        {
            if (provider != null) provider.OnActivated -= AddCoinsGathered;
        }

        altar.OnAltarActivated.RemoveListener(TrySwitchIslands);
    }
}