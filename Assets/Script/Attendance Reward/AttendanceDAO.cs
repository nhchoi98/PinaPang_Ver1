using System.IO;
using LitJson;
using UnityEngine;

namespace Attendance
{
    public class AttendanceDAO
    {
        private const int DATA_SUM = 8;
        private AttendanceVO[] data;
        public AttendanceDAO()
        {
            Read_Data();            
        }
        
        #region IO
        private void Read_Data()
        {
            data = new AttendanceVO[DATA_SUM];
            for (int i = 0; i < DATA_SUM; i++)
            {
                var DATA_PATH = Application.persistentDataPath + "/Attendance_2/" +i.ToString() + ".json";
                AttendanceVO DATA = null;
                if (File.Exists(DATA_PATH))
                {
                    var json_string = File.ReadAllText(DATA_PATH);
                    DATA = JsonMapper.ToObject<AttendanceVO>(json_string);
                }

                else
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Attendance_2");
                    DATA  = new  AttendanceVO();
                    DATA.can_get = false;
                    DATA.is_get = false;
                    var DATA_STR = JsonUtility.ToJson(DATA);
                    File.WriteAllText(DATA_PATH,DATA_STR);   
                    
                    if (i == 0)
                    {
                        data[0] = DATA;
                        data[0].can_get = true;
                        Write_Data(0);
                    }
                }

                data[i] = DATA;
            }
        }

        private void Write_Data(int index)
        {
            var DATA = JsonMapper.ToJson(data[index]);
            var DATA_PATH = Application.persistentDataPath + "/Attendance_2/" +index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);   
            
        }
        #endregion
        
        #region Get
        public bool Get_can_get(int index)
        {
            return data[index].can_get;
        }

        public bool Get_is_get(int index)
        {
            return data[index].is_get;
        }
        #endregion

        #region Set
        public void Set_can_get(int index)
        {
            data[index].can_get = true;
            Write_Data(index);
        }

        public void Set_is_get(int index)
        {
            data[index].is_get = true;
            Write_Data(index);
        }
        #endregion

        public void Next_Day_Set()
        {
            int day = PlayerPrefs.GetInt("Which_Day", 0);

            if (PlayerPrefs.GetInt("Attendance_All_Clear", 0) == 1) // 출석부 다 깼을 때 
            {
                Debug.Log("다깸");
                return;
            }

            if (day > 20)
            {
                Debug.Log("day 다 됨");
                return;
            }
            
            data[day].can_get = true;
            Write_Data(day);
            PlayerPrefs.SetInt("Which_Day", ++day);
            PlayerPrefs.SetInt("Show_Up", 1);
            
            if(day >DATA_SUM)
                PlayerPrefs.SetInt("Attandance_All_Clear",1);
        }
        
    }
}
