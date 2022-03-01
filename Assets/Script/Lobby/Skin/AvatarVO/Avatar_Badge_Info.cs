using Badge;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Avatar
{
    /// <summary>
    /// UI에 아바타의 뱃지 장착여부를 반영해주고, 슬롯을 클릭하면 장착 OR 메시지를 띄워주는 CLASS 
    /// </summary>
    public class Avatar_Badge_Info : MonoBehaviour
    {
        private List<Tuple<int, int, int>> badge_info = new List<Tuple<int, int, int>>(); // 각 아바타의 뱃지 정보를 저장하는 List 
        [SerializeField] private Transform badge_list_TR; 
        private int MAX_INDEX;
        public Sprite plus_img;
        public GameObject unlock_cloud;
        private int targetIndex;
        
        private void OnEnable()
        {
            MAX_INDEX = Calc_Index.Get_Avatar_Max_Index();
            Badge_EquipDAO eqiup_data;
            for (int i = 0; i < MAX_INDEX; i++)
            {
                eqiup_data = new Badge_EquipDAO(Calc_Index.Get_Avatar_Num(i)); 
                badge_info.Add(new Tuple<int, int, int>(eqiup_data.Get_Slot_data(0), eqiup_data.Get_Slot_data(1), eqiup_data.Get_Slot_data(2)));
            }
            // 모든 아바타의 뱃지 장착 정보를 불러옴 
        }

        public bool Get_slot_locked(int index ,int slot)
        {
            int value;
            switch (slot)
            {
                default:
                    value= badge_info[index].Item1;
                    break;
                
                case 1:
                    value= badge_info[index].Item2;
                    break;
                
                case 2:
                    value= badge_info[index].Item3;
                    break;
            }

            if (value == -1)
                return true;

            else
            {
                return false;
            }

        }

        public int Get_Slot_Data(int index, int slot)
        {
            int value;
            switch (slot)
            {
                default:
                    value= badge_info[index].Item1;
                    break;
                
                case 1:
                    value= badge_info[index].Item2;
                    break;
                
                case 2:
                    value= badge_info[index].Item3;
                    break;
            }

            return value;
        }

        public bool Is_Bonus_Badge(ref int percent)
        {
            int sum = 0;
            bool flag = false;
            
            if (badge_info[targetIndex].Item1 / 1000 == 1)
            {
                sum += badge_info[targetIndex].Item1 % 1000;
                flag = true;
            }
            
            if (badge_info[targetIndex].Item2 / 1000 == 1)
            {
                sum += badge_info[targetIndex].Item2 % 1000;
                flag = true;
            }
            
            if (badge_info[targetIndex].Item3 / 1000 == 1)
            {
                sum += badge_info[targetIndex].Item3 % 1000;
                flag = true;
            }
                
            

            percent = sum / 10;
            return flag;
        }
        
        public void OnClick_Avatar(int index, bool is_locked)
        {
            // Step 1. 인덱스에 알맞는 이미지를 띄워줌
            // Step 2. 활성/비활성 유무 결정지어줌 
            targetIndex = index;
            if (badge_info[index].Item1 == -1)
            {
                badge_list_TR.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                badge_list_TR.GetChild(0).GetChild(1).gameObject.SetActive(true);
                if (is_locked)
                    badge_list_TR.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().interactable = false;

                else
                    badge_list_TR.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                
                badge_list_TR.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            else
            { 
                badge_list_TR.GetChild(0).GetChild(1).gameObject.SetActive(false);
                if (!is_locked)
                {
                    badge_list_TR.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    badge_list_TR.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                    if (badge_info[index].Item1 == 0)
                    {
                        badge_list_TR.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            plus_img;
                    }

                    else
                        badge_list_TR.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            Badge_img(badge_info[index].Item1);
                    badge_list_TR.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                }

                else
                {
                    badge_list_TR.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                    badge_list_TR.GetChild(0).GetChild(0).gameObject.SetActive(false);
                }

            }

            
            if (badge_info[index].Item2 == -1)
            {
                badge_list_TR.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
                badge_list_TR.GetChild(1).GetChild(1).gameObject.SetActive(true);
                badge_list_TR.GetChild(1).GetChild(0).gameObject.SetActive(false);
                if (is_locked)
                    badge_list_TR.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = false;

                else
                    badge_list_TR.GetChild(1).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            }
            
            else
            {
                badge_list_TR.GetChild(1).GetChild(0).gameObject.SetActive(true);
                badge_list_TR.GetChild(1).GetChild(1).gameObject.SetActive(false);
                if (!is_locked)
                {
                    badge_list_TR.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                    if (badge_info[index].Item2 == 0)
                        badge_list_TR.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            plus_img;

                    else
                        badge_list_TR.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            Badge_img(badge_info[index].Item2);
                    badge_list_TR.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                    
                }

                else
                {
                    badge_list_TR.GetChild(1).GetChild(0).gameObject.SetActive(false);
                    badge_list_TR.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
                }

            }
            
            if (badge_info[index].Item3 == -1)
            {
                badge_list_TR.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
                badge_list_TR.GetChild(2).GetChild(1).gameObject.SetActive(true);
                badge_list_TR.GetChild(2).GetChild(0).gameObject.SetActive(false);
                if (is_locked)
                    badge_list_TR.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().interactable = false;

                else
                    badge_list_TR.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                badge_list_TR.GetChild(2).GetChild(1).gameObject.SetActive(false);
                if (!is_locked)
                {
                    badge_list_TR.GetChild(2).GetChild(0).gameObject.SetActive(true);
                    badge_list_TR.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
                    if (badge_info[index].Item3 == 0)
                        badge_list_TR.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            plus_img;

                    else
                        badge_list_TR.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            Badge_img(badge_info[index].Item3);
                    
                    badge_list_TR.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                }

                else
                {
                    badge_list_TR.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    badge_list_TR.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
                }
                
            }
            // Step 3. 종료 
        }

        // #1. 클릭시마다 해당 아바타의 뱃지 정보를 반영해주는 class 
        // #2. 뱃지 버튼을 누르면 메시지 띄우기 or 패널 띄워주기 컨트롤 함수 
        // #3. 장착해서 데이터 반영해주는 함수. 

        /// <summary>
        /// 장착 정보 index를 바탕으로, 뱃지 이미지를 지정해줌 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Sprite Badge_img(int value)
        {
            int index = calc_index(value);
            if (value == 0)
                return plus_img;
            
            string path = "Lobby/Badge/Badge_Img/" + index.ToString();
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
        
        /// <summary>
        /// 최종적으로 여기 저장되어있는 형태를 EquipDAO에 저장하게 해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Equip_Data(int index)
        {
            Badge_EquipDAO eqiup_data = new Badge_EquipDAO(Calc_Index.Get_Avatar_Num(index));
            eqiup_data.Set_Equip_data(badge_info[index].Item1,0);
            eqiup_data.Set_Equip_data(badge_info[index].Item2,1);
            eqiup_data.Set_Equip_data(badge_info[index].Item3,2);
            
        }

        /// <summary>
        /// Avatar_Badge info에 현재 캐릭터의 장착 정보를 반영해주는 함수 . 매개변수로 어떤 캐릭터인지, 어떤 뱃지를 장착하는지를 받는다.
        /// </summary>
        /// <param name="avatar_index"></param>
        public void Set_Avatar_Slot(int avatar_index, int badge_index, int which_slot)
        {
            switch (which_slot)
            {
                default:
                    badge_info[avatar_index] = new Tuple<int, int, int>(Set_Equip_data(badge_index),
                        badge_info[avatar_index].Item2, badge_info[avatar_index].Item3);
                    break;
                
                case 1 :
                    badge_info[avatar_index] = new Tuple<int, int, int>(badge_info[avatar_index].Item1,
                        Set_Equip_data(badge_index), badge_info[avatar_index].Item3);
                    break;
                
                case 2:
                    badge_info[avatar_index] = new Tuple<int, int, int>(badge_info[avatar_index].Item1,
                        badge_info[avatar_index].Item2, Set_Equip_data(badge_index));
                    break;

            }
            
        }

        /// <summary>
        /// UI를 바로 반영해주고, 해당 INDEX의 아바타 EQUIP 데이터를 바꾸어줌 
        /// </summary>
        /// <param name="which_slot"></param>
        public void Badge_Unlocked(int which_slot, int avatar_index)
        {
            Vector2 pos;
            Set_Avatar_Slot(avatar_index, 99, which_slot);
            Set_Equip_Data(avatar_index); // 데이터 저장 
            badge_list_TR.GetChild(which_slot).GetChild(0).gameObject.SetActive(true);
            badge_list_TR.GetChild(which_slot).gameObject.GetComponent<Button>().interactable = true;
            badge_list_TR.GetChild(which_slot).GetChild(0).gameObject.GetComponent<Image>().sprite =
                plus_img;
            badge_list_TR.GetChild(which_slot).GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            badge_list_TR.GetChild(which_slot).GetChild(1).gameObject.SetActive(false);
        }


        #region Set_num

        public int Set_Equip_data(int index)
        {
            int quantity = 0;
            switch (index)
            {
                default: // 스코어 관련 Set하게 만듬
                    quantity = Set_Score_data(index);
                    break;

                case 3:
                case 6:
                case 9:
                    quantity = Set_BallSpeed_data(index);
                    break;

                case 4:
                case 8:
                    quantity = Set_ItemDuration(index);
                    break;
                
                case 1:
                case 2:
                case 5:
                    quantity = Set_CandleBox(index);
                    break;
                    
                case 10:
                    quantity = 3000;
                    break;

                case 99:
                    quantity = 0;
                    break;
            }

            return quantity;
        }
        
        
        
        private int Set_Score_data(int index)
        {
            int quantity; // 수치를 저장하는 변수 
            switch (index)
            {
                default:
                    quantity = 1050;
                    break;
                
                case 7:
                    quantity = 1100;
                    break;
                
                case 11:
                    quantity = 1200;
                    break;
                
            }
            return quantity;
        }

        /// <summary>
        /// 볼 스피드와 관련된 정보를 저장해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int Set_BallSpeed_data(int index)
        {
            int quantity; // 수치를 저장하는 변수 
            switch (index)
            {
                default:
                    quantity = 2010;
                    break;
                
                case 6:
                    quantity = 2020;
                    break;    
                
                case 9:
                    quantity = 2030;
                    break;    
            }
            return quantity;
        }

        private int Set_ItemDuration(int index)
        {
            int quantity; // 수치를 저장하는 변수 
            switch (index)
            {
                default:
                    quantity = 5060;
                    break;
                
                case 8:
                    quantity = 5120;
                    break;
                
            }
            return quantity;
        }

        private int Set_CandleBox(int index)
        {
            int quantity;
            switch (index)
            {
                default:
                    quantity = 4030;
                    break;
                
                case 2:
                    quantity = 4050;
                    break;
                
                case 5:
                    quantity = 4100;
                    break;
            }
            
            return quantity;
        }

        #endregion
        
    }
}