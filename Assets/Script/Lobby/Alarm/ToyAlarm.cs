using System.Collections;
using System.Collections.Generic;
using Toy;
using UnityEngine;

namespace Alarm
{
    public class ToyAlarm : MonoBehaviour, IAlarmComponent
    {
        [SerializeField] private GameObject button_Alarm;
        [SerializeField] private Transform item;
        
        private IAlarmMediator _mediator;
        private bool[] is_alarmOn;

        private const int toyNum = 50;
        void Start()
        {
            is_alarmOn = new bool[50];
            SingleToyDAO toyData;
            for (int i = 0; i < toyNum; i++)
            {
                toyData = new SingleToyDAO(i);
                if (toyData.Get_Toy_Count() > 2)
                {
                    is_alarmOn[i] = true;
                    item.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                else
                    is_alarmOn[i] = false;
            }
        }

        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnClick_Single_Toy(int index)
        {
            if (is_alarmOn[index])
            {
                is_alarmOn[index] = false;
                SingleToyDAO toyData = new SingleToyDAO(index);
                item.GetChild(index).GetChild(0).gameObject.SetActive(false);
                Event_Receieve(Event_Alarm.TOY_ALARM_OFF);
            }


        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.TOY_ALARM_ON:
                    if (Get_is_alarm_on())
                        button_Alarm.SetActive(true);
                    break;
                
                case Event_Alarm.TOY_ALARM_OFF:
                    if(!Get_is_alarm_on())
                        button_Alarm.SetActive(false);
                    break;
            }
        }

        public void First_Set()
        {
            if(PlayerPrefs.GetInt("First_Collection",0)==1)
                button_Alarm.SetActive(true);
            
            _mediator.Event_Receieve(Event_Alarm.TOY_ALARM_ON); // Step 2. 있으면 알림 ON
        }

        public void OnClick_Collection_Button()
        {
            if (PlayerPrefs.GetInt("First_Collection", 0) == 1)
            {
                PlayerPrefs.SetInt("First_Collection", 0);
                if(!Get_is_alarm_on())
                    button_Alarm.SetActive(false);
            }
        }

        public bool Get_Alarm()
        {
            return Get_is_alarm_on();
        }

        private bool Get_is_alarm_on()
        {
            for (int i = 0; i < toyNum; i++)
            {
                if (is_alarmOn[i])
                    return true;
            }

            return false;
        }
        
    }
}
