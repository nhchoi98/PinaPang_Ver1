using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

namespace Timer
{
    public static class FirstPurchase_Timer
    {
        private static TimerDAO timerData;
        private static bool is_recording;
        private static int timerCount = 0;
        public static void Init_Data()
        {
            if (PlayerPrefs.GetInt("First_Purchase", 0) == 0)
            {
                is_recording = true;
                timerData = new TimerDAO();

            }

            else
            {
                is_recording = false;
            }
            // 타이머 읽어오기 
        }

        // 다른 시도를 좀 해봐야해 
        public static void First_Purchase(string which_item)
        {
            if (PlayerPrefs.GetInt("First_Purchase", 0) == 0)
            {
                PlayerPrefs.SetInt("First_Purchase", 1);
                int second = timerData.Get_Second();
                FirebaseAnalytics.LogEvent("First_Purchase", new Parameter("First_Buy_Time", second),new Parameter("First_Buy_Item",which_item));
                Debug.Log(second + "+" + which_item);
                // 시간 데이터 로그로 보내기 
                // 시간 데이터 저장 
                is_recording = false;
            }
        }

        public static void Restore_Purchase()
        {
            PlayerPrefs.SetInt("First_Purchase", 1);
            is_recording = false;
        }
        
        public static void Save_Data()
        {
            if (is_recording)
                timerData.Save_Data();
            
        }
        
    }
}
