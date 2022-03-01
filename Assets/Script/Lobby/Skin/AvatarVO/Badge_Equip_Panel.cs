using System.Collections;
using UnityEngine;
using Avatar;
using Data;
using UnityEngine.UI;

namespace Badge
{
    public class Badge_Equip_Panel : MonoBehaviour 
    {
        [Header("UI")]
        [SerializeField] private Transform badge_TR; // 상단 뱃지 이미지를 저장한 변수 
        [SerializeField] private Transform badge_equip_TR; // 장착할 수 있는 List를 담고있는 풀 
        [SerializeField] private GameObject equip_prefab;
        private int target_index; // 몇 번째 배지를 클릭했는지 저장하는 변수 
        private int pre_click_index;
        private int char_index; // 캐릭터 인덱스를 저장하는 함수 
        private int MAX_INDEX;
        public Sprite Plus_img;
        public Text available;
        
        [Header("Equip_Data")]
        //private Badge_EquipDAO _equipDao;
        
        [Header("Badge_Data")] 
        [SerializeField] private Badge_Data _badgeData; // 뱃지 자체의 획득 유무를 저장해두는 Manager
        private Calc_Badge_Index _badgeIndex_calc;

        [Header("Ability_Text")] 
        public Color normal, rare, unique;
        public Transform ability_text;

        private bool[] is_open_slot = new bool[3];
        private bool is_choosing = false;

        [Header("Reset_Panel")] public GameObject resetPanel;
        private int target_slot;
        [SerializeField] private Avatar_Badge_Info badge_info;
        public GameObject unlock_condition; // 슬롯 2를 오픈하지 않았을 떄 뜨는 멘트 
        public GameObject unlockCloud;
        public GameObject unlock_cloud_skin;
        public bool isSkin;

        [Header("Sound")]
        public AudioSource click;
        public AudioSource use;

        public Text gem_Text;
        private void Awake()
        {
            _badgeIndex_calc = new Calc_Badge_Index();
            MAX_INDEX = _badgeIndex_calc.Get_Badge_MaxIndex(); // MAX INDEX 초기화 
        }
        
        #region Set_UI_INFO
        public void Set_Panel_UI(int index)
        {
            char_index = index;
            is_choosing = false;
            int count = 0;
            // Step 1. 상단 뱃지 이미지 결정 
            for (int i = 0; i < 3; i++)
                Set_Upper_Img(i, badge_info.Get_Slot_Data(char_index,i));
            
                
            // Step 2. 하단의 Prefab 뭐 띄워줄지 결정 (일단 다 만들어놓고, 그중에서 켜고 끄는거로 생각중) 
            for (int i = 0; i < MAX_INDEX; i++)
            {
                badge_equip_TR.GetChild(i).GetChild(4).gameObject.SetActive(false); 
                if (_badgeData.Get_achi_data(i))
                {
                    bool isEquipped = false;
                    badge_equip_TR.GetChild(i).gameObject.SetActive(true);
                    for (int k = 0; k < 3; k++)
                    {
                        if ((calc_index(badge_info.Get_Slot_Data(char_index,k))) == _badgeIndex_calc.Get_Badge_Num(i))
                        {
                            int index_num = i;
                            int which_slot = k;
                            Button button = badge_TR.GetChild(k).GetChild(2).gameObject.GetComponent<Button>();
                            badge_equip_TR.GetChild(i).GetChild(3).gameObject.GetComponent<Button>().interactable =
                                false;
                            button.onClick.RemoveAllListeners(); // 리스너 제거 
                            button.onClick.AddListener(delegate
                            {
                                Unequipped_badge(index_num,which_slot);}); // 이벤트 등록 해줌 
                            button.gameObject.SetActive(true);
                            isEquipped = true;
                            break;
                        }
                    }

                    if (!isEquipped)
                    {
                        badge_equip_TR.GetChild(i).GetChild(3).gameObject.GetComponent<Button>().interactable =
                            true;
                        badge_equip_TR.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    }
                }

                else
                {
                    badge_equip_TR.GetChild(i).gameObject.SetActive(false);
                    count++;
                }
            }
            
            if(count == MAX_INDEX)
                available.gameObject.SetActive(true);
            
            else
                available.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// X버튼을 누르면 호출되는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="which_slot"></param>
        public void Unequipped_badge(int index, int which_slot)
        {
            Text ability_text = this.ability_text.GetChild(which_slot).gameObject.GetComponent<Text>();
            GameObject button = badge_TR.GetChild(which_slot).GetChild(2).gameObject;
            button.SetActive(false);
            Set_ability_Text(-1,ref ability_text);
            // Step 2. 슬롯 이미지 바꾸어 주기( + 이미지로) 
            badge_TR.GetChild(which_slot).GetChild(0).gameObject.GetComponent<Image>().sprite = Plus_img;
            badge_TR.GetChild(which_slot).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            
            // STEP 3. 해당 뱃지 EQUIP 버튼 활성화 시켜주기 
            badge_TR.GetChild(which_slot).GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); // 리스너 제거 
            badge_equip_TR.GetChild(index).GetChild(3).gameObject.GetComponent<Button>().interactable = true;
            badge_equip_TR.GetChild(index).GetChild(3).gameObject.SetActive(true);
            
            badge_TR.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            badge_TR.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            // 데이터에 반영해야함 
            badge_info.Set_Avatar_Slot(char_index,99,which_slot);
            badge_info.OnClick_Avatar(char_index,false);
        }
        
