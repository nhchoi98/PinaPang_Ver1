using System;
using System.Collections;
using System.Collections.Generic;
using Challenge;
using Shop;
using UnityEngine;
using UnityEngine.UI;
using Skin;

namespace Alarm
{
    public class Shop_Alarm : MonoBehaviour, IAlarmComponent
    {
        private IAlarmMediator _mediator;
        [SerializeField] private Transform shopTR;
        public GameObject shopBtn_Alarm, tapAlarm;
        
        [Header("Data")]
        [SerializeField]private DailyShop _dailyShop;
        [SerializeField] private Level_UI leveldata; 
        
        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.SHOP_ALARM_ON:
                    if (is_alarm_on())
                    {
                        shopBtn_Alarm.SetActive(true);
                        tapAlarm.SetActive(true);
                    }

                    Set_Alarm_On();
                    break;
                
                case Event_Alarm.SHOP_ALARM_OFF:
                    Set_Alarm_False();
                    if (!is_alarm_on())
                    {
                        shopBtn_Alarm.SetActive(false);
                        tapAlarm.SetActive(false);
                    }

                    break;
                
            } 
        }

        public void First_Set()
        {
            _mediator.Event_Receieve(Event_Alarm.SHOP_ALARM_ON); // Step 2. 있으면 알림 ON
        }

        public bool Get_Alarm()
        {
            return is_alarm_on();
        }
        
        /// <summary>
        /// 시청 다 했으면 알람을 꺼줌 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Alarm_False()
        {
            if(_dailyShop.countData.Get_Count(0) == 0)
                shopTR.GetChild(0).GetChild(4).gameObject.SetActive(false);

            if (leveldata.Get_Level() > 16)
            {
                if(_dailyShop.countData.Get_Count(1) == 0)
                    shopTR.GetChild(1).GetChild(4).gameObject.SetActive(false);
            }

            if (leveldata.Get_Level() > 31)
            {
                if (_dailyShop.countData.Get_Count(2) == 0)
                    shopTR.GetChild(2).GetChild(4).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 시청 할 횟수 남아있으면 알람 켜줌 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Alarm_On()
        {
            if(_dailyShop.countData.Get_Count(0)>0)
                shopTR.GetChild(0).GetChild(4).gameObject.SetActive(true);

            if (leveldata.Get_Level() > 16)
            {
                if(_dailyShop.countData.Get_Count(1)>0)
                    shopTR.GetChild(1).GetChild(4).gameObject.SetActive(true);
            }

            if (leveldata.Get_Level() > 30)
            {
                if (_dailyShop.countData.Get_Count(2) > 0)
                    shopTR.GetChild(2).GetChild(4).gameObject.SetActive(true);
            }
        }

        private bool is_alarm_on()
        {
            if (leveldata.Get_Level() < 16)
            {
                if (_dailyShop.countData.Get_Count(0) == 0)
                    return false;

                else
                    return true;
            }
            
            
            else if (leveldata.Get_Level() >= 16 && leveldata.Get_Level() < 31)
            {
                if (_dailyShop.countData.Get_Count(0) == 0 && _dailyShop.countData.Get_Count(1) == 0)
                    return false;

                else
                    return true;
                
            }

            else
            {
                if (_dailyShop.countData.Get_Count(0) == 0 && _dailyShop.countData.Get_Count(1) == 0 && _dailyShop.countData.Get_Count(2) == 0)
                    return false;

                else
                    return true;
            }
        }

    }
}
