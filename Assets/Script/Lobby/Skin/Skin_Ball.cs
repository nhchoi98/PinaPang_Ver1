using System;
using System.Collections.Generic;
using Ad;
using Alarm;
using UnityEngine;
using UnityEngine.UI;
using Avatar;
using Badge;
using Ball;
using Data;
using Lobby;
using Progetile;

namespace Skin
{
    public class Skin_Ball : MonoBehaviour
    {
        [Header("Ball_Preview")] 
        public SpriteRenderer ball_preview;
        public Preview_Ball ball_preview_flying;
        [SerializeField] private Charater_Info _characterinfo;
        
        [Header("Condition_info")]
        public Transform char_tr; // Child 0: 카드 텍스트, Child 1: 아웃라인, Child 2: 장착 표시, Chilc 3: use 버튼 
        public int pre_click_index;
        public Text gemtext;
        
        [Header("Data")]
        public List<bool> IsLocked; // 카드 데이터에 따라서 잠금 여부를 표시해줌 
        private BallDAO data;
        private int normal_target;
        [SerializeField] private Badge_Data _badgeData;

        [Header("Theme_Data")]
        private int target_index;
        private int MAXINDEX; // 

        [Header("Player_Data")] [SerializeField]
        public GameObject BuyBtn;

        public GameObject packageBtn;

        [Header("Sound")] public AudioSource use, buy;
        
        public GameObject challenge_btn;
        public Image line_ball_img;
        public GameObject Grid_obj;

        public GameObject Badge_Btn;
        public GameObject mediator;

        [Header("PackageBtn")] 
        [SerializeField] private Main_Skin _mainSkin;
        [SerializeField] private int package_index;
        
        private void Awake()
        {
            data = new BallDAO();
            Grid_Init(); // 그리드 정보 초기화 
            Init_Info();
            //Step 1. target_index 초기화 
            target_index = data.Get_BallEquipped_Data();
            pre_click_index = target_index;
            // Step 2. 장착한 친구는 체크 표시 해줌. TRANSFORM을 참조하여 체크표시할 곳 체그하기 
            char_tr.GetChild(target_index).GetChild(0).gameObject.SetActive(true);
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            BuyBtn.SetActive(false);
            ball_preview_flying.ball_img.sprite = Set_Avatar_UI.Set_Ball_Img(target_index); // 광고로 구매하는 공의 경우, 별도의 데이터 처리를 시켜줌 
        }
        
        private void Grid_Init()
        {
            Transform grid_obj;
            MAXINDEX = Calc_Index.Get_Ball_Max_Index();
            Ball_Name Name_Data = new Ball_Name();
            normal_target = Calc_Index.Get_Normal_index();
            for (int i = 0; i < MAXINDEX; i++) // 그리드를 생성하고, 거기에 맞는 패널과 아바타 이미지를 넣어줌 
            {
                int index = i;
                int ball_num = Calc_Index.Get_Ball_Num(i);
                // Step 1. 기본 그리드 오브젝트 생성 
                grid_obj = Instantiate(Grid_obj).transform;
                grid_obj.SetParent(char_tr); // grid에 속하게 만들어줌 
                grid_obj.SetAsLastSibling(); // 마지막으로 보냄 
                // Step 2. 그리드 내의 아바타 이미지 및 패널 변경   
                char_tr.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().sprite =
                    Set_Avatar_UI.Set_Ball_Img(i); // 볼의 미리보기 이미지 변경 
                char_tr.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().SetNativeSize();
                char_tr.GetChild(i).GetChild(9).gameObject.GetComponent<Text>().text =
                    Name_Data.Set_Ball_Name(i); // 볼의 이름 반영해주기 
                
                char_tr.GetChild(i).gameObject.GetComponent<Button>().onClick
                    .AddListener((delegate { Ball_Btn(index); }));
                // 버튼 등록해주기 
                char_tr.GetChild(i).GetChild(6).GetChild(0).gameObject.GetComponent<Button>().onClick
                    .AddListener((delegate { Equipped_Btn(index); }));
            }

        }
        public void Click_BallBtn() => ball_preview_flying.OnClick_Ball(target_index);
        
