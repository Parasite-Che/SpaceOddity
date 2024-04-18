using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    [SerializeField] private ObstaclePool _pool;
    public List<GameObject> activeObstacleList;
    public int obstacleSpawnDelayCounter;

    public static ObstaclesGenerator Instance { get; private set; }

    public void Initialize()
    {
        Instance= this;
        activeObstacleList = new List<GameObject>();
        Events.onPlanetSpawned += GenerateObstacle;
        _pool.Initialize();
    }

    void GenerateObstacle(Planet planet)
    {
        if (obstacleSpawnDelayCounter > 0)
        {
            obstacleSpawnDelayCounter--;
            return;
        }
        if (UnityEngine.Random.Range(1, 100) > Objects.instance.baseNumber * Objects.instance.difficultyMultiplier)
        {
            GameObject obstacle = _pool.pool.Get();
            obstacle.GetComponent<IObstacle>().SetPos(planet);
            activeObstacleList.Add(obstacle);
        }
    }

    public void ResetObstaclesOnRestart()
    {
        obstacleSpawnDelayCounter = 2;
        foreach (GameObject obstacle in activeObstacleList)
        {
            if (obstacle.activeSelf) _pool.pool.Release(obstacle);
        }
        activeObstacleList.RemoveRange(0, activeObstacleList.Count);
    }
}
