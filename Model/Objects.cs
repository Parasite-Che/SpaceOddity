using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Objects : MonoBehaviour
{
    //ниже последовательно: модификатор сложности итоговый, кол-во планет, уровень сложности, выбранный игроком
    public float difficultyMultiplier;
    [NonSerialized]public float quantityOfPlanets = 0;
    public float levelOfDifficulty = 2;
    public float baseNumber = 10;

    public static Objects instance;

    public GameObject CurrentBadge;
    public GameObject TermsPanel;
    public BonusStatusView BonusesPanel;
    public GameObject BadgesNotice;

    [FormerlySerializedAs("items")] public Transform[] tutorialTextObjectsList;
    
    //префабы
    public GameObject bonusPrefab;
    public GameObject coinPrefab;
    public GameObject planetPrefab;                     //таскаю префаб отсюда, чтоб постоянно не хранить его в классе PlanetManager, временная заплатка
    public GameObject obstaclePrefab;
    public GameObject badgePanel;
    public Transform entryGameplayBadgesContainer_;
    public Transform entrySocialBadgesBContainer_;

    public Transform badgePrefab;
    
    public GameObject allCanvas;
    public GameObject badgeContent;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject preLosingPanel;
    public GameObject losingPanel;
    public GameObject selectingDataToUsePanel;
    public GameObject selectingLanguageToDownloadPanel;
    public GameObject bonusCirclePrefab;
    public TMP_Text[] coinsText;
    public TMP_Text[] sessionCoinsText;
    public TMP_Text[] scoreText;
    public TMP_Text[] sessionScoreText;
    public TMP_Text loadingText;

    public TMP_Text debugText;
    
    public string userData = "userData";

    // словарь, в котором будем хранить все наши джейсОны
    public Dictionary<string, string> jsonStorageDictionary;

    public void Initialize()
    {
        instance = this;
        jsonStorageDictionary = new Dictionary<string, string>();
    }

}
