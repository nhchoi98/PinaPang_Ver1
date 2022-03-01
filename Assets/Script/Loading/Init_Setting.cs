
using Battery;
using Daily_Reward;
using Data;
using Setting;
using shop;
using UnityEngine;


namespace  Loading
{
    public class Init_Setting 
    {
        public Init_Setting()
        {
            Set_Time_Data();
            Set_Vibration();
        }
        
        // # 2. 시간 데이터를 받아와서, 시간에 대한 설정을 해줌 
        private void Set_Time_Data()
        {
            DailyDAO timedata = new DailyDAO();
            timedata.Get_reward_time();
            Playerdata_DAO.Init_Data();
        }
        
        // # 1. 진동, 볼륨 관련 설정을 불러옴 , static class에 반영해줌 
        private void Set_Volume()
        {
            
        }

        /// <summary>
        /// 진동관련 내용을 초기화해줌 
        /// </summary>
        private void Set_Vibration()
        {
            if(PlayerPrefs.GetInt("Vibration", 1) == 1)
                Vibration.Set_IsOn(true);

            else
                Vibration.Set_IsOn(false);
            
        }
        
        
    }
}

