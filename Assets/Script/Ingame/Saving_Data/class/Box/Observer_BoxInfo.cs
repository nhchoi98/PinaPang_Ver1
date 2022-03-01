using Ingame;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Block;

namespace Ingame_Data
{
    public class Observer_BoxInfo : MonoBehaviour, IObserver_Ingame
    {
        private List<BoxInfoVO> boxData;
        [SerializeField] private Transform boxGroup; // 박스를 보관하고 있는 Transform 
        
        private bool isInit = false;
        
        
        public void Update_Status()
        {
            if (!isInit)
            {
                boxData = new List<BoxInfoVO>();
                isInit = true;
            }

            if(boxData.Count!=0)
                boxData.Clear(); 
            
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                var info = boxGroup.GetChild(i).gameObject.GetComponent<IBox>();
                var hp = info.Get_HP();
                var candle = info.Get_Candle();
                var pos = info.Get_Position();
                var type = info.Get_Type();
                BoxInfoVO data = new BoxInfoVO();
                data.hp = hp;
                data.pos = pos;
                data.isCandle = candle;
                data.type = type;
                boxData.Add(data);
            }
            Write_Data();
        }

        public void Read_Data()
        {
            
        }

        public void Write_Data()
        {
            
            // Step 1. 파일이 없으면, 만들고 시작 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Boxdata.json";
            if (!File.Exists(DATA_PATH))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                BoxInfoVO data = new BoxInfoVO();
                data.hp = 1;
                data.pos = Vector2.zero;
                data.isCandle = false;
                data.type = blocktype.X2_TRI1;
                // 더미 데이터를 하나 넣어놓음.
                var DATA_STR = JsonUtility.ToJson(data);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }

            // 저장할 애들을 List에서 빼와서 Arr의 형태로 저장함.
        }

        public void LoadData_ToIngame()
        {
            Read_Data();
        }
    }
}
