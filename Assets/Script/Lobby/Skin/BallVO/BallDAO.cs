using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progetile
{
    public class BallDAO 
    {
        #region Data_Access

        public bool Get_BallPur_Data(int index)
        {
            int ball_value = Calc_Index.Get_Ball_Num(index);
            BallPurDAO data = new BallPurDAO(ball_value);
            return data.Get_PurData();
        }

        public void Set_BallPur_Data(int index)
        {
            int ball_value = Calc_Index.Get_Ball_Num(index);
            BallPurDAO data = new BallPurDAO(ball_value);
            data.Purchase();
        }

        public void Set_BallEquipped_Data(int index)
        {
            PlayerPrefs.SetInt("cbmm",index);
        }

        public int Get_BallEquipped_Data()
        {
            return PlayerPrefs.GetInt("cbmm", 0);
        }
        // 장착 데이터 접근 필요 
        #endregion
    }
}
