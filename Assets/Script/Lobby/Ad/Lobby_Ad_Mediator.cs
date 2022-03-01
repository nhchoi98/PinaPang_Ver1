using System;
using System.Collections;
//using GoogleMobileAds.Api;
using Ingame;
using UnityEngine;

namespace Ad
{
    public class Lobby_Ad_Mediator : MonoBehaviour, IMediator
    {
        [Header("Reward_Ad")]
        //private RewardedAd rewardedAd;
        public GameObject adNotshow;

        [Header("IComponent")] 
        private IComponent questAd;
        private IComponent dailyPurchase;
        private IComponent linePurchase;
        private IComponent batteryCharge;
        private Event_num _eventNum;
        
        [Header("Ad_data")]
        private string RewardedAdUnit_Id= "0715368d8c1845b1"; // 로비에서 쓰이는 reward key
        private int rewardedRetryAttempt = 0;
        private bool is_inited = false;

        private void Awake()
        {
            InitializeRewardedAds();
            #if UNITY_IOS
                RewardedAdUnit_Id= "4feba1083bfe243f";
            #endif

            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        questAd = transform.GetChild(i).GetComponent<IComponent>();
                        questAd.Set_Mediator(this);
                        break;
                    
                    case 1:
                        linePurchase = transform.GetChild(i).GetComponent<IComponent>();
                        linePurchase.Set_Mediator(this);
                        break;

                    case 2:
                        dailyPurchase = transform.GetChild(i).GetComponent<IComponent>();
                        dailyPurchase.Set_Mediator(this);
                        break;

                    case 3 :
                        batteryCharge = transform.GetChild(i).GetComponent<IComponent>();
                        batteryCharge.Set_Mediator(this);
                        break;
                }
            }
        }
        
        private void InitializeRewardedAds()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            // 로드할 광고 종류 
            MaxSdk.LoadRewardedAd(RewardedAdUnit_Id);
        }
        
        private void LoadRewardedAd()
        {
            switch (_eventNum)
            {
                case Event_num.QUEST_RESET:
                case Event_num.SHOP_DAILY:
                case Event_num.LINE_PURCHASE:
                case Event_num.BATTERY:
                    MaxSdk.LoadRewardedAd(RewardedAdUnit_Id);
                    break;
            }
        }

        public void Event_Receive(Event_num eventNum)
        {
            this._eventNum = eventNum;
            if (MaxSdk.IsRewardedAdReady(Which_AdUnit()))
                MaxSdk.ShowRewardedAd(Which_AdUnit());
            
            
            else
            {
                Instantiate(adNotshow);
                LoadRewardedAd();
            }

        }

        #region Ad_Action
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
        }
        
        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Time.timeScale = 0f;
            Debug.Log("Rewarded ad clicked");
        }
        
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded ad loaded");
        
            // Reset retry attempt
            rewardedRetryAttempt = 0;
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Time.timeScale = 1f;
            switch (_eventNum)
            {
                case Event_num.QUEST_RESET:
                    questAd.Event_Occur(_eventNum);
                    break;
                
                case Event_num.SHOP_DAILY:
                    dailyPurchase.Event_Occur(_eventNum);
                    break;


                case Event_num.LINE_PURCHASE:
                    linePurchase.Event_Occur(_eventNum);
                    break;
                
                case Event_num.BATTERY:
                    batteryCharge.Event_Occur(_eventNum);
                    break;
            }
            LoadRewardedAd();
        }
        
        #endregion
        
        public void Remove_CallBack()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
        }

        private string Which_AdUnit()
        {
        
            switch (_eventNum)
            {
                case Event_num.QUEST_RESET:
                case Event_num.SHOP_DAILY:
                case Event_num.BATTERY:
                case Event_num.LINE_PURCHASE:
                    return RewardedAdUnit_Id;
            }

   
                return null;
            
        }
        
    }
}
