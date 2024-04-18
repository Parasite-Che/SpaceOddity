using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameObjectRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    private int numOfObject;

    private void OnEnable()
    {
        numOfObject = Random.Range(0, objects.Length);
        objects[numOfObject].SetActive(true);
    }

    private void OnDisable()
    {
        objects[numOfObject].SetActive(false);
    }
}
