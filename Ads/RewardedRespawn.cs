using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class RewardedRespawn : MonoBehaviour
{
    public static RewardedRespawn instance;

    private String message = "";

    private RewardedAdLoader rewardedRespawnAdLoader;
    private RewardedAd rewardedRespawnAd;
    private string rewardedRespawnID = "R-M-3586691-4";

    private void Awake()
    {
        if (instance == null) instance = this;

        this.rewardedRespawnAdLoader = new RewardedAdLoader();
        this.rewardedRespawnAdLoader.OnAdLoaded += this.HandleRewardedAdLoaded;
        this.rewardedRespawnAdLoader.OnAdFailedToLoad += this.HandleRewardedAdFailedToLoad;
    }

    void Start()
    {
        rewardedRespawnID = "R-M-3586691-4";
        Events.onGameStart += RequestRewardedAd;
    }

    public void RequestRewardedAd()
    {
        //meshPro.text = "Requesting Rewarded";
        this.DisplayRewardedMessage("RewardedAd is not ready yet");
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        if (this.rewardedRespawnAd != null)
        {
            this.rewardedRespawnAd.Destroy();
        }

        // Replace demo Unit ID 'demo-rewarded-yandex' with actual Ad Unit ID
        string adUnitId = rewardedRespawnID;

        this.rewardedRespawnAdLoader.LoadAd(this.CreateRewardedAdRequest(adUnitId));
        this.DisplayRewardedMessage("Rewarded Ad is requested");
    }

    public void ShowRewardedAd()
    {
        //meshPro.text = "Showing Rewarded";
        if (this.rewardedRespawnAd == null)
        {
            this.DisplayRewardedMessage("RewardedAd is not ready yet");
            return;
        }

        this.rewardedRespawnAd.OnAdClicked += this.HandleRewardedAdClicked;
        this.rewardedRespawnAd.OnAdShown += this.HandleRewardedAdShown;
        this.rewardedRespawnAd.OnAdFailedToShow += this.HandleRewardedAdFailedToShow;
        this.rewardedRespawnAd.OnAdImpression += this.HandleRewardedImpression;
        this.rewardedRespawnAd.OnAdDismissed += this.HandleRewardedAdDismissed;
        this.rewardedRespawnAd.OnRewarded += this.HandleRewarded;

        this.rewardedRespawnAd.Show();
    }

    private AdRequestConfiguration CreateRewardedAdRequest(string adUnitId)
    {
        return new AdRequestConfiguration.Builder(adUnitId).Build();
    }

    private void DisplayRewardedMessage(String message)
    {
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);
    }


    // CALLBACKS
    public void HandleRewardedAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        this.DisplayRewardedMessage("HandleAdLoaded event received");
        this.rewardedRespawnAd = args.RewardedAd;
        //meshPro.text = "Loaded Rewarded";
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.DisplayRewardedMessage(
            $"HandleAdFailedToLoad event received with message: {args.Message}");
        //meshPro.text = $"HandleAdFailedToLoad event received with message: {args.Message}";
    }

    public void HandleRewardedAdClicked(object sender, EventArgs args)
    {
        this.DisplayRewardedMessage("HandleAdClicked event received");
    }

    public void HandleRewardedAdShown(object sender, EventArgs args)
    {
        this.DisplayRewardedMessage("HandleAdShown event received");
    }

    public void HandleRewardedAdDismissed(object sender, EventArgs args)
    {
        this.DisplayRewardedMessage("HandleAdDismissed event received");


        this.rewardedRespawnAd.Destroy();
        this.rewardedRespawnAd = null;
    }

    public void HandleRewardedImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayRewardedMessage($"HandleImpression event received with data: {data}");
    }

    public void HandleRewarded(object sender, Reward args)
    {
        this.DisplayRewardedMessage($"HandleRewarded event received: amout = {args.amount}, type = {args.type}");
        //GameManager.Instance.Respawn();
        GameManager.Instance.Respawn();
    }


    public void HandleRewardedAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayRewardedMessage(
            $"HandleAdFailedToShow event received with message: {args.Message}");
    }
}
