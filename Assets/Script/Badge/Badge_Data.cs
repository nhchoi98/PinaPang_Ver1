
using UnityEngine;
using System;
using Challenge;
using Data;

namespace Badge
{
    public class Badge_Data : MonoBehaviour
    {
        [Header("Ability_DATA")] private bool[] achi_data; // 뱃지 획득 유무를 저장해주는 변수 
        private int MAX_INDEX;
        Calc_Badge_Index index_data = new Calc_Badge_Index();
        private UserStatDAO userstat;

        [SerializeField] private QuestManager _questManager;
        private event EventHandler<Badge_Get_Args> badge_event;
        private void Awake()
        {
            badge_event += _questManager.Set_Badge_UI;
            badge_event += Set_Badge_Unlocked;
            userstat = new UserStatDAO(badge_event);
            Init_Data();
        }
        
        /// <summary>
        /// 뱃지 데이터를 초기화해주는 함수 
        /// </summary>
        private void Init_Data()
        {
            MAX_INDEX = index_data.Get_Badge_MaxIndex();
            achi_data = new bool[MAX_INDEX];

            // 1. 능력치 정보를 상단 UI에 반영해줌 
            // 2. 뱃지 열림 유무에 따라서 회색처리를 할 지 말지 결정해줌 
            BadgeDAO badge_data = new BadgeDAO();
            for (int i = 0; i < MAX_INDEX; i++)
                achi_data[i] = badge_data.Get_Achi_Data(i,false); // 뱃지 달성 유무 저장 
            
        }

        public bool Get_achi_data(int index)
        {
            return achi_data[index];
        }
        
        //Stat의 데이터를 참조할 수 있도록 해줌 
        #region Stat_Get
        public int Get_score()
        {
            return userstat.Get_score();
        }

        public int Get_PinataCount()
        {
            return userstat.Get_Pinata_Count();
        }

        public int Get_Item_Use()
        {
            return userstat.Get_Item_Use();
        }

        public int Get_Ball()
        {
            return userstat.Get_Ball();
        }

        public int Get_Line()
        {
            return userstat.Get_Line();
        }

        public int Get_Revive()
        {
            return userstat.Get_Revive();
        }
        

        #endregion
        
        // 공 구입시 Stat 데이터 업데이트 할 수 있도록함 
        #region Stat_Set

        /// <summary>
        /// 공 구입시 호출되는 함수 
        /// </summary>
        public void Set_Ball_Buy()
        {
            userstat.Set_User_Ball_Buy();
        }

        public void Purchase_FreeItem_Package()
        {
            userstat.Set_FreeItem_Package();
        }

        public void Set_First_Attendance()
        {
            object obj = new object();
            Badge_Get_Args args = new Badge_Get_Args(0);
            Set_Badge_Unlocked(obj, args);
        }


        private void Set_Badge_Unlocked(object obj, Badge_Get_Args eventArgs)
        {
            achi_data[index_data.Get_Badge_index(eventArgs.type)] = true;
        }
        #endregion
        

    }
}
