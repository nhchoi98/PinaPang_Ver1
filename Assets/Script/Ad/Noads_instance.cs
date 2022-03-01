using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ad
{
    public static class Noads_instance
    {
        private static bool is_Noads;

        public static void Init_isNoads()
        {
            if (PlayerPrefs.GetInt("Noads", 0) == 0)
                is_Noads = false;

            else
                is_Noads = true;
        }
        
        public static bool Get_Is_Noads()
        {
            return is_Noads;
        }

        public static void Set_Is_Noads()
        {
            PlayerPrefs.SetInt("Noads", 1);
            is_Noads = true;
        }
    }
}
