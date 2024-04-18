using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionCollider : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _yOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDeletable deletableObject;
        if (collision.TryGetComponent(out deletableObject))
        {
            deletableObject.GetDeleted();
        }
    }

    private void Update()
    {
        transform.position = new Vector2(0, _player.transform.position.y - _yOffset);
    }
}

public interface IDeletable
{
    public void GetDeleted();
}