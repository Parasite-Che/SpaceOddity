using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class UFO : MonoBehaviour, IObstacle, IPlayerCollision, IDeletable
{
    private IObjectPool<GameObject> _pool;
    [SerializeField]public bool isReleased;
    private bool isAlive = false;
    private double beginningAngle;
    private float bParam;
    private float aParam;
    private double currentAngle;
    public GameObject planet;

    private bool isMoving = true;

    private void Start()
    {
        Events.onGameOver += StopMoving;
        Events.onGameRestart += StartMoving;
        Events.onRespawn += StartMoving;
        Events.onGamePaused += StopMoving;
        Events.onGameUnPaused += StartMoving;
    }

    private void OnDestroy()
    {
        Events.onGameOver -= StopMoving;
        Events.onGameRestart -= StartMoving;
        Events.onRespawn -= StartMoving;
        Events.onGamePaused -= StopMoving;
        Events.onGameUnPaused -= StartMoving;
    }

    public void SetPool(IObjectPool<GameObject> pool)=>_pool= pool;
    private void Update()
    {
        if (isMoving) Move();
    }

    public void Move()
    {
        currentAngle = currentAngle >= (float)beginningAngle + 360f ? currentAngle - 360f : currentAngle + 20f * Time.deltaTime;
        transform.position = new Vector3(planet.transform.position.x + bParam * (float)Math.Cos((2 * Math.PI / 360) * currentAngle),
            planet.transform.position.y + aParam * (float)Math.Sin((2 * Math.PI / 360) * currentAngle));
    }
    
    public void GetDeleted()
    {
        isAlive = false;
        _pool.Release(gameObject);
        ObstaclesGenerator.Instance.activeObstacleList.Remove(gameObject);
    }
    
    public GameObject GetGameObject(){return this.gameObject;}
    public void IsReleased(bool state) => isReleased = state;
    public void SetPos(Planet planet)
    {
        //transform.position = new Vector3(planet.gameObject.transform.position.x,planet.gameObject.transform.position.y+planet.planet.orbitDiameter/2f+2f);
        bParam = Random.Range(planet.planet.orbitDiameter + 6f, planet.planet.orbitDiameter + 10f);
        aParam = Random.Range(planet.planet.orbitDiameter + 12f, planet.planet.orbitDiameter + 14f);
        beginningAngle = Vector3.Angle(Vector3.right, transform.position);
        currentAngle = beginningAngle;
        this.planet = planet.gameObject;
    }

    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        JSONTest.UserLocalDataObject.LocalUserStatistics.total_obstacle++;
        Events.onObstacleCollision?.Invoke(this);
        if (BonusManager.instance.BonusIsActive(Bonuses.type.shield))
        {
            Events.onBonusActivated(Bonuses.type.shield);
            GetDeleted();
        }
        else
        {
            Events.onDeathEvent?.Invoke();
            GetDeleted();
        }
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }

    #endregion

    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
