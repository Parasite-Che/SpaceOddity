using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IObstacle, IPlayerCollision, IDeletable
{
    private IObjectPool<GameObject> _pool;
    [SerializeField]public bool isReleased;
    private bool isAlive=false;
    private float _aroundAngle;
    private float _selfAngle;
    private Transform _target;
    private double _radians;
    private float _currentRadius;
    [SerializeField]private Vector3 position;
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

    private void FixedUpdate()
    {
        if(isAlive && isMoving)
            Move();
    }

    public void SetPool(IObjectPool<GameObject> pool)=>_pool= pool;
    
    public void GetDeleted()
    {
        isAlive = false;
        _pool.Release(gameObject);
        ObstaclesGenerator.Instance.activeObstacleList.Remove(gameObject);
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
    
    public void Move()
    {
        gameObject.transform.Rotate(Vector3.forward, _selfAngle);
        gameObject.transform.RotateAround(_target.position, Vector3.forward, _aroundAngle * Time.deltaTime);
    }

    public void SetPos(Planet planet)
    {
        _aroundAngle = Random.Range(3f, 6f);
        _selfAngle = Random.Range(0.3f, 0.6f);
        _target = planet.gameObject.transform;
        _radians = Random.Range(0f, (float)(2 * Math.PI));
        _currentRadius = planet.planet.orbitDiameter/2+6f;
        transform.position = new Vector3(_target.position.x + _currentRadius * (float)Math.Cos(_radians),_target.position.y + _currentRadius * (float)Math.Sin(_radians),_target.position.z);
        isAlive = true;
        this.planet = planet.gameObject;
    }

    public GameObject GetGameObject(){return this.gameObject;}
    public void IsReleased(bool state) => isReleased = state;

    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
