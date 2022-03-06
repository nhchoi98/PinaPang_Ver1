using System;
using System.Collections;
using System.Collections.Generic;
using Ad;
using UnityEngine;
using Manager;

namespace Ingame
{
    public class Ad_Mediator : MonoBehaviour, IMediator
    {
        [Header("Component")] 
        private IComponent speedAd;
        private IComponent crossAd;
        private IComponent lineAd;
        private IComponent reviveAd;
        [SerializeField] private Transform adTR;
        private Event_num _eventNum;

        [Header("Reward_Ad_Set")]
        private  string RewardedAdUnitId = "0715368d8c1845b1";

        private int rewardedRetryAttempt = 0;
        
        [Header("Interstital_Set")]
        public GameObject adNotshow;
        private string InterstitialAdUnitId = "54555ae032543e1f";
        private int interstitialRetryAttempt = 0;
        
        [Header("Banner_Set")]
        private string BannerAdUnitId = "1d79fb9316093424";

        [Header("GameMediator")] [SerializeField]
        
        public GameObject itemPanel;
        public GameObject pausePanel;
        public GameObject gameover;
        private void Awake()
        {
            
            #if UNITY_IOS
                BannerAdUnitId = "ec5f65d2c159a19c";
                InterstitialAdUnitId = "26b1c976d6c506c2";
                RewardedAdUnitId = "4feba1083bfe243f";
            #endif
            
            InitializeInterstitialAds(); // 인터스티셜 광고 초기화 
            InitializeBannerAds(); // 배너 광고 초기화 
            InitializeRewardedAds();
            MaxSdk.ShowBanner(BannerAdUnitId);
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        speedAd = adTR.GetChild(0).GetComponent<IComponent>();
                        speedAd.Set_Mediator(this);
                        break;
                    
                    case 1:
                        lineAd = adTR.GetChild(1).GetComponent<IComponent>();
                        lineAd.Set_Mediator(this);
                        break;
                    
                    case 2:
                        crossAd = adTR.GetChild(2).GetComponent<IComponent>();
                        crossAd.Set_Mediator(this);
                        break;

                    case 3:
                        reviveAd = adTR.GetChild(3).GetComponent<IComponent>();
                        reviveAd.Set_Mediator(this);
                        break;
                }
            }
            
        }
        
        private void OnApplicationQuit()
        {
            Destroy_Banner();
        }

        #region Ad_Set

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Time.timeScale = 1f;
            itemPanel.SetActive(false);
            pausePanel.SetActive(false);
            // ARGS에 따라서 판단하기 
            switch (_eventNum)
            {
                case Event_num.SPEED_AD:
                    speedAd.Event_Occur(_eventNum);
                    break;
                
                case Event_num.LINE_AD:
                    lineAd.Event_Occur(_eventNum);
                    break;
                
                case Event_num.CROSS_AD:
                    crossAd.Event_Occur(_eventNum);
                    break;

                case Event_num.REVIVE_AD:
                    reviveAd.Event_Occur(_eventNum);
                    break;
            }

            LoadRewardedAd();
        }
        #endregion
        
        #region Action
        /// <summary>
        /// 다른 광고 스크립트에서 이벤트를 수신받으면, 이에 맞는 광고를 송출해줌 
        /// </summary>
        /// <param name="eventNum"></param>
        public void Event_Receive(Event_num eventNum)
        {
            if(eventNum == Event_num.USER_DIE)
            {
                if(!Noads_instance.Get_Is_Noads())
                    ShowInterstitial();
            }
            
            else if (eventNum == Event_num.BANNER)
                Destroy_Banner();

            else
            {
                if (eventNum != Event_num.SET_ITEM)
                {
                    this._eventNum = eventNum;
                    if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
                        MaxSdk.ShowRewardedAd(RewardedAdUnitId);


                    else
                    {
                        Instantiate(adNotshow);
                        LoadRewardedAd();
                    }
                }
            }
        }
        
        
        public void Destroy_Banner()
        {
            MaxSdk.DestroyBanner(BannerAdUnitId);
        }
        
        
        #endregion

        #region INTERSTITIAL_SET
        private void InitializeInterstitialAds()
        {
            // Attach callbacks

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent +=  OnRewardedAdDisplayedEvent;
            // Load the first interstitial
            LoadInterstitial();
        }

        
        private void  OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -=  OnRewardedAdDisplayedEvent;
            Time.timeScale = 1f;
            gameover.SetActive(true);
            LoadInterstitial();
        }
        
        void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        }

        void ShowInterstitial()
        {
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
            else
            {
                Instantiate(adNotshow);
                LoadInterstitial();
            }
        }
        
        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            
            Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);
            Invoke("LoadInterstitial", (float) retryDelay);
        }
        
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

            Debug.Log("Interstitial loaded");
            interstitialRetryAttempt = 0;
        }
        #endregion

        #region  Banner_Set
        private void InitializeBannerAds()
        {
            // Attach Callbacks
            if (!Noads_instance.Get_Is_Noads())
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
     
                MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);

                // Set background or background color for banners to be fully functional.
                MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
            }
        }
        
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Banner ad is ready to be shown.
            // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
            Debug.Log("Banner ad loaded");
        }
        
        private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Banner ad failed to load. MAX will automatically try loading a new ad internally.
            Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
        }
        
        #endregion

        #region  Reward_Set
        private void InitializeRewardedAds()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            // Load the first RewardedAd
            LoadRewardedAd();
        }
        
        private void LoadRewardedAd()
        {
            if(!MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
                MaxSdk.LoadRewardedAd(RewardedAdUnitId);
        }
        
        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
            
            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);
        
            Invoke("LoadRewardedAd", (float) retryDelay);
        }
        
        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
            LoadRewardedAd();
        }
        
        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad clicked");
        }
        
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded ad loaded");
            // Reset retry attempt
            rewardedRetryAttempt = 0;
        }
        
        
        #endregion

        public void Remove_CallBack()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
        }
    }
}
