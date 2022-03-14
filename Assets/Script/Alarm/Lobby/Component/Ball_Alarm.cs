using System.Collections;
using System.Collections.Generic;
using Skin;
using UnityEngine;

namespace Alarm
{
    public class Ball_Alarm : MonoBehaviour, IAlarmComponent
    {
        private IAlarmMediator _mediator;
        [SerializeField] private Transform ballTR;
        [SerializeField] private Skin_Ball ballGrid;
        public GameObject tap_alarm;
        
        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.BALL_ALARM_ON:
                    ballGrid.Reward_Get(index);
                    tap_alarm.SetActive(true);
                    Set_Alarm_On(index);
                    break;
                
                case Event_Alarm.BALL_ALARM_OFF:
                    Set_Alarm_False(index);
                    break;
                
            }
        }

        public void First_Set()
        {
            if (is_alarm_on())
            {
                _mediator.Event_Receieve(Event_Alarm.SKIN_ALARM_ON); // Step 2. 있으면 알림 ON
                tap_alarm.SetActive(true);
                
                if (PlayerPrefs.GetInt("BALL_1008", 0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(1008)).GetChild(8).gameObject.SetActive(true);
                // Step 3. 그리드에 반영 
                if (PlayerPrefs.GetInt("BALL_2005", 0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(2005)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_2006", 0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(2006)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_2007", 0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(2007)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_3000",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(3000)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_3001",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(3001)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_3002",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(3005)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_3003",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(3003)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_3004",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(3004)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_4000",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(4000)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_4001",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(4001)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_4002",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(4002)).GetChild(8).gameObject.SetActive(true);
                
                if(PlayerPrefs.GetInt("BALL_4003",0) == 1)
                    ballTR.GetChild(Calc_Index.Get_Ball_index(4003)).GetChild(8).gameObject.SetActive(true);
            }
        }
        public bool Get_Alarm()
        {
            return is_alarm_on();
        }

        private void Set_Alarm_False(int index)
        {
            PlayerPrefs.SetInt("BALL_" + Calc_Index.Get_Ball_Num(index).ToString(), 0);
            if (!is_alarm_on())
            {
                _mediator.Event_Receieve(Event_Alarm.DETERMINE_SKIN_ALARM);
                tap_alarm.SetActive(false);
            }
        }

        private void Set_Alarm_On(int index)
        {
            ballTR.GetChild(Calc_Index.Get_Ball_index(index)).GetChild(8).gameObject.SetActive(true);
        }

        private bool is_alarm_on()
        {
            
            if (PlayerPrefs.GetInt("BALL_1008", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_2005", 0) == 1)
                return true;

            if (PlayerPrefs.GetInt("BALL_2006", 0) == 1)
                return true;

            if (PlayerPrefs.GetInt("BALL_2007", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_2008", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_3000", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_3001", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_3002", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_3003", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_3004", 0) == 1)
                return true;

            if (PlayerPrefs.GetInt("BALL_4000", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_4001", 0) == 1)
                return true;
    
            if (PlayerPrefs.GetInt("BALL_4002", 0) == 1)
                return true;
            
            if (PlayerPrefs.GetInt("BALL_4003", 0) == 1)
                return true;
            return false;
        }
    }
}
