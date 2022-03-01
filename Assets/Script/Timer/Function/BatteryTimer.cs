using System;
using System.Collections;
using UnityEngine.UI;
using Lobby;
using UnityEngine;

namespace Battery
{
    public class BatteryTimer : MonoBehaviour ,ITimer
    {
        [Header("Time_Data")]
        [SerializeField] private PlayerBtn_Set batteryScript;
        private bool is_charged;
        private TimeSpan targetTime;
        [Header("UI")] public Text timerText;
        public Text timer_panel;
        

        private void Start()
        {
            Set_TargetTime();
            if (batteryScript.Get_Count() > 29)
                timerText.gameObject.SetActive(false);
            else
                StartCoroutine(Timer());


        }

        public IEnumerator Timer()
        {
             while (true)
             {
                 Set_TargetTime(); 
                 var delta_target = targetTime;
                timerText.text = delta_target.ToString(@"mm\:ss");
                timer_panel.text = delta_target.ToString(@"mm\:ss");
                if (delta_target < TimeSpan.FromSeconds(1))
                    break;
                
                yield return new WaitForSecondsRealtime(1.0f);
            }
            Action();
        }

        public void Action()
        {
            batteryScript.Set_Lobby_Charge();
            Set_TargetTime();
            if (batteryScript.Get_Count() > 29)
            {
                timerText.gameObject.SetActive(false);
                timer_panel.gameObject.SetActive(false);
            }

            StartCoroutine(Timer());
        }

        public void Set_TargetTime()
        {
            targetTime =  batteryScript.Get_Target_Time() - DateTime.UtcNow.TimeOfDay;
        }
    }
}
