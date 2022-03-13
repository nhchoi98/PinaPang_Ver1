using System;
using System.Collections.Generic;
using Ad;
using Alarm;
using UnityEngine;
using UnityEngine.UI;
using Avatar;
using Lobby;
using Badge;
using Data;

namespace Skin
{
    public class Skin_Avatar : MonoBehaviour
    {
        [SerializeField] private Main_Skin _mainSkin;
        [Header("Charater_InfoText")] public Text charaterName, bonusPercent;
        [SerializeField] private Avatar_Badge_Info _badgeInfo;
        Charater_Bonus bonus_info = new Charater_Bonus();
        public Text gemText;

        [Header("Condition_Info")]
        public Transform char_tr; // Child 0: 카드 텍스트, Child 1: 아웃라인, Child 2: 장착 표시, Chilc 3: use 버튼 

        public int pre_click_index;

        [Header("Data")] [SerializeField] private Avatar_Lobby_Info avatar_data;
        private List<int> gacha_List; // 뽑기를 시행한 애들이 뽑기를 하면 여기에 데이터가 담김 
        public GameObject skill_show;

        [Header("Theme_Data")] private int target_index;
        private EquippedDAO eqipdata;

        public Animator preview;
        public GameObject Grid_Item;

        [Header("Sound")] public AudioSource use;
        Avatar_Name name_data = new Avatar_Name();
        public AudioSource click;
        public AudioSource buySound;

        [Header("Equip_UI")] [SerializeField] private GameObject equip_panel;
        [SerializeField] private Badge_Equip_Panel _equipPanel_data;

        [Header("Tier_UI")] public Sprite normal, rare, unique;
        public Image tier_img;

        [Header("Button")] public Button buyBtn;
        public Transform conditionBtn;
        public GameObject package_Get;
        public GameObject attendance;
        public int package_index;
        public GameObject mediator;

        public SkillInfo_Set skillInfoSet;

        private void Awake()
        {
            Grid_Init(); // 그리드를 먼저 생성해줌. 
            Avatar_Init(); // 베스트 스코어 정보 불러와서 저장하기
            var equipDATA = new EquippedDAO();
            target_index = Calc_Index.Get_Avatar_index(equipDATA.Get_Equipped_index());
            pre_click_index = target_index;
            // Step 2. 장착한 친구는 체크 표시 해줌. TRANSFORM을 참조하여 체크표시할 곳 체그하기 
            char_tr.GetChild(target_index).GetChild(0).gameObject.SetActive(true);
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            preview.runtimeAnimatorController = 
                Set_Avatar_UI.Set_Charater_GameObject(Calc_Index.Get_Avatar_Num(target_index));
            SpriteRenderer shadow = preview.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                default:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow");
                    break;

                case 1002:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_M");
                    break;

                case 2003:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_L");
                    break;
            }

            Set_Charater_Data(target_index);

            switch (Calc_Index.Get_Avatar_Num(pre_click_index))
            {
                default:
                    skill_show.SetActive(false);
                    break;

                case 1006:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_PartyAnimal");
                    skill_show.SetActive(true);
                    break;

                case 2000:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Astronaut");
                    skill_show.SetActive(true);
                    break;

                case 2001:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Pajama");
                    skill_show.SetActive(true);
                    break;

                case 2002:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_TeddyBear");
                    skill_show.SetActive(true);
                    break;
            }
            _equipPanel_data.Set_Target_index(pre_click_index);
        }

        void Start()
        {
            _badgeInfo.OnClick_Avatar(target_index, false);
            Set_Bonus_ScoreText();
        }

        private void Set_Bonus_ScoreText()
        {
            int percent = 0;
            if(!_badgeInfo.Is_Bonus_Badge(ref percent))
                bonusPercent.text = "Bonus Score\n+" + (bonus_info.bonus_score(pre_click_index)/10) + "%"; // 보너스 스코어 
            
            else
                bonusPercent.text = "Bonus Score\n" + "<color=#94E11B>+" + ((bonus_info.bonus_score(pre_click_index)/10)+percent) + "%</color>"; // 보너스 스코어 
        }

