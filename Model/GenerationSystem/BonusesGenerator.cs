using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusesGenerator : MonoBehaviour
{
    public static BonusesGenerator Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
    }
    public static void Generate(Planet planet)
    {

    }
}
