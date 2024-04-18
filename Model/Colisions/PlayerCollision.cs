using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision) //it describes what happend in case of entiring to new orbit
    {
        IPlayerCollision IPC;

        if (!collision.TryGetComponent(out IPC) && collision.transform.parent != null)
        {
            collision.transform.parent.TryGetComponent(out IPC);
        }

        IPC?.ActOnTriggerEnter(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IPlayerCollision IPC;

        if (!collision.TryGetComponent(out IPC) && collision.transform.parent != null)
        {
            collision.transform.parent.TryGetComponent(out IPC);
        }

        IPC?.ActOnTriggerExit(gameObject);
    }

}

interface IPlayerCollision
{
    public void ActOnTriggerEnter(GameObject player);

    public void ActOnTriggerExit(GameObject player);
}
