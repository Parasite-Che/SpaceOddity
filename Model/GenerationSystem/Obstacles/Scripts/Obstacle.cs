using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour, IPlayerCollision, IDeletable
{
    private IObjectPool<Obstacle> _pool;
    public float currentRadius;
    public Transform target;
    public string type;
    [SerializeField]private Action movementMethod;
    [SerializeField]private float _bParam;
    [SerializeField]private float _aParam;
    [SerializeField]private double _beginningAngle;
    [SerializeField]private float _oneCircle;
    [SerializeField]private float _aroundAngle;
    [SerializeField]private float _selfAngle;

    public bool isReleased;
    

    public void SetPool(IObjectPool<Obstacle> pool)=>_pool= pool;

    public void SetType()
    {
        if (type == "Ufo")
            movementMethod = UfoMovement;
        else if(type == "Satelite")
            movementMethod = SateliteMovement;
        else if (type == "Asteroid")
            movementMethod = AsteroidMovement;
        else if (type == "Comet")
            movementMethod = CometMovement;
        else
            movementMethod = BlackholeMovement;
    }

    public void SetPos(Planet planet)
    {
        target = planet.gameObject.transform;
        currentRadius = planet.planet.orbitDiameter/2+4f;
        
        double radians = Random.Range(0f, (float)(2 * Math.PI));
        Vector3 position = new Vector3(target.position.x + currentRadius * (float)Math.Cos(radians),target.position.y + currentRadius * (float)Math.Sin(radians));
        gameObject.transform.position = position;
        
        _beginningAngle = Vector3.Angle(Vector3.right, transform.position);
        _oneCircle = (float)_beginningAngle + 360f;
        _bParam = Random.Range(currentRadius+ 5f, currentRadius+ 8f);
        _aParam = Random.Range(currentRadius+ 10f, currentRadius+ 12f);
        _aroundAngle = Random.Range(3f, 6f);
        _selfAngle = Random.Range(0.3f, 0.6f);
    }

    private void Update()
    {
        movementMethod();
    }

    public void GetDeleted()
    {
        _pool.Release(this);
        //ObstaclesGenerator.Instance.activeObstacleList.Remove(this);
    }

    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        Events.onDeathEvent?.Invoke();
        JSONTest.UserLocalDataObject.LocalUserStatistics.total_obstacle++;
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }
    #endregion

    void UfoMovement()
    {
        Vector3 newPos = new Vector3(target.position.x+_bParam * (float)Math.Cos((2*Math.PI/360)*_beginningAngle), target.position.y+_aParam * (float)Math.Sin((2*Math.PI/360)*_beginningAngle));
        transform.position = newPos;
        _beginningAngle = _beginningAngle >= _oneCircle? _beginningAngle - 360f: _beginningAngle + 4f * Time.deltaTime;
    }

    void SateliteMovement()
    {
        Vector3 newPos = new Vector3(target.position.x+_bParam * (float)Math.Cos((2*Math.PI/360)*_beginningAngle), target.position.y+_aParam * (float)Math.Sin((2*Math.PI/360)*_beginningAngle));
        transform.position = newPos;
        _beginningAngle = _beginningAngle >= _oneCircle? _beginningAngle - 360f: _beginningAngle + 1f * Time.deltaTime;
    }

    void AsteroidMovement()
    {
        gameObject.transform.Rotate(Vector3.forward,_selfAngle);
        gameObject.transform.RotateAround(target.position,Vector3.forward, _aroundAngle*Time.deltaTime);
    }

    void CometMovement()
    {
        
    }

    void BlackholeMovement()
    {
        
    }
}
