
using UnityEngine;

namespace Challenge
{
    /// <summary>
    /// 레벨별 타깃 경험치를 리턴해주는 함수. 레벨을 얼마 올릴지도 여기서 연산해준다. 
    /// </summary>
    public class EXP_Target
    {
        public bool Get_User_Target(ref int level, ref int exp)
        {
            var exp_const =Get_User_Target(level);
            // 레벨업 기준 아닐경우 리턴 
            if (exp- exp_const< 0 ||  level >44)
                return false;
            
            exp -= exp_const;
            level += 1;
            // 아직 더 연산할 수 있으면, 재귀 호출하기.
            exp_const = Get_User_Target(level);
            if (exp - exp_const >= 0 && level < 45)
                Get_User_Target(ref level, ref exp);

            return true;
        }
        
        public int Get_User_Target(int level)
        {
            var mod = ((level-1) / 5);
            switch (mod)
            {
                default:
                    return 1;
                
                case 0:
                    return 50;
                
                case 1:
                    return 75;
                
                case 2:
                    return 100;
                
                case 3:
                    return 125;
                
                case 4:
                    return 150;
                
                case 5:
                    return 175;
                
                case 6:
                    return 200;
                
                case 7:
                    return 225;
                
                case 8:
                    return 250;
            }
        }
    }
}