        /// <summary>
        /// 상단 뱃지 미리보기의 모양을 결정지어주는 함수. 매개변수로는 슬롯의 번호와 장착한 뱃지 정보를  받아온다. 
        /// </summary>
        /// <param name="index"></param>
        private void Set_Upper_Img(int slot_num, int badge_index)
        {
            Image image;
            GameObject close_button = badge_TR.GetChild(slot_num).GetChild(2).gameObject;
            badge_TR.GetChild(slot_num).GetChild(3).gameObject.SetActive(false); // 장착 버튼 꺼줌 
            Text ability_text = this.ability_text.GetChild(slot_num).gameObject.GetComponent<Text>();
            if(badge_index == 0)
                Set_ability_Text(-1,ref ability_text);
            
            else
                Set_ability_Text(_badgeIndex_calc.Get_Badge_index(calc_index(badge_index)), ref ability_text);
            
            switch (slot_num)
            {
                default: 
                    image = badge_TR.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
                    if (badge_index == 0)
                    {
                        image.sprite = Plus_img;
                        close_button.gameObject.SetActive(false);
                    }

                    else
                    {
                        image.sprite = Badge_img(badge_index);
                        badge_TR.GetChild(0).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate
                        {
                            Unequipped_badge(badge_index, 0 );});
                        badge_TR.GetChild(0).GetChild(2).gameObject.SetActive(true);
                    }

                    is_open_slot[0] = true;
                    image.SetNativeSize();
                    break;
                
                case 1: // 1번 슬롯일 때 
                    image = badge_TR.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                    if (badge_index == -1)
                    {
                        badge_TR.GetChild(1).GetChild(1).gameObject.SetActive(true); // 잠금 이미지 키기 -> 풀려면 잼 내놔~  
                        badge_TR.GetChild(1).GetChild(0).gameObject.SetActive(false); // 이미지 끄기  
                        close_button.gameObject.SetActive(false);
                        image.sprite = null;
                        is_open_slot[1] = false;
                    }
                    
                    else
                    {
                        is_open_slot[1] = true;
                        badge_TR.GetChild(1).GetChild(1).gameObject.SetActive(false);
                        badge_TR.GetChild(1).GetChild(0).gameObject.SetActive(true); // 이미지 켜기
                        if (badge_index == 0) // 비어있는 슬롯일 때 
                        {
                            image.sprite = Plus_img;
                            close_button.gameObject.SetActive(false);
                        }

                        else
                        {
                            image.sprite = Badge_img(badge_index);
                            badge_TR.GetChild(1).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                Unequipped_badge(badge_index, 1 );});
                            badge_TR.GetChild(1).GetChild(2).gameObject.SetActive(true);
                        }

                    }
                    image.SetNativeSize();
                    break;
                
                case 2: // 1번 슬롯일 때 
                    image = badge_TR.GetChild(2).GetChild(0).gameObject.GetComponent<Image>();
                    if (badge_index == -1)
                    {
                        is_open_slot[2] = false;
                        close_button.gameObject.SetActive(false);
                        badge_TR.GetChild(2).GetChild(1).gameObject.SetActive(true); // 잠금 이미지 키기 
                        badge_TR.GetChild(2).GetChild(0).gameObject.SetActive(false); // 이미지 켜기
                        image.sprite = null;
                    }
                    
