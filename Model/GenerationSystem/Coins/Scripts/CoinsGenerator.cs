using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class CoinsGenerator : MonoBehaviour
{
    public static CoinsGenerator Instance { get; private set; }
    public List<Coin> activeCoinList;
    public int coinSpawnDelayCounter;

    private Coin[] _coinsPool=new Coin[40];

    public void Initialize()
    {
        CreateCoins();
        Instance = this;
        activeCoinList = new List<Coin>();
        Events.onPlanetSpawned += GenerateCoin;
        Events.onGameRestart += RestartCoins;
    }


    //������� ��� �������� ������� ����� � ��������
    public void GenerateCoin(Planet planet)
    {
        if (coinSpawnDelayCounter > 0)
        {
            coinSpawnDelayCounter--;
            return;
        }
        if (UnityEngine.Random.Range(1, 100) > Objects.instance.baseNumber*Objects.instance.difficultyMultiplier)
        {
            Coin coin = GetCoin();
            coin.coinTransform.position = GetCoinPos(planet);
            coin.gameObject.SetActive(true);
            activeCoinList.Add(coin);
        }
    }

    private Vector3 GetCoinPos(Planet obj)
    {
        //����� ���������� ��������������� ��������� ���������� ��� �������� ����� ����� � ���������
        Vector3 coinPos;
        double angle=UnityEngine.Random.Range(0,2*3.1415f);
        float radius = UnityEngine.Random.Range(obj.planet.orbitDiameter+5, obj.planet.orbitDiameter+25);
        coinPos.x = obj.transform.position.x + radius * (float)Math.Cos(angle);
        coinPos.y = obj.transform.position.y + radius * (float)Math.Sin(angle);
        coinPos.z = obj.transform.position.z;
        return coinPos;
    }

    public void ResetCoinsOnRestart()
    {
        coinSpawnDelayCounter = 2;
        foreach (Coin coin in activeCoinList)
        {
            if (coin.gameObject.activeSelf) coin.gameObject.SetActive(false);
        }
        activeCoinList.RemoveRange(0, activeCoinList.Count);
    }

    //��� ������ �� 10 ����������� ������ ������ �� �����, ��������� ������
    void CreateCoins()
    {
        for (int i = 0; i < 40; i++)
        {
            Coin coin=Instantiate(Objects.instance.coinPrefab, new Vector3(10000, 10000, -100), new Quaternion(0, 0, 0, 0)).GetComponent<Coin>();
            _coinsPool[i] = coin;
            _coinsPool[i].InitializeCoin(CoinDatabase.Instance.GetCoin(i/10));
            _coinsPool[i].gameObject.SetActive(false);
        }
    }

    //�������� ���������� ������� ������� ���� �� �����
    Coin GetCoin()
    {
        float chanceValue = UnityEngine.Random.Range
            (1, 100 + Objects.instance.levelOfDifficulty * Objects.instance.difficultyMultiplier);

        if (chanceValue < 50)
        {
            for (int i = 0; i < 10; i++)
                if (!_coinsPool[i].gameObject.activeSelf)
                    return _coinsPool[i];
        }
        else if (chanceValue >= 50 && chanceValue < 75)
        {
            for (int k = 10; k < 20; k++)
                if (!_coinsPool[k].gameObject.activeSelf)
                    return _coinsPool[k];
        }
        else if (chanceValue >= 75 && chanceValue < 90)
        {
            for (int l = 20; l < 30; l++)
                if (!_coinsPool[l].gameObject.activeSelf)
                    return _coinsPool[l];
        }
        else
        {
            for (int a = 30; a < 40; a++)
                if (!_coinsPool[a].gameObject.activeSelf)
                    return _coinsPool[a];
        }
        return null;
    }

    void RestartCoins()
    {
        foreach (var coin in _coinsPool)
        {
            coin.gameObject.SetActive(false);
        }
    }
}