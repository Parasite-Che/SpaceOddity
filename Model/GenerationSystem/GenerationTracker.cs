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
        // ���� ����������� ��������� ��� ������
        yDistanceBetweenPlanets = (PlanetManager.instance.ySpawnRange.x + PlanetManager.instance.ySpawnRange.y) / 2;
        //lastSpawnedPlanetYCoordinate = PlanetManager.instance.lastPlanet.transform.position.y;

        // ������� ������� �� ������
        if (transform.position.y - nextSpawnY > yDistanceBetweenPlanets)
        {
            Events.onTimeToSpawnNewPlanet?.Invoke();
        }
    }

    void Update()
    {
        // ��������� ������ ������ � �������
        transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + yOffsetFromPlayer);

        // ������ ����� �������� �������
        if (transform.position.y - nextSpawnY > yDistanceBetweenPlanets)
        {
            Events.onTimeToSpawnNewPlanet?.Invoke();
        }
    }

    private void UpdateDataOnPlanetSpawn(Planet planet)
    {
        nextSpawnY = PlanetManager.instance.nextSpawnPosition.y;
        // ��������� ���������� ��������� ������������ �������
        //lastSpawnedPlanetYCoordinate = planet.transform.position.y;

        // �������� �������� ��������� ��� �����, ����� ������� ����� �������� ����� �������
        yDistanceBetweenPlanets = (PlanetManager.instance.ySpawnRange.x + PlanetManager.instance.ySpawnRange.y) / 2;
    }
}
