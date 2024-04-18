using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlanetPool : MonoBehaviour
{
    public ObjectPool<Planet> pool;
    public int defaultCapacity;
    public int maxCapacity;
    public int activeObjectsInPool;
    public int inactiveObjectsInPool;

    [SerializeField] private Transform planetsParent;

    public void Initialize()
    {
        pool = new ObjectPool<Planet>(CreatePlanet, GetPlanetFromPool, ReturnPlanetToPool, DestroyPlanet, true, defaultCapacity, maxCapacity);
    }

    private void Update()
    {
        activeObjectsInPool = pool.CountActive;
        inactiveObjectsInPool = pool.CountInactive;
    }

    #region PlanetPool
    public Planet CreatePlanet()
    {
        Planet planet = Instantiate(Objects.instance.planetPrefab, 
            PlanetManager.instance.nextSpawnPosition, Quaternion.identity, planetsParent).GetComponent<Planet>();
        planet.SetPool(pool);
        planet.isReleased = false;
        return planet;
    }

    public void GetPlanetFromPool(Planet planet)
    {
        planet.transform.position = PlanetManager.instance.nextSpawnPosition;
        planet.transform.rotation = Quaternion.identity;

        planet.isReleased = false;
        planet.gameObject.SetActive(true);
    }

    public void ReturnPlanetToPool(Planet planet)
    {
        planet.isReleased = true;
        planet.gameObject.SetActive(false);
    }

    public void DestroyPlanet(Planet planet)
    {
        planet.isReleased = true;
        Destroy(planet.gameObject);
    }
    #endregion

}