        private void Determine_Locked(int index)
        {
            bool Is_locked = data.Get_BallPur_Data(index);
            IsLocked.Add(!Is_locked);
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(false); // 선택시 표시되는 외각선 꺼줌 
            Image image = char_tr.GetChild(index).GetChild(2).gameObject.GetComponent<Image>();
            // 1. Transform 참조하여 카드 데이터 반영해주기
            if (!Is_locked) // 구매 했다면
            {
                char_tr.GetChild(index).GetChild(6).gameObject.SetActive(false);
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(true);
                Avatar_Gray.SetGrayScale(ref image,1f);
            }

            else
            {
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
                char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
                image.material = null;
            }
            
            // 금액이 부족하다면 , 글씨를 빨간색으로 표시해주기. 
            if (index < normal_target)
            {
                return;
            }
            

            else
            {
                char_tr.GetChild(index).GetChild(7).GetChild(3).gameObject.SetActive(true);
            }
        }
        
        #region Button_Action
        public void Click_PackageBtn()
        {
            _mainSkin.Click_PackageBtn(package_index);
        }
        
        public void Ball_Btn(int index)
        {
            int ballNum = Calc_Index.Get_Ball_Num(index);
             if(!IsLocked[pre_click_index])
                char_tr.GetChild(pre_click_index).GetChild(6).GetChild(0).gameObject.SetActive(false); // 전에 선택한 곳의 Use button 꺼주기 
            
            char_tr.GetChild(pre_click_index).GetChild(0).gameObject.SetActive(false); // Selected Outline 꺼주기 
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(true);// 1. 선택한 곳에 테두리 활성화
            // 프리뷰 이미지 바꾸어주기 
            ball_preview.sprite = Set_Avatar_UI.Set_Ball_Img(index);
            line_ball_img.sprite = Set_Avatar_UI.Set_Ball_Img(index);
            // 2. 장착 되어있으면 테두리만
            // 3. 해금 되어 있으면 Use 버튼 활성화
            if (!IsLocked[index])
            {
                if(index != target_index)
                    char_tr.GetChild(index).GetChild(6).GetChild(0).gameObject.SetActive(true);
                BuyBtn.SetActive(false);
                challenge_btn.SetActive(false);
                packageBtn.SetActive(false);
            }

            else
            {

                if (index >= normal_target) // 레벨업 보상으로써만 얻는 공일 경우 
                {
                    BuyBtn.SetActive(false);
                    if (index < Calc_Index.Get_Ball_index(4000))
                    {
                        if (index < Calc_Index.Get_Ball_index(3005))
                        {
                            challenge_btn.transform.GetChild(0).gameObject.SetActive(true);
                            challenge_btn.transform.GetChild(1).gameObject.SetActive(true);
                            challenge_btn.transform.GetChild(2).gameObject.SetActive(false);
                            challenge_btn.transform.GetChild(1).gameObject.GetComponent<Text>().text =
                                "LV. " + Ball_Price.Price(index).ToString();
                        }
                        
                        else // 출석부로 얻는 공일 경우 
                        {
                            challenge_btn.transform.GetChild(0).gameObject.SetActive(false);
                            challenge_btn.transform.GetChild(1).gameObject.SetActive(false);
                            challenge_btn.transform.GetChild(2).gameObject.SetActive(true);
                        }

                        packageBtn.SetActive(false);
                        challenge_btn.SetActive(true);
                        
                    }
                    else
                    {
                        challenge_btn.SetActive(false);
                        packageBtn.SetActive(true);
                        Set_Package_index(index);
                    }
                }
                
                else
                {
                    challenge_btn.SetActive(false);
                    BuyBtn.SetActive(true);
                    packageBtn.SetActive(false);
                    BuyBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text =
                    Ball_Price.Price(index).ToString();
                    if (Ball_Price.Price(index) < Playerdata_DAO.Player_Gem())
                    {
                        BuyBtn.transform.GetChild(1).gameObject.SetActive(false);
                        BuyBtn.GetComponent<Button>().interactable = true;
                    }

                    else
                    {
                        BuyBtn.transform.GetChild(1).gameObject.SetActive(true);
                        BuyBtn.GetComponent<Button>().interactable = false;
                    }
                }
            }

            ball_preview_flying.ball_img.sprite = Set_Avatar_UI.Set_Ball_Img(index);
            ball_preview_flying.OnClick_Ball(index);
            // 파티클 있는 공인지 없는 공인지 판단하기 
            pre_click_index = index;

            if (char_tr.GetChild(index).GetChild(8).gameObject.activeSelf)
            {
                mediator.GetComponent<IAlarmMediator>().Event_Receieve(Event_Alarm.BALL_ALARM_OFF,index);
                char_tr.GetChild(index).GetChild(8).gameObject.SetActive(false);
            }
        }

