using System;
using System.IO;
using Attendance;
using Battery;
using Challenge;
using UnityEngine;
using LitJson;
using shop;

namespace Daily_Reward
{
    public class DailyDAO
    {
        private DailyVO data;
        // 일일 뽑기 관련 VO 들어가야함 
        public DailyDAO()
        {
            //1. 파일 읽어오기 
            Read_Data();
            //2. 데이터 Regional var로 저장하기
        }
        #region IO
        //1. File Read
        private void Read_Data()
        {
            string DATA_PATH = Application.persistentDataPath + "/Daily/info.json";
            string PRE_DATA_PATH = "Lobby/Daily_Reward/info";
            DailyVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                string json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<DailyVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Daily");
                DATA = new DailyVO();
                DATA.access_time = 991234;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR); 
            }

            data = DATA;
        }
        //2. File write 
        private void Write_DATA()
        {
            string DATA_PATH = Application.persistentDataPath + "/Daily/info.json";
            string DATA = JsonMapper.ToJson(data);
            File.WriteAllText(DATA_PATH, DATA);
            // 저장 관련 코드 
        }

        #endregion

        

        /// <summary>
        /// 마지막 접속시각을 체크해주는 함수. 이 때, 마지막으로 접속한 시각이 하루가 지나가있으면 show_reward를 초기화 하고, play_count를 초기화 해준다. 
        /// </summary>
        public void Get_reward_time()
        {
            Calc_Time data = new Calc_Time();
            if (this.data.access_time != data.time_data())
            {
                this.data.access_time = data.time_data();
                ChallengeDAO challengeDao = new ChallengeDAO(); // 2. 챌린지 초기화 
                challengeDao.Init_data(true);
                DailyShopDAO shopdata = new DailyShopDAO();
                shopdata.Reset();
                // 출석부 초기화 
                /*
                AttendanceDAO attendanceData = new AttendanceDAO();
                attendanceData.Next_Day_Set();
                */
            }
            Write_DATA();
        }
        
    }
    
    
    public class Calc_Time
    {
        public Calc_Time()
        {
            _dateTime = DateTime.UtcNow;
            year = _dateTime.Year;
            month = _dateTime.Month;
            day = _dateTime.Day;
        }
        
        private int day;
        private int month;
        private int year;
        private DateTime _dateTime;


        public int time_data()
        {
            var value = (year % 100) * 10000 + month * 100 + day;
            return value;
        }
    }
    

}
   
