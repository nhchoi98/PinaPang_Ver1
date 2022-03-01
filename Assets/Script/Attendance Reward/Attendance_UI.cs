using System;
using System.Collections;
using System.Collections.Generic;
using Alarm;
using Avatar;
using Badge;
using Challenge;
using Collection;
using Data;
using Progetile;
using Skin;
using Timer;
using UnityEngine;
using UnityEngine.UI;

namespace Attendance
{
    public class Attendance_UI : MonoBehaviour
    {
        private AttendanceDAO data;
        
        [Header("UI")]
        [SerializeField] private Transform itemTR;
        [SerializeField] private Button getBtn;
        [SerializeField] private GameObject exitBtn;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject ballPanel;
        
        [Header("Gem")] 
        [SerializeField] private Text mainGem;
        [SerializeField] private Text attendanceGem;
        [SerializeField] private Animator gemAnimator;
        public GameObject gemObj;

        [Header("Reward_Panel")] public Image reward_img;
        public Text reward_text;
        public AudioSource gemSound;
        
        private const int ITEM_NUM = 21;
        private int which_item;
        private int gem;

        [Header("Alarm")] public GameObject alarmObj;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private Badge_Data badgeData;

        [Header("Tutorial")] [SerializeField] private Attendnace_Tutorial_Complete tutorial;
        [SerializeField] private Badge_Data _badgeData;

        [Header("SuperSale_Panel")] [SerializeField]
        private CharaterPack_Timer _charaterPackTimer;
        
        public Button exitBack_Btn;
        private bool reward_panel = false;
        void Start()
        {
            bool flag = false;
            data = new AttendanceDAO();
            if (PlayerPrefs.GetInt("Tutorial_Attendance", 0) == 0)
                exitBack_Btn.interactable = false;
            
            for (int i = 0; i < ITEM_NUM; i++)
            {
                Transform tr = itemTR.GetChild(i);
                if (data.Get_can_get(i))
                {
                    if (!data.Get_is_get(i))
                    {
                        if(!flag)
                            which_item = i;
                        getBtn.gameObject.SetActive(true);
                        exitBtn.SetActive(false);
                        tr.GetChild(0).gameObject.SetActive(true);
                        flag = true;
                        exitBack_Btn.interactable = false;
                        alarmObj.SetActive(true);
                    }

                    else
                    {
                        tr.GetChild(2).gameObject.SetActive(true);
                    }
                }
            }

            if (!flag)
            {
                getBtn.gameObject.SetActive(false);
                exitBtn.SetActive(true);
            }

            else
            {
                if (PlayerPrefs.GetInt("Tutorial_Attendance", 0) != 0)
                    panel.SetActive(true);
            }
        }

        public void onClick_Exit()
        {
            if (reward_panel)
            {
                _charaterPackTimer.Open_Lobby_Popup();
                reward_panel = false;
            }
            panel.SetActive(false);
        }
        
        public void OnClick_Open_Panel()
        {
            gem = Playerdata_DAO.Player_Gem();
            attendanceGem.text = String.Format("{0:#,0}", gem);
            panel.SetActive(true);
        }
        public void OnClick_Collect()
        {
            // 아이템 구매 처리 
            gem = Playerdata_DAO.Player_Gem();
            Get_Reward();
            itemTR.GetChild(which_item).GetChild(0).gameObject.SetActive(false);
            itemTR.GetChild(which_item).GetChild(2).gameObject.SetActive(true);
            data.Set_is_get(which_item);
            bool flag = false;
            
            if (PlayerPrefs.GetInt("Tutorial_Attendance", 0) == 0) // 튜토리얼이 끝난걸 감지하는 PlayerPrefs가 필요함
            {
                PlayerPrefs.SetInt("Tutorial_Attendance",1); // 출석부 튜토리얼이 끝났음을 flag 해주고, 로비에 있는 버튼 애니메이션을 가동시켜줌 
                tutorial.enabled = true;
                exitBack_Btn.interactable = true;
            }

            reward_panel = true;

                // 또 있나 찾아보기~
            for (int i = 0; i < ITEM_NUM; i++)
            {
                Transform tr = itemTR.GetChild(i);
                if (data.Get_can_get(i))
                {
                    if (!data.Get_is_get(i))
                    {
                        flag = true;
                        which_item = i;
                        getBtn.gameObject.SetActive(true);
                        exitBtn.SetActive(false);
                        tr.GetChild(0).gameObject.SetActive(true);
                        break;
                    }

                    else
                    {
                        tr.GetChild(2).gameObject.SetActive(true);
                    }
                }
            }

            if (!flag)
            {
                getBtn.gameObject.SetActive(false);
                exitBtn.SetActive(true);
                alarmObj.SetActive(false);
                exitBack_Btn.interactable = true;
            }

        }

        private void Get_Reward()
        {
            BallPurDAO ballDao;
            IsLockedDAO avatarDao;
            data.Set_is_get(which_item);
            if (which_item == 0)
            {
                questManager.Set_First_Attendance();
                badgeData.Set_First_Attendance();
            }

            switch (which_item)
            {
                default:
                    Playerdata_DAO.Set_Player_Gem(10);
                    StartCoroutine(Get_Gem());
                    gemSound.Play();
                    break;
                
                case 6:
                case 13:
                    Playerdata_DAO.Set_Player_Gem(40);
                    StartCoroutine(Get_Gem(40));
                    gemSound.Play();
                    break;

                case 2: // 햄버거공 획득 
                    ballDao = new BallPurDAO(3005);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3005);
                    Set_Special_Panel(3005);
                    break;
                
                case 5: // 감튀공 획득 
                    ballDao = new BallPurDAO(3006);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3006);
                    Set_Special_Panel(3006);
                    break;

