using System;
using System.Collections;
using System.Collections.Generic;
using Alarm;
using Avatar;
using UnityEngine;
using UnityEngine.UI;

namespace Challenge
{
    /// <summary>
    /// 패널의 UI를 Set 해주는 Instance.
    /// </summary>
    public class Quest_Table_UI : MonoBehaviour
    {
        [Header("Level_UI")] 
        [SerializeField] private Level_UI _levelUI;
        public Text userLevel;
        public GameObject levelObj;

        [Header("Page_Data")] 
        private int page;
        private readonly int MAX_PAGE = 8;
        public GameObject rightBtn, leftBtn;

        [Header("Reward_TR")] 
        public Transform rewardTR;

        [Header("Tier_Img")] 
        public Image tierImg;

        //[Header("Reward_Data")] 
        public Quest_Table_Dataset dataSet { get; private set; } // 획득 여부를 저장하고 있는 변수. 

        [Header("Panel_Set")] 
        public GameObject probaility_table;
        
        [Header("LevelUI_Reward")] 
        public Transform rewardImg;
        public Text quantity;
        public AudioSource click;

        // # Pre. Start 하면, 사용자 레벨에 맞는 페이지를 Set 해주고 그에 맞는 UI를 띄워야함.
        
        private void Start()
        {
            EXP_DAO leveldata = new EXP_DAO();
            int level = leveldata.Get_User_Level();
            dataSet = new Quest_Table_Dataset(level);
            Levelup_Reward_Set(level); // 안받은 보상부터 표시해주기.
        }

        // 레벨업 관련 함수 들어가야함 
        #region Page_Set
        // # 1. 페이지가 넘어가는 액션 -> 10개의 에셋을 수정해줘야함 
        /// <summary>
        /// 테이블의 필요한 정보를 Set 해주는 함수. page를 기준으로 필요한 데이터를 Dataset에서 가져오고, 이를 UI에 반영해준다. 
        /// </summary>
        private void Set_Page_UI()
        {
            Determine_ButtonOn();
            Set_Tier_Img();
            for (int i = 0; i < 5; i++)
            {
                if (!dataSet.Get_is_locked(page, i))
                {
                    // 이제 얻어야 할 때 
                    if (!dataSet.Get_is_get(page, i))
                    {
                        Set_Reward_img(i);
                        Set_Should_Get(i);
                    }

                    // 이미 얻었을 때 
                    else
                        Set_Is_Get(i);
                }

                // 잠겨 있으면 
                else
                {
                    Set_Reward_img(i);
                    Set_Is_locked(i);
                }
                Set_LevelText(i);  // 레벨 텍스트를 바꾸어줌 
            }
        }

        /// <summary>
        /// Tier 이미지를 Set 해주는 함수
        /// </summary>
        private void Set_Tier_Img()
        {
            Sprite img = Resources.Load<Sprite>("Lobby/Challenge/Medal/"+ page.ToString());
            tierImg.sprite = img;
        }
        
