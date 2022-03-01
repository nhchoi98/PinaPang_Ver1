using UnityEngine;
using Firebase.Analytics;

namespace log
{
    public static class Tutorial_Log
    {
        public static void Basic_Tutorial_Start()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Basic_Tutorial_Start");
            }

            catch
            {
                
            }
        }

        public static void Basic_Tutorial_Complete()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Basic_Tutorial_Complete");
            }
            
            catch
            {

            }
        }

        public static void Candle_Tutorial_Begin()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Candle_Tutorial_Begin");
            }

            catch
            {
                
            }
        }

        public static void Candle_Tutorial_End()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Candle_Tutorial_End");
            }

            catch
            {
                
            }
        }

        public static void Pinata_Tutorial_Start()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Pinata_Tutorial_Start");
            }

            catch
            {
                
            }
        }

        public static void Pinata_Tutorial_End()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Pinata_Tutorial_End");
            }

            catch
            {
                
            }
        }

        public static void Item_Tutorial_Start()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Item_Tutorial_Start");
            }

            catch
            {
                
            }
        }
        
        public static void Item_Tutorial_End()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Item_Tutorial_End");
            }

            catch
            {
                
            }
        }

        public static void Exchange_Tutorial_Start()
        {
            if (PlayerPrefs.GetInt("Exchange_Tutorial_Log", 0) == 0)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("Exchange_Tutorial_Start");
                }

                catch
                {
                    
                }
            }
            
            else
                return;
        }

        public static void Exchange_Tutorial_End()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Exchange_Tutorial_End");
            }

            catch
            {
                
            }
        }

        public static void Skin_Tutorial_Start()
        {
            if (PlayerPrefs.GetInt("Skin_Tutorial_Log", 0) == 0)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("Skin_Tutorial_Start");
                }

                catch
                {
                    
                }
            }
            
            else
                return;
        }

        public static void Skin_Tutorial_End()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Skin_Tutorial_End");
            }
            
            catch
            {
                
            }
        }

        public static void Tutorial_Skipped()
        {
            try
            {
                FirebaseAnalytics.LogEvent("Tutorial_Skipped");
            }

            catch
            {
                
            }
        }
        
        


    }
}
