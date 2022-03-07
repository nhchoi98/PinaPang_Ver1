
using UnityEngine;
using LitJson;
using System.IO;
using System;
using Avatar;
using Badge;
using DG.Tweening.Core;

namespace Data
{
    public class UserStatDAO
    {
        private UserStatVO Data;
        public UserStatDAO() => Read_Data();
        private Set_Badge_Unlocked _badgeUnlocked;
        private EventHandler<Badge_Get_Args> UI_EVENT;

        public UserStatDAO(EventHandler<Badge_Get_Args> eventHandler)
        {
            UI_EVENT += eventHandler;
            _badgeUnlocked = new Set_Badge_Unlocked(UI_EVENT);
            Read_Data();
        }
        #region IO
        private void Read_Data()
        {
            string DATA_PATH = Application.persistentDataPath + "/Info/UserStat.json";
            UserStatVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                string json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<UserStatVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Info");
                DATA=  new UserStatVO();
                DATA.ball_count = 0;
                DATA.item_use = 0;
                DATA.pinata_count = 0;
                DATA.revive_count = 0;
                DATA.score = 0;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }
            Data = DATA;
        }

        private void Write_DATA()
        {
            string DATA_PATH = Application.persistentDataPath + "/Info/UserStat.json";
            string DATA = JsonMapper.ToJson(Data);
            File.WriteAllText(DATA_PATH, DATA);
            // 저장 관련 코드 
        }
        #endregion

