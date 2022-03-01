
using System;
using Data;
using Firebase.Analytics;
using Ingame;
using shop;
using Shop;
using UnityEngine;
using UnityEngine.UI;

namespace ad
{
    public class DailyAd : MonoBehaviour, IComponent
    {
        [SerializeField] private DailyShop _dailyShop;
        [SerializeField] private Text skinText;
        private IMediator _mediator;

        public void UserChoseToWatchAd(int index)
        {
            _dailyShop.Set_Index(index);
            _mediator.Event_Receive(Event_num.SHOP_DAILY);
        }
        
        private void UserEarnedReward()
        {
            FirebaseAnalytics.LogEvent("Shop_Daily_Ad");
            _dailyShop.Event_Occur();
            skinText.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
        }


        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
        }
        
    }
}
