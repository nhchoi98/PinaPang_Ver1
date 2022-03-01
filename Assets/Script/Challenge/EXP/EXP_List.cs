using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class EXP_List 
    {
        public int Target_return(int index)
        {
            var mod = index % 3;
            var exp_const = 10;
            var value = 0;
            if (index < 15)
                value = (mod + 1) * exp_const;

            else
            {
                exp_const = 20;
                value = (mod + 1) * exp_const;
                if (index == 27)
                    value = 60; 
            }

            return value;
        }
        
        
    }
}
