using System.IO;
using Data;
using UnityEngine;
using LitJson; 

namespace Toy
{
    public class SingleToyDAO
    {
        private SingleToyVO data;
        private int index;
        public SingleToyDAO(int index)
        {
            this.index = index;
            Read_Data();

        }

        #region IO
        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Collection/" + index.ToString() + ".json";
            SingleToyVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<SingleToyVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Collection");
                DATA=  new SingleToyVO();
                DATA.count = 0;
                DATA.is_locked = true;
                DATA.is_new = true;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);  
            }

            data = DATA;


        }


        private void Write_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH =Application.persistentDataPath + "/Collection/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);

        }
        #endregion
        
        
        
        public int Get_Toy_Count()
        {
            return data.count;
        }

        public bool Get_IsLocked()
        {
            return data.is_locked;
        }

        public bool Get_IsNew()
        {
            return data.is_new;
        }

        public void Set_Exchange()
        {
            Playerdata_DAO.Set_Player_Gem(data.count/3);
            data.count %= 3;
            Write_Data();
        }

        public bool Can_Exchange()
        {
            if (data.count < 3)
                return false;

            else
                return true;
        }
        public void Set_Toy_Count()
        {
            if (data.is_locked)
            {
                data.is_locked = false;
                data.is_new = true;
            }

            data.count += 1;
            Write_Data();
        }

        public void Set_IsNew()
        {
            data.is_new = false;
            Write_Data();
        }
    }
}
