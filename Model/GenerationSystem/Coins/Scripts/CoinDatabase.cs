using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDatabase : MonoBehaviour
{
    public static CoinDatabase Instance { get; private set; }

    [SerializeField] public List<CoinScriptableObject> coinList;

    public void Initialize()
    {
        Instance = this;
    }

    public CoinScriptableObject GetCoin(int index)
    {
        return coinList[index];
    }
}
