
using UnityEngine;
using LitJson;
using System.IO;

namespace Data
{   
    
    /// <summary>
    /// USER의 재화 데이터를 읽고 쓸 수 있는 CLASS 
    /// </summary>
    public static class Playerdata_DAO 
    {
        private static Player_InfoVO Player_info;
        // 로비에서 한 번에 재화 데이터 바꾸도록 이벤트 리스너 갖다 붙히기 

        /// <summary>
        /// 로딩시 재화 데이터를 불러오는 class 
        /// </summary>
        public static void Init_Data()
        {
            Read_Data();
        }
        
        #region IO
        private static void Read_Data()
        {
            string DATA_PATH = Application.persistentDataPath + "/Info/Player_info.json";
            Player_InfoVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                string json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<Player_InfoVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Info");
                DATA= new Player_InfoVO();
                DATA.Candy = 0;
                DATA.Gem = 0;
                DATA.Best_Score = 0;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }

            Player_info = DATA;
        }

        private static void Write_DATA()
        {
            string DATA_PATH = Application.persistentDataPath + "/Info/Player_info.json";
            string DATA = JsonMapper.ToJson(Player_info);
            File.WriteAllText(DATA_PATH, DATA);
            // 저장 관련 코드 
        }
        #endregion
        
        public static int Player_Gem()
        {
            return Player_info.Gem;
        }

        /// <summary>
        /// 플레이어의 캔디 개수를 리턴함 
        /// </summary>
        /// <returns></returns>
        public static int Player_Candy()
        {
            return Player_info.Candy;
        }

        /// <summary>
        /// 플레이어의 베스트 스코어를 리턴함 
        /// </summary>
        /// <returns></returns>
        public static int Player_BestScore()
        {
            return Player_info.Best_Score;
        }
        
        #region Set_Player_Info

        /// <summary>
        /// 사용자가 취득한 코인 데이터를 반영해줌 
        /// </summary>
        /// <param name="Coin"></param>
        /// <summary>
        /// 사용자가 취득한 캔디 데이터를 반영해줌 
        /// </summary>
        /// <param name="Candy"></param>
        public static void Set_Player_Candy(int Candy)
        {
            Player_info.Candy += Candy;
            Write_DATA();
        }

        public static void Set_Player_Gem(int Gem)
        {
            Player_info.Gem += Gem;
            Write_DATA();
        }

        /// <summary>
        ///  베스트 스코어가 갱신되었을 경우, 해당 데이터를 반영해줌 
        /// </summary>
        /// <param name="Best_Score"></param>
        public static void Set_Best_Score(int Best_Score)
        {
            Player_info.Best_Score = Best_Score;
            Write_DATA();
        } 
        
        /// <summary>
        /// 데이터 저장시 호출되는 class 
        /// </summary>
  
        
        #endregion
    }
}
