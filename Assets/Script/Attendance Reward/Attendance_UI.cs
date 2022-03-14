using System;
using System.Collections;
using Alarm;
using Avatar;
using Badge;
using Challenge;
using Collection;
using Data;
using DG.Tweening;
using Lobby;
using Progetile;
using Skin;
using Timer;
using UnityEngine;
using UnityEngine.UI;

namespace Attendance
{
    public class Attendance_UI : MonoBehaviour
    {
        public AttendanceDAO data { get; private set; }
        
        [Header("UI")]
        [SerializeField] private Transform itemTR ,itemTR_SecondPage;
        [SerializeField] private Button exitBtn;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject ballPanel;
        
        [Header("Gem")] 
        [SerializeField] private Text mainGem;
        [SerializeField] private Text attendanceGem;
        [SerializeField] private Animator gemAnimator;
        public GameObject gemObj;

        [Header("Reward_Panel")] 
        public Image reward_img_Ball;
        public Image reward_img_charater;
        private int which_item_world;
        
        public Text reward_text;
        public AudioSource gemSound;
        
        private const int ITEM_NUM = 8;
        private int gem;

        [Header("Alarm")] public GameObject alarmObj;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private Badge_Data badgeData;

        [Header("Tutorial")] [SerializeField] private Attendnace_Tutorial_Complete tutorial;
        [SerializeField] private Badge_Data _badgeData;

        [Header("SuperSale_Panel")] [SerializeField]
        private CharaterPack_Timer _charaterPackTimer;

        [Header("Gem_Panel")] public GameObject gemReward_Panel;
        [SerializeField] private Attendance_DoubleAd adScript;
        
        public Button exitBack_Btn;
        private bool reward_panel = false;

        [Header("Move_Btn")] 
        public GameObject leftBtn, rightBtn;
        public Scrollbar scrollbar;
        public GameObject secondPage;
        
