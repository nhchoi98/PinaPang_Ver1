using System.IO;
using UnityEngine;
using LitJson;

namespace Toy
{
    /// <summary>
    /// 더 이상 사용하지 않는 클래스 
    /// </summary>
    public class ToyDAO
    {
        private ToyVO data;
        public ToyDAO()
        {
            Read_Data();
        }

        public int Get_count()
        {
            return data.count;
        }

        public void Set_Count(int count)
        {
            data.count += count;
            Set_Level();
            Set_Data();
        }

        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Toy/data.json";
            var PRE_DATA_PATH = "Toy/data";
            ToyVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<ToyVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Toy");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<ToyVO>(_DATA);
            }

            data = DATA;
        }

        private void Set_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH =Application.persistentDataPath + "/Toy/data.json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }

        /// <summary>
        /// 레벨을 설정해주는 함수 
        /// </summary>
        private void Set_Level()
        {
            int target = Get_Target();

            if (data.level == 11)
                return;

            if (target <= data.count)
            {
                data.level += 1;
            }
            
        }


        public int Get_Target()
        {
            int target = 0;
            switch (data.level)
            {
                case 1:
                    target = 7;
                    break;
                
                case 2:
                    target = 16;
                    break;
                
                case 3:
                    target = 29;
                    break;
                
                case 4:
                    target = 44;
                    break;
                
                case 5:
                    target = 64;
                    break;
                
                case 6:
                    target = 89;
                    break;
                
                case 7:
                    target = 120;
                    break;
                
                case 8:
                    target = 158;
                    break;
                
                case 9:
                    target = 202;
                    break;
                
                case 10:
                    target = 262;
                    break;
            }

            return target;
        }

        public int Get_Previous_Target()
        {
            int target = 0;
            switch (data.level)
            {
                case 1:
                    target = 0;
                    break;
                
                case 2:
                    target = 7;
                    break;
                
                case 3:
                    target = 16;
                    break;
                
                case 4:
                    target = 29;
                    break;
                
                case 5:
                    target = 44;
                    break;
                
                case 6:
                    target = 64;
                    break;
                
                case 7:
                    target = 89;
                    break;
                
                case 8:
                    target = 120;
                    break;
                
                case 9:
                    target = 158;
                    break;
                
                case 10:
                    target = 205;
                    break;
            }

            return target;
            
            
        }
        
        
        public int Get_Interval()
        {
            int target = 0;
            switch (data.level)
            {
                case 1:
                    target = 7;
                    break;
                
                case 2:
                    target = 9;
                    break;
                
                case 3:
                    target = 12;
                    break;
                
                case 4:
                    target = 16;
                    break;
                
                case 5:
                    target = 20;
                    break;
                
                case 6:
                    target = 25;
                    break;
                
                case 7:
                    target = 31;
                    break;
                
                case 8:
                    target = 38;
                    break;
                
                case 9:
                    target = 47;
                    break;
                
                case 10:
                    target = 57;
                    break;
            }

            return target;
            
        }
        

        public int Get_Level()
        {
            return data.level;
        }

    }
}

