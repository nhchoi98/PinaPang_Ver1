using System.Collections;
using System.IO;
using System.Collections.Generic;
using Ad;
using Ingame_Data;
using Item;
using UnityEngine;
using Newtonsoft.Json;

namespace Ingame
{
    public class Observer_ItemTime : MonoBehaviour, IObserver_Ingame
    {
        [Header("Load_Data")] private bool isInit = false;

        [Header("Item_Status")] [SerializeField]
        private BallSpeedAd _ballSpeedAd;
        [SerializeField] private Both_Ad crossAd; // 교차형 필드 아이템
        [SerializeField] private Line_Ad lineAd; // 꺾임선 아이템 
            
        [Header("Setting")]
        JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings(); // 세팅 정보에 관한 정보 저장. 

        [Header("Data")] private ItemTimeVO speedData;
        private ItemTimeVO crossData;
        private ItemTimeVO lineData;
        
        public void Update_Status()
        {
            if (!isInit)
            {
                Read_Data();
                isInit = true;
            }
            crossAd.Save_Data(ref crossData.time, ref crossData.active);
            lineAd.Save_Data(ref lineData.time, ref lineData.active);
            _ballSpeedAd.Save_Data(ref speedData.time, ref speedData.active);
            Write_Data();
            // 세 개 데이터 저장하기. 
        }

        #region  Set_DATA

        private void Set_Time_Data()
        {
            crossAd.Load_Data(crossData.time, crossData.active);
            lineAd.Load_Data(lineData.time, lineData.active);
            _ballSpeedAd.Load_Data(speedData.time, speedData.active);
            return;
        }
        
        #endregion
        public void LoadData_ToIngame()
        {
            Read_Data();
            if (Noads_instance.Get_ItemAds()) // 무한대 아이템을 산 경우, 따로 처리 해주지 않음 
                return;

            else Set_Time_Data();
            // 데이터 읽어오기 


        }
        
        private void Read_Data()
        {
            // Step 1. 파일이 없으면, 만들고 시작 
            string DATA_PATH = Application.persistentDataPath + "/Ingame_Data/crossData.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                ItemTimeVO data = new ItemTimeVO();
                data.time = 0f;
                data.active = false;
                var str_data = JsonUtility.ToJson(data);// 아예 비어있으면?
                File.WriteAllText(DATA_PATH,str_data);
                crossData = data;
            }

            else
            {
                var json_string = File.ReadAllText(DATA_PATH);
                crossData = JsonUtility.FromJson<ItemTimeVO>(json_string);
            }
            
            DATA_PATH = Application.persistentDataPath + "/Ingame_Data/speedData.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                ItemTimeVO data = new ItemTimeVO();
                data.time = 0f;
                data.active = false;
                var str_data = JsonUtility.ToJson(data);// 아예 비어있으면?
                File.WriteAllText(DATA_PATH,str_data);
                speedData = data;
            }

            else
            {
                var json_string = File.ReadAllText(DATA_PATH);
                speedData = JsonUtility.FromJson<ItemTimeVO>(json_string);
            }
            
            DATA_PATH = Application.persistentDataPath + "/Ingame_Data/lineData.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                ItemTimeVO data = new ItemTimeVO();
                data.time = 0f;
                data.active = false;
                var str_data = JsonUtility.ToJson(data);// 아예 비어있으면?
                File.WriteAllText(DATA_PATH,str_data);
                lineData = data;
            }

            else
            {
                var json_string = File.ReadAllText(DATA_PATH);
                lineData = JsonUtility.FromJson<ItemTimeVO>(json_string);
            }
        }

        private void Write_Data()
        {
            string DATA_PATH = Application.persistentDataPath + "/Ingame_Data/speedData.json";
            string str = JsonUtility.ToJson(speedData);
            File.WriteAllText(DATA_PATH,str); 
            
            
            DATA_PATH = Application.persistentDataPath + "/Ingame_Data/lineData.json";
            str = JsonUtility.ToJson(lineData);
            File.WriteAllText(DATA_PATH,str); 
            
            DATA_PATH = Application.persistentDataPath + "/Ingame_Data/crossData.json";
            str = JsonUtility.ToJson(crossData);
            File.WriteAllText(DATA_PATH,str);
            
            // 저장할 애들을 List에서 빼와서 Arr의 형태로 저장함.
        }
    }
}
