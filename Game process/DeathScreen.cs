using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance { get; private set; }

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject highscoreTable;

    private void Awake()
    {
        if (Instance) Debug.LogError("Больше одного экземпляра DeathScreen");
        Instance = this;
    }
    public void Activate(bool state)
    {
        deathScreen.SetActive(state);
    }

    public void OpenHighscoreTable()
    {
        highscoreTable.SetActive(true);
    }
    public void CloseHighscoreTable()
    {
        highscoreTable.SetActive(false);
    }
}