                    else
                    {
                        is_open_slot[2] = true;
                        badge_TR.GetChild(2).GetChild(1).gameObject.SetActive(false);
                        badge_TR.GetChild(2).GetChild(0).gameObject.SetActive(true); // 이미지 켜기
                        if (badge_index == 0)
                        {
                            // 비어있는 슬롯일 때 
                            image.sprite = Plus_img;
                            close_button.gameObject.SetActive(false);
                        }

                        else
                        {
                            image.sprite = Badge_img(badge_index);
                            badge_TR.GetChild(2).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                Unequipped_badge(badge_index, 2 );});
                            badge_TR.GetChild(2).GetChild(2).gameObject.SetActive(true);
                        }

                    }
                    image.SetNativeSize();
                    break;
            }

        }
        #endregion

        #region OnClick
        public void OnClick_Badge_Slot(int slot)
        {
            use.Play();
            int pre_num = _badgeIndex_calc.Get_Badge_index(calc_index(badge_info.Get_Slot_Data(char_index,slot)));
            int unequip_target = target_index;
            Image slot_img = badge_TR.GetChild(slot).GetChild(0).gameObject.GetComponent<Image>(); 
            Text badge_equip = ability_text.GetChild(slot).gameObject.GetComponent<Text>();
            // Step 1. target index에 해당하는 애들 slot에 넣어줌 
            StartCoroutine(Target_Button_Set(unequip_target, pre_num));

            // Step 4. 닫기 버튼 활성화 해주고, Delegate를 변경해줌 
            badge_TR.GetChild(slot).GetChild(2).gameObject.SetActive(true);
            badge_TR.GetChild(slot).GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            badge_TR.GetChild(slot).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                Unequipped_badge(unequip_target, slot );});

            // Step 5. 모든 버튼의 장착 버튼을 꺼줌
            for (int i = 0; i < 3; i++)
            {
                badge_TR.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }

            badge_TR.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            badge_TR.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            close_badge_animation();
            // Step 6. 현재 장착 상태 저장 
            badge_info.Set_Avatar_Slot(char_index,target_index,slot);
            badge_info.OnClick_Avatar(char_index,false);
            // Step 3. slot의 이미지 정보를 변경해줌. 
            slot_img.sprite = Badge_img(badge_info.Get_Slot_Data(char_index,slot));
            slot_img.SetNativeSize();
            
            is_choosing = false;
            badge_equip.gameObject.SetActive(true);
            Set_ability_Text(target_index, ref badge_equip);
        }

        IEnumerator Target_Button_Set(int target_index, int pre_num)
        {
            StartCoroutine(Pre_Button_Set(pre_num));
            badge_equip_TR.GetChild(target_index).GetChild(4).gameObject.SetActive(false); //장착한 애는 닫기버튼 비활성화 
            badge_equip_TR.GetChild(target_index).GetChild(3).gameObject.GetComponent<Button>().interactable = false; // 이제 장착한 배지 비활성화 
            badge_equip_TR.GetChild(target_index).GetChild(3).gameObject.SetActive(true);
            yield return null;
        }

        IEnumerator Pre_Button_Set(int pre_num)
        {
            // Step 2. 아래 뱃지를 선택하는 창에는, 장착한 애 버튼은 disable, 전에 꼈던 애 버튼은 다시 활성화 
            if (pre_num < 99)
            {
                badge_equip_TR.GetChild(pre_num).GetChild(3).gameObject.GetComponent<Button>().interactable = true; // 이제 장착한 배지 비활성화 
                badge_equip_TR.GetChild(pre_num).GetChild(3).gameObject.SetActive(true);
            }
            yield return null;
        }
        
        /// <summary>
        /// close 버튼을 누르게 되면 호출되는 함수. 원래 설정으로 모두 되돌려놓는다
        /// </summary>
        /// <param name="index"></param>
        public void OnClick_Close_Btn(int index)
        {
            // Step 1. 모든 버튼의 장착 버튼을 꺼줌
            for (int i = 0; i < 3; i++)
                badge_TR.GetChild(i).GetChild(3).gameObject.SetActive(false); //활성화된 버튼 꺼주기 

            close_badge_animation();
            // Step 2. Target_index의 close 버튼 꺼주기 
            badge_equip_TR.GetChild(index).GetChild(3).gameObject.SetActive(true);
            badge_equip_TR.GetChild(index).GetChild(4).gameObject.SetActive(false);
            is_choosing = false;
            Set_Panel_UI(char_index);
            
            click.Play();
        }

        public void OnClick_Badge_Equip(int index)
        {
            // Step 1. 먼저 빈 슬롯이 있는지 탐색. 있는경우, 자동으로 장착 진행함 
            for (int i = 0; i < 3; i++)
            {
                // 활성화 가능하고, 비어있는 슬롯일 경우 
                if (is_open_slot[i] && (badge_info.Get_Slot_Data(char_index,i) == 0))
                {
                    Image slot_img = badge_TR.GetChild(i).GetChild(0).gameObject.GetComponent<Image>();
                    Text badge_equip = ability_text.GetChild(i).gameObject.GetComponent<Text>();
                    int unequip_target = index;
                    target_index = index;
                    badge_info.Set_Avatar_Slot(char_index,target_index,i);
                    // Step 1. 장착해제 Delegate를 변경해줌 
                    badge_TR.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    badge_TR.GetChild(i).GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    badge_TR.GetChild(i).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        Unequipped_badge(unequip_target, i );});
                    
                    // Step 2. 현재 장착 상태 저장 
                    // 데이터 저장 
                    badge_info.OnClick_Avatar(char_index,false);
                    is_choosing = false;
                    badge_equip_TR.GetChild(target_index).GetChild(3).gameObject.GetComponent<Button>().interactable = false; // 이제 장착한 배지 비활성화 
                    
                    // Step 3. 이미지를 변경해줌 
                    slot_img.sprite = Badge_img(badge_info.Get_Slot_Data(char_index,i));
                    slot_img.SetNativeSize();
                    use.Play();
                    // Step 4. 텍스트를 변경해줌 
                    badge_equip.gameObject.SetActive(true);
                    Set_ability_Text(target_index, ref badge_equip);
                    close_badge_animation();
                    return;
                }
                
                if(is_open_slot[i])
                    badge_TR.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("equip");
            }
            
            click.Play();
            if (is_choosing)
            {
                badge_equip_TR.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
                badge_equip_TR.GetChild(target_index).GetChild(3).gameObject.SetActive(true);
            }
            
            target_index = index;
            // Step 1. 자기 자신의 close 버튼 활성화 
            badge_equip_TR.GetChild(index).GetChild(4).gameObject.SetActive(true);
            badge_equip_TR.GetChild(index).GetChild(3).gameObject.SetActive(false);

            // Step 2. 상단 슬롯의 장착 가능 여부 버튼 활성화
            if (!is_choosing)
            {
                is_choosing = true;
                for (int i = 0; i < 3; i++)
                {
                    if(!is_open_slot[i])
                        continue;
                    
                    else
                        badge_TR.GetChild(i).GetChild(3).gameObject.SetActive(true); // 장착 버튼 활성화 
                }
            }

            
        }
        
        #endregion
        
        private Sprite Badge_img(int value)
        {
            int num;
            if (value == 0)
                return Plus_img;

            else
                num = calc_index(value);
            
            
            string path = "Lobby/Badge/Badge_Img/" + num.ToString();
            Sprite img = Resources.Load<Sprite>(path);
            return img;
        }
        
        private int calc_index(int value)
        {
            int index = 0;
            if (value == 0 || value == -1)
                return 100;

            switch (value/1000)
            {
                default:
                    if (value == 1050)
                        index = 0;
                    
                    else if  (value == 1100)
                        index = 1001;
                    
                    else if (value == 1200)
                        index = 2002;

                    break;
                
                case 2: // 볼 스피드 
                    if (value == 2010)
                        index = 3;
                    else if (value == 2020)
                        index = 1000;
                    else if (value == 2030)
                        index = 2000;
                    break;
                
                case 3: // Life
                    index = 2001;
                    break;
                
                case 4:
                    if (value == 4030)
                        index = 1;
                    else if (value == 4050)
                        index = 2;

                    else
                        index = 5;
                    break;
                
                case 5:
                    if (value == 5060)
                        index = 4;

                    else
                        index = 1002;
                    break;

                
            }

            return index;
        }

        private void Set_ability_Text(int index, ref Text text)
        {
            switch (index)
            {
                default:
                    text.text = "-";
                    break;
                
                case 0:
                    text.text = "BONUS SCORE +5%";
                    break;
                
                case 1:
                    text.text = "Candle Box +3%";
                    break;
                
                case 2:
                    text.text = "Candle Box +5%";
                    break;
                
                case 3:
                    text.text = "Ball Speed + 10%";
                    break;
                
                case 4:
                    text.text = "Item Duration + 60s";
                    break;
                
                case 5:
                    text.text = "Candle Box +10%";
                    break;
                
                case 6:
                    text.text = "Ball Speed + 20%";
                    break;
                
                case 7:
                    text.text = "BONUS SCORE + 10%";
                    break;
                
                case 8:
                    text.text = "Item Duration +120s";
                    break;
                
                case 9:
                    text.text = "Ball Speed + 30%";
                    break;
                
                case 10:
                    text.text = "Life + 1";
                    break;
                
                case 11:
                    text.text= "BONUS SCORE + 20%";
                    break;
                
            }
        }

        public void OnClick_Reset_PanelOpen(int slot)
        {
            click.Play();
            if (slot / 3 ==1)
            {
                isSkin = true;
                slot = slot % 3;
            }
            else
                isSkin = false;
            
            Transform Button = resetPanel.transform.GetChild(0).GetChild(0);
            if (slot == 2)
            {
                if (badge_info.Get_slot_locked(char_index,1))
                {
                    Button.GetComponent<Button>().interactable = false;
                    unlock_condition.gameObject.SetActive(true);
                }

                else
                {
                    unlock_condition.gameObject.SetActive(false);
                    if (Playerdata_DAO.Player_Gem() < 100)
                        Button.GetComponent<Button>().interactable = false;

                    else
                        Button.GetComponent<Button>().interactable = true;
                }
            }

            else
            {
                unlock_condition.gameObject.SetActive(false);
                if (Playerdata_DAO.Player_Gem() < 100)
                    Button.GetComponent<Button>().interactable = false;

                else
                    Button.GetComponent<Button>().interactable = true;
            }

            target_slot = slot;
            resetPanel.SetActive(true);
        }

        private void close_badge_animation()
        {
            for (int i = 0; i < 3; i++)
                badge_TR.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("done");
            
        }
        #region Reset
        public void OnClick_Reset_Close()
        {
            click.Play();
            resetPanel.SetActive(false);
        }
        
        /// <summary>
        /// 잠겨있던 배지를 풀어주는 함수 
        /// </summary>
        public void Reset(bool is_gem)
        {
            Vector2 target_slot_pos;

            click.Play();
            // # 2. UI에 반영해줌  (둘 다) 
            badge_TR.GetChild(target_slot).GetChild(0).gameObject.GetComponent<Image>().sprite = Plus_img;
            badge_TR.GetChild(target_slot).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            badge_TR.GetChild(target_slot).GetChild(0).gameObject.SetActive(true);
            badge_TR.GetChild(target_slot).GetChild(1).gameObject.SetActive(false);
            if (!isSkin)
            {
                switch (target_slot)
                {
                    default:
                        target_slot_pos = new Vector2(0f, 458f);
                        break;
                    
                    case 2:
                        target_slot_pos = new Vector2(288f, 470f);
                        break;
                }
                Instantiate(unlockCloud, target_slot_pos, Quaternion.identity);
            }

            else
            {
                if(target_slot == 1)
                    target_slot_pos = new Vector2(211f, 606f);
            
                else
                    target_slot_pos = new Vector2(359f, 606f);
            
                Instantiate(unlock_cloud_skin, target_slot_pos, Quaternion.identity);
                
            }

            is_open_slot[target_slot] = true;
            badge_info.Badge_Unlocked(target_slot,char_index);
            if (is_gem)
            {
                Playerdata_DAO.Set_Player_Gem(-100);
                gem_Text.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            }
        }

        public void Set_Target_index(int avatar_index)
        {
            char_index = avatar_index;
        }
        

        #endregion
       
    }
}