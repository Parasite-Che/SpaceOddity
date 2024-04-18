using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager instance;

    public ScrollBackground background;
    public Camera camera;

    public Queue<Planet> activePlanetsQueue;
    public int activePlanetCount;
    public int planetSpawnDelayCounter;

    public Planet currentPlanet;
    public Planet previousPlanet;
    public Planet lastPlanet;
    public Planet nullPlanet;
    public Planet firstPlanet
    {
        get
        {
            return activePlanetsQueue.Peek();
        }

        private set { }
    }

    [SerializeField] private PlanetPool planetPool;
    [SerializeField] private GenerationTracker generationTracker;

    // пока что задаётся в редакторе и не изменяется во время игры, будет скейлиться в зависимости от сложности
    public Vector2 xSpawnRange;
    public Vector2 ySpawnRange;

    public Vector2 nextSpawnPosition;

    private void Update()
    {
        activePlanetCount = activePlanetsQueue.Count;
    }


    public void Initialize()
    {
        instance = this;
        currentPlanet = nullPlanet;

        Events.onTimeToSpawnNewPlanet += SpawnPlanet;
        Events.onPlanetSpawned += UpdateNextSpawnPosition;
        Events.onOrbitEntered += UpdateCurrentPlanetOnOrbitEntered;
        Events.onOrbitLeft += UpdateCurrentPlanetOnOrbitLeft;
        Events.onPlanetSpawned += UpdateLastPlanetSpawned;
        
        planetPool.Initialize();
        activePlanetsQueue = new Queue<Planet>();
        UpdateNextSpawnPosition(null);
        SpawnPlanet();

        currentPlanet = firstPlanet;

        generationTracker.Initialize();
    }

    public void SpawnPlanet()
    {
        Objects.instance.quantityOfPlanets += 1;
        // получаем случайный тип планеты
        PlanetScriptableObject type = PlanetDatabase.instance.GetRandomPlanet();
        nextSpawnPosition.x = Mathf.Clamp(nextSpawnPosition.x, background.leftSide.position.x + type.orbitDiameter / 1.6f, background.rightSide.position.x - type.orbitDiameter / 1.6f);

        if (lastPlanet != null)
        {
            nextSpawnPosition.y = Mathf.Clamp
                (nextSpawnPosition.y, lastPlanet.transform.position.y, 
                lastPlanet.transform.position.y + camera.ScreenToWorldPoint
                (new Vector3(0, Screen.height, 0)).y + type.orbitDiameter / 1.95f);
        }
        // спавним неинициализированную планету
        Planet planet = planetPool.pool.Get();
        // инициализируем полученным ранее типом
        planet.InitializePlanet(type);
        activePlanetsQueue.Enqueue(planet);//записываем планету в очередь 
        // говорим всем подписчикам, что заспавнилась новая планета
        Events.onPlanetSpawned?.Invoke(planet);
    }

    public void DespawnPlanet(Planet planet)
    {
        if (planet.isReleased) return;
        activePlanetsQueue.Dequeue();
        planetPool.pool.Release(planet);
    }

    public void ResetPlanetsOnRestart()
    {
        while (activePlanetsQueue.Count != 0)
        {
            Planet planet = activePlanetsQueue.Peek();
            DespawnPlanet(planet);
        }

        ClearPlanets();
        UpdateNextSpawnPosition(null);

        SpawnPlanet();
        PlayerController.Instance.transform.position =
            lastPlanet.transform.position + new Vector3(lastPlanet.planet.orbitDiameter / 2 - 1, 0);
    }


    // получить случайную позицию в заданном диапазоне для спавна
    private Vector2 GetRandomPosition()
    {
        float yPosition = UnityEngine.Random.Range(ySpawnRange.x + 35 * Objects.instance.difficultyMultiplier,
                                                   ySpawnRange.y + 55 * Objects.instance.difficultyMultiplier);
        float xPosition = UnityEngine.Random.Range(-Utils.GetScreenSizeInUnits().x / 2, Utils.GetScreenSizeInUnits().x / 2);
        Vector2 spawnPosition = new Vector2(xPosition, yPosition);
        return spawnPosition;
    }

    public Vector2 GetXBordersFromSpawnedPlanets()
    {
        Planet[] planetQueueCopy = activePlanetsQueue.ToArray();
        float minX = planetQueueCopy[0].transform.position.x;
        float maxX = planetQueueCopy[0].transform.position.x;
        float posX;

        for (int i = 0; i < planetQueueCopy.Length; i++)
        {
            posX = planetQueueCopy[i].transform.position.x;
            if (posX < minX) minX = posX;
            if (posX > maxX) maxX = posX;
        }

        return new Vector2(minX, maxX);
    }

    public Vector2 GetYBordersFromSpawnedPlabets()
    {
        Planet[] planetQueueCopy = activePlanetsQueue.ToArray();
        float minY = planetQueueCopy[0].transform.position.y;
        float maxY = planetQueueCopy[0].transform.position.y;
        float posY;

        for (int i = 0; i < planetQueueCopy.Length; i++)
        {
            posY = planetQueueCopy[i].transform.position.y;
            if (posY < minY) minY = posY;
            if (posY > maxY) maxY = posY;
        }

        return new Vector2(minY, maxY);
    }

    //подумать про событие для рестарта
    //public void ClearQuantity()
    //{
    //    Objects.instance.QuantityOfPlanets= 0;
    //}

    private void UpdateNextSpawnPosition(Planet planet)
    {
        if (planet != null) nextSpawnPosition = (Vector2)planet.transform.position + new Vector2(0, planet.planet.orbitDiameter / 4)  + GetRandomPosition();
        else nextSpawnPosition = Vector2.zero;
    }

    private void UpdateCurrentPlanetOnOrbitEntered(Planet planet)
    {
        currentPlanet = planet;
    }

    private void UpdateCurrentPlanetOnOrbitLeft(Planet planet)
    {
        previousPlanet = planet;
        currentPlanet = nullPlanet;
    }

    private void UpdateLastPlanetSpawned(Planet planet)
    {
        lastPlanet = planet;
    }

    private void ClearPlanets()
    {
        lastPlanet = null;
        currentPlanet = null;
        previousPlanet = null;
    }
}
