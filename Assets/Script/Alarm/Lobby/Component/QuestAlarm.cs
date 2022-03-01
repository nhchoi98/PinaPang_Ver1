using System.Collections;
using System.Collections.Generic;
using Challenge;
using UnityEngine;
using UnityEngine.UI;

namespace Alarm
{
    public class QuestAlarm : MonoBehaviour, IAlarmComponent
    {
        [Header("Lobby_UI")] public GameObject alarm;
        [Header("Alarm_Data")] private Quest_Alarm alarmData;
        private IAlarmMediator _mediator;
        public GameObject reward_alarm;

        [Header("Reward_Data")] 
        [SerializeField] private Quest_Table_UI dataSet_parent;

        [SerializeField] private Level_UI _levelUI;
        [SerializeField] private Challenge_UI _challengeUI;
        public GameObject challengePanel;
        public Animator questAnimator;
        public void Set_Mediator(IAlarmMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Receieve(Event_Alarm _event, int index)
        {
            switch (_event)
            {
                case Event_Alarm.QUEST_ALARM_FALSE:
                    Set_Alarm_false(index);
                    break;
                
                case Event_Alarm.QUEST_ALARM_TRUE:
                    Set_Alarm_true(index);
                    break;
                
                case Event_Alarm.QUEST_REWARD_TRUE:
                    reward_alarm.SetActive(true);
                    break;

                case Event_Alarm.QUEST_REWARD_FALSE:
                    Determine_Alarm();
                    break;
                
            }
            
        }

        public void First_Set()
        {
            ChallengeDAO challenge_data = new ChallengeDAO();
            alarmData = new Quest_Alarm(ref challenge_data);
            Determine_Alarm();
        }

        public bool Get_Alarm()
        {
            return false;
        }

        private bool Determine_Alarm()
        {
            bool main_flag = false;
            for (int i = 0; i < 5; i++)
            {
                if (alarmData.Get_Data(i))
                {
                    main_flag = true;
                    break;
                }
            }

            if (dataSet_parent.dataSet.Get_reward_not_have())
            {
                main_flag = true;
                _challengeUI.rewardFlag = true;
                questAnimator.SetTrigger("Alarm_on");
                reward_alarm.SetActive(true);
            }


            else
            {
                questAnimator.SetTrigger("Alarm_off");
                reward_alarm.SetActive(false);
                _challengeUI.rewardFlag = false;
            }

            if (!main_flag)
            {
                alarm.SetActive(false);
                _challengeUI.rewardFlag = false;
            }

            else
            {
                alarm.SetActive(true);
                _challengeUI.rewardFlag = true;
            }

            return false;
        }
        
        /// <summary>
        /// 퀘스트에서 보상을 수령하게 되면 이걸 false로 만들어줌 
        /// </summary>
        private void Set_Alarm_false(int index)
        {
            alarmData.Set_Data(index,true);
            Determine_Alarm(); // 알람을 계속 띄울지 말지 결정함 
        }
        
        /// <summary>
        /// 로비에서 미션을 달성하게 되면, 알람을 띄워주는 함수 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Alarm_true(int index)
        {
            alarmData.Set_Data(index,false);
            Determine_Alarm(); // 알람을 계속 띄울지 말지 결정함 
        }
    }
}
