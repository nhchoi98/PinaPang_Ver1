
using System.Collections.Generic;
using Badge;
using UnityEngine;

namespace Avatar
{
    /// <summary>
    /// 아바타의 카드와 관련된 내용을 저장하고 쓸 수 있는 클래스 
    /// </summary>
    public class Avatar_Manager 
    {
        private List<AvatarDAO> _avatarDaos = new List<AvatarDAO>();
        private int AVATAR_NUM;
        /// <summary>
        /// 씬 로드시 아바타에 대한 정보를 메모리에 LOAD 
        /// </summary>
        public Avatar_Manager()
        {
            AVATAR_NUM = Calc_Index.Get_Avatar_Max_Index();
            for(int i =0; i<AVATAR_NUM; i++) _avatarDaos.Add(new AvatarDAO(i)); // 아바타에 관련된 정보 삽입 
        }

        #region Skin
        /// <summary>
        /// 아바타의 잠금 조건 달성 여부를 리턴시켜주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_Locked(int index)
        {
            return _avatarDaos[index].Get_Locked_DATA();
        }

        public void Set_Locked(int index)
        {
            _avatarDaos[index].Set_Locked_DATA();
        }
        #endregion
        
        
    }
}
