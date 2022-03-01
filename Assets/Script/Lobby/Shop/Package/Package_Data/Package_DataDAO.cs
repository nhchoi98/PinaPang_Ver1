using System.IO;
using UnityEngine;
using LitJson;

namespace Shop
{
    /// <summary>
    /// 0번: No_Ads
    /// 1번: starter
    /// 2번: partygirl
    /// 3번: astro
    /// 4번: bear
    /// </summary>
    public class Package_DataDAO
    {
        private Package_DataVO data;
        private int index;
        
        public Package_DataDAO(int index)
        {
            this.index = index;
            Read_Data();
        }

        #region IO
        private void Read_Data()
        {
            Package_DataVO data;
            var DATA_PATH = Application.persistentDataPath + "/Shop/data/" + index.ToString() + ".json";
            var PRE_DATA_PATH = "Lobby/Shop/Data/" +index ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonMapper.ToObject<Package_DataVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Shop/data");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                data = JsonMapper.ToObject<Package_DataVO>(_DATA);
            }

            this.data = data;
        }


        private void Write_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Shop/data/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        #endregion

        #region Getter,Setter
        public bool Get_Data()
        {
            return data.is_purchased;
        }

        public void Set_Data()
        { 
            data.is_purchased = true;
            Write_Data();
        }
        
        #endregion

    }
}
