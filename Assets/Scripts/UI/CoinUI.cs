using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int displayedCoins;
    private int maxCoins;
    private Coroutine coinCountCoroutine;

    // public async void Start()
    // {
    //     InitText(30);
        
    //     Debug.Log("Starting");
    //     await Task.Delay(5000);
    //     StartAnimateCoins(20);

    //     await Task.Delay(500);
    //     StartAnimateCoins(30);
    // }

    public void InitText(int maxCoins)
    {
        this.maxCoins = maxCoins;

        text.text = $"0/{maxCoins}";
    }

    // target coin total semua coin
    public void StartAnimateCoins(int targetCoins)
    {
        if (coinCountCoroutine != null) StopCoroutine(coinCountCoroutine);
        
        coinCountCoroutine = StartCoroutine(UpdateCoins(targetCoins));
    }

    public IEnumerator UpdateCoins(int targetCoins)
    {
        while (displayedCoins != targetCoins)
        {
            int difference = targetCoins - displayedCoins;
            displayedCoins += Mathf.CeilToInt(difference * Time.deltaTime);

            if (displayedCoins > targetCoins) displayedCoins = targetCoins;

            text.text = $"{displayedCoins}/{maxCoins}";

            yield return new WaitForSeconds(0.02f);
        }

        coinCountCoroutine = null;
    }
}