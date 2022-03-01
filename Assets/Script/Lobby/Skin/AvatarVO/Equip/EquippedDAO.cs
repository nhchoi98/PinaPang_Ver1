
using UnityEngine;
using LitJson;
using System.IO;

namespace Avatar
{
    public class EquippedDAO
    {
        private EquippedVO DATA;

        
        /// <summary>
        /// 클래스 생성자 
        /// </summary>
        public EquippedDAO()
        {
            Read_Data();
        }

        /// <summary>
        /// 장착 index를 변경시켜주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Equipped_index(int index)
        {
            DATA.whichIndex = index;
            Save_Data();

        }
        
        public int Get_Equipped_index()
        {
            return DATA.whichIndex;
        }

        #region IO

        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Avatar/Info/equipInfo.json";
            var PRE_DATA_PATH = "Avatar/Info/equipInfo" ;
            EquippedVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<EquippedVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Avatar/Info");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<EquippedVO>(_DATA);
            }
            
            this.DATA = DATA;
        }


        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.DATA);
            var DATA_PATH = Application.persistentDataPath + "/Avatar/Info/equipInfo.json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        #endregion

    }
}

