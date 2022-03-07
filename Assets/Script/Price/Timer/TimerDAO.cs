using System;
using System.Collections;
using LitJson;
using UnityEngine;
using System.IO;
using System.Timers;

namespace Timer
{
    public class TimerDAO
    {
        private TimerVO data;
        private static int second = 0;
        private static System.Timers.Timer aTimer;
        public TimerDAO()
        {
            Read_Data();
            SetTimer();

        }
        
        private static void SetTimer()
        {
            // Create a timer with a second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        
        private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            ++second;
        }
        
        
        private void Read_Data()
        {
            TimerVO _data;
            var DATA_PATH = Application.persistentDataPath + "/Shop/data/Timer/timer.json";
            var PRE_DATA_PATH = "Lobby/Shop/Timer/timer" ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                _data = JsonMapper.ToObject<TimerVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Shop/data/Timer");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                _data = JsonMapper.ToObject<TimerVO>(_DATA);
            }

            this.data = _data;
        }

        private void Write_Data()
        {
            data.second += second;
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Shop/data/Timer/timer.json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }

        public int Get_Second()
        {
            aTimer.Stop();
            aTimer.Dispose();
            Write_Data();
            return data.second;
        }

        public void Save_Data()
        {
            aTimer.Stop();
            aTimer.Dispose();
            Write_Data();
        }
        
        
    }
}


