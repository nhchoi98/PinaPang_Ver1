using Ingame;
using UnityEngine;
using System.IO;

namespace Ingame_Data
{
    public class Observer_BallInfo : MonoBehaviour, IObserver_Ingame
    {
        private BallInfo_VO ballData;
        [SerializeField] private BallManage ballData_instance;
        private bool isInit = false;
        
        public void Update_Status()
        {
            if (!isInit)
            {
                Read_Data();
                isInit = true;
            }

            ballData =  ballData_instance.Set_BallInfo();
            Write_Data();
        }

        public void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/BallInfo.json";
            BallInfo_VO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonUtility.FromJson<BallInfo_VO>(json_string); //JsonMapper.ToObject<BallInfo_VO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                ballData = new BallInfo_VO();
                ballData.ballNum = 0;
                ballData.ballPos = new Vector2(1f, 1f);
                ballData.charPos = new Vector2(1f, 1f);
                ballData.is_Fliped = false;
                var DATA_STR = JsonUtility.ToJson(ballData);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }

            ballData = DATA;
        }

        public void Write_Data()
        {
            var DATA = JsonUtility.ToJson(ballData);
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/BallInfo.json";
            File.WriteAllText(DATA_PATH,DATA);   
        }

        public void LoadData_ToIngame()
        {
            Read_Data();
        }
    }
}

