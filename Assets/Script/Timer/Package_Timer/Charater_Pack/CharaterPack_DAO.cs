using System;
using System.Collections.Generic;
using System.IO;
using Avatar;
using UnityEngine;
using LitJson;

namespace Timer
{
    public class CharaterPack_DAO 
    {
        private Determine_StarterVO data;
        public CharaterPack_DAO ()
        {
            Read_Data();
        }

        private void Set_DateTime()
        {
            data.targetTime = DateTime.UtcNow + TimeSpan.FromHours(2);
        }

        #region IO
        private void Read_Data()
        {
            Determine_StarterVO data;
            var DATA_PATH = Application.persistentDataPath + "/Package/Charater/data.json";
            var PRE_DATA_PATH = "Package/Charater/data" ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonMapper.ToObject<Determine_StarterVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Package/Charater");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                data = JsonMapper.ToObject<Determine_StarterVO>(_DATA);
            }

            this.data = data;
        }

        private void Write_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Package/Charater/data.json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        #endregion

        #region Set_Data
        public void Determine_DateTime(bool is_reset = false)
        {
            var delta_target = data.targetTime.Subtract(DateTime.UtcNow);
            if (delta_target < TimeSpan.FromSeconds(1))
            {
                Set_DateTime();
                Set_Which_Charater();
            }

            if (is_reset)
            {
                Set_DateTime();
                Set_Which_Charater();
            }
        }

        public bool Can_Popup_Open()
        {
            List<int> targetNum = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                if(!Is_Charater_Purchased(i))
                    targetNum.Add(i);
            }

            if (targetNum.Count != 0)
                return true;

            else
                return false;
            
        }
        
        /// <summary>
        /// 로딩시 기간이 남았는지 안남았는지를 판단해주는 함수 
        /// </summary>
        private void Set_Which_Charater()
        {
            List<int> targetNum = new List<int>();
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if(!Is_Charater_Purchased(i))
                    targetNum.Add(i);
            }

            if (targetNum.Count == 0)
            {
                data.which_charater = 99;
                Write_Data();
                return;
            }

            else
            {
                Set_Target_Charater(ref targetNum , ref count);
                Write_Data();
            }
        }
        
        private bool Is_Charater_Purchased(int index)
        {
            IsLockedDAO lockedDao;
            switch (index)
            {
                default:
                    throw new Exception();
                
                // 과학자 패키지를 사용자가 구매한 경우. 
                case 3:
                    lockedDao = new IsLockedDAO(13);
                    break;

                // 파티광 
                case 0:
                    lockedDao = new IsLockedDAO(1006);
                    break;
                
                // 우주비행사 
                case 1:
                    lockedDao = new IsLockedDAO(2000);
                    break;
                
                case 2:
                    lockedDao = new IsLockedDAO(2002);
                    break;
                
            }
            
            if (!lockedDao.Get_Locked())
                return true;

            else
                return false;
        }

        /// <summary>
        /// 다음에 어떤거 세일해줄지 결정해주는 함수 
        /// </summary>
        private void Set_Target_Charater(ref List<int> target, ref int count)
        {               
            if (data.which_charater < 4)
            {
                data.which_charater += 1;
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == data.which_charater)
                    {
                        return;
                    }
                }
                ++count;
                if(count !=4)
                    Set_Target_Charater(ref target, ref count);
                
                else
                    data.which_charater = 99;
            }

            else
            {
                data.which_charater = 0;
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == data.which_charater)
                        return;
                }

                ++count;
                if(count !=4)
                    Set_Target_Charater(ref target, ref count);

                else
                {
                    data.which_charater = 99;
                }
            }
        }
        
        public int Get_Which_Charater()
        {
            return data.which_charater;
        }
        
        public void Set_is_first()
        {
            data.is_first = false;
            Set_Which_Charater();
            // 타깃 설정하기 
            Set_DateTime();
            Write_Data();
        }
        #endregion

        public bool Get_is_first()
        {
            return data.is_first;
        }

        public DateTime Get_TargetTime()
        {
            return data.targetTime;
        }
        
    }
}
