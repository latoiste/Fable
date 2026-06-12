using System;
using UnityEngine;

public interface ICoinProvider
{
    event Action<int> OnActivated;
    int CoinAmount();
    bool IsActive { get; }
    Vector3 Position { get; }
}