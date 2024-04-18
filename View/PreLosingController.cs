using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreLosingController : MonoBehaviour
{
    [SerializeField] private PopUpController MissTheBoatButton;
    private int timer;
    public TMP_Text text;
    [SerializeField] private Image circle;
    public static PreLosingController instance;
    
    private void Awake()
    {
        instance = this;
        Events.onGameOver += StartCountdown;
    }
    
    public IEnumerator TimerController()
    {
        timer = 60;
        text.text = timer.ToString();
        do
        {
            Debug.Log(timer);
            circle.fillAmount = (float)timer / 60;

            yield return new WaitForSeconds(1);
            timer--;
            Debug.Log(timer);
            text.text = timer.ToString();
        } while (timer != 0);
        MissTheBoatButton.StaticPopUp();
    }

    private void StartCountdown()
    {
        StartCoroutine(TimerController());
    }

    public void StopTimer()
    {
        StopCoroutine(TimerController());
    }
    
}
