using System.Collections;
using System.Collections.Generic;
using Avatar;
using Progetile;
using UnityEngine;

namespace Challenge
{
    public class LevelUp_Reward
    {
        private int reward_type; // 어떤 보상 줄 거야? 0: 캔디, 1: 젬, 2: 볼, 3: 아바타 
        private int reward_num; // 얼마만큼 줄 거야?

        /// <summary>
        /// 레벨을 바탕으로 보상내용을 생성자에서 정해줌
        /// </summary>
        /// <param name="index"></param>
        public LevelUp_Reward(int level)
        {
            Set_reward_type(level);
            Set_reward_num(level);
        }

        public int Get_Reward_TYPE()
        {
            return reward_type;
        }

        public int Get_reward_num()
        {
            return reward_num;
        }
        
         /// <summary>
         /// 리워드 타입을 리턴. 이거에 따라서 연출을 다르게 보여줌 
         /// </summary>
         /// <param name="level"></param>
         /// <returns></returns>
        private int Set_reward_type(int level)
        {
            if (level % 5 == 0)
            {
                switch (level)
                {
                    default:
                        reward_type = 1;
                        break;

                    case 20:
                    case 40:
                    case 60:
                    case 80:
                    case 120:
                    case 140:
                    case 160:
                    case 170:
                    case 180:
                    case 190:
                        reward_type = 2;
                        break;

                    case 100:
                    case 200:
                        reward_type = 3;
                        break;
                }
            }
            else
                reward_type = 0;

            return reward_type;
        }

        private int Set_reward_num(int level)
        {
            
            if (level % 5 == 0)
            {
                switch (level)
                {
                    default: // 젬의 보상 
                        int defalut_value = 10; // 캔디 기본값 
                        int value = level / 20;
                        int delta = 5;
                        if (level < 141)
                            reward_num = 10 + (value * delta);
                        else
                            reward_num = 45;
                        break;

                    case 20:// 보상하고자 하는 볼의 index 결정 
                    case 40:
                    case 60:
                    case 80:
                    case 120:
                    case 140:
                    case 160:
                    case 170:
                    case 180:
                    case 190:
                        reward_num = level/10;
                        BallDAO data = new BallDAO();
                        data.Set_BallPur_Data(reward_num); // 구매 처리 
                        break;

                    /*
                    case 100: // 보상하고자 하는 아바타의 index 결정 
                    case 200:
                        reward_num = level/20;
                        AvatarDAO avatarDao = new AvatarDAO(reward_num);
                        avatarDao.Set_Locked_DATA(); // 아바타 구매 처리 
                        break;
                        */
                }
            }
            
            else
            {
                int defalut_value = 10; // 캔디 기본값 
                int value = level / 20;
                int delta = 5;
                if (level < 141)
                    reward_num = 10 + (value * delta);
                else
                    reward_num = 45;
            }

            return reward_num;
        }
    }
}
