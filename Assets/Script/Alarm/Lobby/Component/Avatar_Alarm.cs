using System.Collections;
using System.Collections.Generic;
using Ingame;
using Skin;
using UnityEngine;

namespace Alarm
{
    public class Avatar_Alarm : MonoBehaviour, IAlarmComponent
    {
        private IAlarmMediator _mediator;
        [SerializeField] private Transform avatarTR;
        [SerializeReference] private Skin_Avatar avatargrid;
        public GameObject tap_alarm;

        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.AVATAR_ALARM_ON:
                    avatargrid.Reward_Get(index);
                    tap_alarm.SetActive(true);
                    Set_Alarm_On(index); // 알람 켜줌 
                    break;
                
                case Event_Alarm.AVATAR_ALARM_OFF:
                    Set_Alarm_False(index); // 알람 꺼줌 
                    break;
            }
        }

        public void First_Set()
        {
            if (is_alarm_on())
            {
                _mediator.Event_Receieve(Event_Alarm.SKIN_ALARM_ON); // Step 2. 있으면 알림 ON
                tap_alarm.SetActive(true);
                // Step 3. 그리드에 반영 
                
                if (PlayerPrefs.GetInt("AVATAR_9", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(9)).GetChild(8).gameObject.SetActive(true);
                
                if (PlayerPrefs.GetInt("AVATAR_13", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(13)).GetChild(8).gameObject.SetActive(true);
                
                if (PlayerPrefs.GetInt("AVATAR_19", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(19)).GetChild(8).gameObject.SetActive(true);
                
                if (PlayerPrefs.GetInt("AVATAR_1000", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(1000)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("AVATAR_1002", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(1002)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("AVATAR_1005", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(1005)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("AVATAR_2000", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(2000)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("AVATAR_2002", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(2002)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("AVATAR_1006", 0) == 1)
                    avatarTR.GetChild(Calc_Index.Get_Avatar_index(1006)).GetChild(8).gameObject.SetActive(true);
            }
        }

        public bool Get_Alarm()
        {
            return is_alarm_on();
        }

        private void Set_Alarm_False(int index)
        {
            PlayerPrefs.SetInt("AVATAR_" + Calc_Index.Get_Avatar_Num(index).ToString(), 0);
            if (!is_alarm_on())
            {
                tap_alarm.SetActive(false);
                _mediator.Event_Receieve(Event_Alarm.DETERMINE_SKIN_ALARM);
            }

        }

        private void Set_Alarm_On(int index)
        {
            avatarTR.GetChild(Calc_Index.Get_Avatar_index(index)).GetChild(8).gameObject.SetActive(true);
        }

        private bool is_alarm_on()
        {
            
            if (PlayerPrefs.GetInt("AVATAR_9", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_13", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_19", 0) == 1)
                return true;

            if (PlayerPrefs.GetInt("AVATAR_1000", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_1002", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_1005", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_1006", 0) == 1)
                return true;    
            
            if (PlayerPrefs.GetInt("AVATAR_2000", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("AVATAR_2002", 0) == 1)
                return true;    

            return false;
        }
    }
}

