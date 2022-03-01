using System;
using System.IO;
using UnityEngine;
using LitJson;

namespace Timer
{
    public class Determine_StarterDAO
    {
        private Determine_StarterVO data;
        public Determine_StarterDAO()
        {
            Read_Data();
        }

        private void Set_DateTime()
        {
            data.targetTime = DateTime.UtcNow + TimeSpan.FromDays(2);
        }

        #region IO
        private void Read_Data()
        {
            Determine_StarterVO data;
            var DATA_PATH = Application.persistentDataPath + "/Package/Starter/data.json";
            var PRE_DATA_PATH = "Package/Starter/data" ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonMapper.ToObject<Determine_StarterVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Package/Starter");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                data = JsonMapper.ToObject<Determine_StarterVO>(_DATA);
            }

            this.data = data;
        }

        private void Write_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Package/Starter/data.json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        #endregion

        #region Set_Data
        
        /// <summary>
        /// 로딩시 기간이 남았는지 안남았는지를 판단해주는 함수 
        /// </summary>
        public void Set_Purchasable()
        {
            if (!data.is_purchasable || data.is_first)
                return;
            
            TimeSpan targetTime =  data.targetTime.Subtract(DateTime.UtcNow);
            if (targetTime < TimeSpan.FromSeconds(1))
            {
                data.is_purchasable = false;
                Write_Data();
            }
        }

        public void Set_is_first()
        {
            data.is_first = false;
            Set_DateTime();
            Write_Data();
        }
        #endregion

        public bool Get_is_first()
        {
            return data.is_first;
        }

        public DateTime Get_TargetTime()
        {
            return data.targetTime;
        }

        /// <summary>
        /// 구매 가능 여부를 리턴하는 함수. 
        /// </summary>
        /// <returns></returns>
        public bool Get_Purchasable()
        {
            return data.is_purchasable;
        }
    }
}
