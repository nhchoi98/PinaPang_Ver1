using System.Globalization;
using System.IO;
using Avatar;
using Collection;
using LitJson;
using Toy;
using UnityEngine;

namespace Badge
{
    /// <summary>
    /// 사용자의 능력치를 관리해주는 class
    /// 게임에 입장할 때, 사용자가 장착한 캐릭터를 바탕으로 어빌리티 데이터를 불러옴.
    /// 
    /// </summary>
    public static class AbilityDAO
    {
        private static AbilityVO data;
        public static int charater_bonus; 
        public static int badge_bonus;
        private static void Init_ability_data(int theme_num)
        {
            Read_Data();
            Determine_Which_Ability(theme_num);
        }

        /// <summary>
        /// 캐릭터를 장착하고, 게임에 입장하게 되면 호출되는 함수. 인게임에서는 이 정보를 바탕으로 관련 내용을 처리한다. 
        /// </summary>
        public static void Set_Ability_Data(int theme_num)
        {
            charater_bonus = 0;
            badge_bonus = 0;
            Init_ability_data(theme_num);
        }
        
        #region IO

        private static void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Ability/data.json";
            AbilityVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<AbilityVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ability");
                DATA = new AbilityVO();
                DATA.duration = 1000;
                DATA.bonus = 1000;
                DATA.candle = 0;
                DATA.speed = 0;
                DATA.life = false;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR); 
            }

            data = DATA;
        }
        

        #endregion

        #region Get
        public static int Get_Duration()
        {
            return data.duration;
        }

        public static int Get_Bonus()
        {
            return data.bonus;
        }

        public static int Get_Candle()
        {
            return data.candle;
        }

        public static int Get_Ball_Speed()
        {
            return data.speed;
        }

        public static bool Get_Extra_life()
        {
            return data.life;
        }

        #endregion
        
        #region Init_Data

        /// <summary>
        /// 슬롯에서 불러온 정보를 바탕으로 어떤 능력치들을 넣어줄 지 결정해주는 함수 
        /// </summary>
        private static void Determine_Which_Ability(int theme_num)
        {
            EquippedDAO equippedDao = new EquippedDAO();
            Badge_EquipDAO equipDao = new Badge_EquipDAO(equippedDao.Get_Equipped_index()); // 장착한 캐릭터의 슬롯 DAO 할당
            Charater_Bonus bonus_info = new Charater_Bonus();
            ToyDAO levelData = new ToyDAO();
            // Step 1. 모든 정보 기본 정보로 초기화 (읽을 때 이 스텝은 이미 진행됨)
            // Step 2. 배지 장착 정보를 여기에 반영해줌
            for (int i = 0; i < 3; i++)
                Determine_Ability_Type(equipDao.Get_Slot_data(i));
            // Step 3. 장착한 캐릭터의 bonus_score 정보를 반영해줌 (별도의 class를 만들자!)
            charater_bonus = bonus_info.bonus_score(Calc_Index.Get_Avatar_index(equippedDao.Get_Equipped_index()))%1000;
            data.bonus += (bonus_info.bonus_score(Calc_Index.Get_Avatar_index(equippedDao.Get_Equipped_index())))%1000;
            if (levelData.Get_Level() > 1)
                data.candle += 170 + 20 * (levelData.Get_Level() - 1);

            else
                data.candle = 170;

        }

        /// <summary>
        /// EQUIPDAO에서 정보를 불러오면, 어떤 슬롯에 의해 능력치가 얼마나 바뀌는지를 결정해주는 CLASS 
        /// </summary>
        private static void Determine_Ability_Type(int type)
        {
            int mode = type / 1000;
            int quantity = type%1000; // 정확한 수치를 저장하는곳 
            if (mode == 0 || mode == 9) // 빈 슬롯이거나, 잠긴 슬롯인경우 return
                return;
            
            switch (mode)
            {
                default: // bonus score
                    data.bonus += quantity;
                    badge_bonus += quantity;
                    break;
                
                case 2:
                    data.speed += quantity;
                    break;
                
                case 3:
                    data.life = true;
                    break;
                
                case 4:
                    data.candle += quantity;
                    break;
                
                case 5:
                    data.duration += quantity;
                    break;
            }
        }
        

        #endregion
    }
}
