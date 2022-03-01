
using UnityEngine;
using LitJson;
using System.IO;

namespace Avatar
{
    public class IsLockedDAO
    {
        private IsLockedVO DATA;
        private int index;
        
        /// <summary>
        /// 클래스 생성자 
        /// </summary>
        public IsLockedDAO(int index)
        {
            this.index = index;
            Read_Data();
        }

        /// <summary>
        /// 장착 index를 변경시켜주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Locked_Condition()
        {
            DATA.Is_locked = false;
            Save_Data();
        }
        
        public bool Get_Locked()
        {
            return DATA.Is_locked;
        }
        
        
        #region IO

        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Avatar/locked/" + index.ToString() + ".json";
            IsLockedVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<IsLockedVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Avatar/locked");
                DATA=  new IsLockedVO();
                if (index != 0 ) // 수정 필요 
                    DATA.Is_locked = true;
                
                else
                    DATA.Is_locked = false;
                
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);  
            }
            
            this.DATA = DATA;
        }


        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.DATA);
            var DATA_PATH = Application.persistentDataPath + "/Avatar/locked/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        #endregion

    }
}

