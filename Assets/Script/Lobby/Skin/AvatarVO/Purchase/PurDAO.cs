using UnityEngine;
using System.IO;
using LitJson;

namespace Avatar
{
    public class CardDAO
    {
        private CardVO DATA;
        private int index;

        public CardDAO(int index)
        {
            Read_Data(index);
        }
        #region IO

        private void Read_Data(int index)
        {
            var DATA_PATH = Application.persistentDataPath + "/Avatar/card/"+ index.ToString() + ".json";
            var PRE_DATA_PATH = "Avatar/card/"+ index ;
            CardVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<CardVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Avatar/card");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<CardVO>(_DATA);
            }
            
            this.DATA = DATA;
            this.index = index;
        }

        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.DATA);
            var DATA_PATH = Application.persistentDataPath + "/Avatar/card/"+ index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        
        #endregion

        #region  Set_Data

        /// <summary>
        /// 캐릭터 구매시 호출되는 함수 
        /// </summary>
        public void Set_Data(int num)
        {
            DATA.card_num += num;
            Save_Data();
        }

        #endregion

        #region Get_Data

        public int Get_data()
        {
            return DATA.card_num;
        }
        
        #endregion

    }
}

