using System;
using System.Collections.Generic;
using Ad;
using Alarm;
using Avatar;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Lobby;

namespace Skin
{
    public class Skin_Line : MonoBehaviour
    {
        //[Header("Line_Preview")]
        [Header("Condition_info")]
        public Transform char_tr; // Child 0: 카드 텍스트, Child 1: 아웃라인, Child 2: 장착 표시, Chilc 3: use 버튼 
        public int pre_click_index;

        [SerializeField] private LineAd ad;

        [Header("Data")]
        public List<bool> IsLocked; // 카드 

        public LineRenderer line_preview;
        private LineDAO data; 
        
        [Header("Theme_Data")]
        private int target_index;
        private readonly int MAXINDEX = 15; // 

        public GameObject adBtn;
        private Vector2 targetPos = new Vector2(-347, 520);
        
        [Header("Sound")] public AudioSource buy, use;

        public GameObject buyPackage;
        public GameObject mediator;

        [Header("Package_Btn")] 
        private int package_number;
        [SerializeField] private Main_Skin _mainSkin;
        
        private void Start()
        {
            data = new LineDAO(true);
            Lineinfo_Init();
            //Step 1. target_index 초기화 
            target_index = data.Get_Equip_Data();
            pre_click_index = target_index;
            Preview_Change(target_index);
            // Step 2. 장착한 친구는 체크 표시 해줌. TRANSFORM을 참조하여 체크표시할 곳 체그하기 
            char_tr.GetChild(target_index).GetChild(0).gameObject.SetActive(true);
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            
        }

        ///
        /// 라인의 구매정보를 스크립트로 업데이트 해주고, 이를 UI에 반영해숨 
        /// </summary>
        public void Lineinfo_Init()
        {
            if (data == null)
                data = new LineDAO(true);
            
            IsLocked = new List<bool>();
            for(int i =0; i< MAXINDEX; i++)
                Determine_Locked(i);
        }
        
        private void Determine_Locked(int index)
        {
            bool Is_locked = data.Get_Is_Locked(index);
            IsLocked.Add(!Is_locked);
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(false); // 선택시 표시되는 외각선 꺼줌 
            Image image = char_tr.GetChild(index).GetChild(2).gameObject.GetComponent<Image>();
            // 1. Transform 참조하여 카드 데이터 반영해주기
            if (!Is_locked) // 구매 안했다면
            {
                char_tr.GetChild(index).GetChild(6).gameObject.SetActive(false);
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(true);
                if (index != 13 && index != 14)
                {
                    char_tr.GetChild(index).GetChild(7).GetChild(2).gameObject.SetActive(true);
                }
                
                Avatar_Gray.SetGrayScale(ref image,1f);
            }

            else
            {
                char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
                char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
                switch (index)
                {
                    case 13:
                        if(PlayerPrefs.GetInt("LINE_13", 0) == 1)
                            char_tr.GetChild(index).GetChild(8).gameObject.SetActive(true);
                        break;
                        
                    case 14:
                        if(PlayerPrefs.GetInt("LINE_14", 0) == 1)
                            char_tr.GetChild(index).GetChild(8).gameObject.SetActive(true);
                        break;
                }
                image.material = null;

            }
            
            
        }

        public void Reward_Get(int num)
        {
            IsLocked[num] = false;
            LineDAO lineDao = new LineDAO(num);
            lineDao .Set_Line_Purchased(num);
            Image image = char_tr.GetChild(num).GetChild(2).gameObject.GetComponent<Image>();
            image.material = null;
            char_tr.GetChild(num).GetChild(7).gameObject.SetActive(false);
            char_tr.GetChild(num).GetChild(6).gameObject.SetActive(true);
        }

        
        
        #region Button
        /// <summary>
        /// 라인 그리드를 클릭하면 실행되는 함수. 프리뷰도 보여준다.
        /// </summary>
        /// <param name="index"></param>
        public void Line_Btn(int index)
        {
            if(!IsLocked[pre_click_index])
                char_tr.GetChild(pre_click_index).GetChild(6).GetChild(0).gameObject.SetActive(false); // 전에 선택한 곳의 Use button 꺼주기 
            
            char_tr.GetChild(pre_click_index).GetChild(0).gameObject.SetActive(false); // Selected Outline 꺼주기 
            char_tr.GetChild(index).GetChild(0).gameObject.SetActive(true);// 1. 선택한 곳에 테두리 활성화
            pre_click_index = index;
            // 프리뷰 이미지 바꾸어주기 
            // 2. 장착 되어있으면 테두리만
            // 3. 해금 되어 있으면 Use 버튼 활성화
            if (!IsLocked[index])
            {
                adBtn.SetActive(false);
                buyPackage.SetActive(false);
                if(index != target_index)
                    char_tr.GetChild(index).GetChild(6).GetChild(0).gameObject.SetActive(true);
            }

            else
            {
                if (index != 13 && index != 14)
                {
                    buyPackage.SetActive(false);
                    adBtn.SetActive(true);
                    ad.Set_Index(pre_click_index);
                }

                else
                {
                    Set_Package_index();
                    buyPackage.SetActive(true);
                    adBtn.SetActive(false);
                }
                // 광고 버튼 관련 설정 들어가야함 
            }
            
            if (char_tr.GetChild(index).GetChild(8).gameObject.activeSelf)
            {
                mediator.GetComponent<IAlarmMediator>().Event_Receieve(Event_Alarm.LINE_ALARM_OFF,index);
                char_tr.GetChild(index).GetChild(8).gameObject.SetActive(false);
            }
            Preview_Change(index);
        }

        private void Set_Package_index()
        {
            switch (pre_click_index)
            {
                case 13:
                    package_number = 2;
                    break;
                
                case 14:
                    package_number = 1;
                    break;
            }
        }

        public void OnClick_Package_Btn()
        {
            _mainSkin.Click_PackageBtn(package_number);
        }
        
        public void Equipped_Btn(int index)
        {
            // Step 2. UI상 체크 표시를 다른 친구로 바꾸어줌 
            use.Play();
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            data.Set_Equip_Data(target_index);
        }

        public void Get_Line(int index)
        {
            char_tr.GetChild(index).GetChild(6).gameObject.SetActive(true);
            char_tr.GetChild(index).GetChild(7).gameObject.SetActive(false);
            IsLocked[index] = false;
            Line_Btn(index);
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(false);
            target_index = index;
            char_tr.GetChild(target_index).GetChild(4).gameObject.SetActive(true);
            // Step 4. use 버튼 꺼줌 
            char_tr.GetChild(target_index).GetChild(6).GetChild(0).gameObject.SetActive(false);
            data.Set_Equip_Data(target_index);
        }

        #endregion

        private void Preview_Change(int index)
        {
            float distance = Vector3.Distance(line_preview.GetPosition(0), line_preview.GetPosition(1));
            float const_tiling = Determine_Line.Get_Line_tilingconst(index);
            line_preview.startWidth = Determine_Line.line_width(index);
            line_preview.endWidth = Determine_Line.line_width(index);
            line_preview.material = Determine_Line.line_material(index);
            line_preview.material.mainTextureScale = new Vector2(const_tiling*distance,1);
        }
    }
}
