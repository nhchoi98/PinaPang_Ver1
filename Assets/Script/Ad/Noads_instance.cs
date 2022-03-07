using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ad
{
    public static class Noads_instance
    {
        private static bool is_ItemAds;
        private static bool is_Noads;

        public static void Init_Data()
        {
            if (PlayerPrefs.GetInt("ItemAds", 0) == 0)
                is_Noads = false;

            else
                is_Noads = true;
            
            
            if (PlayerPrefs.GetInt("Noads", 0) == 0)
                is_ItemAds = false;

            else
                is_ItemAds = true;
        }

        public static bool Get_Is_Noads()
        {
            return is_Noads;
        }

        public static void Set_Is_Noads()
        {
            PlayerPrefs.SetInt("Noads", 1);
            Set_ItemAds();
            is_Noads = true;
        }

        public static bool Get_ItemAds()
        {
            return is_ItemAds;
        }

        public static void Set_ItemAds()
        {
            PlayerPrefs.SetInt("ItemAds",1);
            is_ItemAds = true;
        }
    }
}
