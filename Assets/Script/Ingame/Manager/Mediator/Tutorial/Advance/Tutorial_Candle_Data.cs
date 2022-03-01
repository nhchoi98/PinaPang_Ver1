using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public static class Tutorial_Candle_Data
    {
        private static bool tutorial_complete;
        private static bool tutorial_respawn = false;
        private static bool isPanelOpen;

        public static void Set_Data()
        {
            if (PlayerPrefs.GetInt("Tutorial_Advance_Candle_Show", 0) == 0)
                tutorial_complete = false;
            
            else
                tutorial_complete = true;
            
        }

        public static bool Get_Done_Data()
        {
            return tutorial_complete;
        }

        public static void Set_respawn_Data()
        {
            if (!tutorial_respawn)
            {
                tutorial_respawn = true;
                tutorial_complete = true;
                isPanelOpen = true;
            }

            else
                tutorial_respawn = false;
        }

        public static bool Get_respawn_Data()
        {
            return tutorial_respawn;
        }

        public static bool IsPanelOpen()
        {
            bool return_value = isPanelOpen;
            if (isPanelOpen)
                isPanelOpen = false;
            return return_value;
        }
    }
}
