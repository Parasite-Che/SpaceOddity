using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Device.SystemInfo;
using TMPro;


public class Main : MonoBehaviour
{
    public LoadingManager LoadManger;
    public PlayerMovementController playerMovementController;
    public GameInput gameInput;
    public GyroscopeController gyroscopeController;
    public LocalizationController LocalizationController;
    public BonusDatabase bonusDatabase;
    public BonusManager bonusManager;
    public PlanetManager planetManager;
    public PlanetDatabase planetDatabase;
    public CoinsGenerator coinsGenerator;
    public CoinDatabase coinDatabase;
    public ObstaclesGenerator obstaclesGenerator;
    public SoundDatabase soundDatabase;
    public TutorialController tutorialController;

    public CameraController cameraController;
    public CameraConfiner cameraConfiner;
    public JSONTest JsonTest;
    public WebTest WebTest;


    private Objects _objects;
    public static ShipStatesLogic ShipStatesLogic;

    public static bool movementIsAllowed = true;

    //public delegate void Function();

    private void Awake()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(LongProcessList());

        tutorialController.GI = gameInput;
        
        // ����������� �������� � ����� ������ �������
        gameObject.TryGetComponent<Objects>(out _objects);
        //gyroscopeController.GyroOn();

        // ������������� ���� ������
        Events.Initialize();
        Initialize();

        YandexBanner.instance?.RequestBanner();
        RewardedCoin.instance?.RequestRewardedAd();
        RewardedRespawn.instance?.RequestRewardedAd();

        Events.onGameOver += StopMovement;
        Events.onGameRestart += StartMovement;
        Events.onGamePaused += StopMovement;
        Events.onGameUnPaused += StartMovement;
        Events.onRespawn += StartMovement;

        Events.onGameRestart += ResetGameSystems;
    }
    
    private void Start()
    {
        //ShipStatesLogic = new ShipStatesLogic(new MovingAroundPlanet(), this);
        //playerMovementController.ChangePlanetInfo(planetManager.currentPlanet);
        
    }

    private void OnApplicationQuit()
    {
        JSONTest.UserLocalDataObject.GameStretchList[JSONTest.UserLocalDataObject.GameStretchList.Count - 1].end =
            DateTime.Now;
        JSONTest.SaveUserLocalDataOnDevice();
    }

    private void FixedUpdate()
    {
        if (movementIsAllowed) PlayerMovement();
    }

    /// <summary>
    /// 
    /// </summary>

    private void PlayerMovement() // �������� �������
    {
        if (ShipStatesLogic != null)
        {
            ShipStatesLogic.Moving();
        }
    }

    public void Initialize() // ������������� ���� ������� �� _objects
    {
        cameraConfiner.Initialize();
        _objects.Initialize();
        coinDatabase.Initialize();
        coinsGenerator.Initialize();
        obstaclesGenerator.Initialize();
        bonusDatabase.Initialize();
        bonusManager.Initialize();
        planetDatabase.Initialize();
        planetManager.Initialize();
        soundDatabase.Initialize();
        
        cameraController.Initialize();
        playerMovementController.Initialize(planetManager.firstPlanet);
        // ������������� ���������� PlayrMovementController'�
    }

    public void ResetGameSystems()
    {
        planetManager.ResetPlanetsOnRestart();
        bonusManager.ResetBonusesOnRestart();
        coinsGenerator.ResetCoinsOnRestart();
        obstaclesGenerator.ResetObstaclesOnRestart();
        playerMovementController.ResetMovementOnRestart();
        cameraController.UpdateConfiner();
    }

    public void MovingBetweenPlanet() // �������� ����� ���������
    {
        playerMovementController.MoveBetweenPlanets(gameInput);
    }

    public void MovingAroundPlanet() // �������� ������ �������
    {
        playerMovementController.MoveAroundPlanet();
    }

    public void StopSpirallingIntoPlanet()
    {
        playerMovementController.StopSpiralling();
    }

    public void ShipUncouplingJump()  // ������������ �� �������
    {
        //_objects.PlayerMovementController.Jump();
        Events.onOrbitLeft?.Invoke(planetManager.currentPlanet);
    }

    private void StopMovement()
    {
        movementIsAllowed = false;
    }

    private void StartMovement()
    {
        movementIsAllowed = true;
    }

    private IEnumerator LongProcessList()
    {
        LoadManger = gameObject.GetComponent<LoadingManager>();

        yield return StartCoroutine(LoadManger.StartScreenProcess());

        LoadManger.LoadingPanel.SetActive(true);

        yield return StartCoroutine(WebTest.Initialize());
        
        yield return StartCoroutine(JsonTest.UserLocalDataInitializer());

        JSONTest.UserLocalDataObject.GameStretchList.Add(new GameStretch());
        JSONTest.UserLocalDataObject.GameStretchList[JSONTest.UserLocalDataObject.GameStretchList.Count - 1].start =
            DateTime.Now;

        JSONTest.AllAchievementCheck();
        
        Events.onSetAudioSettings();
        
        yield return StartCoroutine(SceneTextController.TextCounterInitializer());

        yield return StartCoroutine(LocalizationController.LocalizationInitializer(_objects.selectingLanguageToDownloadPanel));
        
        yield return StartCoroutine(ServerManager.LoadBadgesInPath(JsonTest.achievementDTOList, 
            _objects.loadingText, LoadManger.Bar));
        
        yield return StartCoroutine(LocalizationController.LocalizeText(
            LocalizationController.FindTheObjects("LocalizationText"), 
            JsonController.LoadLocalizationfromRes(JSONTest.UserLocalDataObject.LocalUserInfo.language),
            LoadManger.Bar, _objects.loadingText));


        yield return StartCoroutine(BadgesMenu.instance.FillBadgesMenu(JsonTest.achievementDTOList, 
            _objects.loadingText, LoadManger.Bar));

        ShipStatesLogic = new ShipStatesLogic(new MovingAroundPlanet(), this);

        if (JSONTest.UserLocalDataObject.LocalUserInfo.isAgreePrivacy == false)
        {
            Objects.instance.TermsPanel.SetActive(true);
        }
        LoadManger.LoadingPanel.SetActive(false);
    }
}
