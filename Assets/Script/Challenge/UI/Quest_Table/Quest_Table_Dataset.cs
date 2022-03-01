using System.IO;
using LitJson;
using System.Collections.Generic;
using Challnge;
using UnityEngine;

namespace Challenge
{
    public class Quest_Table_Dataset
    {
        private List<bool> is_get;
        private int target_level;
        private int lower_get_level =1;
        private bool lower_set;
        public Quest_Table_Dataset(int level)
        {
            target_level = level;
            is_get = new List<bool>();
            for (int i = 1; i <= target_level; i++)
                Read_Data(i);
        }
        
        #region IO
        private void Read_Data(int level)
        {
            var DATA_PATH = Application.persistentDataPath + "/Challenge/exp/reward/" + level.ToString() + ".json";
            var PRE_DATA_PATH = "Challenge/exp/Reward/" + level.ToString();
            QuestTableVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<QuestTableVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Challenge/exp/reward");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<QuestTableVO>(_DATA);
            }
            is_get.Add(DATA.is_get);
            if (!DATA.is_get)
            {
                if (!lower_set)
                {
                    if (lower_get_level < level)
                    {
                        lower_get_level = level;
                        lower_set = true;
                    }

                }
            }

        }

        private void Write_Data(int level)
        {
            QuestTableVO _DATA = new QuestTableVO();
            _DATA.is_get = true;
            var DATA = JsonMapper.ToJson(_DATA);
            var DATA_PATH = Application.persistentDataPath + "/Challenge/exp/reward/" + level.ToString() + ".json";
            File.WriteAllText(DATA_PATH, DATA);

        }
        #endregion


        #region Get

        /// <summary>
        /// 해당 레벨 보상을 받았는지를 리턴해주는 함수 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_is_get(int page, int index)
        {
            var _target = (page) * 5;
            // 내 레벨보다 높다면 당연히 안얻었겠지 
            if (_target > target_level)
                return false;

            else
                return is_get[_target+ index];
        }

        public bool Get_reward_not_have()
        {
            for (int i = lower_get_level-1; i < target_level; i++)
            {
                if (!is_get[i])
                {
                    if ((lower_get_level-1) < i)
                        lower_get_level = (i+1);
                    
                    return true;
                }

            }

            lower_get_level = target_level;
            return false;
        }
        /// <summary>
        /// 해당 레벨이 잠겼는지 여부를 판단해주는 함수 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_is_locked(int page, int index)
        {
            var _target = ((page) * 5);
            // 내 레벨보다 높다면 
            if ((_target+(index+1)) > target_level)
                return true;

            else
                return false;
        }

        #endregion

        #region Set

        /// <summary>
        /// 해당 리워드를 획득하면 호출되는 함수 
        /// </summary>
        public void Set_Get_Reward(int page, int index)
        {
            int target_index = ((page) * 5 + index);
            is_get[target_index] = true;
            Write_Data(target_index+1);
        }


        /// <summary>
        /// 레벨업 하면 호출되는 함수 
        /// </summary>
        public void Level_UP()
        {
            is_get.Add(false); // 리스트에 새거 하나 추가
            target_level += 1;
        }
        

        #endregion
    }
    
}