        private void Set_Package_index(int index)
        {
            int num = Calc_Index.Get_Ball_Num(index);
            switch (num)
            {
                case 4000:
                    package_index = 0;
                    break;
                
                case 4001:
                    package_index = 2;
                    break;
                
                case 4002:
                    package_index = 1;
                    break;
                
                case 4003:
                    package_index = 3;
                    break;
            }
        }
        public void Init_Info()
        {
            //Step 1. 공의 잠금 데이터를 읽어와야함. 카드를 통해 잠금 유무를 결정함 
            if (data == null)
                data = new BallDAO();
            
            IsLocked = new List<bool>();
            for (int i = 0; i < MAXINDEX; i++)
                Determine_Locked(i);
        }
     
        
        #endregion

        #region  Buy_Action
        public void Reward_Get(int num)
        {
            int index = Calc_Index.Get_Ball_index(num);
            IsLocked[index] = false;
            data.Set_BallPur_Data(index);
            Image image = char_tr.GetChild(index).GetChild(2).gameObject.GetComponent<Image>();
            image.material = null;
            char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
            char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
        }
        
        public void Equipped_Btn(int index)
        {
            // Step 2. UI상 체크 표시를 다른 친구로 바꾸어줌 
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            data.Set_BallEquipped_Data(index);
            _characterinfo.Set_Ball(index);
            use.Play();
        }

        /// <summary>
        /// 구매 버튼을 누르면 호출되는 함수 
        /// </summary>
        public void Buy(bool is_ad)
        {
            IsLocked[pre_click_index] = false;
            data.Set_BallPur_Data(pre_click_index);
            if(!is_ad)
                Playerdata_DAO.Set_Player_Gem(-Ball_Price.Price(pre_click_index));
            
            char_tr.GetChild(pre_click_index).GetChild(6).gameObject.SetActive(true);
            char_tr.GetChild(pre_click_index).GetChild(7).gameObject.SetActive(false);
            Image image = char_tr.GetChild(pre_click_index).GetChild(2).gameObject.GetComponent<Image>();
            image.material = null;
            // 장착 UI 만들어주기 
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = pre_click_index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            
            
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            data.Set_BallEquipped_Data(target_index);
            _characterinfo.Set_Ball(target_index);

            BuyBtn.SetActive(false);
            // badge에 구매처리 
            //char_tr.GetChild(pre_click_index).GetChild()
            if (Playerdata_DAO.Player_Gem() < 10)
            {
                for (int i = 0; i < MAXINDEX; i++)
                    Determine_Locked(i);
            }
            
            _badgeData.Set_Ball_Buy();
            buy.Play();
            Skin_Log.Buy_Ball_Log(target_index);
            gemtext.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
        }
        
        #endregion
    }

   
}
