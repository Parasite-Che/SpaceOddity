using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;

    public static ScoreCounter instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Events.onOrbitEntered += UpdateScore;
    }

    public void UpdateScore(Planet planet)
    {
        score += Mathf.RoundToInt(planet.planet.scoreReward * BonusManager.instance.rewardMultiplier);
        if (JSONTest.UserLocalDataObject != null)
        {
            JSONTest.UserLocalDataObject.LocalUserInfo.scores +=
                Mathf.RoundToInt(planet.planet.scoreReward * BonusManager.instance.rewardMultiplier);
            //new SceneTextController().TextListUpdater(Objects.instance.coinsText,
            //JSONTest.UserLocalDataObject.LocalUserInfo.money.ToString());
        }
        SetScore(score);
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
