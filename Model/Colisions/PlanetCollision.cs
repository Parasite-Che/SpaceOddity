using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour, IPlayerCollision, IDeletable
{
    private Planet _planet;

    private Coroutine _cor;

    private void Awake()
    {
        if (!transform.parent.TryGetComponent(out _planet)) Debug.LogError("PlanetCollision не нашёл компонент Planet");
    }

    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        Events.onDeathEvent?.Invoke();
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }
    #endregion

    public void GetDeleted()
    {
        Debug.Log($"Despawning planet {_planet.planet.Name}");
        PlanetManager.instance.DespawnPlanet(_planet);
        Events.onPlanetDeleted?.Invoke();
    }
}
