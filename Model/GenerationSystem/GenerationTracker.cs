using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTracker : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float yOffsetFromPlayer;
    [SerializeField] private float yDistanceBetweenPlanets;
    private float nextSpawnY;

    public void Initialize()
    {
        Events.onPlanetSpawned += UpdateDataOnPlanetSpawn;

        nextSpawnY = PlanetManager.instance.nextSpawnPosition.y;

        transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + yOffsetFromPlayer);
        // берём изначальную дистанцию для спавна
        yDistanceBetweenPlanets = (PlanetManager.instance.ySpawnRange.x + PlanetManager.instance.ySpawnRange.y) / 2;
        //lastSpawnedPlanetYCoordinate = PlanetManager.instance.lastPlanet.transform.position.y;

        // спавним планеты на старте
        if (transform.position.y - nextSpawnY > yDistanceBetweenPlanets)
        {
            Events.onTimeToSpawnNewPlanet?.Invoke();
        }
    }

    void Update()
    {
        // двигаемся вперед вместе с игроком
        transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + yOffsetFromPlayer);

        // пришло время спавнить планету
        if (transform.position.y - nextSpawnY > yDistanceBetweenPlanets)
        {
            Events.onTimeToSpawnNewPlanet?.Invoke();
        }
    }

    private void UpdateDataOnPlanetSpawn(Planet planet)
    {
        nextSpawnY = PlanetManager.instance.nextSpawnPosition.y;
        // обновляем координату последней заспавненной планеты
        //lastSpawnedPlanetYCoordinate = planet.transform.position.y;

        // выбираем середину диапазона как точку, после которой нужно спавнить новую планету
        yDistanceBetweenPlanets = (PlanetManager.instance.ySpawnRange.x + PlanetManager.instance.ySpawnRange.y) / 2;
    }
}
