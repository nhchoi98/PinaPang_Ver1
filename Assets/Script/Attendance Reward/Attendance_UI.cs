using System;
using System.Collections;
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

        [Header("Reward_Panel")] 
        public Image reward_img_Ball;
        public Image reward_img_charater;
        
        public Text reward_text;
        public AudioSource gemSound;
        
        private const int ITEM_NUM = 8;
        private int which_item;
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
        
        /// <summary>
        /// 2배 보상 버튼 -> isGem = true
        /// </summary>
        /// <param name="isGem"></param>
        private void Get_Reward()
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
            switch (which_item)
            {
                default:
                    adScript.Set_Index(which_item);
                    Set_GemReward_Panel();
                    break;
                
                case 1: // 벛꽃공 획득 
                    ballDao = new BallPurDAO(3010);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3010);
                    Set_Special_Panel(3010);
                    break;
                
                case 3: // 드라큘라 셋 획득 . Set_Special Panel 매개변수 변경해야함
                    avatarDao = new IsLockedDAO(9);
                    avatarDao.Set_Locked_Condition();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Avatar(9);

                    ballDao = new BallPurDAO(3011);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(3011);
                    Set_Special_Panel(1011);// 보상 패널 Set

                    break;

                case 5: // 음표공 획득 
                    ballDao = new BallPurDAO(3011);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Set_Special_Panel(3011);
                    Skin_Log.Buy_Ball_Log(3011);
                    break;
                
                case 7: // 아바타 획득 + 베이비 드라이버 세트공 연결해야함. 
                    avatarDao = new IsLockedDAO(1000);
                    avatarDao.Set_Locked_Condition();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Avatar(1000);

                    ballDao = new BallPurDAO(1008);
                    ballDao.Purchase();
                    _badgeData.Set_Ball_Buy();
                    Skin_Log.Buy_Ball_Log(1008);
                    Set_Special_Panel(1000);
                    break;
            }
        }

        private void Set_GemReward_Panel()
        {
            int gem;
            Sprite firstimg = gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject
                .GetComponent<Image>().sprite;
            
            Sprite secondImg = gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject
                .GetComponent<Image>().sprite;
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
            gemReward_Panel.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = (gem * 2).ToString();
            
            // # 3. 젬의 이미지를 바꾸어줌 
            if (gem < 15)
            {
                firstimg = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_1");
                secondImg = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_2");
            }
            
            else if (gem >= 15 && gem < 30)
            {
                firstimg  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_2");
                secondImg  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_3");

            }
            else if (gem >= 30 && gem < 60)
            {
                firstimg  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_3");
                secondImg  = Resources.Load<Sprite>("Lobby/Shop/Goods/gem_img/Shop_Gem_4");
            }

            gemReward_Panel.SetActive(true);
        }

        /// <summary>
        /// 2배 버튼 안누르고 그냥 젬 얻는 경우 
        /// </summary>
        public void Get_Gem(bool is_Doubled = false)
        {
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

            if(is_Doubled) reward_text.text = (gem*2).ToString();
            
            else
                reward_text.text = gem.ToString();
            
            if (!is_Doubled)
            {
                Playerdata_DAO.Set_Player_Gem(gem);
                Set_Gem_ResultPanel(false,gem);
            }

            else
            {
                Playerdata_DAO.Set_Player_Gem(gem*2);
                Set_Gem_ResultPanel(true,gem);
            }
            
        }

        private void Set_Gem_ResultPanel(bool isDoubled, int rewardType)
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
                StartCoroutine(Get_Gem(gem));

            else
                StartCoroutine(Get_Gem(gem * 2));
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
                    reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                    reward_img_Ball.SetNativeSize();
                    reward_img_Ball.transform.localScale = new Vector3(1f, 1f, 1f);
                    
                }

                else // 드라큘라 Set인 경우 
                {
                    reward_img_charater.sprite = Set_Avatar_UI.Set_Avatar_Img(9);
                    reward_img_charater.SetNativeSize();
                    reward_img_charater.gameObject.SetActive(true);
                    
                    // Step 2. ball 이미지의 지정 및 크기 지정해주기. 
                    reward_img_Ball.rectTransform.rect.Set(0,20,48f,48f);
                    reward_img_Ball.gameObject.transform.position = new Vector3(121f, -20f, 0f);
                    reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                    reward_img_Ball.SetNativeSize();
                    reward_img_Ball.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                
                reward_text.text =( name_data.Set_Charater_Name(type,false)+ " Set"); // 보상 타이틀명 지정 
                
            }
            
            else if(type/ 1000 == 3) // 볼만 받는 경우 
            {
                reward_img_charater.gameObject.SetActive(false);
                reward_img_Ball.rectTransform.rect.Set(0,28,144f,144f);
                reward_img_Ball.gameObject.transform.position = new Vector3(0f, 40f, 0f);
                reward_img_Ball.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                reward_img_Ball.SetNativeSize();
                reward_img_Ball.transform.localScale = new Vector3(3f, 3f, 1f);
                switch (type)
                {
                    case 3010:
                        reward_text.text = "Cherry blossom";
                        break;
                    
                    case 3011:
                        reward_text.text = "Note";
                        break;
                }
 
                // 볼명 지정 
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
            data.Set_is_get(which_item);
            ballPanel.SetActive(false);
        }
        
        #region  Commodity_Flight
        public IEnumerator Get_Gem(int gem = 10)
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
