using System;
using System.Collections;
using Alarm;
using Avatar;
using Badge;
using Collection;
using Data;
using Progetile;
using Skin;
using UnityEngine;
using UnityEngine.UI;


namespace Challenge
{
    public class Quest_LevelUP_Reward : MonoBehaviour
    {
        
        [Header("UI_Info")] [SerializeField] private Quest_Table_UI questTable;

        [Header("Commodity_info")] 
        public Text gem_text;
        public Text candy_text;

        [Header("Commodity_UI")] public GameObject gem_obj;
        public GameObject candy_obj;
        public Animator gem_animator, candy_animator;
        private int gem, candy;

        [Header("Lobby_candy")] public Text lobbyCandy;

        [Header("reward_panel")] [SerializeField]
        private GameObject reward_panel;
        public Text reward_text;
        public Image reward_img;

        public AudioSource gemFlying;
        public AudioSource click;
        public AudioSource reward_get;
        
        [SerializeField] private Badge_Data _badgeData;
        private IAlarmMediator _mediator;
        private void OnDisable()
        {
            lobbyCandy.text =  String.Format("{0:#,0}", Playerdata_DAO.Player_Candy());  
        }

        private void Awake()
        {
            _mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
        }

        public void Set_Commodity()
        {
            gem = Playerdata_DAO.Player_Gem();
            candy = Playerdata_DAO.Player_Candy();
        }

        public void OnClick_Reward(int index)
        {
            int quantity = 0;
            int type = questTable.Get_Reward_Type(ref quantity, index,true);  // Quantity, type을 받아옴 
            switch (type)
            {
                default:
                    Set_Reward_Panel(type); // Step 1. 패널 Set 해줌 
                    reward_panel.SetActive(true);
                    break;
                
                case 0:
                    StartCoroutine(Candy_Animation(quantity, index));
                    break;
                
                case 1:
                    StartCoroutine(Gem_Animation(quantity,index));
                    break;
            }
            _mediator.Event_Receieve(Event_Alarm.QUEST_REWARD_FALSE);
        }
        // 이 Region은 젬, 캔디 애니메이션을 담당함 
        #region Animation
        IEnumerator Candy_Animation(int quantity, int index)
        {
            Playerdata_DAO.Set_Player_Candy(quantity);
            gemFlying.Play();
            for (int i = 0; i < quantity; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Set_Position(index);
                GameObject obj = Instantiate(candy_obj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                obj.transform.localScale = new Vector2(1f, 1f);
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.candy_animator;
                script.start_pos = start_pos;
                script.gem_text = this.candy_text;
                script.arrive += candy_set_text;
                script.Target_Pos = new Vector2(-103f, 861f);
            }

            yield return null;
        }

        IEnumerator Gem_Animation(int quantiy, int index)
        {
            Playerdata_DAO.Set_Player_Gem(quantiy);
            gemFlying.Play();
            for (int i = 0; i < quantiy; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Set_Position(index);
                GameObject obj = Instantiate(gem_obj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gem_animator;
                script.start_pos = start_pos;
                script.gem_text = this.gem_text;
                script.arrive += gem_set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }

            yield return null;
  
        }

        #region Reward_Panel
        private void Set_Reward_Panel(int type)
        {
            Avatar_Name name_data = new Avatar_Name();
            reward_img.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            reward_get.Play();
            // Step 1. 이미지 바꾸어주기 
            if (type >= 10 && type < 3000)
            {
                reward_img.sprite = Set_Avatar_UI.Set_Avatar_Img(Calc_Index.Get_Avatar_index(type));
                Skin_Log.Buy_Avatar(type);
                reward_img.rectTransform.rect.Set(0,28,120f,120f);
                reward_img.SetNativeSize();
            }
            else
            {
                reward_img.rectTransform.rect.Set(0,28,144f,144f);
                reward_img.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                reward_img.SetNativeSize();
                reward_img.transform.localScale = new Vector3(3f, 3f, 1f);
                Skin_Log.Buy_Ball_Log(type);
            }
            
            // Step 2. 이름 적용하기 
            if (type >= 10 && type < 3000)
            {
                reward_text.text = name_data.Set_Charater_Name(type,false);
            }
            else
            {
                switch (type)
                {
                    default:
                        break;
                    
                    case 3000:
                        reward_text.text = "Dalgona";
                        break;
                    
                    case 3001:
                        reward_text.text = "Toe Beans";
                        break;
                         
                    case 3002:
                        reward_text.text = "Smile";
                        break;
                    
                    case 3003:
                        reward_text.text = "Cute Devil";
                        break;
                    
                    case 3004:
                        reward_text.text = "Rose";
                        break;
                }
            }
            
            // Step 3. 실질적으로 잠금해제 해주기 

            IAlarmMediator mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
            // 아바타 잠금 해제
            if (type >= 10 && type < 3000)
            {
                PlayerPrefs.SetInt("AVATAR_" +type.ToString(), 1);
                mediator.Event_Receieve(Event_Alarm.AVATAR_ALARM_ON,type);
            }

            else
            {
                PlayerPrefs.SetInt("BALL_" +type.ToString(), 1);
                _badgeData.Set_Ball_Buy();
                mediator.Event_Receieve(Event_Alarm.BALL_ALARM_ON,type);
            }
            // Step 1. Grid Set
            // Step 3. 알람 띄워주기 

        }

        public void OnClick_Get_Btn()
        {
            click.Play();
            reward_panel.SetActive(false);
        }
        #endregion

        
        /// <summary>
        /// 애니메이션 하는 친구들의 시작 위치를 지정해줌 
        /// </summary>
        /// <returns></returns>
        private Vector2 Set_Position(int index)
        {
            Vector2 pos;
            switch (index)
            {
                default:
                    pos = new Vector2(-325f, -283f);
                    break;
                
                case 1:
                    pos = new Vector2(-168f, -283f);
                    break;
                
                case 2:
                    pos = new Vector2(-8f, -283f);
                    break;
                
                case 3:
                    pos = new Vector2(152f, -283f);
                    break;
                
                case 4:
                    pos = new Vector2(314f, -283f);
                    break;
                
                case 5:
                    pos = new Vector2(-325f, -489f);
                    break;
                
                case 6:
                    pos = new Vector2(-168f, -489f);
                    break;
                
                case 7:
                    pos = new Vector2(-8f, -489f);
                    break;
                
                case 8:
                    pos = new Vector2(152f, -489f);
                    break;
                
                case 9:
                    pos = new Vector2(314f, -489f);
                    break;
            }
            return pos;
        }
        
        private void gem_set_text(object sender, EventArgs e)
        {
            gem += 1;
            gem_text.text = string.Format("{0:#,0}", gem);
        }

        private void candy_set_text(object sender, EventArgs e)
        {
            candy += 1;
            candy_text.text = string.Format("{0:#,0}", candy);
        }
        #endregion


    }
}
