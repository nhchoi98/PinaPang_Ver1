using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public static class Ball_Price
    {
        public static int Price(int index)
        {
            int ball_num = Calc_Index.Get_Ball_Num(index);
            switch (ball_num/1000)
            {
                default:
                    return ad_ball();
                
                case 1:// 그냥 구매해서 사는 공 
                    return Normal_Ball(ball_num % 1000);
                
                case 2:
                    return ad_ball();
                
                case 3:
                    return Level_Ball((ball_num % 3000));
                    
            }
        }

        private static int ad_ball()
        {
            return 40;
        }

        private static int Normal_Ball(int index)
        {
            if (index < 8 )
                return 10;

            else 
                return 40;
        }

        private static int Level_Ball(int index)
        {
            switch (index)
            {
                default:
                    return 6;
                case 1:
                    return 16;
                case 2:
                    return 26;
                case 3:
                    return 36;
                case 4:
                    return 41;
            }
        }
    }
}
