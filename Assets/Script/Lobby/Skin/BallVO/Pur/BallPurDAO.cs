
using UnityEngine;
using System.IO;
using LitJson;


namespace Progetile
{
    /// <summary>
    /// 공의 구매정보를 조정하고 조회할 수  있는 Ball Pur DAO.
    /// </summary>
    
    public class BallPurDAO 
    {
        private bool is_purchased;
        private BallPurVO data;
        private int index;
        public BallPurDAO(int index) =>Read_Data(index);

        #region IO
        private void Read_Data(int index)
        {
            var DATA_PATH = Application.persistentDataPath + "/Ball/Pur/"+ index.ToString() + ".json";
            BallPurVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<BallPurVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ball/Pur");
                DATA = new BallPurVO();
                if (index != 0)
                    DATA.is_purchased = false;

                else
                    DATA.is_purchased = true;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR); 
            }

            this.data = DATA;
            this.index = index;
            this.is_purchased = DATA.is_purchased;
        }
        
        
        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH =  Application.persistentDataPath + "/Ball/Pur/"+ index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        
        #endregion

        #region Data_Access

        public void Purchase()
        {
            data.is_purchased = true;
            Save_Data();
        }

        public bool Get_PurData()
        {
            return is_purchased;
        }
        
        
        #endregion
        
    }
}

