
using Alarm;
using Data;
using UnityEngine.UI;
using UnityEngine;

namespace Badge
{
    public class Badge_UI : MonoBehaviour
    {
        [Header("Badge_Data")]
        private int MAX_INDEX;
        [SerializeField] private Badge_Data badgeData;
        private Calc_Badge_Index calcBadgeIndex;

        [Header("UI")] 
        public GameObject detail_panel;
        public GameObject locked_Condition;
        public Text badge_name;
        public Text badge_ability;
        public Text badge_condition_title;
        public Text badge_condition;
        public Image badge_img;
        public Image tier_img;
        public Transform badge_TR;

        [Header("Tier_img")] public Sprite normal, rare, unique;
        public AudioSource click;
        private IAlarmMediator _mediator;

        // # 1. 클릭 액션을 담당하는 함수  (클릭 시 UI에 표시할 정보 Update) 
        // # 2. 데이터 초기화를 담당하는 함수  (잠금 여부, 달성 정도 업데이트 분리하기) 
        // # 3. 

        /// <summary>
        /// 처음 패널을 열면, 데이터를 초기화함. 
        /// </summary>
        private void Awake()
        {
            calcBadgeIndex = new Calc_Badge_Index();
            MAX_INDEX = calcBadgeIndex .Get_Badge_MaxIndex();
            _mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < MAX_INDEX; i++)
            {
                if (badgeData.Get_achi_data(i))
                    badge_TR.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                // 뒷 판떼기 추가되면 여기도 색깔 바뀌어야함 

                else badge_TR.GetChild(i).gameObject.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1f);
                //뒷 판떼기 추가되면 여기도 색깔 바뀌어야함

            }
        }

        public void OnClick_Badge(int index)
        {
            click.Play();
            int value = calcBadgeIndex.Get_Badge_Num(index);
            // Step 1. 뱃지의 획득 유무 조회 넘겨주기  
            // Step 2. 뱃지의 이미지 세팅 
            Set_Badge_img(index);
            // Step 3. 뱃지의 달성 정도 넘겨줌 
            Set_Target_Title(value);
            Set_Ability_Condition(value);
            // Step 4. 뱃지의 Ability 텍스트 세팅해주기 
            Set_Ability_Text(value);
            // Step 5. 뱃지의 tier에 따라서 이미지 변경해주기 
            Set_Tier_img(value);
            // Step 6. 뱃지의 이름 넘겨주기 
            Set_Badge_Name(value);
            _mediator.Event_Receieve(Event_Alarm.BADGE_ALARM_FALSE,index);
            detail_panel.SetActive(true);
        }

        #region Set_Detail_Panel

        private void Set_Badge_img(int index)
        {
            // Step 1. 장착 유무에 따라서 회색조 표시 유무 표시하기
            if (!badgeData.Get_achi_data(index)) badge_img.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            else badge_img.color = new Color(1f, 1f, 1f, 1f);

            // Step 2. 뱃지 이미지 불러오기. 
            Get_Badge_img(calcBadgeIndex.Get_Badge_Num(index));
        }

        private void Get_Badge_img(int value)
        {
            string path = "Lobby/Badge/Badge_Img/" + value.ToString();
            Sprite img = Resources.Load<Sprite>(path);
            badge_img.sprite = img;
        }

        private void Set_Tier_img(int value)
        {
            switch (value / 1000)
            {
                default:
                    tier_img.sprite = normal;
                    break;
                
                case 1:
                    tier_img.sprite = rare;
                    break;
                
                case 2:
                    tier_img.sprite = unique;
                    break;
            }
        }

        private void Set_Ability_Text(int value)
        {
            switch (value)
            {
                default:
                    badge_ability.text = "BONUS SCORE +5%";
                    break;
                
                case 1:
                    badge_ability.text = "Candle Box +3%";
                    break;
                
                case 2:
                    badge_ability.text = "Candle Box +5%";
                    break;
                
                case 3:
                    badge_ability.text = "Ball Speed + 10%";
                    break;
                
                case 4:
                    badge_ability.text = "Item Duration + 60s";
                    break;
                
                case 5:
                    badge_ability.text = "Candle Box +10%";
                    break;
                
                case 1000:
                    badge_ability.text = "Ball Speed + 20%";
                    break;
                
                case 1001:
                    badge_ability.text = "BONUS SCORE + 10%";
                    break;
                
                case 1002:
                    badge_ability.text = "Item Duration +120s";
                    break;
                
                case 2000:
                    badge_ability.text = "Ball Speed + 30%";
                    break;
                
                case 2001:
                    badge_ability.text = "Life + 1";
                    break;
                
                case 2002:
                    badge_ability.text = "BONUS SCORE + 20%";
                    break;
            }
            
        }

        private void Set_Badge_Name(int value)
        {
            switch (value)
            {
                default:
                    badge_name.text = "Gummy Bear";
                    break;
                
                case 1:
                    badge_name.text = "Chocolate";
                    break;
                
                case 2:
                    badge_name.text = "Birthday Cake";
                    break;
                
                case 3:
                    badge_name.text = "Bonbon Candy";
                    break;
                
                case 4:
                    badge_name.text = "Chocolate Muffin";
                    break;
                
                case 5:
                    badge_name.text = "Choco Doughnut";
                    break;
                
                case 1000:
                    badge_name.text = "Sweet Candy";
                    break;
                
                case 1001:
                    badge_name.text = "Orange Jelly Cup";
                    break;
                
                case 1002:
                    badge_name.text = "Cream Muffins";
                    break;            
                
                case 2000:
                    badge_name.text = "Spiral Lollipop";
                    break;
                
                case 2001:
                    badge_name.text = "Chocochip Cookie";
                    break;
                
                case 2002:
                    badge_name.text = "Jell-O";
                    break;
                
            }
        }

        private void Set_Target_Title(int value)
        {
            switch (value)
            {
                default:
                    badge_condition_title.text = "Login for 1 day";
                    break;
                
                case 1:
                    badge_condition_title.text = "Remove 10 pinatas";
                    break;
                
                case 2:
                    badge_condition_title.text = "Remove 30 pinatas";
                    break;
                
                case 3:
                    badge_condition_title.text = "Get 10 balls";
                    break;
                
                case 4:
                    badge_condition_title.text = "Use item 10 times";
                    break;
                
                case 5:
                    badge_condition_title.text = "Remove 70 pinatas";
                    break;
                
                case 1000:
                    badge_condition_title.text = "Get 20 balls";
                    break;
                
                case 1001:
                    badge_condition_title.text = "Achieve 500,000 Score";
                    break;
                
                case 1002:
                    badge_condition_title.text = "Use Item 20 Times";
                    break;
                
                case 2000:
                    badge_condition_title.text = "Get 30 balls";
                    break;
                
                case 2001:
                    badge_condition_title.text = "Revive 30 times";
                    break;
                
                case 2002:
                    badge_condition_title.text = "Achieve 3,000,000 Score";
                    break;
                
            }
        }

        private void Set_Ability_Condition(int value)
        {
            // Step 1. Target_num 조회
            int target_num = Get_Ability_Target(value);
            
            // Step 2. 이미 획득했다면, 다 달성한걸로 처리. 잠금 이미지 안보이게.
            if (badgeData.Get_achi_data(calcBadgeIndex.Get_Badge_index(value)))
            {
                badge_condition.text = "(" + string.Format("{0:#,0}",target_num) + "/" + string.Format("{0:#,0}",target_num) + ")";
                locked_Condition.SetActive(false);   
            }
            
            // Step 3. 획득 안했다면, 뒤에 숫자 카운트.
            else
            {
                int num = Get_Ability_Now(value);
                badge_condition.text = "(" +  string.Format("{0:#,0}",num) + "/" + string.Format("{0:#,0}",target_num) + ")";
                locked_Condition.SetActive(true);
            }
        }

        private int Get_Ability_Target(int value)
        {
            switch (value)
            {
                
                case 0:
                    return 1;
                
                case 1:
                    return 10;
                
                case 2:
                    return 30;
                
                case 3:
                    return 10;
                
                case 4:
                    return 10;
                
                case 5:
                    return 70;
                
                case 1000:
                    return 20;
                
                case 1001:
                    return 500000;
                
                case 1002:
                    return 20;
                
                case 2000:
                    return 30;
                
                case 2001:
                    return 30;
                
                case 2002:
                    return 3000000;
                
            }

            return -1;
        }

        private int Get_Ability_Now(int value)
        {
            switch (value)
            {
                case 0:
                case 1001:
                case 2002:
                   return badgeData.Get_score();

                case 1:
                case 2:
                case 5:
                    return badgeData.Get_PinataCount();
                
                case 3:
                case 1000:
                case 2000:
                    return badgeData.Get_Ball();
                
                case 4:
                case 1002:
                    return badgeData.Get_Item_Use();
                
                case 2001:
                    return badgeData.Get_Revive();

            }

            return -1;
        }
        
        #endregion

        public void Onclick_Exit()
        {
            click.Play();
            this.gameObject.SetActive(false);
        }
        
    }
}
