using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleDatabase : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefab;

    public void Initialize()
    {
        obstaclesPrefab = Resources.LoadAll<GameObject>("ObstaclesPrefabs");
    }

    public GameObject GetRandomObstacle()
    {
        int ind = Random.Range(0, obstaclesPrefab.Length);
        return obstaclesPrefab[ind];
    }
}
