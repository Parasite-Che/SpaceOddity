using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour, IPlayerCollision
{
    public float scrollSpeed;
    public float tileSize;
    private Transform currentObject;

    public Transform leftSide;
    public Transform rightSide;

    void Start()
    {
        currentObject = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        currentObject.position = new Vector3(
            currentObject.position.x,
            currentObject.position.y,
            Mathf.Repeat(Time.time * scrollSpeed, tileSize)
            );
    }

    public void ActOnTriggerEnter(GameObject player)
    {
        ;
    }

    public void ActOnTriggerExit(GameObject player)
    {
        Debug.Log("Вышли за границу камеры. Ивент на смерть игрока");
        Events.onDeathEvent?.Invoke();
    }
}
