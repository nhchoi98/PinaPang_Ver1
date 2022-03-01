using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

namespace Log
{
    public static class Ingame_Log 
    {

        public static void LogIn()
        {
            try
            {
                FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin); // 나 이제 어플켰어요 하고 로그를 적어줌 
            }

            catch
            {

            }
        }

        #region Ingame_Log
        public static void Revive(bool is_gem)
        {
            if (is_gem)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("Revive_Gem");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            else
            {
                try
                {
                    FirebaseAnalytics.LogEvent("Revive_Ad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        public static void Line_Item(bool is_gem)
        {
            if (is_gem)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("LineItem_Gem");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            else
            {
                try
                {
                    FirebaseAnalytics.LogEvent("LineItem_Ad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
        }

        public static void Cross_Razer(bool is_gem)
        {
            if (is_gem)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("CrossItem_Gem");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            else
            {
                try
                {
                    FirebaseAnalytics.LogEvent("CrossItem_Ad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
        }
        public static void Extra_Score(bool is_gem)
        {
            if (is_gem)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("ExtraScore_Gem");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            else
            {
                try
                {
                    FirebaseAnalytics.LogEvent("ExtraScore_Ad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
        }
        #endregion

        #region Heart_Log
        public static void Heart_Charge(bool is_gem)
        {
            if (is_gem)
            {
                try
                {
                    FirebaseAnalytics.LogEvent("HeartCharge_Gem");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            else
            {
                try
                {
                    FirebaseAnalytics.LogEvent("HeartCharge_Ad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        

        #endregion

        public static void Log_User_Count()
        {
            FirebaseAnalytics.LogEvent("GameStart");
        }
        
        public static void Set_Toy(int toy)
        {
            
        }

        #region Quest

        public static void Log_Score(int score)
        {
            try
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    "USER_SCORE",
                    "PARM_USER_SCORE", score); // 스코어를 로그로써 남겨줌 
            }

            catch
            {
                
            }
        }

        public static void Log_Turn(int stage)
        {
            try
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("User_Stage", "USER_STAGE", stage);
            }
            catch
            {
                
            }
        }
        #endregion
        
        #region Tutorial

        public static void Tutorial_End()
        {
            try
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    Firebase.Analytics.FirebaseAnalytics.EventTutorialComplete); // 스코어를 로그로써 남겨줌 
            }

            catch
            {
                
            }
        }
        #endregion
        
        
    }
}