        // # 5. 보상 이미지와 보상 정도를 Set 해주는 함수
        private void Set_Reward_img(int index)
        {
            int _quantity = 0;
            int type = Get_Reward_Type(ref _quantity, index);
            Text quantity_text = rewardTR.GetChild(index).GetChild(2).GetChild(4).gameObject.GetComponent<Text>();
            quantity_text.text = _quantity.ToString();
            switch (type)
            {
                // 볼 or 캐릭터 일 때 
                default:
                    rewardTR.GetChild(index).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(1).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(4).gameObject.SetActive(false);
                    if (type >= 10 && type < 3000)
                    {
                        rewardTR.GetChild(index).GetChild(2).GetChild(2).gameObject.SetActive(false);
                        rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.GetComponent<Image>().sprite =
                            Set_Avatar_UI.Set_Avatar_Img(Calc_Index.Get_Avatar_index(type));
                        rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.GetComponent<Image>().SetNativeSize();
                        rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.SetActive(true);
                    }
                    
                    else
                    {
                        rewardTR.GetChild(index).GetChild(2).GetChild(2).gameObject.SetActive(true);
                        rewardTR.GetChild(index).GetChild(2).GetChild(2).gameObject.GetComponent<Image>().sprite =
                            Resources.Load<Sprite>("Ball/" + type.ToString());
                    
                        rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.SetActive(false);
                    } 

                    break;
                
                
                // 캔디를 반영해야할 때 
                case 0:
                    rewardTR.GetChild(index).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    rewardTR.GetChild(index).GetChild(2).GetChild(1).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(2).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(4).gameObject.SetActive(true);
                    break;
                
                // 젬을 반영해야할 때 
                case 1:
                    rewardTR.GetChild(index).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(1).gameObject.SetActive(true);
                    rewardTR.GetChild(index).GetChild(2).GetChild(2).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(3).gameObject.SetActive(false);
                    rewardTR.GetChild(index).GetChild(2).GetChild(4).gameObject.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// 이미 획득했다면, UI를 이에 맞게 바꾸어줌 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Is_Get(int index)
        {
            rewardTR.GetChild(index).GetChild(0).gameObject.SetActive(false);
            rewardTR.GetChild(index).GetChild(1).gameObject.SetActive(false);
            rewardTR.GetChild(index).GetChild(2).gameObject.SetActive(false);
            rewardTR.GetChild(index).GetChild(3).gameObject.SetActive(true);
        }

        private void Set_Is_locked(int index)
        {
            rewardTR.GetChild(index).GetChild(0).gameObject.SetActive(true);
            rewardTR.GetChild(index).GetChild(2).gameObject.SetActive(true);
            rewardTR.GetChild(index).GetChild(1).gameObject.SetActive(false);
            rewardTR.GetChild(index).GetChild(3).gameObject.SetActive(false);
            
        }

        /// <summary>
        /// 이제 얻아야함 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Should_Get(int index)
        {
            rewardTR.GetChild(index).GetChild(0).gameObject.SetActive(false);
            rewardTR.GetChild(index).GetChild(1).gameObject.SetActive(true);
            rewardTR.GetChild(index).GetChild(2).gameObject.SetActive(true);
            rewardTR.GetChild(index).GetChild(3).gameObject.SetActive(false);
            
        }

        /// <summary>
        /// 레벨 텍스트를 바꾸어주는 함수 
        /// </summary>
        /// <param name="index"></param>
        private void Set_LevelText(int index)
        {
            rewardTR.GetChild(index).GetChild(4).gameObject.GetComponent<Text>().text = "LV." + ((page*5)+(index+1)).ToString();
        }
        
        /// <summary>
        /// 버튼을 띄울지 말지를 결정해주는 함수
        /// </summary>
        private void Determine_ButtonOn()
        {
            if (page == MAX_PAGE)
            {
                rightBtn.SetActive(false);
                leftBtn.SetActive(true);
            }
            else if (page == 0)
            {
                leftBtn.SetActive(false);
                rightBtn.SetActive(true);
            }

            else
            {
                rightBtn.SetActive(true);
                leftBtn.SetActive(true);
            }
            
        }
        #endregion
        
        #region  Button_Set

        /// <summary>
        /// 들어오기 버튼을 누르면 실행됨
        /// </summary>
        public void OnClick_Panel_On()
        {
            page = ((_levelUI.Get_Level()-1)/5);
            userLevel.text = "LV."+_levelUI.Get_Level().ToString();
            Set_Page_UI();
            Determine_ButtonOn(); // 좌우버튼 띄울지 말지 결정함 
            probaility_table.SetActive(true);
            click.Play();
        }

        /// <summary>
        /// 나가기 버튼을 누르면 실행됨 
        /// </summary>
        public void OnClick_Panel_Exit()
        {
            click.Play();
            probaility_table.SetActive(false);
        }
        
        public void Left_Btn()
        {
            --page;
            Set_Page_UI();
            click.Play();
        }

        public void Right_Btn()
        {
            ++page;
            Set_Page_UI();
            click.Play();
        }
        #endregion
        
        #region Animation

        

        #endregion

        #region Level_Up_Set

        public void Levelup_Reward_Set(int level)
        {
            int _quantity = 0;
            int type;
            if(level>44)
                type = Get_Reward_Type(ref _quantity);
            
            else
                type = Get_Reward_Type(ref _quantity, -1);
            
            Text quantity_text = quantity;
            quantity_text.text = _quantity.ToString();
            userLevel.text = "LV."+ level.ToString();
            if (level > 44)
            {
                levelObj.SetActive(false);
                return;
            }

            switch (type)
            {
                // 볼 or 캐릭터 일 때
                default:
                    quantity_text.gameObject.SetActive(false);
                    if (type >= 10 && type < 3000)
                    {
                        Image image = rewardImg.GetChild(2).gameObject.GetComponent<Image>();
                        image.sprite = Set_Avatar_UI.Set_Avatar_Img(Calc_Index.Get_Avatar_index(type));
                        image.SetNativeSize();
                        rewardImg.GetChild(2).gameObject.SetActive(true);
                        rewardImg.GetChild(1).gameObject.SetActive(false);
                        rewardImg.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        Image image = rewardImg.GetChild(1).gameObject.GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>("Ball/" + type.ToString());
                        image.SetNativeSize();
                        rewardImg.GetChild(2).gameObject.SetActive(false);
                        rewardImg.GetChild(1).gameObject.SetActive(true);
                        rewardImg.GetChild(0).gameObject.SetActive(false);
                    }
                    break;
                
                // 젬을 반영해야할 때 
                case 1:
                    quantity_text.gameObject.SetActive(true);
                    rewardImg.GetChild(2).gameObject.SetActive(false);
                    rewardImg.GetChild(1).gameObject.SetActive(false);
                    rewardImg.GetChild(0).gameObject.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// 레벨업 되면 호출되는 함수. 
        /// </summary>
        public void LevelUp()
        {
            if (_levelUI.Get_Level() % 5 == 1)
                ++page;
            
            dataSet.Level_UP();
            Levelup_Reward_Set(_levelUI.Get_Level());
        }
        #endregion
        /// <summary>
        /// 받기 버튼을 클릭하면, 페이지를 바탕으로 type을 정해주고 수량도 정해주는 함수 
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int Get_Reward_Type(ref int quantity, int index = -1 ,bool is_get = false)
        {
            int level_value;
            int type = 0;
            int value = 0;

            // 메인에서 이 함수를 호출하는 경우 
            if (index == -1)
                level_value = _levelUI.Get_Level()+1;
            
            else if (index > 100)
                level_value = index % 100;

            // 패널에서 이 함수를 호출하는 경우 
            else
            {
                level_value = ((page) * 5 + index)+1;
                if (is_get)
                {
                    dataSet.Set_Get_Reward(page, index); // 획득 반영 
                    Set_Is_Get(index);
                }
            }
            if (level_value % 5 == 0 )
            {

                switch ((level_value-1) / 5)
                {
                    default:
                        type = 1; // 젬 
                        break;

                    case 0:
                        type = 3000; // 달고나 공 
                        break;

                    case 1:
                        type = 19; // 황제아바타
                        break;

                    case 2:
                        type = 3001; // 냥발바닥
                        break;

                    case 3:
                        type = 1002; // 인어
                        break;

                    case 4:
                        type = 3002; //웃는공
                        break;

                    case 5:
                        type = 1005; // 원숭이
                        break;

                    case 6:
                        type = 3003; // 악마 
                        break;

                    case 7:
                        type = 3004; // 장미공 
                        break;
                    
                    case 8:
                        type = 1;
                        value = 80;
                        break;
                }
            }

            else
            {
                type = 1;
                switch ((level_value-1)/5)
                {
                    default:
                        value = 5;
                        break;
                    
                    case 1:
                        value = 7;
                        break;
                    
                    case 2:
                        value = 10;
                        break;
                    
                    case 3:
                        value = 15;
                        break;
                    
                    case 4:
                        value = 20;
                        break;
                    
                    case 5:
                        value = 25;
                        break;
                    
                    case 6:
                        value = 30;
                        break;
                    
                    case 7:
                        value = 35;
                        break;
                    
                    case 8:
                        value = 40;
                        break;
                    
                }
                
            }
            quantity = value;
            return type;
        }
        
        
    }
}
