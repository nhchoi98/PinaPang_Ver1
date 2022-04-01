using Ingame;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Block;
using Manager;
using Newtonsoft.Json;
using UnityEngine.AI;

/*
 * 필드에 존재하는 박스의 정보를 저장하는 스크립트 
 */
namespace Ingame_Data
{
    public class Observer_BoxInfo : MonoBehaviour, IObserver_Ingame
    {
        private List<BoxInfoVO> boxData;
        [SerializeField] private Transform boxGroup; // 박스를 보관하고 있는 Transform 
        
        private bool isInit = false;
        
        [Header("Setting")]
        JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings(); // 세팅 정보에 관한 정보 저장. 

        [SerializeField] private RespawnBox _respawnBox;

        public void Update_Status()
        {
            if (!isInit)
            {
                boxData = new List<BoxInfoVO>();
                isInit = true;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }

            if(boxData.Count!=0)
                boxData.Clear(); 
            
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                IBox info = boxGroup.GetChild(i).gameObject.GetComponent<IBox>();
                var hp = info.Get_HP();
                var candle = info.Get_Candle();
                var type = info.Get_Type();
                BoxInfoVO data = new BoxInfoVO();
                data.hp = hp;
                data.pos_x = boxGroup.GetChild(i).transform.position.x;
                data.pos_y = info.Get_Row();
                data.isCandle = candle;
                data.type = type;
                boxData.Add(data);
            }
            Write_Data();
        }

        private void Read_Data()
        {
            // Step 1. 파일이 없으면, 만들고 시작 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Boxdata.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                var str = JsonConvert.SerializeObject(boxData,settings); // 아예 비어있으면?
                File.WriteAllText(DATA_PATH,str);    
            }

            else
            {
                var json_string = File.ReadAllText(DATA_PATH);
                boxData = JsonConvert.DeserializeObject<List<BoxInfoVO>>(json_string);
            }
        }

        private void Write_Data()
        {
            // Step 1. 파일이 없으면, 만들고 시작 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Boxdata.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                var str = JsonConvert.SerializeObject(boxData,settings);
                File.WriteAllText(DATA_PATH,str);   
            }

            else
            {
                // 호출됨 
                var str = JsonConvert.SerializeObject(boxData,settings);
                File.WriteAllText(DATA_PATH,str); 
            }
            // 저장할 애들을 List에서 빼와서 Arr의 형태로 저장함.
        }

        internal class Wrapper
        {
            [JsonProperty("JsonValues")]
            public BoxInfoVO data { get; set; }
            
        }
        public void LoadData_ToIngame()
        {
            Read_Data();
            _respawnBox.Load_SpawnData(ref boxData);
            isInit = true;
            boxData = new List<BoxInfoVO>();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
