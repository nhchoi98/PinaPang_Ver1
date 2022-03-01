using System;
using UnityEngine;
using System.IO;
using LitJson;


namespace Block
{
    /// <summary>
    /// JSON 형태로 저장되어있는 확률 데이터를 읽어오는 클래스. 
    /// 각각 함수들은 이 객체의 형태로 데이터를 리턴하게 된다. 
    /// </summary>
    public class Probability_Set
    {
        private Probability_VO probability_set;

        public Probability_VO _probabilty_Set(int Wave)
        {
            Set_Probability(Wave);
            return probability_set;
        }

        private void Set_Probability(int wave)
        {
            switch (wave)
            {
                default:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 0; // 2
                    probability_set.Three_Target = 0; // 3
                    probability_set.Four_Target = 2; // 4
                    probability_set.Fifth_Target = 3; // 5
                    probability_set.Sixth_Target = 3; // 6
                    probability_set.Seventh_Target = 2; // 7
                    break;
                
                case 0:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 1; // 1 
                    probability_set.Two_Target = 2; // 2
                    probability_set.Three_Target = 4; // 3
                    probability_set.Four_Target = 2; // 4
                    probability_set.Fifth_Target = 1; // 5
                    probability_set.Sixth_Target = 0; // 6
                    probability_set.Seventh_Target = 0; // 7
                    break;
                
                case 1:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 2; // 2
                    probability_set.Three_Target = 3; // 3
                    probability_set.Four_Target = 4; // 4
                    probability_set.Fifth_Target = 1; // 5
                    probability_set.Sixth_Target = 0; // 6
                    probability_set.Seventh_Target = 0; // 7
                    break;
                
                case 2:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 1; // 2
                    probability_set.Three_Target = 3; // 3
                    probability_set.Four_Target = 3; // 4
                    probability_set.Fifth_Target = 2; // 5
                    probability_set.Sixth_Target = 1; // 6
                    probability_set.Seventh_Target = 0; // 7
                    break;
                
                case 3:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 1; // 2
                    probability_set.Three_Target = 1; // 3
                    probability_set.Four_Target = 3; // 4
                    probability_set.Fifth_Target = 3; // 5
                    probability_set.Sixth_Target = 2; // 6
                    probability_set.Seventh_Target = 0; // 7
                    break;
                
                case 4:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 1; // 2
                    probability_set.Three_Target = 1; // 3
                    probability_set.Four_Target = 3; // 4
                    probability_set.Fifth_Target = 4; // 5
                    probability_set.Sixth_Target = 2; // 6
                    probability_set.Seventh_Target = 0; // 7
                    break;
                
                case 5:
                case 6:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 0; // 2
                    probability_set.Three_Target = 1; // 3
                    probability_set.Four_Target = 3; // 4
                    probability_set.Fifth_Target = 3; // 5
                    probability_set.Sixth_Target = 2; // 6
                    probability_set.Seventh_Target = 1; // 7
                    break;
                
                
                case 7:
                case 8:
                case 9:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 0; // 2
                    probability_set.Three_Target = 1; // 3
                    probability_set.Four_Target = 2; // 4
                    probability_set.Fifth_Target = 3; // 5
                    probability_set.Sixth_Target = 3; // 6
                    probability_set.Seventh_Target = 1; // 7
                    break;
                
                case 10:
                case 11:
                case 12:
                case 13:
                    probability_set = new Probability_VO();
                    probability_set.One_Target = 0; // 1 
                    probability_set.Two_Target = 0; // 2
                    probability_set.Three_Target = 1; // 3
                    probability_set.Four_Target = 1; // 4
                    probability_set.Fifth_Target = 3; // 5
                    probability_set.Sixth_Target = 3; // 6
                    probability_set.Seventh_Target = 2; // 7
                    break;
                
            }   
        }


    }
}