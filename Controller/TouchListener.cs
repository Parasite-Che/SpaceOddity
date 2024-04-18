using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TouchListener : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private bool isListeningToClicks = true;

    private void Awake()
    {
        clip = Resources.Load<AudioClip>("MusicAndSounds/ShipSounds/Spaceship final");
    }

    private void Start()
    {
        Events.onGameStart += StartListeningToClicks;
        Events.onGameOver += StopListeningToClicks;
        Events.onRespawn += StartListeningToClicks;
    }

    private void OnMouseDown()
    {
        if (!isListeningToClicks) return;

        if (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] == false)
        {
            JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] = true;
        }

        if (Main.ShipStatesLogic.ShipState.IsAroundPlanet())
        {
            Debug.Log("Отцепился");
            Events.onPlaySoundEffect(clip);
            Main.ShipStatesLogic.ChangeStates(); // Изменение состояния коробля на движение между планетами
        }
        else
        {
            Debug.Log("Не отцепился");
        }
    }

    private void StopListeningToClicks()
    {
        isListeningToClicks = false;
    }

    private void StartListeningToClicks()
    {
        isListeningToClicks = true;
    }
}
