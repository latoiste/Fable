using System;

public interface ICoinProvider
{
    event Action<int> OnActivated;
    int CoinAmount();
}