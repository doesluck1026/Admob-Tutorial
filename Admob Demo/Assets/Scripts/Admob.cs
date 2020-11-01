using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class Admob : MonoBehaviour
{
    public Text debugText;

    public bool TestMode = true;

    public string BannerID = "ca-app-pub-9174890030718712/6138798718";
    public string InterstitialID = "ca-app-pub-9174890030718712/5241801398";
    public string RewardedID = "ca-app-pub-9174890030718712/9092265113";

    private BannerView Banner;
    private InterstitialAd Interstitial;
    private RewardedAd Rewarded;
    private bool isBannerLoaded = false;
    private bool isRewardEarned = false;


    // Start is called before the first frame update
    void Start()
    {
        if (TestMode)
        {
            BannerID = "ca-app-pub-3940256099942544/6300978111";
            InterstitialID = "ca-app-pub-3940256099942544/1033173712";
            RewardedID = "ca-app-pub-3940256099942544/5224354917";
        }

        MobileAds.Initialize(initStatus => { });

        RequestInterstitial();
        RequestRewardedAd();
    }

    #region Public Functions

    public void ShowBanner()
    {
        RequestBanner();
    }
    public void CloseBanner()
    {
        Banner.Destroy();
    }

    public void ShowInterstitial()
    {
        if(Interstitial.IsLoaded())
        {
            Interstitial.Show();
        }
    }
    public void ShowRewardedAd()
    {
        if(Rewarded.IsLoaded())
        {
            Rewarded.Show();
        }
    }
    #endregion

    #region Private Functions

    private void RequestBanner()
    {
        Banner = new BannerView(BannerID, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        Banner.LoadAd(request);
    }
    private void RequestInterstitial()
    {
        Interstitial = new InterstitialAd(InterstitialID);
        Interstitial.OnAdClosed += Interstitial_OnAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        Interstitial.LoadAd(request);
    }

    private void Interstitial_OnAdClosed(object sender, System.EventArgs e)
    {
        RequestInterstitial();
    }
    private void RequestRewardedAd()
    {
        Rewarded = new RewardedAd(RewardedID);
        Rewarded.OnUserEarnedReward += Rewarded_OnUserEarnedReward;
        Rewarded.OnAdClosed += Rewarded_OnAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        Rewarded.LoadAd(request);

    }

    private void Rewarded_OnAdClosed(object sender, System.EventArgs e)
    {

        if(!isRewardEarned)
        {
            debugText.text = "You have no idea what you missed!";
        }
        RequestRewardedAd();
        isRewardEarned = false;
    }

    private void Rewarded_OnUserEarnedReward(object sender, Reward e)
    {
        debugText.text = "GG! You Earned a good Reward";
        isRewardEarned = true;
    }
    #endregion
}