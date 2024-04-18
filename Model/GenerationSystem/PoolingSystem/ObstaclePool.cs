using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstaclePool : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    public ObstacleDatabase _obstacleDatabase;
    public int defaultCapacity=10;
    public int maxCapacity=30;
    public int activeObjectsInPool;
    public int inactiveObjectsInPool;

    [SerializeField] private Transform obstacleParent;

    public void Initialize()
    {
        _obstacleDatabase.Initialize();
        pool = new ObjectPool<GameObject>(CreateObstacle,GetObstacleFromPool, ReturnObstacle, DestroyObstacle,true,defaultCapacity,maxCapacity);
    }

    private void Update()
    {
        activeObjectsInPool = pool.CountActive;
        inactiveObjectsInPool = pool.CountInactive;
    }
    
    private GameObject CreateObstacle()
    {
        GameObject obstacle = Instantiate(_obstacleDatabase.GetRandomObstacle(), new Vector3(0, 0, -100), Quaternion.identity, obstacleParent);
        obstacle.GetComponent<IObstacle>().SetPool(pool);
        obstacle.GetComponent<IObstacle>().IsReleased(false);
        return obstacle;
    }

    private void GetObstacleFromPool(GameObject obstacle)
    {
        obstacle.SetActive(true);
        obstacle.GetComponent<IObstacle>().IsReleased(false);
    }

    private void ReturnObstacle(GameObject obstacle)
    {
        Debug.Log("Obstacle back to pool");
        obstacle.GetComponent<IObstacle>().IsReleased( true);
        obstacle.SetActive(false);
    }

    private void DestroyObstacle(GameObject obstacle)
    {
        obstacle.GetComponent<IObstacle>().IsReleased( true);
        Destroy(obstacle);
    }
}
