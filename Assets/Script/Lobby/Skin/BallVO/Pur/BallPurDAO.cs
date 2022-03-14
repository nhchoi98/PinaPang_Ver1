
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
            BallPurVO DATA;

            if (index / 1000 == 2 || index / 1000 == 3)
            {
                var oldNum = Calc_Old_Num(index);
                var DATA_PATH = Application.persistentDataPath + "/Ball/Pur_2/"+ index.ToString() + ".json";
                var OLD_DATA_PATH = Application.persistentDataPath + "/Ball/Pur/"+ oldNum.ToString() + ".json";
                BallPurVO DATA_TEMP; // 옛날 파일의 변수값을 읽어오기 위한 변수 
                
                if (File.Exists(OLD_DATA_PATH)) // 옛날 파일이 있는 경우, 그 정보 그대로 새 파일을 만들어줌. 
                {
                    var json_string = File.ReadAllText(OLD_DATA_PATH);
                    DATA_TEMP = JsonMapper.ToObject<BallPurVO>(json_string);
                    DATA = DATA_TEMP;
                    var DATA_STR = JsonUtility.ToJson(DATA);
                    Directory.CreateDirectory(Application.persistentDataPath + "/Ball/Pur_2");
                    File.WriteAllText(DATA_PATH, DATA_STR);
                    Debug.Log("호출됨" + index.ToString());
                    if(PlayerPrefs.GetInt("cbmm",index) == oldNum)
                        PlayerPrefs.SetInt("cbmm",index); // 장착한 공이 옛날공일 경우, 정보를 바꾸어줌. 
                    try
                    {
                        System.IO.File.Delete(OLD_DATA_PATH); // 옛날 파일 삭제 

                    }

                    catch
                    {
                        
                    }
                }

                
                else if (File.Exists(DATA_PATH))
                {
                    var json_string = File.ReadAllText(DATA_PATH);
                    DATA = JsonMapper.ToObject<BallPurVO>(json_string);
                }
                
                
                else
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Ball/Pur_2");
                    DATA = new BallPurVO();
                    if (index != 0)
                        DATA.is_purchased = false;

                    else
                        DATA.is_purchased = true;
                    var DATA_STR = JsonUtility.ToJson(DATA);
                    File.WriteAllText(DATA_PATH, DATA_STR);
                }
            }

            else
            {
                var DATA_PATH = Application.persistentDataPath + "/Ball/Pur/"+ index.ToString() + ".json";
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
                    File.WriteAllText(DATA_PATH, DATA_STR);
                }
                    
            }

            this.data = DATA;
            this.index = index;
            this.is_purchased = DATA.is_purchased;
        }


        #region Set_Old_Data

        private int Calc_Old_Num(int index)
        {
            switch (index)
            {
                default:
                    return 0;
                
                case 2000:
                    return 3001;
                case 2001:
                    return 3004;
                case 2002:
                    return 3005;
                case 2003:
                    return 3003;
                case 2004:
                    return 3000;
                
                case 2005:
                    return 3002;
                
                case 2006:
                case 2007:
                    return 9999;

                case 3000:
                    return 3006;
                
                case 3001:
                    return 3007;
            
                case 3002:
                    return 3008;
                
                case 3003:
                    return 3009;
                
                case 3004:
                    return 3010;
            }
            
        }
        #endregion
        
        private void Save_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            if (index/ 1000 == 2 || index / 1000 == 3)
            {
                var DATA_PATH = Application.persistentDataPath + "/Ball/Pur_2/" + index.ToString() + ".json";
                File.WriteAllText(DATA_PATH, DATA);
            }

            else
            {
                var DATA_PATH = Application.persistentDataPath + "/Ball/Pur/" + index.ToString() + ".json";
                File.WriteAllText(DATA_PATH, DATA);
            }
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

