using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexAdsScript : MonoBehaviour
{
    public static YandexAdsScript instance;

    private String message = "";
    private Banner banner;
    private string bannerID = "R-M-3586691-2";

    private RewardedAdLoader rewardedCoinAdLoader;
    private RewardedAd rewardedCoinAd;
    private string rewardedCoinID = "R-M-3586691-1";

    private RewardedAdLoader rewardedRespawnAdLoader;
    private RewardedAd rewardedRespawnAd;
    private string rewardedRespawnID = "R-M-3586691-4";


    public TMPro.TMP_Text meshPro;

    

    // Update is called once per frame
    void Update()
    {
        
    }



    #region RewardedAd
    public void RequestRewardedAd()
    {
        //meshPro.text = "Requesting Rewarded";
        this.DisplayRewardedMessage("RewardedAd is not ready yet");
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        if (this.rewardedCoinAd != null)
        {
            this.rewardedCoinAd.Destroy();
        }

        // Replace demo Unit ID 'demo-rewarded-yandex' with actual Ad Unit ID
        string adUnitId = rewardedCoinID;

        this.rewardedCoinAdLoader.LoadAd(this.CreateRewardedAdRequest(adUnitId));
        this.DisplayRewardedMessage("Rewarded Ad is requested");
    }

    public void ShowRewardedAd()
    {
        //meshPro.text = "Showing Rewarded";
        if (this.rewardedCoinAd == null)
        {
            this.DisplayRewardedMessage("RewardedAd is not ready yet");
            return;
        }

        this.rewardedCoinAd.OnAdClicked += this.HandleRewardedAdClicked;
        this.rewardedCoinAd.OnAdShown += this.HandleRewardedAdShown;
        this.rewardedCoinAd.OnAdFailedToShow += this.HandleRewardedAdFailedToShow;
        this.rewardedCoinAd.OnAdImpression += this.HandleRewardedImpression;
        this.rewardedCoinAd.OnAdDismissed += this.HandleRewardedAdDismissed;
        this.rewardedCoinAd.OnRewarded += this.HandleRewarded;

        this.rewardedCoinAd.Show();
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

    #region Rewarded Ad callback handlers

    public void HandleRewardedAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        this.DisplayRewardedMessage("HandleAdLoaded event received");
        this.rewardedCoinAd = args.RewardedAd;
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


        this.rewardedCoinAd.Destroy();
        this.rewardedCoinAd = null;
    }

    public void HandleRewardedImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayRewardedMessage($"HandleImpression event received with data: {data}");
    }

    public void HandleRewarded(object sender, Reward args)
    {
        this.DisplayRewardedMessage($"HandleRewarded event received: amout = {args.amount}, type = {args.type}");
        if (args.type == "coins") GameManager.Instance.AddCoins(50);
        if (args.type == "respawn") GameManager.Instance.Respawn();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayRewardedMessage(
            $"HandleAdFailedToShow event received with message: {args.Message}");
    }

    #endregion

    #endregion
}
