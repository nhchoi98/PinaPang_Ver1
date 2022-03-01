using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alarm
{
    public class Lobby_AlarmMediator : MonoBehaviour, IAlarmMediator
    {
        private IAlarmComponent questAlarm;
        private IAlarmComponent badgeAlarm;
        private IAlarmComponent avatarAlarm;
        private IAlarmComponent ballAlarm;
        private IAlarmComponent lineAlarm;
        private IAlarmComponent shopAlarm;
        private IAlarmComponent collectionAlarm;
        [SerializeField] private GameObject skin;

        public Transform tr;

        [Header("Skin_Alarm")] public GameObject skinAlarm_obj;

        private void Start()
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                switch (i)
                {
                    default:
                        questAlarm = tr.GetChild(0).gameObject.GetComponent<IAlarmComponent>();
                        questAlarm.Set_Mediator(this);
                        questAlarm.First_Set();
                        break;
                    
                    case 1:
                        badgeAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        badgeAlarm.Set_Mediator(this);
                        badgeAlarm.First_Set();
                        break;

                    case 2:
                        avatarAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        avatarAlarm.Set_Mediator(this);
                        avatarAlarm.First_Set();
                        break;
                    
                    case 3:
                        ballAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        ballAlarm.Set_Mediator(this);
                        ballAlarm.First_Set();
                        break;
                    
                    case 4:
                        lineAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        lineAlarm.Set_Mediator(this);
                        lineAlarm.First_Set();
                        break;
                    
                    case 5:
                        shopAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        shopAlarm.Set_Mediator(this);
                        shopAlarm.First_Set();
                        break;
                    
                    
                    case 6:
                        collectionAlarm = tr.GetChild(i).gameObject.GetComponent<IAlarmComponent>();
                        collectionAlarm.Set_Mediator(this);
                        collectionAlarm.First_Set();
                        break;
                }
                
            }
            

        }

        public void Event_Receieve(Event_Alarm _event, int index = -1)
        {
            switch (_event)
            {
                case Event_Alarm.QUEST_REWARD_TRUE:
                case Event_Alarm.QUEST_REWARD_FALSE:
                case Event_Alarm.QUEST_ALARM_FALSE:
                case Event_Alarm.QUEST_ALARM_TRUE:
                    questAlarm.Event_Receieve(_event, index);
                    break;
                
                case Event_Alarm.SHOP_ALARM_ON:
                case Event_Alarm.SHOP_ALARM_OFF:
                    shopAlarm.Event_Receieve(_event);
                    break;
                
                case Event_Alarm.BADGE_ALARM_TRUE:
                    skinAlarm_obj.SetActive(true);
                    badgeAlarm.Event_Receieve(_event,index);
                    break;
                
                case Event_Alarm.BADGE_ALARM_FALSE:
                    badgeAlarm.Event_Receieve(_event,index);
                    break;
                
                case Event_Alarm.SKIN_ALARM_ON:
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.AVATAR_ALARM_ON:
                    avatarAlarm.Event_Receieve(_event,index);
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.AVATAR_ALARM_OFF:
                    avatarAlarm.Event_Receieve(_event,index);
                    break;
                
                case Event_Alarm.BALL_ALARM_ON:
                    ballAlarm.Event_Receieve(_event,index);
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.BALL_ALARM_OFF:
                    ballAlarm.Event_Receieve(_event,index);
                    break;
                
                case Event_Alarm.LINE_ALARM_ON:
                    lineAlarm.Event_Receieve(_event,index);
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.LINE_ALARM_OFF:
                    lineAlarm.Event_Receieve(_event,index);
                    break;
                
                case Event_Alarm.DETERMINE_SKIN_ALARM:
                    Determine_SkinAlarm();
                    break;
                
                case Event_Alarm.ASTRO_ALARM:
                    avatarAlarm.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, 2000);
                    ballAlarm.Event_Receieve(Event_Alarm.BALL_ALARM_ON,4002);
                    lineAlarm.Event_Receieve(Event_Alarm.LINE_ALARM_ON,14);
                    if (skin.activeSelf) // 스킨창이 띄워져 있는경우
                    {
                        Transform tr = skin.transform.GetChild(4);
                        for (int i = 0; i < skin.transform.GetChild(4).childCount; i++)
                        {
                            if (tr.GetChild(i).gameObject.activeSelf)
                            {
                                tr.GetChild(i).gameObject.SetActive(false);
                                skin.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                break;
                            }
                        }
                    }
                    // 라인 알람 띄워줘야함 
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.PARTY_ALARM:
                    avatarAlarm.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, 1006);
                    ballAlarm.Event_Receieve(Event_Alarm.BALL_ALARM_ON,4000);
                    if (skin.activeSelf) // 스킨창이 띄워져 있는경우
                    {
                        Transform tr = skin.transform.GetChild(4);
                        for (int i = 0; i < skin.transform.GetChild(4).childCount; i++)
                        {
                            if (tr.GetChild(i).gameObject.activeSelf)
                            {
                                tr.GetChild(i).gameObject.SetActive(false);
                                skin.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).gameObject.SetActive(false); // package_get 꺼줌 
                                break;
                            }
                        }
                    }
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.BEAR_ALARM:
                    avatarAlarm.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, 2002);
                    ballAlarm.Event_Receieve(Event_Alarm.BALL_ALARM_ON,4001);
                    lineAlarm.Event_Receieve(Event_Alarm.LINE_ALARM_ON,13);
                    if (skin.activeSelf) // 스킨창이 띄워져 있는경우
                    {
                        Transform tr = skin.transform.GetChild(4);
                        for (int i = 0; i < skin.transform.GetChild(4).childCount; i++)
                        {
                            if (tr.GetChild(i).gameObject.activeSelf)
                            {
                                tr.GetChild(i).gameObject.SetActive(false);
                                skin.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                break;
                            }
                        }
                    }
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.SCIENCE_ALARM:
                    avatarAlarm.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, 13);
                    ballAlarm.Event_Receieve(Event_Alarm.BALL_ALARM_ON,4003);
                    if (skin.activeSelf) // 스킨창이 띄워져 있는경우
                    {
                        Transform tr = skin.transform.GetChild(4);
                        for (int i = 0; i < skin.transform.GetChild(4).childCount; i++)
                        {
                            if (tr.GetChild(i).gameObject.activeSelf)
                            {
                                tr.GetChild(i).gameObject.SetActive(false);
                                skin.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(3).gameObject.SetActive(false); // package_get 꺼줌 
                                skin.transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).gameObject.SetActive(false); // package_get 꺼줌 
                                break;
                            }
                        }
                    }
                    skinAlarm_obj.SetActive(true);
                    break;
                
                case Event_Alarm.TOY_ALARM_ON:
                case Event_Alarm.TOY_ALARM_OFF:
                    collectionAlarm.Event_Receieve(_event);
                    break;
            }
        }

        private void Determine_SkinAlarm()
        {
            bool alarmFlag;
            if(!badgeAlarm.Get_Alarm() && !avatarAlarm.Get_Alarm() && !badgeAlarm.Get_Alarm() && !lineAlarm.Get_Alarm())
               skinAlarm_obj.SetActive(false);

            else
                skinAlarm_obj.SetActive(true);
            
        }

    }
    
}

