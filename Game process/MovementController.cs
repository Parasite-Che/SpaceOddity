using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject PlanetToDelete, Camera;
    public GameObject Dot;
    public static MovementController Instance { get; private set; }

    public Vector3 PlayerForce;
    public Touch touch = Input.GetTouch(0);
    public float inverter;
    public float speed = 0.00002f;
    public float turnDegrees;
    public float ticksFor2PI;

    public float distanceToPlanet;
    public float deltaPerFullTurn;
    public int revolutionsLimit_;
    private bool turnLimitReached_;
    private bool gyroOn;
    public float turnCounter = 0;

    public event GameManager.NewPlanet DeletePlanetsTime;

    private Rigidbody2D _rb;

    private delegate void Move();
    Move move;

    private float xPos;
    private float yPos;
    private Vector3 planetPos;

    Planet planet = null;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        if (PlayerPrefs.GetInt("gyroOn") != 0 && PlayerPrefs.GetInt("gyroOn") != 1)
        {
            gyroOn = true;
            PlayerPrefs.SetInt("gyroOn", 1);
        }
        else {
            gyroOn = Convert.ToBoolean(PlayerPrefs.GetInt("gyroOn"));
        }
        
    }

    private void Start()
    {
        Events.onOrbitEntered += UpdatePlanetInfo;
        planet = GameManager.Instance.CurrentPlanet.GetComponent<Planet>();
        planetPos = new Vector3(planet.gameObject.transform.position.x, planet.gameObject.transform.position.y);
        PlayerForce = new Vector2(planetPos.x - transform.position.x, planetPos.y - transform.position.y);
        inverter = 1;
        move = MoveAroundPlanet;
        ticksFor2PI = (360f / turnDegrees);
        UpdatePlanetInfo(planet);
        //Events.onControlSchemeChange += GyroOnChanging;
        EnterStartingPlanet();
    }

    private async void EnterStartingPlanet()
    {
        await System.Threading.Tasks.Task.Delay(1);
        Events.onOrbitEntered?.Invoke(planet);
    }

    private void FixedUpdate()
    {
        move();
        transform.up = Vector2.MoveTowards(transform.up, _rb.velocity, Time.deltaTime * 20);
    }

    private void Update()
    {
        // контроль ввода от пользователя; при нажатии на экран отлетаем от планеты, если мы у планеты
        Debug.DrawLine(PlayerForce,PlayerForce,Color.yellow);
        if (Input.GetMouseButtonDown(0) == true && !GameManager.Instance.IsBetweenPlanets && !GameManager.Instance.isInMenu)//jump to space from planet's orbit
        {
            PlayerForce = new Vector2(planetPos.x - transform.position.x, planetPos.y - transform.position.y);
            GameManager.Instance.IsBetweenPlanets=true;
            move = MoveBetweenPlanets;
            _rb.velocity = Vector3.zero;
            //planet = GameManager.Instance.CurrentPlanet.GetComponent<Planet>();
            Events.onOrbitLeft?.Invoke(planet);
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.debugMode)
            {
                Events.onDeathEvent.Invoke();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)//it describes what happend in case of entiring to new orbit
    {
        Debug.Log(collision.name);
        Planet _planet = collision.GetComponentInParent<Planet>();
        if (_planet != null && GameManager.Instance.IsBetweenPlanets)
        {
            Events.onOrbitEntered?.Invoke(_planet);
            Destroy(GameManager.Instance.CurrentPlanet, 1);

            planet = _planet;
            GameManager.Instance.CurrentPlanet = _planet.gameObject;
            
            DeletePlanetsTime(_planet.gameObject);
            GameManager.Instance.IsBetweenPlanets = false;
            move = MoveAroundPlanet;

            _rb.velocity = Vector3.zero;
            //Events.onOrbitEntered?.Invoke(planet);
            Rotator(planet.gameObject);
            return;
        }
    }

    private void GyroOnChanging() {
        gyroOn ^= true;
        Debug.Log(gyroOn);
        Debug.Log(Convert.ToInt32(gyroOn));
        PlayerPrefs.SetInt("gyroOn", Convert.ToInt32(gyroOn));
    }

    // метод для движения вокруг планеты
    void MoveAroundPlanet()
    {
        // двигаемся вокруг точки
        transform.RotateAround(planetPos, Vector3.forward, turnDegrees * inverter);
        PlayerForce = planetPos - transform.position;

        // считаем угол поворота
        turnCounter += GameManager.Instance.isInMenu ? 0 : turnDegrees / 360;
        if (turnCounter >= revolutionsLimit_ && !turnLimitReached_)
        {
            // после некоторого числа поворотов корабль начинает падать на планету
            move += SpiralIntoPlanet;
            turnLimitReached_ = true;
        }

        //transform.up = new Vector3(PlayerForce.y, -PlayerForce.x, 0);
        Debug.DrawRay(transform.position, PlayerForce, Color.green);
        Debug.DrawRay(transform.position, planetPos - transform.position, Color.red);
    }

    void MoveBetweenPlanets()
    {
        // применения силы для движения объекта
        _rb.AddForce(PlayerForce * speed, ForceMode2D.Force);
        //if (Input.GetTouch(0).position.x > Camera.transform.position.x && Input.GetTouch(0).phase == TouchPhase.Stationary)
        //{
        //    PlayerForce = Quaternion.Euler(0, 0, -1f) * PlayerForce;
        //    _rb.velocity = PlayerForce;
        //}
        //if (Input.GetTouch(0).position.x < Camera.transform.position.x && Input.GetTouch(0).phase == TouchPhase.Stationary)
        //{
        //    PlayerForce = Quaternion.Euler(0, 0, 1f) * PlayerForce;
        //    _rb.velocity = PlayerForce;
        //}

        // в редакторе
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
        {
            //PlayerForce = Quaternion.Euler(0, 0, -1f) * PlayerForce;
            //_rb.velocity = PlayerForce;
            _rb.AddForce(-(transform.position - Dot.transform.position) * 0.004f);
            PlayerController.ChangeFuel(-2f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A)) {
            //PlayerForce = Quaternion.Euler(0, 0, 1f) * PlayerForce;
            //_rb.velocity = PlayerForce;
            _rb.AddForce((transform.position - Dot.transform.position) * 0.004f);
            PlayerController.ChangeFuel(-2f * Time.deltaTime);
        }
        // на телефоне
#elif UNITY_ANDROID
        if (gyroOn == true) {
            if (GyroscopeController.gyroscope.gravity.x > 0.1) {
                //PlayerForce = Quaternion.Euler(0, 0, -1f) * PlayerForce;
                //_rb.velocity = PlayerForce;
                _rb.AddForce(-(gameObject.transform.position - Dot.transform.position) * 0.001f);
                Debug.Log(GyroscopeController.gyroscope.gravity.x);
            }
            else if (GyroscopeController.gyroscope.gravity.x < -0.1) {
                //PlayerForce = Quaternion.Euler(0, 0, 1f) * PlayerForce;
                //_rb.velocity = PlayerForce;
                _rb.AddForce((gameObject.transform.position - Dot.transform.position) * 0.001f);
                Debug.Log(GyroscopeController.gyroscope.gravity.x);
            }
        }
        else
        {
            if (Input.GetTouch(0).position.x > Camera.transform.position.x && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                PlayerForce = Quaternion.Euler(0, 0, -1f) * PlayerForce;
                _rb.velocity = PlayerForce;
            }
            if (Input.GetTouch(0).position.x < Camera.transform.position.x && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                PlayerForce = Quaternion.Euler(0, 0, 1f) * PlayerForce;
                _rb.velocity = PlayerForce;
            }
        }
#endif
        Debug.DrawRay(transform.position, PlayerForce, Color.red);
        Debug.DrawRay(transform.position, planetPos - transform.position, Color.red);
    }

    // метод для падения корабля на планету
    void SpiralIntoPlanet()
    {
        xPos = transform.position.x + (planetPos.x - transform.position.x > 0 ? (deltaPerFullTurn / ticksFor2PI) : -(deltaPerFullTurn / ticksFor2PI));
        yPos = transform.position.y + (planetPos.y - transform.position.y > 0 ? (deltaPerFullTurn / ticksFor2PI) : -(deltaPerFullTurn / ticksFor2PI));
        gameObject.transform.position = new Vector2(xPos, yPos);
    }

    // прыжок, придаём силу, чтобы объект быстро отлетел
    void Jump()
    {
        PlayerForce = new Vector2(inverter * PlayerForce.y, -1 * PlayerForce.x * inverter);
        PlayerForce = 10 * PlayerForce.normalized;
        _rb.AddForce(PlayerForce * 0.0004f, ForceMode2D.Impulse);
    }

    // для вращения в правильную сторону
    void Rotator(GameObject Planet)
    {
        float angle;
        angle = Vector3.SignedAngle(transform.up, planetPos - transform.position, Vector3.forward);
        Debug.Log(angle);

        if (angle < 0)
        {
            transform.Rotate(0,0,90+angle);
            inverter = -1;
        }
        else if (angle>0)
        {
            transform.Rotate(0, 0, -90 + angle);
            inverter = 1;
        }
    }

    private void UpdatePlanetInfo(Planet planet)
    {
        planetPos = planet.gameObject.transform.position;
        distanceToPlanet = Mathf.Sqrt(Mathf.Pow(planetPos.x - transform.position.x, 2) + Mathf.Pow(planetPos.y - transform.position.y, 2));
        revolutionsLimit_ = planet.planet.revolutionsLimit;
        deltaPerFullTurn = distanceToPlanet * 0.2f / revolutionsLimit_;
        turnCounter = 0;
        turnLimitReached_ = false;
    }

    //To use mouse as a directing instrument use this piece of code
    //if (Input.GetMouseButton(0) == true)
    //        {
    //            PlayerForce = Quaternion.Euler(0, 0, 1f) * PlayerForce;
    //            GetComponent<Rigidbody2D>().velocity = PlayerForce;
    //        }
    //    if (Input.GetMouseButton(1) == true)
    //{
    //    PlayerForce = Quaternion.Euler(0, 0, -1f) * PlayerForce;
    //    GetComponent<Rigidbody2D>().velocity = PlayerForce;
    //}
}