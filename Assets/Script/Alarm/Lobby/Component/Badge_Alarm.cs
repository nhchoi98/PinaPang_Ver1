using UnityEngine.UI;
using Badge;
using UnityEngine;

namespace Alarm
{
    public class Badge_Alarm : MonoBehaviour, IAlarmComponent
    {
        [Header("UI")] 
        public Transform badge_UI;
        public GameObject button_alarm;

        private Calc_Badge_Index badgeCalc;
        private bool[] alarm_check;
        

        private IAlarmMediator _mediator;
        
        
        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.BADGE_ALARM_TRUE:
                    Set_Alarm_true(index);
                    break;
                
                case Event_Alarm.BADGE_ALARM_FALSE:
                    Set_Alarm_False(index);
                    _mediator.Event_Receieve(Event_Alarm.DETERMINE_SKIN_ALARM);
                    break;
            }
        }

        public void First_Set()
        {
            BadgeDAO badgeDao;
            bool main_flag = false;
            badgeCalc = new Calc_Badge_Index();
            alarm_check = new bool[badgeCalc.Get_Badge_MaxIndex()];
            // Step 1. 새로 획득한 친구 있는지 검사
            for (int i = 0; i < badgeCalc.Get_Badge_MaxIndex(); i++)
            {
                badgeDao= new BadgeDAO(i);
                if (badgeDao.Get_Is_new(i))
                {
                    alarm_check[i] = true;
                    main_flag = true;
                    badge_UI.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                else
                    alarm_check[i] = false;
                // Step 2. 있으면 해당 Index에 alarm 반영, UI에도 ALARM 반영 해주기 
            }

            if (main_flag)
            {
                button_alarm.SetActive(true);
                _mediator.Event_Receieve(Event_Alarm.SKIN_ALARM_ON);
            }
        }

        public bool Get_Alarm()
        {
            if (is_alarm_on())
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void Set_Alarm_False(int index)
        {
            BadgeDAO badgeDao = new BadgeDAO(index);
            badge_UI.GetChild(index).GetChild(0).gameObject.SetActive(false);// 걔 알람 꺼주기 
            alarm_check[index] = false;
            Determine_Alarm();
            badgeDao.Set_New_Data(index);

        }

        private bool Determine_Alarm()
        {
            if (!is_alarm_on())
            {
                button_alarm.SetActive(false);
                return false;
            }

            else
            {
                button_alarm.SetActive(true);
                return true;
            }
            
        }

        private bool is_alarm_on()
        {
            for (int i = 0; i < badgeCalc.Get_Badge_MaxIndex(); i++)
            {
                if (alarm_check[i])
                    return true;
            }

            return false;
        }


        /// <summary>
        // Outgame에서 뱃지 획득하면, 획득 표시가 뜨게 만들어주는 함수 
        /// </summary>
        private void Set_Alarm_true(int index)
        {
            alarm_check[index] = true;
            badge_UI.GetChild(index).GetChild(0).gameObject.SetActive(true); // 해당 뱃지 알림 활성화
            badge_UI.GetChild(index).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            button_alarm.SetActive(true);
        }
    }
}
