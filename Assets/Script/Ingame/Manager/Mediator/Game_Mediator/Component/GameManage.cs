
using System.Collections;
using Ad;
using UnityEngine.UI;
using Challenge;
using Data;
using Ingame_Data;
using Manager;
using Score;
using Timer;
using Tutorial;
using UnityEngine;
using Warn;

namespace Ingame
{
    public class GameManage : MonoBehaviour, IMediator
    {

        [Header("User_Condition")] private bool is_newBlock;
        private bool is_Die;
        private bool gameOver;
        private bool charater_move;
        private bool get_all_charater;
        private bool tutorial = false;
        private bool ad_on;
        public GameObject tutorial_obj;
        [SerializeField] private Tutorial_Basic tutorial_control;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private GameObject revive;
        [SerializeField] private GameObject gameOver_Panel;
        [SerializeField] private BonusCharater _bonusCharater;
        [SerializeField] private SettingManager _settingManager;

        [Header("Img_Manage")] private IComponent img_manage;
        [SerializeField] private Transform img_tr;
        public GameObject newBlockInfo;

        private IComponent ground;
        [SerializeField] private Transform groundTR;

        [Header("Component_BOX")] [SerializeField]
        private Transform blockComponent;

        private IComponent determine_type; // 박스의 리스폰을 담당하는 Component
        private IComponent movedown;
        private IComponent removeBox;

        [Header("Component_Ball")] [SerializeField]
        private Transform ballComponent;

        private IComponent ballManage;
        private IComponent launchManage;
        private IComponent charater;
        private IComponent speedControl;

        [SerializeField] private DataManager dataManager;
        [SerializeField] private QuestManager _questManager;

        [Header("Exit")] public GameObject pausePanel;
        public GameObject exit_check;
        public GameObject pause_exitCheck;

        [Header("Skill")] [SerializeField] private Determine_AvatarSkill _avatarSkill;
        private bool skill_set = false;

        [Header("Tutorial")] [SerializeField] private GameObject tutorial_Candle;
        [SerializeField] private GameObject tutorial_Charater;
        [SerializeField] private GameObject tutorial_Pinata;
        [SerializeField] private GameObject tutorial_basic;
        [SerializeField] private GameObject tutorial_background;

        [Header("Warn_Sign")] [SerializeField] private Ingame_Warn warn;

        [Header("Item_Tutorial")] [SerializeField]
        private Button pauseBtn;

        [SerializeField] private GameObject tutorial_item;
        public GameObject itemBtn;

        [SerializeField] private Ad_Mediator ad;
        private float timescale;

        [Header("Subject")] [SerializeField] private SaveIngame_Subject saveGame_Obj;
        [Header("Load_GameData")] private bool is_StillGame;
        [SerializeField] private SaveIngame_Subject gameObserver;
        private void Awake()
        {
            determine_type = blockComponent.gameObject.GetComponent<IComponent>();
            movedown = blockComponent.GetChild(2).gameObject.GetComponent<IComponent>();
            removeBox = blockComponent.GetChild(3).gameObject.GetComponent<IComponent>();
            determine_type.Set_Mediator(this);
            movedown.Set_Mediator(this);
            removeBox.Set_Mediator(this);
            Application.targetFrameRate = 60;
            for (int i = 0; i < ballComponent.childCount; i++)
            {
                IComponent component = ballComponent.GetChild(i).gameObject.GetComponent<IComponent>();
                component.Set_Mediator(this);
                switch (i)
                {
                    default:
                        ballManage = component;
                        break;

                    case 1:
                        launchManage = component;
                        break;

                    case 2:
                        speedControl = component;
                        break;
                }
            }


            ground = groundTR.gameObject.GetComponent<IComponent>();
            ground.Set_Mediator(this);
            img_manage = img_tr.gameObject.GetComponent<IComponent>();
            img_manage.Event_Occur(Event_num.BGM_SET_START);
            if (PlayerPrefs.GetInt("Tutorial_Item_End", 0) == 0)
                itemBtn.SetActive(false);

        }
        
        private void OnApplicationQuit()
        {
            FirstPurchase_Timer.Save_Data();
        }

        public void Party_Set(IComponent component)
        {
            GameObject charater_TR = GameObject.FindWithTag("Player");
            charater = component;
            charater.Set_Mediator(this);
        }

        #region Exit

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pausePanel.activeSelf)
                {
                    timescale = Time.timeScale;
                    Time.timeScale = 0;
                    exit_check.SetActive(true);
                }