        void OnEnable()
        {
            bool flag = false;
            data = new AttendanceDAO();
            if (PlayerPrefs.GetInt("Tutorial_Attendance", 0) == 0)
                exitBack_Btn.interactable = false;
            
            for (int i = 0; i < ITEM_NUM-4; i++)
            {
                Transform tr = itemTR.GetChild(i);
                if (data.Get_can_get(i))
                {
                    if (!data.Get_is_get(i))
                    {

                        // 보상 받기 버튼 활성화 
                        exitBtn.interactable = false;
                        tr.GetChild(0).gameObject.SetActive(true);
                        flag = true;
                        exitBack_Btn.interactable = false;
                        alarmObj.SetActive(true);
                    }

                    else
                        tr.GetChild(2).gameObject.SetActive(true);
                    
                }
            }

            for (int i = ITEM_NUM-4 ; i<ITEM_NUM; i++)
            {
                Transform tr = itemTR_SecondPage.GetChild(i-4);
                if (data.Get_can_get(i))
                {
                    if (!data.Get_is_get(i))
                    {
                        // 보상 받기 버튼 활성화 
                        exitBtn.interactable = false;
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
                exitBtn.interactable = true;
                // 보상 받기 버튼 활성화 
            }

            else
            {
                if (PlayerPrefs.GetInt("Tutorial_Attendance", 0) != 0)
                {
                    panel.SetActive(true);
                }
            }
        }

        #region Move_Page

        public void OnClick_LeftBtn()
        {
            leftBtn.SetActive(false);
            DOTween.To(()=> scrollbar.value, x=> scrollbar.value = x, 0f, 0.7f)
                .OnComplete(() =>
                {
                    rightBtn.SetActive(true);
                });
        }


        public void OnClick_RightBtn()
        {
            rightBtn.SetActive(false);
            DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 1f, 0.7f)
                .OnComplete(() =>
                {
                    leftBtn.SetActive(true);
                });
        }
        #endregion

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
        
        public void OnClick_Collect(int which_item)
        {
            // 아이템 구매 처리 
            gem = Playerdata_DAO.Player_Gem();
            Get_Reward(which_item);
            if (which_item < 4)
            {
                itemTR.GetChild(which_item).GetChild(0).gameObject.SetActive(false);
                itemTR.GetChild(which_item).GetChild(2).gameObject.SetActive(true);
            }

            else
            {
                itemTR_SecondPage.GetChild(which_item-4).GetChild(0).gameObject.SetActive(false);
                itemTR_SecondPage.GetChild(which_item-4).GetChild(2).gameObject.SetActive(true);
            }

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
                for (int i = 0; i < ITEM_NUM-4; i++)
                {
                    Transform tr = itemTR.GetChild(i);
                    if (data.Get_can_get(i))
                    {
                        if (!data.Get_is_get(i))
                        {
                            if(!flag)
                                which_item = i;
                        
                            // 보상 받기 버튼 활성화 
                            exitBtn.interactable = false;
                            tr.GetChild(0).gameObject.SetActive(true);
                            flag = true;
                            exitBack_Btn.interactable = false;
                            alarmObj.SetActive(true);
                        }

                        else
                            tr.GetChild(2).gameObject.SetActive(true);
                    }
                }

                for (int i = ITEM_NUM-4 ; i<ITEM_NUM; i++)
                {
                    Transform tr = itemTR_SecondPage.GetChild(i-4);
                    if (data.Get_can_get(i))
                    {
                        if (!data.Get_is_get(i))
                        {
                            if(!flag)
                                which_item = i;
                        
                            // 보상 받기 버튼 활성화 
                            exitBtn.interactable = false;
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
                // 보상 받기 버튼 활성화 
                exitBtn.interactable = true;
                alarmObj.SetActive(false);
                exitBack_Btn.interactable = true;
            }

        }
        
        /// <summary>
        /// 2배 보상 버튼 -> isGem = true
        /// </summary>
        /// <param name="isGem"></param>
        private void Get_Reward(int which_item)
        {
            BallPurDAO ballDao;
            IsLockedDAO avatarDao;
            if (which_item == 0)
            {
                questManager.Set_First_Attendance();
                badgeData.Set_First_Attendance();
            }
            // 보상 패널 띄워줘야함 
            // # 1. 그냥 받는 아이템이냐, 젬이냐에 따라서 먼저 구분지어줌. 
            // # 2. 젬일 경우, ad의 index을 set 해줌.
            which_item_world = which_item;
            switch (which_item)
            {
                default:
                    adScript.Set_Index(which_item);
                    Set_GemReward_Panel(which_item);
                    break;
                
                case 1: // 벛꽃공 획득 
                    ballDao = new BallPurDAO(2005);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(2005);
                    Set_Special_Panel(2005);
                    break;
                
                case 3: // 드라큘라 셋 획득 . Set_Special Panel 매개변수 변경해야함
                    avatarDao = new IsLockedDAO(9);
                    avatarDao.Set_Locked_Condition();
                    Skin_Log.Buy_Avatar(9);

                    ballDao = new BallPurDAO(2006);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(2006);
                    Set_Special_Panel(1009);// 보상 패널 Set

                    break;

                case 5: // 음표공 획득 
                    ballDao = new BallPurDAO(2007);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Set_Special_Panel(2007);
                    Skin_Log.Buy_Ball_Log(2007);
                    break;
                
                case 7: // 아바타 획득 + 베이비 드라이버 세트공 연결해야함. 
                    avatarDao = new IsLockedDAO(1000);
                    avatarDao.Set_Locked_Condition();
                    Skin_Log.Buy_Avatar(1000);

                    ballDao = new BallPurDAO(1008);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(1008);
                    Set_Special_Panel(1000);
                    break;
            }
        }

        private void Set_GemReward_Panel(int which_item)
        {
            int gem;
            Image firstimg = gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject
                .GetComponent<Image>();
            
            Image secondImg = gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).gameObject
                .GetComponent<Image>();
            switch (which_item)
            {
                default:
                    gem = 10;
                    break;
                
                case 2:
                    gem = 15;
                    break;
                
                case 4:
                    gem = 20;
                    break;
                
                case 6:
                    gem = 30;
                    break;
            }
            
            // # 2. 패널의 텍스트를 반영해줘야함. 
            gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = gem.ToString();
            gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(2).gameObject.GetComponent<Text>().text = (gem * 2).ToString();
            
            // # 3. 젬의 이미지를 바꾸어줌 
            if (gem < 15)
            {
                firstimg.sprite = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_1");
                secondImg.sprite  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_2");
            }
            
            else if (gem >= 15 && gem < 30)
            {
                firstimg.sprite   = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_2");
                secondImg.sprite   = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_3");

            }
            else if (gem >= 30 && gem < 60)
            {
                firstimg.sprite   = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_3");
                secondImg .sprite  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_4");
            }

            gemReward_Panel.SetActive(true);
        }

        /// <summary>
        /// 2배 버튼 안누르고 그냥 젬 얻는 경우 
        /// </summary>
        public void Get_Gem(bool is_Doubled)
        {
            int gem;
            switch (which_item_world)
            {
                default:
                    gem = 10;
                    break;
                
                case 2:
                    gem = 15;
                    break;
                
                case 4:
                    gem = 20;
                    break;
                
                case 6:
                    gem = 30;
                    break;
            }

            if(is_Doubled) reward_text.text = (gem*2).ToString();
            
            else
                reward_text.text = gem.ToString();
            
            if (!is_Doubled)
            {
                Playerdata_DAO.Set_Player_Gem(gem);
                Set_Gem_ResultPanel(false,gem, which_item_world);
            }

            else
            {
                Playerdata_DAO.Set_Player_Gem(gem*2);
                Set_Gem_ResultPanel(true,gem, which_item_world);
            }
            
        }

        private void Set_Gem_ResultPanel(bool isDoubled, int rewardType, int which_item)
        {
            reward_img_charater.gameObject.SetActive(false);
            gemReward_Panel.SetActive(false);
            
            if(rewardType<15)
                reward_img_Ball.sprite = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_1");
            
            else if (rewardType >= 15 && rewardType <30)
                reward_img_Ball.sprite = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_2");
            
            else if (rewardType>=30 && rewardType <60)
                reward_img_Ball.sprite = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_3");
            
            else 
                reward_img_Ball.sprite = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_4");
            
            ballPanel.SetActive(true);   
            reward_img_Ball.SetNativeSize();
            reward_img_Ball.transform.localScale = new Vector3(1f, 1f, 1f);
            reward_img_Ball.rectTransform.rect.Set(0,50,144f,144f);
            reward_img_Ball.gameObject.transform.position = new Vector3(0f, 50f, 0f);

            if (!isDoubled)
                StartCoroutine(Get_Gem(rewardType, which_item));

            else
                StartCoroutine(Get_Gem(rewardType * 2, which_item));
        }
        
        private void Set_Special_Panel(int type)
        {
            ballPanel.SetActive(true);
            Avatar_Name name_data = new Avatar_Name();
            reward_img_Ball.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            reward_img_charater.gameObject.SetActive(false);
            reward_text.color = Color.white;
            // Step 1. 이미지 바꾸어주기 
            if (type / 1000 == 1) // 아바타 + ball 세트인 경우 
            {
                reward_img_charater.gameObject.SetActive(true);
                if (type == 1000) // 베이비 드라이버 인 경우 
                {
                    reward_img_charater.sprite = Set_Avatar_UI.Set_Avatar_Img(Calc_Index.Get_Avatar_index(type));
                    reward_img_charater.SetNativeSize();
                    reward_img_charater.gameObject.SetActive(true);
                    
                    // Step 2. ball 이미지의 지정 및 크기 지정해주기. 
                    reward_img_Ball.rectTransform.rect.Set(0,20,48f,48f);
                    reward_img_Ball.gameObject.transform.position = new Vector3(121f, -20f, 0f);
                    reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/1008");
                    reward_img_Ball.SetNativeSize();
                    reward_img_Ball.transform.localScale = new Vector3(1f, 1f, 1f);
                    reward_text.text =("BABY DRIVER Set"); // 보상 타이틀명 지정 
                    
                }

                else // 드라큘라 Set인 경우 
                {
                    reward_img_charater.sprite = Set_Avatar_UI.Set_Avatar_Img(9);
                    reward_img_charater.SetNativeSize();
                    reward_img_charater.gameObject.SetActive(true);
                    
                    // Step 2. ball 이미지의 지정 및 크기 지정해주기. 
                    reward_img_Ball.rectTransform.rect.Set(0,20,48f,48f);
                    reward_img_Ball.gameObject.transform.position = new Vector3(121f, -20f, 0f);
                    reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/2006");
                    reward_img_Ball.SetNativeSize();
                    reward_img_Ball.transform.localScale = new Vector3(1f, 1f, 1f);
                    reward_text.text =("DRACULA Set"); // 보상 타이틀명 지정 
                    
                }
            }
            
            else if(type/ 1000 == 2) // 볼만 받는 경우 
            {
                reward_img_charater.gameObject.SetActive(false);
                reward_img_Ball.rectTransform.rect.Set(0,28,84f,84f);
                reward_img_Ball.gameObject.transform.position = new Vector3(0f, 40f, 0f);
                reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                reward_img_Ball.SetNativeSize();
                reward_img_Ball.transform.localScale = new Vector3(2f, 2f, 1f);
                switch (type)
                {
                    case 2005:
                        reward_text.text = "Cherry blossom";
                        break;
                    
                    case 2007:
                        reward_text.text = "Note";
                        break;
                }
 
                // 볼명 지정 
            }
            
            
            // Step 3. 실질적으로 잠금해제 해주기 

            IAlarmMediator mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
            // 아바타 잠금 해제
            if (type / 1000 == 1)
            {
                if (type == 1000) // 베이비드라이버 인 경우 
                {
                    PlayerPrefs.SetInt("AVATAR_" + type.ToString(), 1);
                    mediator.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, type);

                    PlayerPrefs.SetInt("BALL_1008", 1);
                    mediator.Event_Receieve(Event_Alarm.BALL_ALARM_ON,1008);
                }

                else // 드라큘라 인 경우
                {
                    PlayerPrefs.SetInt("AVATAR_9",1);
                    mediator.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON, 9);

                    PlayerPrefs.SetInt("BALL_2006", 1);
                    mediator.Event_Receieve(Event_Alarm.BALL_ALARM_ON,2006);
                    
                }
            }

            else if (type/1000 == 2)
            {
                PlayerPrefs.SetInt("BALL_" +type.ToString(), 1);
                mediator.Event_Receieve(Event_Alarm.BALL_ALARM_ON,type);
            }
            // Step 1. Grid Set
            // Step 3. 알람 띄워주기 
        }

        public void OnClick_Special_Panel_Close()
        {
            data.Set_is_get(which_item_world);
            ballPanel.SetActive(false);
            if (which_item_world == 3) // 드라큘라 공을 획득할 경우, 연출을 보여줌 
            {
                secondPage.transform.localScale = Vector3.zero;
                DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 1f, 0.7f)
                    .OnComplete(() =>
                    {
                        leftBtn.SetActive(true);
                    });
                secondPage.transform.DOScale(new Vector3(1f, 1f, 1f), 0.7f)
                    .SetEase(Ease.OutCubic);
            }
        }
        
        #region  Commodity_Flight
        public IEnumerator Get_Gem(int gem , int which_item)
        {
            int index = which_item;
            for (int i = 0; i < gem; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Gem_Start_pos();
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
        private Vector2 Gem_Start_pos()
        {
            Vector2 target_pos = Vector2.zero;
            // 화면상 가운데 지점 
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