                case 8: // 쿠키공 획득 
                    ballDao = new BallPurDAO(3007);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3007);
                    Set_Special_Panel(3007);
                    break;
                
                case 11: // 무당벌레공 획득 
                    ballDao = new BallPurDAO(3008);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3008);
                    Set_Special_Panel(3008);
                    break;
                
                case 14: // 해골공 획득 
                    ballDao = new BallPurDAO(3009);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3009);
                    Set_Special_Panel(3009);
                    break;
                
                case 17: // 벛꽃공 획득 
                    ballDao = new BallPurDAO(3010);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3010);
                    Set_Special_Panel(3010);
                    break;
                
                case 20: // 아바타 획득 
                    avatarDao = new IsLockedDAO(1000);
                    avatarDao.Set_Locked_Condition();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Avatar(1000);
                    Set_Special_Panel(1000);
                    break;
            }
        }

        private void Set_Special_Panel(int type)
        {
            ballPanel.SetActive(true);
            Avatar_Name name_data = new Avatar_Name();
            reward_img.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            // Step 1. 이미지 바꾸어주기 
            if (type == 1000)
            {
                reward_img.sprite = Set_Avatar_UI.Set_Avatar_Img(Calc_Index.Get_Avatar_index(type));
                reward_img.rectTransform.rect.Set(0,28,120f,120f);
                reward_img.SetNativeSize();
            }
            
            else
            {
                reward_img.rectTransform.rect.Set(0,28,144f,144f);
                reward_img.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                reward_img.SetNativeSize();
                reward_img.transform.localScale = new Vector3(3f, 3f, 1f);
            }
            
            // Step 2. 이름 적용하기 
            if (type ==1000)
            {
                reward_text.text = name_data.Set_Charater_Name(type,false);
            }
            
            else
            {
                switch (type)
                {
                    default:
                        break;
                    
                    case 3005:
                        reward_text.text = "Hamburger";
                        break;
                    
                    case 3006:
                        reward_text.text = "French fries";
                        break;
                         
                    case 3007:
                        reward_text.text = "Cookie";
                        break;
                    
                    case 3008:
                        reward_text.text = "LadyBug";
                        break;
                    
                    case 3009:
                        reward_text.text = "Skull";
                        break;
                    
                    case 3010:
                        reward_text.text = "Cherry blossom";
                        break;
                }
            }
            
            // Step 3. 실질적으로 잠금해제 해주기 

            IAlarmMediator mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
            // 아바타 잠금 해제
            if (type == 1000)
            {
                PlayerPrefs.SetInt("AVATAR_" +type.ToString(), 1);
                mediator.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON,type);
            }

            else
            {
                PlayerPrefs.SetInt("BALL_" +type.ToString(), 1);
                mediator.Event_Receieve(Event_Alarm.BALL_ALARM_ON,type);
            }
            // Step 1. Grid Set
            // Step 3. 알람 띄워주기 
        }

        public void OnClick_Special_Panel_Close()
        {
            ballPanel.SetActive(false);
        }
        
        #region  Commodity_Flight
        public IEnumerator Get_Gem(int gem = 10)
        {
            int index = which_item;
            for (int i = 0; i < gem; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Gem_Start_pos(index);
                GameObject obj = Instantiate(gemObj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gemAnimator;
                script.start_pos = start_pos;
                script.gem_text = this.attendanceGem;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }
            yield return null;
        }

        /// <summary>
        /// 버튼의 위치를 기반으로, 젬이 비행 시작할 위치를 지정해줌 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 Gem_Start_pos(int index)
        {
            Vector2 target_pos;
            switch (index)
            {
                default:
                    target_pos = new Vector2(-361f, 338f);
                    break;

                case 1:
                    target_pos = new Vector2(-184f, 338f);
                    break;
                
                case 3:
                    target_pos = new Vector2(170f, 338f);
                    break;
                
                case 4:
                    target_pos = new Vector2(334f, 338f);
                    break;

                case 6:
                    target_pos = new Vector2(-188f, 111f);
                    break;
                
                case 7:
                    target_pos = new Vector2(-8f, 111f);
                    break;
                
                case 9:
                    target_pos = new Vector2(328f, 111f);
                    break;
                
                case 10:
                    target_pos = new Vector2(-350f, -97f);
                    break;
                
                case 12:
                    target_pos = new Vector2(8f, -97f);
                    break;
                
                case 13:
                    target_pos = new Vector2(173f, -97f);
                    break;
                
                case 15:
                    target_pos = new Vector2(-356f, -321f);
                    break;
                
                case 16:
                    target_pos = new Vector2(-176f, -321f);
                    break;
                
                case 18:
                    target_pos = new Vector2(169f, -321f);
                    break;
                
                case 19:
                    target_pos = new Vector2(349f, -321f);
                    break;
            }
            return target_pos;
        }
        
        
        private void set_text(object sender, EventArgs e)
        {
            gem += 1;
            attendanceGem.text = string.Format("{0:#,0}", gem);
        }
        #endregion
    }
}
