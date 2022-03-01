using System.Collections.Generic;
using System.Collections;

namespace Challenge
{
    /// <summary>
    /// 등급을 판단해주고, 이를 토대로 슬롯을 리턴해주는 함수를 가짐. 
    /// </summary>
    public static class Challenge_Slot 
    {
        /// <summary>
        /// 사용자의 랭크를 계산해 값을 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public static int Calc_Rank()
        {
            EXP_DAO expDao = new EXP_DAO();
            int level = expDao.Get_User_Level();
            int mode = level / 10; // 티어를 계산해줌 
            return mode;

        }

        /// <summary>
        /// return 0: 브론즈 아이템, 1: 실버 아이템, 2: 골드 아이템 
        /// </summary>
        /// <param name="which_rank"></param>
        /// <returns></returns>
        public static int Set_Second_Slot(int which_rank)
        {
            int rand = UnityEngine.Random.Range(0,100);
            int return_value;
            // 브론즈 티어일 경우 
            if (which_rank < 10)
            {
                if (rand < 75) // 브론즈, 실버중 하나 
                    return 0; // 브론즈 슬롯 하나 

                else
                    return 1;

            }
            
            else
            {
                if (rand < 75)
                    return 1;

                else
                    return 2;
            }
        }

        /// <summary>
        /// 세 번째 슬롯에서 어떤 랭크의 아이템을 추가시킬지 지정해주는 함수 
        /// </summary>
        /// <param name="which_rank"></param>
        /// <returns></returns>
        public static int Set_Third_Slot(int which_rank)
        {
            int rand = UnityEngine.Random.Range(0,100);
            int return_value;
            
            // 브론즈 티어일 경우 
            if (which_rank < 4)
            {
                if (rand < 50) // 브론즈, 실버중 하나 
                    return 0; // 브론즈 슬롯 하나 

                else
                    return 1;

            }
            
            // 실버 티어인경우 
            else if (which_rank >3 && which_rank<10)
            {
                if (rand < 25)
                    return 0;

                else if(rand >= 25 && rand <75)
                    return 1;
                
                else
                    return 2;
            }

            else
            {
                if (rand < 50)
                    return 1;
                
                else 
                    return 2;
            }
        }


        public static int Set_Forth_Slot(int which_rank)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            int return_value;

            // 브론즈 티어일 경우 
            if (which_rank < 4)
                return 1;

            // 실버 티어인경우 
            else if (which_rank > 3 && which_rank < 10)
            {
                if (rand < 75)
                    return 1;

                else
                    return 2;
            }

            else
            {
                if (rand < 50)
                    return 1;

                else
                    return 2;
            }
        }
        
        public static int Set_Fifth_Slot(int which_rank)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            int return_value;

            // 브론즈 티어일 경우 
            if (which_rank < 4)
            {
                if(rand<75)
                    return 1;

                else
                    return 2;
                

            }

            // 실버 티어인경우 
            else if (which_rank > 3 && which_rank < 10)
            {
                if (rand < 50)
                    return 1;

                else
                    return 2;
            }

            else
            {
                if (rand < 25)
                    return 1;

                else
                    return 2;
            }
        }
    }
}
