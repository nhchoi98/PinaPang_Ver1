using Ingame;
using LitJson;
using UnityEngine;
using System.IO;

namespace Ingame_Data
{
    public class Observer_StageInfo : MonoBehaviour,IObserver_Ingame
    {
        private StageInfoVO stageData;
        [SerializeField] private Determine_BoxType boxData;
        private bool isInit = false;
        public void Update_Status()
        {
            if (!isInit)
            {
                Read_Data();
                isInit = true;
            }

            this.stageData = boxData.Get_StageData();
            Write_Data();
        }

        #region  IO
        public void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/StageInfo.json";
            StageInfoVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<StageInfoVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                DATA = new StageInfoVO();
                DATA.score = 0;
                DATA.stage = 0;
                DATA.wave = 0;
                var DATA_STR = JsonUtility.ToJson(DATA) ;
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }

            stageData = DATA;
        }

        public void Write_Data()
        {
            var DATA = JsonMapper.ToJson(stageData);
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/StageInfo.json";
            File.WriteAllText(DATA_PATH,DATA);   
        }
        #endregion

        public void LoadData_ToIngame()
        {
            Read_Data();
            boxData.Set_StageData(stageData);
        }
    }
}
