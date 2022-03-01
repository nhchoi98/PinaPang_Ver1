using System;
using System.Collections;
using System.Collections.Generic;
using Alarm;
using UnityEngine;

namespace Shop
{
    public class Shop_AlarmSetter
    {
        private IAlarmMediator _alarmMediator;
        private Transform tr;
        public Shop_AlarmSetter()
        {
            _alarmMediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>(); // 변수 초기화 
        }
        
        /// <summary>
        ///  구매한 패키지를 기준으로, 알람을 설정해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Alarm(int index)
        {
            switch (index)
            {
                case 0:
                    Set_Astro_Alarm();
                    break;
                
                case 1:
                    Set_Party_Alarm();
                    break;
                
                case 2:
                    Set_Bear_Alarm();
                    break;

                case 3:
                    Set_Science_Alarm();
                    break;
            }
        }

        private void Set_Science_Alarm()
        {
            PlayerPrefs.SetInt("AVATAR_13",1); // PREFS를 설정해줌
            PlayerPrefs.SetInt("BALL_4003",1);
            _alarmMediator.Event_Receieve(Event_Alarm.SCIENCE_ALARM);
        }
        

        private void Set_Astro_Alarm()
        {
            PlayerPrefs.SetInt("AVATAR_2000",1); // PREFS를 설정해줌
            PlayerPrefs.SetInt("BALL_4002",1);
            PlayerPrefs.SetInt("LINE_14", 1);
            _alarmMediator.Event_Receieve(Event_Alarm.ASTRO_ALARM);
            // BALL 알람 변수 설정
            // LINE 알람 변수 설정 
        }

        private void Set_Party_Alarm()
        {
            PlayerPrefs.SetInt("AVATAR_1006",1); // PREFS를 설정해줌
            PlayerPrefs.SetInt("BALL_4000",1);
            _alarmMediator.Event_Receieve(Event_Alarm.PARTY_ALARM);
            // BALL 알람 변수 설정
            // LINE 알람 변수 설정 
        }
        
        private void Set_Bear_Alarm()
        {
            PlayerPrefs.SetInt("AVATAR_2002",1); // PREFS를 설정해줌
            PlayerPrefs.SetInt("BALL_4001",1);
            PlayerPrefs.SetInt("LINE_13", 1);
            _alarmMediator.Event_Receieve(Event_Alarm.BEAR_ALARM);
            // BALL 알람 변수 설정
            // LINE 알람 변수 설정 
        }
        
    }
}

