
using System.Collections.Generic;
using UnityEngine;
using Avatar;

namespace Lobby
{
    public class Avatar_Lobby_Info : MonoBehaviour
    {
        [Header("Avatar_Data")]
        private List<bool> islocked;
        private IsLockedDAO data;
        private int MAXINDEX;
        
        [Header("Gacha_Data")]
        private bool gacha_flag;
        private bool avatar_alarm;
        
        
        private void Awake()
        {
            MAXINDEX = Calc_Index.Get_Avatar_Max_Index();
            // Step 1. 잠금 데이터 초기화, 읽어오기 
            islocked = new List<bool>();
            for (int i = 0; i < MAXINDEX; i++)
            {
                data = new IsLockedDAO(Calc_Index.Get_Avatar_Num(i));
                islocked.Add((data.Get_Locked()));
            }
        }

        #region Get
        /// <summary>
        /// 아바타 해금 여부를 리턴해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_Locked(int index)
        {
            return islocked[index];
        }

        #endregion

        #region Set
        public void Buy_Avatar(int index)
        {
            islocked[index] = false;
            IsLockedDAO data = new IsLockedDAO(Calc_Index.Get_Avatar_Num(index));
            data.Set_Locked_Condition();
            // 데이터에 반영해줌 
        }
        #endregion
    }
}
