using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexBanner : MonoBehaviour
{
    public static YandexBanner instance;

    private String message = "";
    private Banner banner;
    private string bannerID = "R-M-3586691-2";


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bannerID = "R-M-3586691-2";
        Events.onGameStart += DestroyBanner;
        Events.onGameOver += RequestBanner;
    }

    public void RequestBanner()
    {
        //meshPro.text = "Requesting Banner";
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(false);

        // Replace demo Unit ID 'demo-banner-yandex' with actual Ad Unit ID
        string adUnitId = bannerID;

        if (this.banner != null)
        {
            this.banner.Destroy();
        }
        // Set sticky banner width
        BannerAdSize bannerSize = BannerAdSize.StickySize(GetScreenWidthDp());
        // Or set inline banner maximum width and height
        // BannerAdSize bannerSize = BannerAdSize.InlineSize(GetScreenWidthDp(), 300);
        this.banner = new Banner(adUnitId, bannerSize, AdPosition.BottomCenter);

        this.banner.OnAdLoaded += this.HandleBannerAdLoaded;
        this.banner.OnAdFailedToLoad += this.HandleBannerAdFailedToLoad;
        this.banner.OnReturnedToApplication += this.HandleBannerReturnedToApplication;
        this.banner.OnLeftApplication += this.HandleBannerLeftApplication;
        this.banner.OnAdClicked += this.HandleBannerAdClicked;
        this.banner.OnImpression += this.HandleBannerImpression;

        this.banner.LoadAd(this.CreateBannerAdRequest());
        this.DisplayMessageBanner("Banner is requested");
    }

    public void DestroyBanner()
    {
        //meshPro.text = "Destroying Banner";
        if (this.banner != null)
        {
            this.banner.Destroy();
        }
    }


    // Example how to get screen width for request
    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private AdRequest CreateBannerAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void DisplayMessageBanner(String message)
    {
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);
    }


    public void HandleBannerAdLoaded(object sender, EventArgs args)
    {
        this.DisplayMessageBanner("HandleAdLoaded event received");
        this.banner.Show();
        //meshPro.text = "Banner Loaded";
    }

    public void HandleBannerAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessageBanner("HandleAdFailedToLoad event received with message: " + args.Message);
        //meshPro.text = "HandleAdFailedToLoad event received with message: " + args.Message;
    }

    public void HandleBannerLeftApplication(object sender, EventArgs args)
    {
        this.DisplayMessageBanner("HandleLeftApplication event received");
    }

    public void HandleBannerReturnedToApplication(object sender, EventArgs args)
    {
        this.DisplayMessageBanner("HandleReturnedToApplication event received");
    }

    public void HandleBannerAdLeftApplication(object sender, EventArgs args)
    {
        this.DisplayMessageBanner("HandleAdLeftApplication event received");
    }

    public void HandleBannerAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessageBanner("HandleAdClicked event received");
    }

    public void HandleBannerImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessageBanner("HandleImpression event received with data: " + data);
        //meshPro.text += "HandleImpression event received with data: " + data;
    }
}
