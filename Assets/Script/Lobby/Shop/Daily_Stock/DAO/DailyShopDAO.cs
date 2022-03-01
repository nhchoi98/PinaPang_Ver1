using LitJson;
using System.IO;
using UnityEngine;

namespace shop
{
    public class DailyShopDAO
    {
        private DailyShopVO data;

        public DailyShopDAO()
        {
            Read_Data();
        }
        
        private void Read_Data()
        {
            DailyShopVO data;
            var DATA_PATH = Application.persistentDataPath + "/shop/daily/info.json";
            var PRE_DATA_PATH = "Lobby/Shop/daily/info" ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonMapper.ToObject<DailyShopVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/shop/daily");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                data = JsonMapper.ToObject<DailyShopVO>(_DATA);
            }
            this.data = data;
        }

        /// <summary>
        /// 데이터 저장 
        /// </summary>
        private void Set_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/shop/daily/info.json";
            File.WriteAllText(DATA_PATH,DATA);
        }


        /// <summary>
        /// 당일에 해당 아이템을 획득한 카운트를 리턴해주는 함수 
        /// </summary>
        /// <param name="which_count"></param>
        public int Get_Count(int which_count)
        {
            switch (which_count)
            {
                default:
                    return data.ad_count_1;
                
                case 1:
                    return data.ad_count_2;
                
                case 2:
                    return data.ad_count_3;

            }
        }

        /// <summary>
        /// 광고를 시청하면 카운트를 내려주는 함수 
        /// </summary>
        /// <param name="which_count"></param>
        public bool Set_Count(int which_count)
        {
            switch (which_count)
            {
                case 0:
                    data.ad_count_1 -= 1;
                    break;
                
                case 1:
                    data.ad_count_2 -= 1;
                    break;
                
                case 2:
                    data.ad_count_3 -= 1;
                    break;
           
            }
            Set_Data();
            switch (which_count)
            {
                case 0:
                    if (data.ad_count_1 == 0)
                        return false;
                    break;
                
                case 1:
                    if (data.ad_count_2 == 0)
                        return false;
                    break;
                
                case 2:
                    if (data.ad_count_3 == 0)
                        return false;
                    break;
           
            }

            return true;
        }

        public void Reset()
        {
            data.ad_count_1 = 3;
            data.ad_count_2 = 2;
            data.ad_count_3 = 1;
            Set_Data();

        }
    }
}
