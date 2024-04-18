using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public bool debugMode = false;
    
    public int coins;
    public SpriteRenderer levelSprite;

    public bool IsBetweenPlanets = false;
    public GameObject CurrentPlanet;

    public bool isInMenu = true;

    public delegate void NewPlanet(GameObject currentPlanet);
    
    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("Coins", 0);
        CurrentPlanet = PlanetManager.instance.currentPlanet.gameObject;
    }

    void Start()
    {
        Events.onOrbitEntered += GetRewardFromPlanet;
        Events.onShipPurchased += DeductCoinsFromShipPurchase;
        Events.onLevelSelected += ChangeLevel;
        Events.onDeathEvent += Die;
        Events.onCoinPickedUp += PickUpCoin;

        //Objects.instance.sessionCoinsText.text = PlayerPrefs.GetInt("money").ToString();
    }

    // поменять, чтобы при наличии бонуса респавниться на ближайшей планете
    private void Die()
    {
        if (BonusManager.instance.BonusIsActive(Bonuses.type.secondChance))
        {
            Events.onBonusActivated(Bonuses.type.secondChance);
            Respawn();
            return;
        }
        
        JSONTest.UserLocalDataObject.LocalUserInfo.scores += PlayerPrefs.GetInt("score");
        JSONTest.UserLocalDataObject.LocalUserStatistics.total_death++;
        Objects.instance.preLosingPanel.SetActive(true);
        Events.onGameOver?.Invoke();
        //Restart();

        //Time.timeScale = 0;
        //DeathScreen.Instance.Activate(true);
    }

    public void Respawn()
    {
        Events.onRespawn?.Invoke();
        Objects.instance.preLosingPanel.SetActive(false);
        Planet planet = PlanetManager.instance.currentPlanet;
        if (planet == null)
        {
            planet = PlanetManager.instance.previousPlanet;
        }
        if (planet == PlayerController.Instance.planetNone)
        {
            planet = PlanetManager.instance.lastPlanet;
        }
        if (planet != null && planet != PlayerController.Instance.planetNone)
        {
            PlayerController.Instance.transform.position =
            planet.transform.position + new Vector3(planet.planet.orbitDiameter / 2 - 1, 0);
            PlayerMovementController.instance.Rotator();
            UnityEngine.Debug.Log(planet.transform.position);
        }
        
        Objects.instance.losingPanel.SetActive(false);

        
        //Restart();
    }

    public void GetRewardFromPlanet(Planet planet)
    {
        AddCoins(planet.planet.coinReward);
    }

    public void DeductCoinsFromShipPurchase(ShipThumbnailSO ship)
    {
        AddCoins(-ship.cost);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        if (JSONTest.UserLocalDataObject != null)
        {
            if(amount >= 0)
                JSONTest.UserLocalDataObject.LocalUserStatistics.total_coins += amount;
            JSONTest.UserLocalDataObject.LocalUserInfo.money += amount;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + amount);
            SceneTextController.TextListUpdater(Objects.instance.sessionCoinsText, PlayerPrefs.GetInt("money").ToString());
            SceneTextController.TextListUpdater(Objects.instance.coinsText,
                JSONTest.UserLocalDataObject.LocalUserInfo.money.ToString());
        }
    }

    public void PickUpCoin(Coin coin)
    {
        AddCoins(Mathf.RoundToInt(coin.reward * BonusManager.instance.rewardMultiplier));
    }

    public void ChangeLevel(LevelScriptableObject level)
    {
        levelSprite.sprite = level.sprite;
    }

    public void PauseGame()
    {
        Events.onGamePaused?.Invoke();
    }
    public void UnPauseGame()
    {
        Events.onGameUnPaused?.Invoke();
    }

    public void Restart()
    {
        //LiveController.lifeCount--;
        StartCoroutine(SceneTextController.TextCounterInitializer());

        PlayerPrefs.SetInt("Coins", coins);
        
        JSONTest.AllAchievementCheck();
        JSONTest.SaveUserLocalDataOnDevice();
        
        Events.onGameRestart?.Invoke();
        Objects.instance.losingPanel.SetActive(false);
        Objects.instance.preLosingPanel.SetActive(false);
        Objects.instance.gameplayUI.SetActive(false);
        Objects.instance.mainMenuUI.SetActive(true);
        SceneTextController.TextListUpdater(Objects.instance.sessionCoinsText, PlayerPrefs.GetInt("money").ToString());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    

    public void SetInMainMenu(bool state)
    {
        isInMenu = state;
    }
}
