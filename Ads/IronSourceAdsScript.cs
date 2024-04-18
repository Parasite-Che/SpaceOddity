//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class IronSourceAdsScript : MonoBehaviour
//{
//    private string appKey = "1917c7a25";
//    //#if UNITY_ANDROID
//    //    appKey = "1917c7a25";
//    //#elif UNITY_IOS
//    //    appKey = "";
//    //#else
//    //    appKey = "1917c7a25";
//    //#endif

//    public static IronSourceAdsScript instance;
//    public TMPro.TMP_Text meshPro;

//    private bool isRewardAdAvailable = false;
//    private bool isInterstitialAdReady = false;
//    private bool isOfferwallReady = false;

//    private void Awake()
//    {
//        instance = this;
//    }


//    void Start()
//    {
//        IronSource.Agent.init(appKey);
//        IronSource.Agent.validateIntegration();
//        Events.onGameStart = DestroyBanner;
//    }

//    private void OnEnable()
//    {
//        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

//        //Add AdInfo Banner Events
//        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
//        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
//        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
//        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
//        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
//        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

//        //Add AdInfo Rewarded Video Events
//        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
//        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
//        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
//        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
//        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
//        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
//        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

//        //Add AdInfo Interstitial Events
//        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
//        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
//        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
//        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
//        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
//        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
//        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

//        // OfferWall
//        IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
//        IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
//        IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
//        IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
//        IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
//        IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;
//    }

//    void OnApplicationPause(bool isPaused)
//    {
//        IronSource.Agent.onApplicationPause(isPaused);
//    }

//    private void SdkInitializationCompletedEvent()
//    {
//        IronSource.Agent.validateIntegration();
//    }

//    public void LoadBanner()
//    {
//        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
//    }

//    public void DestroyBanner()
//    {
//        IronSource.Agent.destroyBanner();
//    }

//    public void ShowRewardAd()
//    {
//        if (isRewardAdAvailable) IronSource.Agent.showRewardedVideo();
//    }

//    public void LoadInterstitialAd()
//    {
//        if (!isInterstitialAdReady) IronSource.Agent.loadInterstitial();
//    }

//    public void ShowInterstitialAd()
//    {
//        IronSource.Agent.showInterstitial();
//    }

//    public void ShowOfferwall()
//    {
//        if (isOfferwallReady) IronSource.Agent.showOfferwall();
//    }

//    #region Banner Callbacks
//    /************* Banner AdInfo Delegates *************/
//    //Invoked once the banner has loaded
//    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
//    {
//        meshPro.text += $"Баннер загрузился: {adInfo}";
//    }
//    //Invoked when the banner loading process has failed.
//    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
//    {
//        meshPro.text += $"Баннер не загрузился: {ironSourceError}";
//    }
//    // Invoked when end user clicks on the banner ad
//    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
//    {

//    }
//    //Notifies the presentation of a full screen content following user click
//    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    //Notifies the presented screen has been dismissed
//    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    //Invoked when the user leaves the app
//    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    #endregion

//    #region Interstitial Callbacks
//    /************* Interstitial AdInfo Delegates *************/
//    // Invoked when the interstitial ad was loaded succesfully.
//    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
//    {
//        meshPro.text += $"Интер загрузился: {adInfo}";
//    }
//    // Invoked when the initialization process has failed.
//    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
//    {
//        meshPro.text += $"Интер не загрузился: {ironSourceError}";
//    }
//    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
//    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    // Invoked when end user clicked on the interstitial ad
//    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    // Invoked when the ad failed to show.
//    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
//    {
//    }
//    // Invoked when the interstitial ad closed and the user went back to the application screen.
//    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
//    // This callback is not supported by all networks, and we recommend using it only if  
//    // it's supported by all networks you included in your build. 
//    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    #endregion

//    #region Reward Callbacks
//    // Reward Callbacks

//    /************* RewardedVideo AdInfo Delegates *************/
//    // Indicates that there’s an available ad.
//    // The adInfo object includes information about the ad that was loaded successfully
//    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
//    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
//    {
//        isRewardAdAvailable = true;
//        meshPro.text += $"Реворд загрузился: {adInfo}";

//    }
//    // Indicates that no ads are available to be displayed
//    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
//    void RewardedVideoOnAdUnavailable()
//    {
//        isRewardAdAvailable = false;

//    }
//    // The Rewarded Video ad view has opened. Your activity will loose focus.
//    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
//    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
//    {
//    }
//    // The user completed to watch the video, and should be rewarded.
//    // The placement parameter will include the reward data.
//    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
//    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
//    {
//        GameManager.Instance.AddCoins(50);
//    }
//    // The rewarded video ad was failed to show.
//    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
//    {
//    }
//    // Invoked when the video ad was clicked.
//    // This callback is not supported by all networks, and we recommend using it only if
//    // it’s supported by all networks you included in your build.
//    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
//    {
//    }

//    #endregion

//    #region Offerwall Callbacks
//    /**
//* Invoked when there is a change in the Offerwall availability status.
//* @param - available - value will change to YES when Offerwall are available. 
//* You can then show the video by calling showOfferwall(). Value will change to NO when Offerwall isn't available.
//*/
//    void OfferwallAvailableEvent(bool canShowOfferwall)
//    {
//        isOfferwallReady = true;
//    }
//    /**
//     * Invoked when the Offerwall successfully loads for the user.
//     */
//    void OfferwallOpenedEvent()
//    {

//    }
//    /**
//     * Invoked when the method 'showOfferWall' is called and the OfferWall fails to load.  
//    *@param desc - A string which represents the reason of the failure.
//     */
//    void OfferwallShowFailedEvent(IronSourceError error)
//    {

//    }
//    /**
//      * Invoked each time the user completes an offer.
//      * Award the user with the credit amount corresponding to the value of the ‘credits’ 
//      * parameter.
//      * @param dict - A dictionary which holds the credits and the total credits.   
//      */
//    void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
//    {
//        ;
//    }
//    /**
//      * Invoked when the method 'getOfferWallCredits' fails to retrieve 
//      * the user's credit balance info.
//      * @param desc -string object that represents the reason of the  failure. 
//      */
//    void GetOfferwallCreditsFailedEvent(IronSourceError error)
//    {

//    }
//    /**
//      * Invoked when the user is about to return to the application after closing 
//      * the Offerwall.
//      */
//    void OfferwallClosedEvent()
//    {

//    }
//    #endregion
//}
