using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class Set_Achi
    {

        #region Set_Data

        public bool Set_box(ref ChallengeVO data)
        {
            int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;

            switch (value)
            {
                default:
                    return false;
                
                case 1:
                    data.count += 1;
                    if (data.count >= (200 * (mod + 1)))
                    {
                        if (!data.achi)
                        {
                            data.achi = true;
                            return_value = true;
                        }
                    }
                    break;
            }
            return return_value;
        }
        
        public bool Set_Tri(ref ChallengeVO data)
        {
            int mod;
            int value;
            int target_value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;
            data.count += 1;
            switch (mod)
            {
                default:
                    target_value = 1;
                    break;
                
                case 1:
                    target_value = 2;
                    break;
                
                case 2:
                    target_value = 3;
                    break;
            }
            
            if (data.count >= (50 * (target_value)))
            {
                if (!data.achi)
                {
                    data.achi = true;
                    return_value = true;
                }
            }
            return return_value;
        }

        public bool Set_Combo(ref ChallengeVO data)
        {   int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;
            switch (value)
            {
                
                default:
                    return false;
                
                case 3:
                    data.count += 1;
                    if (data.count >= (20 * (mod + 1)))
                    {
                        if (!data.achi)
                        {
                            data.achi = true;
                            return_value = true;
                        }
                    }
                    break;
                
            }

            return return_value;

        }

        public bool Set_Score(ref ChallengeVO data, int score)
        {
            switch (data.item/3)
            {
                default:
                    return false;
                
                case 4:
                    int mod = data.item % 3;
                    data.count += score;
                    if (data.count >= 5000 * (mod + 1))
                    {
                        if (!data.achi)
                        {
                            data.achi = true;
                            return true;
                        }
                    }

                    return false;
                
            }
        }

        public bool Set_Pinata(ref ChallengeVO data)
        {
            int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;
            switch (value)
            {
                default:
                    return false;
                
                case 0:
                    data.count += 1;
                    if (data.count >= (2 * (mod + 1)))
                    {
                        if (!data.achi)
                        {
                            data.achi = true;
                            return_value = true;
                        }
                    }

                    break;

            }
            return return_value;
        }

        public bool Set_Revive(ref ChallengeVO data)
        {
            int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;
            data.count += 1;
            if (data.count >= (mod + 1))
            {
                if (!data.achi)
                {
                    data.achi = true;
                    return_value = true;
                }
            }
                
        

            return return_value;
        }

        public bool Set_Item_Use(ref ChallengeVO data)
        {
            int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            value = data.item / 3;

            switch (value)
            {
                default:
                    return false;
                
                case 5:
                    data.count += 1;
                    if (data.count >= 2 * (mod + 1))
                    {
                        if (!data.achi)
                        {
                            data.achi = true;
                            return_value = true;
                        }
                    }

                    break;
            }

            return return_value;
        }

        public bool Set_Quest(ref ChallengeVO data)
        {
             data.count += 1;
             if (data.count >= 4)
             {
                 if (!data.achi)
                 {
                     data.achi = true;
                     return true;
                 }
             }
             return false;
         
        }

        public bool Set_Trade_Gem(ref ChallengeVO data)
        {
            data.count += 1;
            int mod = data.item % 3;
            if (data.count >= (mod + 1))
            {
                if (!data.achi)
                {
                    data.achi = true;
                    return true;
                }
            }
            return false;
        }
        
        public bool Set_Trade_Toy(ref ChallengeVO data)
        {
            data.count += 1;
            int mod = data.item % 3;
            if (data.count >= (mod + 1))
            {
                if (!data.achi)
                {
                    data.achi = true;
                    return true;
                }
            }
            return false;
        }
        
        
        public bool Set_Collection(ref ChallengeVO data, int collection_num)
        {
            int mod;
            int value;
            bool return_value = false;

            mod = data.item % 3;
            data.count += collection_num;

            if(mod == 2)
            {
                if(data.count>=10)
                {
                    if (!data.achi)
                    {
                        data.achi = true;
                        return_value = true;
                    }

                }
            }

            else
            {
                if (data.count >= 3 + (2 * (mod + 1)))
                {
                    if (!data.achi)
                    {
                        data.achi = true;
                        return_value = true;
                    }

                }
                
            }

            return return_value;
        }
        #endregion
    }
}
