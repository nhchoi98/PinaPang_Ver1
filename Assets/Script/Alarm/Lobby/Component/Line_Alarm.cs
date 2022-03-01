
using Skin;
using UnityEngine;

namespace Alarm
{
    public class Line_Alarm : MonoBehaviour, IAlarmComponent
    {
        private IAlarmMediator _mediator;
        [SerializeField] private Transform lineTR;
        [SerializeField] private Skin_Line lineGrid;
        public GameObject tap_alarm;
        
        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.LINE_ALARM_ON:
                    lineGrid.Reward_Get(index);
                    tap_alarm.SetActive(true);
                    Set_Alarm_On(index);
                    break;
                
                case Event_Alarm.LINE_ALARM_OFF:
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
            }
        }
        public bool Get_Alarm()
        {
            return is_alarm_on();
        }

        private void Set_Alarm_False(int index)
        {
            PlayerPrefs.SetInt("LINE_" + index.ToString(), 0);
            if (!is_alarm_on())
            {
                _mediator.Event_Receieve(Event_Alarm.DETERMINE_SKIN_ALARM);
                tap_alarm.SetActive(false);
            }
        }

        private void Set_Alarm_On(int index)
        {
            lineTR.GetChild(index).GetChild(8).gameObject.SetActive(true);
        }

        private bool is_alarm_on()
        {
            if (PlayerPrefs.GetInt("LINE_13", 0) == 1)
                return true;

            if (PlayerPrefs.GetInt("LINE_14", 0) == 1)
                return true;
            
            return false;
        }
        
    }
}