        #region Set
        /// <summary>
        /// 게임 오버시 기록을 바탕으로 유저 스탯을 기록해주는 함수 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="pinata"></param>
        public void Set_User_Score(int score)
        {
            if( Data.score<int.MaxValue)
                Data.score += score;
            
            if(_badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.SCORE, Data.score))
                Set_Data();
        }

        public void Set_User_Pinata()
        {
            if(Data.pinata_count<int.MaxValue)
                Data.pinata_count += 1;
            if(_badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.PINATA, Data.pinata_count))
                Set_Data();
            Set_Data();
        }
        

        public void Set_User_ItemCount()
        {
            if(Data.item_use<int.MaxValue)
                Data.item_use++;
            _badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.ITEM, Data.item_use);
            Set_Data();
        }
        
        public void Set_User_Ball_Buy()
        {
            Data.ball_count++;
            _badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.BALL, Data.ball_count);
            Set_Data();
        }

        public void Set_User_Revive()
        {
            Data.revive_count++;
            _badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.REVIVE
                , Data.revive_count);
            Set_Data();
        }

        /// <summary>
        /// 출석부에서 1일차 보상 받으면 뜨는 패널 
        /// </summary>
        public void Set_User_First_Badge_Get()
        {
            _badgeUnlocked.Determine_Unlocked(Set_Badge_Unlocked.Set_Type.ATTENDENCE, 1);
            Set_Data();
        }

        /// <summary>
        /// 데이터 저장 
        /// </summary>
        public void Set_Data()
        {
            Write_DATA();
        }
        
        
        #endregion

        #region Get
        
        public int Get_score()
        {
            return Data.score;
        }

        public int Get_Pinata_Count()
        {
            return Data.pinata_count;
        }

        public int Get_Item_Use()
        {
            return Data.item_use;
        }

        public int Get_Ball()
        {
            return Data.ball_count;
        }

        public int Get_Line()
        {
            return Data.revive_count;
        }

        public int Get_Revive()
        {
            return Data.revive_count;
        }
        #endregion
    }


    /// <summary>
    /// 뱃지의 조건 달성 여부를 판단해주는 class. 
    /// </summary>
    public class Set_Badge_Unlocked
    {
        private Set_Type _setType;
        private BadgeDAO _badgeDao;
        private EventHandler<Badge_Get_Args> _eventHandler;
        public enum Set_Type
        {
            SCORE,
            ITEM,
            BALL,
            REVIVE,
            PINATA,
            ATTENDENCE
        }

        public Set_Badge_Unlocked(EventHandler<Badge_Get_Args> eventHandler)
        {
            _eventHandler = eventHandler;
        }
        public bool Determine_Unlocked(Set_Type setType, int quantity)
        {
            bool event_pos = false;
            int type = 0;
            switch (setType)
            {
                case Set_Type.SCORE:
                    event_pos = Determine_Score_Locked(quantity, ref type);
                    break;

                case Set_Type.ITEM:
                    event_pos = Determine_Item(quantity, ref type);
                    break;
                
                case Set_Type.BALL:
                    event_pos = Determine_Ball(quantity, ref type);
                    break;
                
                case Set_Type.REVIVE:
                    event_pos = Determine_Revive(quantity, ref type);
                    break;
                
                case Set_Type.PINATA:
                    event_pos = Determine_Pinata(quantity, ref type);
                    break;
                
                case Set_Type.ATTENDENCE:
                    event_pos = true;
                    type = 0;
                    _badgeDao = new BadgeDAO(0);
                    if (!_badgeDao.Get_Achi_Data(0,true))
                        _badgeDao.Set_Achi_Data(0);
                    
                    break;
            }

            if (event_pos){
                
                object obj = new object();
                Badge_Get_Args args = new Badge_Get_Args(type);
                _eventHandler.Invoke(obj, args);
            }

            return event_pos;
        }
        
        public bool Determine_Score_Locked(int score, ref int type)
        {

                if (score >= 500000 && score < 500500)
                {
                    _badgeDao = new BadgeDAO(7);
                    if (!_badgeDao.Get_Achi_Data(7,true))
                    {
                        _badgeDao.Set_Achi_Data(7);
                        type = 7;
                        return true;
                    }
                }
                
                else if (score >= 3000000 && score < 3000500)
                {
                    _badgeDao = new BadgeDAO(11);
                    if (!_badgeDao.Get_Achi_Data(11,true))
                    {
                        _badgeDao.Set_Achi_Data(11);
                        type = 11;
                        return true;
                    }
                }
                
            return false;
        }

        public bool Determine_Pinata(int quantity,ref int type)
        {
            if (quantity < 10)
                return false;

            else
            {
                if (quantity == 10)
                {
                    _badgeDao = new BadgeDAO(1);
                    if (!_badgeDao.Get_Achi_Data(1,true))
                    {
                        _badgeDao.Set_Achi_Data(1);
                        type = 1;
                        return true; // 이벤트 발생 
                    }
                }

                else if (quantity == 30)
                {
                    _badgeDao = new BadgeDAO(2);
                    if (!_badgeDao.Get_Achi_Data(2,true))
                    {
                        _badgeDao.Set_Achi_Data(2);
                        type = 2;
                        return true; // 이벤트 발생 
                    }
                    
                }
                
                else if (quantity == 70)
                {
                    _badgeDao = new BadgeDAO(5);
                    if (!_badgeDao.Get_Achi_Data(5,true))
                    {
                        _badgeDao.Set_Achi_Data(5);
                        type = 5;
                        return true; // 이벤트 발생 
                    }
                    
                }
            }

            return false;
        }
        

        public bool Determine_Item(int quantity,ref int type)
        {
            if (quantity < 10)
                return false;

            else
            {
                if (quantity == 10)
                {
                    _badgeDao = new BadgeDAO(4);
                    if (!_badgeDao.Get_Achi_Data(4,true))
                    {
                        _badgeDao.Set_Achi_Data(4);
                        type = 4;
                        return true; // 이벤트 발생 
                    }
                }

                else if (quantity == 20)
                {
                    _badgeDao = new BadgeDAO(8);
                    if (!_badgeDao.Get_Achi_Data(8,true))
                    {
                        _badgeDao.Set_Achi_Data(8);
                        type = 8;
                        return true; // 이벤트 발생 
                    }
                    
                }
            }

            return false;
        }

        public bool Determine_Ball(int quantity,ref int type)
        {
            if (quantity < 10)
                return false;

            else
            {
                if (quantity == 10)
                {
                    _badgeDao = new BadgeDAO(3);
                    if (!_badgeDao.Get_Achi_Data(3,true))
                    {
                        _badgeDao.Set_Achi_Data(3);
                        type = 3;
                        return true; // 이벤트 발생 
                    }
                }

                else if (quantity == 20)
                {
                    _badgeDao = new BadgeDAO(6);
                    if (!_badgeDao.Get_Achi_Data(6,true))
                    {
                        _badgeDao.Set_Achi_Data(6);
                        type = 6;
                        return true; // 이벤트 발생 
                    }
                }

                else if (quantity == 30)
                {
                    _badgeDao = new BadgeDAO(9);
                    if (!_badgeDao.Get_Achi_Data(9,true))
                    {
                        _badgeDao.Set_Achi_Data(9);
                        type = 9;
                        return true; // 이벤트 발생 
                    }
                }
            }

            return false;

        }

        public bool Determine_Revive(int quantity,ref int type)
        {
            if (quantity < 30)
                return false;

            else
            {
                if (quantity == 30)
                {
                    _badgeDao = new BadgeDAO(10);
                    if (!_badgeDao.Get_Achi_Data(10,true))
                    {
                        _badgeDao.Set_Achi_Data(10);
                        type = 10;
                        return true; // 이벤트 발생 
                    }
                }
            }

            return false;
        }
        
    }

    /// <summary>
    /// 획득 시, 어떤 뱃지를 획득하였는가에 대한 정보를 담고 있음 
    /// </summary>
    public class Badge_Get_Args: EventArgs
    {
        public int type;

        public Badge_Get_Args(int type)
        {
            this.type = type;
        }
    }
}