        /// <summary>
        /// 그리드를 생성해주는 함수 
        /// </summary>
        private void Grid_Init()
        {
            Transform grid_obj;
            
            for(int i =0; i<Calc_Index.Get_Avatar_Max_Index(); i ++) // 그리드를 생성하고, 거기에 맞는 패널과 아바타 이미지를 넣어줌 
            {
                int index = i;
                // Step 1. 기본 그리드 오브젝트 생성 
                grid_obj = Instantiate(Grid_Item).transform;
                grid_obj.SetParent(char_tr); // grid에 속하게 만들어줌 
                grid_obj.SetAsLastSibling(); // 마지막으로 보냄 
                
                // Step 2. 그리드 내의 아바타 이미지 및 패널 변경 
                char_tr.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().sprite =
                    Set_Avatar_UI.Set_Avatar_Img(i); // 아바타의 미리보기 이미지 변경 
                char_tr.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().SetNativeSize();
                char_tr.GetChild(i).GetChild(9).gameObject.GetComponent<Text>().text =
                    name_data.Set_Charater_Name(i,true); // 아바타위의 이름 변경 
                if (Calc_Index.Get_Avatar_Num(index) > 999) // 아바타 뒤의 판떼기 변경
                    char_tr.GetChild(i).GetChild(1).gameObject.GetComponent<Image>().sprite = Calc_Index.Panel_Img(i);
                
                char_tr.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener((delegate{Avatar_Btn(index);}));
                // 버튼 등록해주기 
                char_tr.GetChild(i).GetChild(6).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener((delegate
                {Equipped_Btn(index);}));
            }

        }
        
        /// <summary>
        /// 아바타 정보 초기화 
        /// </summary>
        private void Avatar_Init()
        {
            for (int i = 0; i < Calc_Index.Get_Avatar_Max_Index(); i++)
                Determine_Locked(i);
        }

        /// <summary>
        /// 아바타 카드 정보를 바탕으로 잠금 유무를 리턴해주는 함수. stat도 같이 여기서 처리해준다. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private void Determine_Locked(int index)
        {
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(false); // 선택시 표시되는 외각선 꺼줌 
            // 1. Transform 참조하여 카드 데이터 반영해주기
            if (avatar_data.Get_Locked(index)) // 잠겨있으면
            {
                var price = Avatar_Price(index);
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(true);
                switch (price)
                {
                    case 0:
                        char_tr.GetChild(index).GetChild(7).GetChild(3).gameObject.SetActive(true);
                        break;
                }
                
            }

            else
            {
                char_tr.GetChild(index).GetChild(2).gameObject.GetComponent<Image>().material = null;
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
                char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
            }
            // 2. Transform 참조하여 블랙패널 유무 반영해주기 
        }
        
        #region Button_Action
        /// <summary>
        /// 아바타 프리뷰를 클릭하면 실행되는 함수. 해금 조건에 따라서 , 장착 조건에 따라서 어떤걸 띄워줄지 결정함 
        /// </summary>
        /// <param name="index"></param>
        public void Avatar_Btn(int index)
        {
            if(!avatar_data.Get_Locked(pre_click_index))
                char_tr.GetChild(pre_click_index).GetChild(6).GetChild(0).gameObject.SetActive(false); // 전에 선택한 곳의 Use button 꺼주기 
            
            char_tr.GetChild(pre_click_index).GetChild(0).gameObject.SetActive(false); // Selected Outline 꺼주기 
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(true);// 1. 선택한 곳에 테두리 활성화
            pre_click_index = index;
            preview.runtimeAnimatorController = Set_Avatar_UI.Set_Charater_GameObject(Calc_Index.Get_Avatar_Num(index));
            SpriteRenderer shadow = preview.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                default:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow");
                    break;
                
                case 1000:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_M");
                    break;

                case 19:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_L");
                    break;
            }
            // 2. 장착 되어있으면 테두리만
            // 3. 해금 되어 있으면 Use 버튼 활성화
            if (!avatar_data.Get_Locked(index))
            {
                if(index != target_index)
                    char_tr.GetChild(index).GetChild(6).GetChild(0).gameObject.SetActive(true);
                _badgeInfo.OnClick_Avatar(index,false);
                buyBtn.gameObject.SetActive(false);
                conditionBtn.gameObject.SetActive(false);
                package_Get.gameObject.SetActive(false);
            }

            else // 잠금 되어 있는경우 
            {
                _badgeInfo.OnClick_Avatar(index, true);
                Determine_Which_Btn(); // 클릭하면 어떤 버튼을 띄워줄지 결정하는 함수 호출 
                // 캐릭터 애니메이션 SPRITE 이미지 회색처리 
            }
            Set_Bonus_ScoreText();

