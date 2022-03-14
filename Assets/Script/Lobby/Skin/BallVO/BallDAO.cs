
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

        /// <summary>
        /// 공을 장착하면 호출되는 함수. cbmm이라는 playerprefs에 해당 변수를 저장한다. 
        /// </summary>
        /// <param name="index"></param>
        public void Set_BallEquipped_Data(int index)
        {
            PlayerPrefs.SetInt("cbmm",index);
        }

        
        /// <summary>
        /// 어떤 공을 장착하고 있는지 리턴해주는 함수. 
        /// </summary>
        /// <param name="index"></param>
        public int Get_BallEquipped_Data()
        {
            return PlayerPrefs.GetInt("cbmm", 0);
        }
        // 장착 데이터 접근 필요 
        #endregion
    }
}
