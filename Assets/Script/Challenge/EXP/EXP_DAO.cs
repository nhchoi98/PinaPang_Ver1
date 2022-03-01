
using UnityEngine;
using LitJson;
using System.IO;

namespace Challenge
{
    public class EXP_DAO 
    {
        private EXP_VO data; // 경험치를 저장해놓는 class

        public EXP_DAO() => Read_Exp_Data(); // EXP_VO에 맞게 데이터를 읽어오는 함수 호출 
        

        #region IO

        /// <summary>
        /// exp 데이터를 읽어오는 함수 
        /// </summary>
        private void Read_Exp_Data()
        {
             var DATA_PATH = Application.persistentDataPath + "/Challenge/exp/data.json";
             var PRE_DATA_PATH = "Challenge/exp/data";
             EXP_VO DATA = null;
             if (File.Exists(DATA_PATH))
             {
                    var json_string = File.ReadAllText(DATA_PATH);
                    DATA = JsonMapper.ToObject<EXP_VO>(json_string);
             }

             else
             {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Challenge/exp");
                    TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                    var _DATA = DATA_ASSET.ToString();
                    File.WriteAllText(DATA_PATH, _DATA);
                    DATA = JsonMapper.ToObject<EXP_VO>(_DATA);
             }
            
             this.data = DATA;
                
        }
        
        /// <summary>
        /// exp data를 update 해주는 함수 
        /// </summary>
        private void Write_Exp_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Challenge/exp/data.json";
            File.WriteAllText(DATA_PATH,DATA);
        
        }

        #endregion

        #region Get

        /// <summary>
        /// 유저의 경험치를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public int Get_User_EXP()
        {
            return data.exp;
        }

        /// <summary>
        /// 유저의 레벨을 리턴해주는 함수 
        /// </summary>
        ///  <returns></returns>
        public int Get_User_Level()
        {
            return data.level;
        }

        #endregion

        #region Set

        /// <summary>
        /// 보상을 획득하면, 유저의 경험치를 조절해주는 함수 . return값으로 레벨업 유무를 보낸다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Set_User_Exp(int index)
        {
            bool return_value;
            var target_data = new EXP_Target();
            var exp_return = new EXP_List();
            if(data.exp<int.MaxValue)
                data.exp += exp_return.Target_return(index);
            // 인덱스에 따라서 얼마의 경험치를 더 얹어줄 지 결정함 
            if (target_data.Get_User_Target(ref data.level, ref data.exp))
                return_value = true; // 레벨업 함

            else
                return_value = false;

            Write_Exp_Data(); // 데이터 저장 
            return return_value;
        }
        #endregion
    }
}
