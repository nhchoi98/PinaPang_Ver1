using System;
using System.IO;
using Data;
using LitJson;
using UnityEngine;
using UnityEngine.AI;


namespace Battery
{
    public class BatteryDAO
    {
        private BatteryVO data;
        private const int full_count = 30;
        private const int game_discharge = 8;
        public BatteryDAO()
        {
             Read_Data();
        }

        #region IO

        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Battery/data.json";
            BatteryVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<BatteryVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Battery");
                DATA  = new BatteryVO();
                DATA.count = 30;
                DATA.accessTime = DateTime.UtcNow;
                DATA.adCount = 5;
                DATA.gemCount = 10;
                var DATA_STR = JsonMapper.ToJson(DATA);
                Debug.Log(DATA_STR);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }
            
            data = DATA;
        }


        private void Write_Data()
        {
            var DATA = JsonMapper.ToJson(data);
            var DATA_PATH= Application.persistentDataPath + "/Battery/data.json";
            File.WriteAllText(DATA_PATH,DATA);   
            
        }
        #endregion
        

        public void Reset_Count()
        {
            data.adCount = 5;
            data.gemCount = 10;
            Write_Data();
        }

        public void Ingame_Start()
        {
            var prevCount = data.count;
            data.count -= game_discharge;
            if(prevCount>29 && data.count<30)
                data.accessTime = DateTime.UtcNow;
            
            Write_Data();
        }

        public bool Is_Playable()
        {
            if (data.count < game_discharge)
                return false;

            else
                return true;
            
        }
        // 여기서 부터는 상품에 의한 충전 
        #region Charge

        public void Ad_Charge()
        {
            data.count += 10;
            data.adCount -= 1;
            Write_Data();
        }

        public void Gem_Charge()
        {
            data.count += full_count;
            data.gemCount -= 1;
            Playerdata_DAO.Set_Player_Gem(-30);
            Write_Data();
        }
        #endregion

        #region Charge_Count

        public int Get_Ad_Count()
        {
            return data.adCount;
        }
        

        public int Get_GemCount()
        {
            return data.gemCount;
        }
        

        #endregion
        public int Get_Count()
        {
            return data.count;
        }

        #region Periodic_Charge
        public void Loading_Charge()
        {
            TimeSpan delta = DateTime.UtcNow - data.accessTime;
            
            if (delta < TimeSpan.FromMinutes(6))
                return;
            
            //  기본 차지 할 필요 없다면 
            if (data.count > 29)
            {
                while (true)
                {
                    data.accessTime += TimeSpan.FromMinutes(6);
                    delta = DateTime.UtcNow - data.accessTime;
                    if (delta < TimeSpan.FromMinutes(6))
                        break;
                    
                    if (data.adCount < 5)
                        data.adCount += 1;

                    if (data.gemCount < 10)
                        data.gemCount += 1;
                    
                    if (data.adCount == 5 && data.gemCount == 10 )
                        break;
                }
                Write_Data();
            }

            else // 충전 해야 한다면 
            {
                while (true)
                {
                    data.accessTime += TimeSpan.FromMinutes(6);
                    delta = DateTime.UtcNow - data.accessTime;
                    if (delta < TimeSpan.FromMinutes(6))
                        break;
                    
                    if (data.count < 30)
                        data.count += 1;

                    if (data.adCount < 5)
                        data.adCount += 1;

                    if (data.gemCount < 10)
                        data.gemCount += 1;

                    if (data.adCount == 5 && data.gemCount == 10 && data.count > 29)
                    {
                        data.accessTime = DateTime.UtcNow;
                        break;
                    }
                }
                // 시간 차이에 따른 충전량 계산 
                // 가장 마지막 충전량을 기준으로 access time 재작성 
                Write_Data();
            }
        }

        /// <summary>
        /// 로비에서 가만히 있을 때 충전. 타이머가 시간이 되면 알아서 채워준다. 
        /// </summary>
        public void Lobby_Charge()
        {
            if (data.count < 30)
                data.count += 1;

            if (data.adCount < 5)
                data.adCount += 1;

            if (data.gemCount < 10)
                data.gemCount += 1;
            
            data.accessTime = DateTime.UtcNow; // 타깃 시간 업데이트 
            Write_Data();
        }

        /// <summary>
        /// 얼마후에 재충전되는지 Target time을 리턴해줌 
        /// </summary>
        /// <returns></returns>
        public TimeSpan Get_TargetTime()
        {
            TimeSpan target = (data.accessTime.TimeOfDay +  TimeSpan.FromMinutes(6));
            return target;
        }
        #endregion
        
        
        
        
    }
}
