using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
    [SerializeField] int rewardAmount;
    [SerializeField] Button rewardButton;
    [SerializeField] TMP_Text rewardAmountText;
    [SerializeField] GameObject rewardNotification;
    DateTime lastTimeRewardClaimed;

    void Start()
    {
        CheckRewardAvailability();
        Events.onDeathEvent += CheckRewardAvailability;
    }

    public void CheckRewardAvailability()
    {
        string tmpDateString = PlayerPrefs.GetString("lastTimeClaimed", "");
        if (string.IsNullOrEmpty(tmpDateString))
        {
            PlayerPrefs.SetString("lastTimeClaimed", DateTime.Today.ToString());
            ActivateRewardButton();
        }
        else
        {
            lastTimeRewardClaimed = DateTime.Parse(tmpDateString);
            if (lastTimeRewardClaimed.Date != DateTime.Today)
            {
                ActivateRewardButton();
            }
            else
            {
                DeactivateRewardButton();
            }
        }
    }

    public void ActivateRewardButton()
    {
        rewardButton.interactable = true;
        rewardAmountText.text = rewardAmount.ToString();
        rewardNotification.SetActive(true);
    }

    public void DeactivateRewardButton()
    {
        rewardButton.interactable = false;
        rewardAmountText.text = 0.ToString();
        rewardNotification.SetActive(false);
    }

    public void ClaimReward()
    {
        PlayerPrefs.SetString("lastTimeClaimed", DateTime.Today.ToString());
        DeactivateRewardButton();
        GameManager.Instance.AddCoins(rewardAmount);
    }
}
