using UnityEngine;
using LitJson;
using System.IO;

namespace Badge
{
    /// <summary>
    /// data 저장 format:  0 = 빈 슬롯 1 = bonus score, 2 = Ball speed, 3 = Life,  5 = duration, 6 = rect score, 7 = tri score, -1 = 장착 불가 
    /// 아바타의 뱃지 장착 Preset을 불러올 수 있게 만들어주는 DAO 
    /// </summary>
    public class Badge_EquipDAO
    {
        private Badge_EquipVO data;
        private int avatar_num;
        public Badge_EquipDAO(int avatarNum)
        {
            this.avatar_num = avatarNum;
            Read_Data(avatarNum);
        }

        #region IO
        private void Read_Data(int index)
        {
            var DATA_PATH = Application.persistentDataPath + "/Badge/Equip_info/" + avatar_num.ToString() + ".json";
            var PRE_DATA_PATH = "Badge/Equip_info/" +avatar_num ;
            Badge_EquipVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<Badge_EquipVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Badge/Equip_info");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<Badge_EquipVO>(_DATA);
            }
            
            this.data = DATA;
        }


        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Badge/Equip_info/" + avatar_num.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        

        #endregion
        
        #region Get
        public int Get_Slot_data(int index)
        {
            if(index == 0)
                return data.slot_1;
            
            else if (index == 1)
                return data.slot_2;

            else
                return data.slot_3;
        }
        #endregion

        #region  Set

        /// <summary>
        /// 뱃지의 index를 받아와서 이에 맞는 data를 저장시켜주는 함수. index가 99가 들어오는 경우에는 해당 슬롯을 비운다는 의미이다. 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Equip_data(int quantity, int which_slot)
        {
            if (which_slot == 0)
                data.slot_1 = quantity;
            
            else if (which_slot == 1)
                data.slot_2 = quantity;

            else
                data.slot_3 = quantity;
            
            Save_Data();
        }
        
        
        #endregion
    }
}