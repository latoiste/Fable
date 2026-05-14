using System;
using UnityEngine;
using UnityEngine.Events;

public class Island : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Transform coinProviders;

    private int totalCoins;
    private int coinsRequired;
    private int coinsGathered = 0;

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
                totalCoins += provider.GetCoinAmount();
            } else
            {
                Debug.LogWarning($"{child.name} in {this} is not a CoinProvider");
            }
        }

        coinsRequired = (int)(totalCoins * 0.5);

        OnEnoughCoins.AddListener(GameManager.instance.StartPreloadIsland);
    }

    public Vector3 GetSpawnPoint() => altar.transform.position;

    public void AddCoinsGathered(int amount)
    {
        coinsGathered += amount;

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
        foreach (Transform child in coinProviders)
        {
            bool isCoinProvider = child.TryGetComponent<ICoinProvider>(out ICoinProvider provider);

            if (isCoinProvider)
            {
                provider.OnActivated -= AddCoinsGathered;
            }
        }

        OnEnoughCoins.RemoveListener(GameManager.instance.StartPreloadIsland);
    }
}