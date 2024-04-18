using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public interface IObstacle 
{
    
    public void SetPos(Planet planet)
    {
    }

    public void Move()
    {
        
    }
    
    public void SetPool(IObjectPool<GameObject> pool)
    {}

    public GameObject GetGameObject()
    {
        return null;
    }

    public void StopMoving();
    public void StartMoving();

    public void IsReleased(bool state)
    {
    }
}
