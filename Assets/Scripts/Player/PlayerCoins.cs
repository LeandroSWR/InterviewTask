using System;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField] private int currentCoins = 0;
    public int CurrentCoins => currentCoins;

    public Action<int> OnRecievedCoins;
    public Action<int> OnSpendCoins;

    public void AddCoins(int coins)
    {
        currentCoins += coins;
        OnRecievedCoins?.Invoke(coins);
    }

    public bool TrySpendCoins(int coins)
    {
        if (currentCoins - coins < 0)
        {
            return false;
        }
        else
        {
            currentCoins -= coins;
            OnSpendCoins?.Invoke(coins);
            return true;
        }
    }
}
