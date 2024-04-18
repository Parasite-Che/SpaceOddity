using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController instance;

    // планета не должна храниться здесь, нужно получать её откуда-то
    //public Planet CurrentPlanet;
    public GameObject Camera;
    public GameObject Dot;

    public Transform planetTransform;
    public Vector3 planetPos;
    public float planetPull;
    public Vector3 playerForce;

    private bool isSpirallingAllowed;
    private bool isMovementAllowed;
    
    // rigidbody
    private Rigidbody2D _rb;

    // скорость движения
    public float moveSpeed;
    // Скорость поворота
    public float rotationSpeed; 
    // угол поворота в секунду для орбиты с диаметром basePlanetDiameter
    public float turnDegreesBase;
    [SerializeField]
    private float turnDegreesActual;
    [SerializeField]
    private float linearSpeed;
    [SerializeField]
    private float basePlanetDiameter;
    // направление вращения
    public float inverter;

    public int revolutionsLimit_;
    public bool turnLimitReached_;
    public float turnCounter = 0;

    private delegate void SpiralIntoPlanetDel();
    private SpiralIntoPlanetDel spiralIntoPlanet;

    public void Initialize(Planet planet)
    {
        instance = this;
        isSpirallingAllowed = false;
        isMovementAllowed = true;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        Events.onOrbitEntered += ChangePlanetInfo;
        Events.onGameStart += AllowSpiralling;

        Events.onGameOver += StopMoving;
        Events.onGamePaused += StopMoving;
        Events.onGameUnPaused += AllowMovement;
        Events.onGameRestart += AllowMovement;
        Events.onRespawn += AllowMovement;

        linearSpeed = turnDegreesBase * (basePlanetDiameter / 2);

        ChangePlanetInfo(planet);

        transform.position =
            planetTransform.position + new Vector3(planet.planet.orbitDiameter / 2 - 1, 0);
        turnLimitReached_ = false;
    }

    public void MoveAroundPlanet(/* Vector3 planetPos */)
    {
        if (!isMovementAllowed) return;
        spiralIntoPlanet?.Invoke();

        _rb.velocity = Vector3.zero;

        // двигаемся вокруг точки
        transform.RotateAround(planetPos, Vector3.forward, turnDegreesActual * inverter * Time.fixedDeltaTime);
        playerForce = planetPos - transform.position;

        if (!isSpirallingAllowed) return;

        // считаем угол поворота
        turnCounter += turnDegreesActual / 360 * Time.fixedDeltaTime;
        if (turnCounter >= revolutionsLimit_ && !turnLimitReached_)
        {
            // после некоторого числа поворотов корабль начинает падать на планету
            spiralIntoPlanet = SpiralIntoPlanet;
            turnLimitReached_ = true;
        }

        // луч до планеты
        Debug.DrawRay(transform.position, playerForce, Color.green);
        Debug.DrawRay(transform.position, planetPos - transform.position, Color.red);

    }

    // Вынести в скрипт с работай планет
    public void ChangePlanetInfo(Planet currentPlanet)
    {
        moveSpeed += 1/400;
        planetTransform = currentPlanet.transform;
        planetPos = currentPlanet.transform.position;
        planetPull = currentPlanet.planet.attractionForce;
        playerForce = new Vector3(planetPos.x - transform.position.x, planetPos.y - transform.position.y);
        revolutionsLimit_ = currentPlanet.planet.revolutionsLimit;
        turnCounter = 0;
        turnLimitReached_ = false;
        turnDegreesActual = linearSpeed / (currentPlanet.planet.orbitDiameter / 2);
        StopSpiralling();
    }

    public void MoveBetweenPlanets(GameInput GI)
    {
        if (!isMovementAllowed) return;
        //_rb.velocity = playerForce * speed;
        ShipRotation();
        _rb.AddForce(playerForce * moveSpeed, ForceMode2D.Force);
        _rb.AddForce(GI.ShipControl() * (gameObject.transform.position - Dot.transform.position/*(-gameObject.transform.right)*/) * rotationSpeed);
        _rb.velocity = Vector3.zero;

    }

    public void Jump()
    {
        playerForce = new Vector2(inverter * playerForce.y, -1 * playerForce.x * inverter);
        playerForce = 10 * playerForce.normalized;
        _rb.AddForce(playerForce * 0.0004f, ForceMode2D.Impulse);
    }

    private void ShipRotation()
    {
        transform.up = Vector2.MoveTowards(gameObject.transform.up, _rb.velocity, Time.deltaTime * 20);
        playerForce = transform.up;
        Debug.DrawRay(transform.position, playerForce, Color.red);
        Debug.DrawRay(transform.position, planetPos - transform.position, Color.red);
    }

    void SpiralIntoPlanet()
    {
        Utils.PullByGravity(_rb, planetTransform, planetPull);
    }

    public void StopSpiralling()
    {
        spiralIntoPlanet = null;
        turnLimitReached_ = false;
    }

    public void Rotator()
    {
        float angle;
        angle = Vector3.SignedAngle(transform.up, planetPos - transform.position, Vector3.forward);
        Debug.Log(angle);

        if (angle < 0)
        {
            transform.Rotate(0, 0, 90 + angle);
            inverter = -1;
        }
        else if (angle > 0)
        {
            transform.Rotate(0, 0, -90 + angle);
            inverter = 1;
        }
    }

    private void StopMoving()
    {
        _rb.velocity = Vector2.zero;
        isMovementAllowed = false;
    }
    

    public void ResetMovementOnRestart()
    {
        moveSpeed = 0.2f;
        turnLimitReached_ = false;
        isSpirallingAllowed = false;
        StopSpiralling();
        ChangePlanetInfo(PlanetManager.instance.firstPlanet);
    }

    private void AllowSpiralling()
    {
        isSpirallingAllowed = true;
    }

    private void AllowMovement()
    {
        isMovementAllowed = true;
    }
}
