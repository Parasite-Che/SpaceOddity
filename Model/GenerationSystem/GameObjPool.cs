using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOblPool : MonoBehaviour
{
    List<GameObject> _noneActiveObjects;
    List<GameObject> _activeObjects;
    Vector3 _position=new Vector3(0,0,0);
    Quaternion _rotation=new Quaternion(0,0,0,0);

    GameOblPool(int qauntityOfObj, GameObject obj)
    {
        Create(qauntityOfObj,obj);
    }

    //to fill the pool
    public void Create(int quantity, GameObject obj)
    {
        for(int i=0;i<quantity;i++)
        {
            _noneActiveObjects.Add(Instantiate(obj,_position,_rotation));
            _noneActiveObjects[i].SetActive(false);
        }
    }

    //to clear all objects on scene and in containers
    public void Clear() 
    {
        foreach(GameObject obj in _noneActiveObjects)
        {
            _noneActiveObjects.Remove(obj);
            Destroy(obj);
        }
        foreach (GameObject obj in _activeObjects)
        {
            _activeObjects.Remove(obj);
            Destroy(obj);
        }
    }

    //to make object inactive 
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        _activeObjects.Remove(obj);
        _noneActiveObjects.Add(obj);
    }

    //to get object from pool
    public GameObject Get() 
    {
        try
        {
            GameObject obj = _noneActiveObjects[0];
            obj.SetActive(true);
            _activeObjects.Add(obj);
            return obj;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }

    }
}