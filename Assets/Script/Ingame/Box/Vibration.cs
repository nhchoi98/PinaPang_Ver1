using UnityEngine;
using System.Collections;

namespace Setting
{
    public static class Vibration
    {
        private static bool isOn = true; // 설정이 켜져있는지를 저장하는 함수  
#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator =
 currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        /// <summary>
        /// 진동을 킬 지 말지를 이 함수를 통해 결정함 
        /// </summary>
        /// <param name="On"></param>
        public static void Set_IsOn(bool On)
        {
            if (On)
            {
                isOn = true;
                PlayerPrefs.SetInt("Vibration", 1);
            }

            else
            {
                isOn = false;
                PlayerPrefs.SetInt("Vibration", 0);
            }
        }

        public static bool Get_IsOn()
        {
            return isOn;
        }

        public static void Vibrate()
        {
            if (isOn)
            {
                if (isAndroid())
                    vibrator.Call("vibrate");
                else
                    Handheld.Vibrate();
            }

            else return;
        }

        public static void Vibrate(long milliseconds)
        {
            if (isOn)
            {
            
                if (isAndroid())
                    vibrator.Call("vibrate", milliseconds);
                else
                    Handheld.Vibrate();
            }

            else isOn = false;
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
            if (isAndroid())
                vibrator.Call("vibrate", pattern, repeat);
            else
                Handheld.Vibrate();
        }

        public static bool HasVibrator()
        {
            return isAndroid();
        }

        public static void Cancel()
        {
            if (isAndroid())
                vibrator.Call("cancel");
        }

        private static bool isAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                return true;
#else
            return false;
#endif
        }
    }
}

