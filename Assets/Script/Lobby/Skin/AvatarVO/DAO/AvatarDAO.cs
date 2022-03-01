using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avatar
{
    public class AvatarDAO
    {
        private IsLockedDAO _DATA_LOCKED;

        /// <summary>
        /// 상점에서 호출되는 DAO 
        /// </summary>
        public AvatarDAO(int index)
        {
            _DATA_LOCKED = new IsLockedDAO(index); 
        }

        
        public AvatarDAO(int index, int num)
        {
            Debug.Log(index.ToString());
            _DATA_LOCKED = new IsLockedDAO(index);
        }

        #region Locked
        public bool Get_Locked_DATA()
        {
            return _DATA_LOCKED.Get_Locked();
        }
        
        /// <summary>
        /// 해금시 호출되는 함수 
        /// </summary>
        public void Set_Locked_DATA()
        {
            _DATA_LOCKED.Set_Locked_Condition();
        }
        #endregion
        
        
    }
}