                else
                {
                    pause_exitCheck.SetActive(true);
                }
            }
        }

        public void OnClick_Home()
        {
            ad.Destroy_Banner();
            _questManager.Set_User_Stat_Save();
            Application.Quit();
        }


        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && !is_Die)
                pausePanel.SetActive(true);

        }

        public void Set_Ad_value(bool is_on)
        {
            if (is_on)
                ad_on = true;

            else
                ad_on = false;
        }


        public void OnClick_Exit_no()
        {
            exit_check.SetActive(false);
            if (!pausePanel.activeSelf)
                Time.timeScale = timescale;
        }


        #endregion

        private void Start()
        {
            Tutorial_Candle_Data.Set_Data();
            if (!Tutorial_Candle_Data.Get_Done_Data() || PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 0)
            {
                tutorial_obj.SetActive(true);
                tutorial_background.SetActive(false);
            }

            if (PlayerPrefs.GetInt("Tutorial_Basic_Ingame", 0) == 0)
            {
                tutorial_obj.SetActive(true);
                tutorial = true;
                tutorial_basic.SetActive(true);
                tutorial_background.SetActive(true);
                Event_Receive(Event_num.Tutorial_INIT);
            }

            else
            {
                if (PlayerPrefs.GetInt("Still_Game", 0) == 1) // 게임 중간에 나갔을 경우 
                {
                    is_StillGame = true;
                    gameObserver.Load_Game();
                    // 이어하기 동작하게 만들어야함  
                }

                else // 첨 부터 시작인 경우 
                {
                    is_StillGame = false;
                    Event_Receive(Event_num.INIT_DATA);
                }
            }
        }

        #region Box

        private void Respawn_Box()
        {
            ballManage.Event_Occur(Event_num.BOX_SPAWN);
            _bonusCharater.Do_BonusCheck();
            if (!_bonusCharater.Get_BonusCheck())
            {
                determine_type.Event_Occur(Event_num.BOX_SPAWN); // # 2. 박스 생성 
                movedown.Event_Occur(Event_num.MOVE_DOWN); // # 3. 내려보냄 
            }
        }

        public void OnClick_Down()
        {
            Event_Receive(Event_num.Abort_Launch);
        }

        private void Determine_Die()
        {
            skill_set = false;
            if (is_Die)
            {
                warn.TurnOff_Warn();
                if (dataManager.Get_Extra_Life())
                {
                    removeBox.Event_Occur(Event_num.BADGE_BOX_REMOVE);
                    _settingManager.ItemBtn_LaunchSet(true);
                    is_Die = false;
                    if (is_newBlock)
                    {
                        newBlockInfo.SetActive(true);
                        is_newBlock = false;
                    }
                }
                else
                {
                    if (gameOver)
                    {
                        if (!Noads_instance.Get_Is_Noads() && !Noads_instance.Get_Is_Noads_New())
                        {
                            IMediator _mediator = GameObject.FindWithTag("adcontrol").GetComponent<IMediator>();
                            _mediator.Event_Receive(Event_num.USER_DIE);
                        }

                        else
                        {
                            gameOver_Panel.SetActive(true);
                        }
                    }

                    else
                    {
                        PlayerPrefs.SetInt("Still_Game", 0); // 부활하지 않으면 revive 안되게 하기 위함 (revive창 도중 나갔을 떄의 처리)) 
                        revive.SetActive(true);
                        gameOver = true;
                    }
                }
            }

            else
            {
                warn.Set_Animation(); // 경고 애니메이션 켜줄지 말지 결정함 
                if (is_newBlock)
                {
                    newBlockInfo.SetActive(true);
                    is_newBlock = false;
                }

                else
                {
                    determine_type.Event_Occur(Event_num.SET_NEW_STAGE);
                }

                Event_Receive(Event_num.Launch_Green); // 발사 가능 상태로 전환하기 
                if (is_StillGame && !tutorial)
                {
                    saveGame_Obj.notifyObserver(); // 정보를 저장하라고 신호를 보냄 
                    PlayerPrefs.SetInt("Still_Game", 1);
                }   
                else if (!is_StillGame) // 첨 시작일 경우, Flag만 활성화 시켜줌. 정보를 불러오는 경우에는 켜지면 안되자너. 
                    is_StillGame = true;
                

                if (Tutorial_Candle_Data.IsPanelOpen())
                    tutorial_Candle.SetActive(true);

                _avatarSkill.Determine_Skill_Active();
            }

        }

        #endregion

        public void Event_Receive(Event_num eventNum)
        {
            if (tutorial)
            {
                switch (eventNum)
                {
                    case Event_num.CHARATER_ARRIVE:
                        charater_move = false;
                        break;

                    case Event_num.CHARATER_MOVE:
                        tutorial_control.StartCoroutine(tutorial_control.Launch_Done());
                        break;


                    case Event_num.Tutorial_Basic_Done:
                        tutorial = false;
                        PlayerPrefs.SetInt("Tutorial_Basic_Ingame", 1);
                        ballManage.Event_Occur(Event_num.Tutorial_First);
                        launchManage.Event_Occur(Event_num.Tutorial_Basic_Done);
                        determine_type.Event_Occur(Event_num.INIT_DATA);
                        movedown.Event_Occur(Event_num.MOVE_DOWN);
                        break;

                    case Event_num.Tutorial_INIT:
                        ballManage.Event_Occur(Event_num.INIT_DATA);
                        launchManage.Event_Occur(Event_num.INIT_DATA);
                        launchManage.Event_Occur(Event_num.Launch_Red); // 발사가 안되게 막아놓음 
                        determine_type.Event_Occur(Event_num.Tutorial_INIT);
                        break;

                }
            }

            else
            {
                switch (eventNum)
                {

                    // 볼을 강제로 내리는 버튼을 누르면 호출되는 이벤트 
                    case Event_num.BALL_DOWN:
                        speedControl.Event_Occur(eventNum);
                        break;
                    
                    // 새로운 블록이 리스폰 되면 호출되는 이벤트 
                    case Event_num.New_block:
                        is_newBlock = true;
                        break;

                    // 박스가 내려오는 이벤트가 발생하면 호출되는 이벤트. 
                    case Event_num.MOVE_DOWN:
                        movedown.Event_Occur(eventNum);
                        break;

                    // 박스 및 필드형 아이템이 이동하는게 끝나면 호출되는 이벤트.
                    case Event_num.MOVE_DONE:
                        _questManager.Set_User_Stat_Save();
                        if (PlayerPrefs.GetInt("Tutorial_Item_End", 0) == 0 &&
                            PlayerPrefs.GetInt("Tutorial_Pinata_Done", 0) == 1)
                            tutorial_item.SetActive(true);

                        Determine_Die(); // GameOver 여부를 판단해주는 함수 호출 
                        break;

                    // 박스와 플러스볼, 필드 아이템을 스폰함 
                    case Event_num.BOX_SPAWN:
                        speedControl.Event_Occur(eventNum);
                        _scoreManager.Set_Count_Zero();
                        warn.Set_Flag(false);
                        StartCoroutine(Spawn_Box());
                        break;

                    // 처음 게임에 들어오면 (이어하기 말고, 최초 진입시) 호출되는 이벤트 
                    case Event_num.INIT_DATA:
                        ballManage.Event_Occur(Event_num.INIT_DATA);
                        launchManage.Event_Occur(Event_num.INIT_DATA);
                        determine_type.Event_Occur(Event_num.INIT_DATA);
                        movedown.Event_Occur(Event_num.MOVE_DOWN);
                        break;
                    
                    // 발사가 가능하도록 하는 이벤트 
                    case Event_num.Launch_Green:
                        _settingManager.ItemBtn_LaunchSet(true);
                        launchManage.Event_Occur(eventNum);
                        ground.Event_Occur(eventNum);
                        break;

                    // 발사를 막는 이벤트
                    case Event_num.Launch_Red:
                        _settingManager.ItemBtn_LaunchSet(false);
                        launchManage.Event_Occur(Event_num.Launch_Red);
                        break;

                    // 캐릭터가 발사 모션을 취하게 되면 호출되는 이벤트 
                    case Event_num.Launch_MOTION:
                        skill_set = true;
                        _settingManager.ItemBtn_LaunchSet(false);
                        launchManage.Event_Occur(eventNum);
                        ground.Event_Occur(eventNum);
                        speedControl.Event_Occur(eventNum);
                        break;

                    // 캐릭터가 공이 떨어진 지점에 도착하면 호출되는 이벤트. 
                    case Event_num.CHARATER_ARRIVE:
                        charater_move = false;
                        break;

                    // 캐릭터가 이동을 시작하면 호출되는 이벤트. 캐릭터가 이동 중 공이 발사가 되는것을 방지하기 위함
                    case Event_num.CHARATER_MOVE:
                        charater_move = true;
                        break;

                    // 아바타의 스킬 유무를 지정해주는 이벤트 
                    case Event_num.AVATAR_SET:
                        charater.Event_Occur(eventNum);
                        break;

                    // 발사 위치를 지정해주는 이벤트 
                    case Event_num.SET_LAUNCH_INFO:
                        ballManage.Event_Occur(eventNum);
                        break;

                    // 부활이벤트가 일어나는 경우 호출되는 이벤트. 
                    case Event_num.SET_REVIVE:
                        // 부활 됐다고 알려줌 
                        removeBox.Event_Occur(eventNum);
                        if (is_newBlock) // 만약 처음 생성되는 블록이 있다면.. 
                        {
                            newBlockInfo.SetActive(true);
                            is_newBlock = false;
                        }

                        else
                            determine_type.Event_Occur(Event_num.SET_NEW_STAGE);

                        break;

                    // 유저가 한 번 죽게되면 호출되는 이벤트. 
                    case Event_num.USER_DIE:
                        is_Die = true;
                        break;

                    // 유저가 크로스아이템을 쓸 때 호출되는 이벤트. 
                    case Event_num.CROSS_ITEM:
                        determine_type.Event_Occur(eventNum);
                        break;

                    // 부활 후, 상단 3개행의 박스를 부시기 위한 이벤트 
                    case Event_num.BOX_REMOVE:
                        gameObserver.gameObject.transform.GetChild(0).gameObject.GetComponent<Observer_StageInfo>().Set_Revive();
                        PlayerPrefs.SetInt("Still_Game", 1);
                        is_Die = false;
                        removeBox.Event_Occur(eventNum);
                        break;

                    // 피냐타가 필드에서 파괴되면 실행되는 이벤트 
                    case Event_num.PINATA_DIE:
                        _questManager.Set_Pinata();
                        speedControl.Event_Occur(eventNum);
                        determine_type.Event_Occur(eventNum);
                        _bonusCharater.Bonus_Extinguish();
                        warn.TurnOff_Warn();
                        break;

                    // 피냐타가 필드에 스폰되면 실행되는 이벤트 
                    case Event_num.PINATA_SPAWN:
                        determine_type.Event_Occur(eventNum);
                        _scoreManager.Set_Count_Zero();
                        movedown.Event_Occur(Event_num.MOVE_DOWN); // # 3. 내려보냄 
                        break;

                    // 피냐타가 파괴된 후, 장난감을 얻을 때 실행되는 이벤트 
                    case Event_num.COLLECTION_GET:
                        img_manage.Event_Occur(Event_num.BGM_SET_START); // 이미지 변환 
                        speedControl.Event_Occur(Event_num.BOX_SPAWN);
                        _scoreManager.Set_Count_Zero();
                        StartCoroutine(Spawn_Box());
                        break;

                    // 피냐타가 있을 때 발사를 중지하면 호출되는 함수. 
                    case Event_num.Abort_Launch_PINATA:
                        if (PlayerPrefs.GetInt("Tutorial_Item_end", 0) == 0) // 아이템 튜토리얼 시작 안했을 때,
                            pauseBtn.interactable = false; // pause 버튼 못 쓰게 막음 

                        speedControl.Event_Occur(Event_num.Abort_Launch_PINATA);
                        launchManage.Event_Occur(eventNum);

                        break;
                    
                    // 발사 중지시 호출되는 함수 
                    case Event_num.Abort_Launch:
                        speedControl.Event_Occur(Event_num.Abort_Launch);
                        launchManage.Event_Occur(eventNum);
                        break;

                    case Event_num.SET_NEW_STAGE:
                        determine_type.Event_Occur(eventNum);
                        break;
                    
                    case Event_num.BEAR_SKILL:
                        _avatarSkill.Bear_Skill();
                        break;

                    case Event_num.TUTORIAL_CHAR:
                        tutorial_Charater.gameObject.SetActive(true);
                        break;

                    case Event_num.TUTORIAL_PINATA:
                        tutorial_Pinata.SetActive(true);
                        break;
                    
                    case Event_num.SET_GAMEOVER:
                        gameOver = true;
                        break;
                }
            }
        }

        IEnumerator Spawn_Box()
        {
            if (!charater_move)
            {
                Respawn_Box();
                yield return null;
            }

            else
            {
                while (true)
                {
                    if (!charater_move)
                        break;
                    yield return null;
                }

                Respawn_Box();
                yield return null;
            }

        }

    }
}
