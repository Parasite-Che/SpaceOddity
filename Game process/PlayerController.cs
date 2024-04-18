using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public ShipThumbnailSO shipData;

    public float health = 100;
    public float maxHealth = 100;
    public float fuel = 100;
    public float maxFuel = 100;
    public static float fuelUsage = 1f;
    public bool canSteer = true;

    public CircleCollider2D attractionRange;

    public bool IsTakingEffectPlanet = true;
    public bool isImmortal = false;

    public Planet currentPlanet;
    public Planet planetNone;

    private void Awake()
    {
        Instance = this;
        canSteer = true;
        /*
        Localization local = new Localization();
        local.localText.Add(new Hashtable());
        local.localText.Add(new Hashtable());
        local.localText[0].Add("PlayButton", "Играть");
        local.localText[0].Add("SettingsButton", "Настройки");
        local.localText[1].Add("PlayButton", "Play");
        local.localText[1].Add("SettingsButton", "Settings");
        JsonController.SaveLocalInRes(local);*/
    }

    private void Start()
    {
        ChangeShip(shipData);
        planetNone.GetComponent<Planet>().InitializePlanet(planetNone.GetComponent<Planet>().planet);
        Events.onShipSelected += ChangeShip;
        Events.onOrbitEntered += EnterPlanet;
        Events.onOrbitLeft += LeavePlanet;
        Events.onBonusActivated += FreezeFuel;
        Events.onBonusDeactivated += UnFreezeFuel;
        Events.onGameRestart += Reset;
        Events.onRespawn += Reset;
        currentPlanet = PlanetManager.instance.currentPlanet;
    }

    private void Update()
    {
        if (IsTakingEffectPlanet)
        {
            currentPlanet.AffectPlayer(this);
        }
        if (fuel <= 0 || health <= 0)
        {
            canSteer = false;
        }
        if (fuel > 0)
        {
            canSteer = true;
        }
    }

    public void FreezeFuel(Bonuses.type type)
    {
        if (type == Bonuses.type.fuel)
        {
            fuel = maxFuel;
            fuelUsage = 0f;
        }
    }

    public void UnFreezeFuel(Bonuses.type type)
    {
        if (type == Bonuses.type.fuel)
        {
            fuelUsage = 1f;
        }
    }

    static public void ChangeFuel(float amount)
    {
        Instance.fuel += amount * fuelUsage;
    }

    static public void ChangeHealth(float amount)
    {
        Instance.health += amount;
    }

    public void ChangeShip(ShipThumbnailSO ship)
    {
        ShipScriptableObject shipSO = ShipDatabase.instance.GetShip(ship.name);
        gameObject.GetComponent<SpriteRenderer>().sprite = shipSO.sprite;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = shipSO.animator;
    }

    public void EnterPlanet(Planet _planet)
    {
        IsTakingEffectPlanet = true;
        currentPlanet = _planet;
    }

    public void LeavePlanet(Planet _planet)
    {
        IsTakingEffectPlanet = true;
        currentPlanet = planetNone;
    }

    private void Reset()
    {
        fuel = maxFuel;
        canSteer = true;
        UnFreezeFuel(Bonuses.type.fuel);
    }
}