            if (char_tr.GetChild(index).GetChild(8).gameObject.activeSelf)
            {
                mediator.GetComponent<IAlarmMediator>().Event_Receieve(Event_Alarm.AVATAR_ALARM_OFF,index);
                char_tr.GetChild(index).GetChild(8).gameObject.SetActive(false);

            }
            
            switch (Calc_Index.Get_Avatar_Num(pre_click_index))
            {
                default:
                    skill_show.SetActive(false);
                    break;
                
                case 1006:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_PartyAnimal");
                    skill_show.SetActive(true);
                    break;
                
                case 2000:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Astronaut");
                    skill_show.SetActive(true);
                    break;
                
                case 2001:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Pajama");
                    skill_show.SetActive(true);
                    break;
                
                case 2002:
                    skill_show.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_TeddyBear");
                    skill_show.SetActive(true);
                    break;
            }
            _equipPanel_data.Set_Target_index(index);
            Set_Charater_Data(index); // Stat 데이터 위에 반영해주기 
        }

        private void Set_PackageBtn_Index()
        {
            switch (pre_click_index)
            {
                case 13:
                    package_index = 3;
                    break;
                
                case 26:
                    package_index = 0;
                    break;
                
                case 27:
                    package_index = 1;
                    break;
                
                case 29:
                    package_index = 2;
                    break;
            }
        }
        
        // 장착 버튼을 누르면 호출되는 함수. 
        public void Equipped_Btn(int index)
        {
            //Step 1. 장착 데이터를 반영해줌 
            if (eqipdata == null)
                eqipdata = new EquippedDAO();
            // Step 2. UI상 체크 표시를 다른 친구로 바꾸어줌 
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            //Step 3. 탭에서 나갈 때 데이터를 저장해주기 위한 FLAG 활성화 
            _mainSkin.Set_Avatar_Equip(Calc_Index.Get_Avatar_Num(target_index));
            use.Play();
        }
        
        
        /// <summary>
        /// 뱃지의 활성화 되어있는 버튼을 누르면, 뱃지 장착 패널을 띄워주는 함수 
        /// </summary>
        public void Onclick_Badge()
        {
            click.Play();
            _equipPanel_data.Set_Panel_UI(pre_click_index); // 뱃지 패널 UI 정보를 조정해줌 
            equip_panel.SetActive(true);
        }

        public void OnClick_Badge_Equip_Exit()
        {
            click.Play();
            _badgeInfo.Set_Equip_Data(pre_click_index); // 장착 데이터 저장 
            Set_Bonus_ScoreText();
            equip_panel.SetActive(false);
        }

        public void OnClick_PackageBtn()
        {
            _mainSkin.Click_PackageBtn(package_index);
        }

        private void Determine_Which_Btn()
        {

            switch (pre_click_index)
            {
                default:
                    var price = Avatar_Price();
                    attendance.SetActive(false);
                    buyBtn.gameObject.SetActive(true);
                    package_Get.gameObject.SetActive(false);
                    conditionBtn.gameObject.SetActive(false);
                    buyBtn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text
                        =  String.Format("{0:#,0}", price);
                    if (Playerdata_DAO.Player_Gem() < price)
                    {
                        buyBtn.interactable = false;
                        buyBtn.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        buyBtn.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        buyBtn.interactable = true;
                    }

                    // 살 수 있는지 없는지 조건 판단
                    break;
                
                // 퀘스트로 획득하는 친구들 
                case 19:
                    attendance.SetActive(false);
                    buyBtn.gameObject.SetActive(false);
                    package_Get.gameObject.SetActive(false);
                    conditionBtn.gameObject.SetActive(true);
                    conditionBtn.GetChild(1).gameObject.GetComponent<Text>().text
                        = "LV.10";
                    break;
                
                case 9:
                case 20:
                    attendance.SetActive(true);
                    buyBtn.gameObject.SetActive(false);
                    package_Get.gameObject.SetActive(false);
                    conditionBtn.gameObject.SetActive(false);
                    break;
                
                case 22:
                    attendance.SetActive(false);
                    buyBtn.gameObject.SetActive(false);
                    package_Get.gameObject.SetActive(false);
                    conditionBtn.gameObject.SetActive(true);
                    conditionBtn.GetChild(1).gameObject.GetComponent<Text>().text
                        = "LV. 20";
                    break;
                
                case 25:
                    attendance.SetActive(false);
                    buyBtn.gameObject.SetActive(false);
                    package_Get.gameObject.SetActive(false);
                    conditionBtn.gameObject.SetActive(true);
                    conditionBtn.GetChild(1).gameObject.GetComponent<Text>().text
                        = "LV. 30";
                    break;

                case 13:
                case 26:
                case 27:
                case 29:
                    attendance.SetActive(false);
                    buyBtn.gameObject.SetActive(false);
                    package_Get.gameObject.SetActive(true);
                    conditionBtn.gameObject.SetActive(false);
                    Set_PackageBtn_Index();
                    break;

            }
            // # 1. 구매버튼을 띄울지, 광고 버튼을 띄울지, 잠금 여부를 띄울지 결정 
        }

        public void Reward_Get(int num)
        {
            int index = Calc_Index.Get_Avatar_index(num);
            avatar_data.Buy_Avatar(index);
            Image image = char_tr.GetChild(index).GetChild(2).gameObject.GetComponent<Image>();
            image.material = null;
            char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
            char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
        }
        
        public void OnClick_BuyAvatar()
        {
            Image image = char_tr.GetChild(pre_click_index).GetChild(2).gameObject.GetComponent<Image>();
            // Step 1 .구매 처리
            // Step 2. 데이터 반영
            buySound.Play();
            avatar_data.Buy_Avatar(pre_click_index);
            // Step 3. 그리드도 데이터 반영하여 풀어주기 
            char_tr.GetChild(pre_click_index).GetChild(7).gameObject.SetActive(false);
            char_tr.GetChild(pre_click_index).GetChild(6).gameObject.SetActive(true);
            image.material = null;
            buyBtn.gameObject.SetActive(false);
            package_Get.gameObject.SetActive(false);
            conditionBtn.gameObject.SetActive(false);
            Playerdata_DAO.Set_Player_Gem(-Avatar_Price());
            gemText.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            // Step 4. 장착 처리 
            if (eqipdata == null)
                eqipdata = new EquippedDAO();
            // Step 2. UI상 체크 표시를 다른 친구로 바꾸어줌 
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = pre_click_index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            //Step 3. 탭에서 나갈 때 데이터를 저장해주기 위한 FLAG 활성화 
            _mainSkin.Set_Avatar_Equip(Calc_Index.Get_Avatar_Num(target_index));
            _badgeInfo.OnClick_Avatar(pre_click_index,false);
            _equipPanel_data.Set_Target_index(pre_click_index);
            Set_Charater_Data(pre_click_index); // Stat 데이터 위에 반영해주기 
            Skin_Log.Buy_Avatar(pre_click_index);
        }
        #endregion
        
        /// <summary>
        /// 캐릭터의 기록 텍스트를 바꾸어 주고 이름도 바꾸어주는 함수  
        /// </summary>
        private void Set_Charater_Data(int index)
        {
            charaterName.text = name_data.Set_Charater_Name(index,true);
            Set_Color(index);
        }

        private Color Set_Color(int index)
        {
            Color return_value;
            if (Calc_Index.Get_Avatar_Num(index) < 1000)
            {
                return_value = Color.white;
                tier_img.sprite = normal;
            }
            
            else if (Calc_Index.Get_Avatar_Num(index) >= 1000 && Calc_Index.Get_Avatar_Num(index) < 2000)
            {
                return_value = new Color(100f / 255f, 214f / 255f, 255f / 255f);
                tier_img.sprite = rare;
            }
            
            else
            {
                return_value = new Color(255f / 255f, 124f / 255f, 193f / 255f);
                tier_img.sprite = unique;
            }

            return return_value;
        }

        private int Avatar_Price(int index =  -1)
        {
            if (index == -1)
                index = pre_click_index;
            switch (Calc_Index.Get_Avatar_Num(index ))
            {
                default:
                    return 0;
                
                case 1:
                    return 30;
                case 2:
                    return 60;
                
                case 3:
                case 4:
                case 5:
                case 6:
                    return 80;
                
                case 7:
                case 8:
                    return 95;
                
                case 10:
                case 11:
                case 12:
                    return 100;
                
                case 14:
                case 15:
                    return 110;
                
                case 16:
                case 17:
                case 18:
                    return 150;

                case 1001:
                    return 500;
                
                case 1003:
                    return 550;
                
                case 1004:
                    return 580;
                
                case 2001:
                    return 1000;
                
            }   
            
        }

        public void OnClick_SkillInfo()
        {
            skillInfoSet.Set_Panel(pre_click_index);
        }

    }
}
