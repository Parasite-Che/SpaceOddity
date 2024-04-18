using System;
using UnityEngine;

public class CollisionSoundEffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private string path;

    private void Awake()
    {
        clip = Resources.Load<AudioClip>(path);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MainMovingObject(player)")
            Events.onPlaySoundEffect(clip);
    }
}
