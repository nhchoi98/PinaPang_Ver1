using UnityEngine;
using System.IO;
using LitJson;

namespace Badge
{

    class BadgeDAO
    {
        private BadgeVO[] Combi_data;
        private BadgeVO single_data;
        private Calc_Badge_Index calc_index;
        public BadgeDAO()
        {
            calc_index = new Calc_Badge_Index();
            Combi_data = new BadgeVO[calc_index.Get_Badge_MaxIndex()];
            Read_Data(); // 데이터를 arr에 저장함 
        }

        public BadgeDAO(int index)
        {
            calc_index = new Calc_Badge_Index();
            Read_Single_Data(index);
        }
        
        #region IO

        private void Read_Data()
        {
            for (int i = 0; i < calc_index.Get_Badge_MaxIndex() ; i++)
            {
                var badge_value = calc_index.Get_Badge_Num(i);
                var DATA_PATH = Application.persistentDataPath + "/Badge/" +badge_value.ToString() + ".json";
                BadgeVO DATA = null;
                if (File.Exists(DATA_PATH))
                {
                    var json_string = File.ReadAllText(DATA_PATH);
                    DATA = JsonMapper.ToObject<BadgeVO>(json_string);
                }

                else
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Badge");
                    DATA= new BadgeVO();
                    DATA.combi_achi = false;
                    DATA.is_new = false;
                    var DATA_STR = JsonUtility.ToJson(DATA);
                    File.WriteAllText(DATA_PATH, DATA_STR);
                }
                Combi_data[i] = DATA;
            }
        }


        private void Write_Data(int index)
        {
            int value = calc_index.Get_Badge_Num(index);
            var DATA = JsonMapper.ToJson(this.Combi_data[index]);
            var DATA_PATH = Application.persistentDataPath + "/Badge/" + value.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }

        private void Read_Single_Data(int index)
        {
            int value = calc_index.Get_Badge_Num(index);
            
            var DATA_PATH = Application.persistentDataPath + "/Badge/" + value + ".json";
            BadgeVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<BadgeVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Badge");
                DATA= new BadgeVO();
                DATA.combi_achi = false;
                DATA.is_new = false;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH, DATA_STR);
            }
            
            single_data = DATA;
            
            
        }

        private void Write_Single_Data(int index)
        {
            var DATA = JsonMapper.ToJson(single_data);
            var DATA_PATH = Application.persistentDataPath + "/Badge/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        
        
        #endregion

        #region Get

        /// <summary>
        /// 조합 달성 유무를 저장하는 변수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_Achi_Data(int index, bool single)
        {
            if (!single)
                return Combi_data[index].combi_achi;

            else
            {
                return single_data.combi_achi;
            }
        }
        

        /// <summary>
        /// 싱글로 생성했을 때, 새로 얻었는지 안얻었는지를 판단해주는 함수 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Get_Is_new(int value)
        {
            return single_data.is_new;
        }
        
        
        #endregion

        #region Set
        
        public void Set_Achi_Data(int index)
        {
            int value = calc_index.Get_Badge_Num(index);
            single_data.combi_achi = true;
            single_data.is_new = true;
            Write_Single_Data(value);
        }

        public void Set_New_Data(int index)
        {
            int value = calc_index.Get_Badge_Num(index);
            single_data.is_new = false;
            Write_Single_Data(value);
        }
        
        #endregion
        
    }
}
