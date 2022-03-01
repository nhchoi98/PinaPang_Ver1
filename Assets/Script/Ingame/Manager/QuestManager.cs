
using System;
using Challenge;
using UnityEngine;
using System.Collections.Generic;
using Alarm;
using Badge;
using Block;
using Data;
using Ingame;
using Manager;
using UnityEngine.UI;


/// <summary>
/// 퀘스트의 조건별 달성정도를 기록해주는 class 
/// </summary>
///
namespace Challenge
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private Challenge_Item quest_set;
        private Set_Achi _achi_data; // 각종 통계 데이터 저장 
        private ChallengeDAO quest_data; // 챌린지 목록을 불러올 수 있는 DAO 
        private Queue<int> ingame_achi_list = new Queue<int>(); // achi 달성시 메시지 띄워주기 위해 queue에 해당 데이터를 삽입함. 
        private Challenge_Descrip descrip;
        public Animator panel_animation;
        public GameObject quest_panel, quest_alarm;
        private bool is_wait;
        private event EventHandler<Badge_Get_Args> badge_event;

        [Header("User_Stat")] private UserStatDAO userstat;
        public AudioSource alarm_sound;

        private void Awake()
        {
            badge_event += Set_Badge_UI;
            _achi_data = new Set_Achi(); // 각종 통계 데이터 저장 
            quest_data = new ChallengeDAO(); // 챌린지 목록을 불러올 수 있는 DAO 
            userstat = new UserStatDAO(badge_event); // 유저 스탯 클래스 가져오기 
        }

        /// <summary>
        /// 박스 한 개 제거시 해당 함수가 호출되어 메시지 띄울지 말지를 결정함.
        /// </summary>
        public void Set_Box(blocktype type)
        {
            // 삼각형이거나, 사각형일 경우 
            switch (type)
            {
                case blocktype.NORMAL_RECT:
                case blocktype.X2_RECT:
                    if (quest_data.Set_Ingame_Sqaure(ref _achi_data, ref ingame_achi_list))
                        Set_Message_UI(); // 메시지 띄우기 
                    
                    break;
                
                case blocktype.X2_TRI1:
                case blocktype.X2_TRI2:
                case blocktype.X2_TRI3:
                case blocktype.X2_TRI4:
                case blocktype.NORMAL_TRI1:
                case blocktype.NORMAL_TRI2:
                case blocktype.NORMAL_TRI3:
                case blocktype.NORMAL_TRI4:
                    if (quest_data.Set_Ingame_Tri(ref _achi_data, ref ingame_achi_list))
                        Set_Message_UI(); // 메시지 띄우기 
                    break;
            }
            // 뱃지 잠금 여부 판단 함수도 들어가야함 
        }

        public void Set_Combo()
        {
            if (quest_data.Set_Combo(ref _achi_data, ref ingame_achi_list))
                Set_Message_UI(); // 메시지 띄우기 

        }

        public void Set_Pinata()
        {
            if (quest_data.Set_Pinata(ref _achi_data, ref ingame_achi_list))
                Set_Message_UI(); // 메시지 띄우기 
            userstat.Set_User_Pinata();
        }
        

        public void Set_Collection(int collection_num)
        {
            if (quest_data.Set_Collection(ref _achi_data, ref ingame_achi_list, collection_num))
                Set_Message_UI(); // 메시지 띄우기 
        }

        public void Set_Item()
        {
            if (quest_data.Set_Item(ref _achi_data, ref ingame_achi_list))
                Set_Message_UI(); // 메시지 띄우기 

            userstat.Set_User_ItemCount();
        }

        public void Set_Revive()
        {
            if (quest_data.Set_Revive(ref _achi_data, ref ingame_achi_list))
                Set_Message_UI(); // 메시지 띄우기 

            userstat.Set_User_Revive();
        }

        public void Set_Score(int score)
        {
            if (quest_data.Set_Score(ref _achi_data, score, ref ingame_achi_list))
                Set_Message_UI();

            userstat.Set_User_Score(score);
            // 뱃지 잠금 해제 판단 함수도 들어가야함 
        }

        /// <summary>
        /// 캔디를 젬으로 교환했을 때 알림을 띄움 
        /// </summary>
        public void Set_Trade_Candy()
        {
            int index = 0;
            quest_set.quest_mission = true;
            if (quest_data.Set_Trade_Gem(ref _achi_data, ref ingame_achi_list, ref index))
            {
                IAlarmMediator _alarmMediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
                Set_Message_UI();
                _alarmMediator.Event_Receieve(Event_Alarm.QUEST_ALARM_TRUE,index);
            }
        }

        public void Set_Quest()
        {
            int index = 0;
            quest_set.quest_mission = true;
            if (quest_data.Set_Quest(ref _achi_data, ref ingame_achi_list, ref index))
            {
                IAlarmMediator _alarmMediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
                quest_alarm.SetActive(true);
                Set_Message_UI();
                _alarmMediator.Event_Receieve(Event_Alarm.QUEST_ALARM_TRUE,index);
            }
            
        }
        
        /// <summary>
        /// 레벨업 시 그에 대한 보상, 혹은 아이템의 잠금해제를 표현해주기 위해 호출되는 함수 
        /// </summary>
        /// <param name="type"></param>
        public void Set_Quest(int type)
        {
            quest_alarm.SetActive(true);
            if (type != -1)
            {
                ingame_achi_list.Enqueue(type);
                Set_Message_UI();
            }
        }
        
        private void Set_Message_UI()
        {
            // 퀘스트 관련 UI SET 해주기 
            if (!panel_animation.GetBool("showing"))
            {
                int type = ingame_achi_list.Dequeue(); // 어떤 타입의 메시지인지 먼저 알아냄 
                UI_SET(type); // Step 2. UI를 SET 해줌 
                panel_animation.SetTrigger("show");
                panel_animation.SetBool("showing", true);
                alarm_sound.Play();
            }

            else
                is_wait = true;
        }

        public void Set_Badge_UI(object obj, Badge_Get_Args eventArgs)
        {
            Calc_Badge_Index badgeIndex;
            int type_value = 1000 + eventArgs.type;
            ingame_achi_list.Enqueue(type_value);
            if (quest_set != null)
            {
                IAlarmMediator _alarmMediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
                badgeIndex = new Calc_Badge_Index();
                _alarmMediator.Event_Receieve(Event_Alarm.BADGE_ALARM_TRUE,badgeIndex.Get_Badge_index(eventArgs.type));
            }

            Set_Message_UI();
        }

        public void Another_info()
        {
            if (ingame_achi_list.Count == 0)
            {
                is_wait = false;
                panel_animation.SetBool("showing", false);
            }

            else
            {
                int type = ingame_achi_list.Dequeue(); // 어떤 타입의 quest인지 먼저 알아냄 
                if (ingame_achi_list.Count == 0)
                    is_wait = false;

                UI_SET(type);
                panel_animation.SetTrigger("show");
            }
        }

        /// <summary>
        /// 메시지를 띄울 때 메시지 양식에 맞는 UI를 띄워줌 
        /// </summary>
        /// <param name="type"></param>
        private void UI_SET(int type)
        {
            // 퀘스트 관련 UI_SET 일 경우 
            if (type < 1000)
            {
                Challenge_Descrip descrip = new Challenge_Descrip(quest_data.Get_Item_index(type));
                Text title = quest_panel.transform.GetChild(2).gameObject.GetComponent<Text>();
                descrip.Get_Title(ref title); // # 1.네임 셋
                quest_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Quest Clear!";
                quest_panel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite =
                    Quest_Img.Get_Quest_Panel(quest_data.Get_Item_index(type));
                quest_panel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite =
                    Quest_Img.Get_Quest_main(quest_data.Get_Item_index(type));
                quest_panel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                quest_panel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            }
            
            // 뱃지 잠금해제 조건일경우 
            else if (type >= 1000 & type <2000)
            {
                Calc_Badge_Index badge_info = new Calc_Badge_Index();
                quest_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "BADGE UNLOCKED!";
                quest_panel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite =
                    badge_info.Set_Badge_Img(type%1000); // 뱃지에 맞는 이미지가 들어가야함 
                quest_panel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f,0f,0f,0f);
                quest_panel.transform.GetChild(2).gameObject.GetComponent<Text>().text = badge_info.name(badge_info.Get_Badge_Num(type%1000)); // 뱃지명 들어가야함 
                
            }
        }

        /// <summary>
        /// user stat 데이터를 저장하는 함수 
        /// </summary>
        public void Set_User_Stat_Save()
        {
            userstat.Set_Data();
        }

        public void Set_First_Attendance()
        {
            userstat.Set_User_First_Badge_Get();
        }

    }
}


