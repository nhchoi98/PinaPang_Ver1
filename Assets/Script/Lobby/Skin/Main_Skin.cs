using System;
using Ad;
using Alarm;
using Avatar;
using Data;
using Lobby;
using Progetile;
using shop;
using Timer;
using UnityEngine;
using UnityEngine.UI;

namespace  Skin
{
    /// <summary>
    /// 스킨의 전반적인 탭들을 관리하고, 들어오고 나갈 때 어떤걸 띄울지 관리한다.
    /// </summary>
    public class Main_Skin : MonoBehaviour
    {
        public Transform Tap_Transform;
        public GameObject Main, Badge;
        [Header("Avatar_Equip")]
        private bool equipped_on; // 아바타를 
        private int avatar_index;
        [SerializeField] private Charater_Info main_UI;
        [SerializeField] private Image ballimg_skin_line;
        public SpriteRenderer ball_preview;
        public Text main_gem;
        public Text skin_gem;
        public Shop_Main ShopMain;
        public AudioSource click;

        public Transform packagePanel;
        [SerializeField] private CharaterPack_Timer _charaterPackTimer;

        [Header("Tutorial")] [SerializeField] private GameObject tutorial_Snack_first; // 첫 번째로 뜨는 스낵 튜토리얼 
        [SerializeField] private GameObject tap;
        [SerializeField] private GameObject tapBtn;
        [SerializeField] private GameObject badgeBtn;
        void OnEnable()
        {
            BallDAO data = new BallDAO();
            ball_preview.sprite = Set_Avatar_UI.Set_Ball_Img(data.Get_BallEquipped_Data());
            ballimg_skin_line.sprite = Set_Avatar_UI.Set_Ball_Img(data.Get_BallEquipped_Data());
            skin_gem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            if (PlayerPrefs.GetInt("Tutorial_Snack", 0) == 0) // 스낵 튜토리얼을 진행하지 않았을 경우 
            {
                Set_Snack_Tutorial();
            }
        }

        #region Tutorial
        /// <summary>
        /// 스낵 튜토리얼을 활성화 시켜줌 
        /// </summary>
        private void Set_Snack_Tutorial()
        {
            // Step 1. 탭과 관련된 요소들을 모두 꺼줌 
            tap.SetActive(false);
            tapBtn.SetActive(false);
            badgeBtn.SetActive(false);
            // Step 2. 튜토리얼 활성화 
            tutorial_Snack_first.SetActive(true);
        }
        
        
        /// <summary>
        /// 처음 튜토리얼 들어와서 껐던거 다 원상복구 시켜줌 
        /// </summary>
        public void Set_Tutorial_Reset()
        {
            tap.SetActive(true);
            tapBtn.SetActive(true);
        }

        public void TurnOn_SnackBtn()
        {
            badgeBtn.SetActive(true);
        }

        

        #endregion

        /// <summary>
        /// 스킨탭을 나갈 떄 
        /// </summary>
        private void OnDisable()
        {
            BallDAO data = new BallDAO();
            ball_preview.sprite = Set_Avatar_UI.Set_Ball_Img(data.Get_BallEquipped_Data());
            ballimg_skin_line.sprite = Set_Avatar_UI.Set_Ball_Img(data.Get_BallEquipped_Data());

            if (equipped_on)
            {
                var equipDATA = new EquippedDAO();
                equipDATA.Set_Equipped_index(avatar_index);
                // 로비의 캐릭터 이미지와 텍스트도 바꾸어 주어야함 
                equipped_on = false;
                main_UI.Set_charater_UI(avatar_index); // 메인의 캐릭터 바꿔줌 
            }

            main_gem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
        }

        #region Button_Action
        public void OnClick_Exit()
        {
            OnClick_Avatar_Tap();
            Main.SetActive(true);
            click.Play();
            this.gameObject.SetActive(false);
        }

        public void OnClick_Avatar_Tap()
        {
            click.Play();
            Tap_Transform.GetChild(0).gameObject.SetActive(true);
            Tap_Transform.GetChild(1).gameObject.SetActive(false);
            Tap_Transform.GetChild(2).gameObject.SetActive(false);
        }
        
        public void OnClick_Ball_Tap()
        {
            click.Play();
            Tap_Transform.GetChild(0).gameObject.SetActive(false);
            Tap_Transform.GetChild(1).gameObject.SetActive(true);
            Tap_Transform.GetChild(2).gameObject.SetActive(false);
        }
        
        public void OnClick_Line_Tap()
        {
            click.Play();
            Tap_Transform.GetChild(0).gameObject.SetActive(false);
            Tap_Transform.GetChild(1).gameObject.SetActive(false);
            Tap_Transform.GetChild(2).gameObject.SetActive(true);
        }

        public void OnClick_Badge()
        {
            click.Play();
            Badge.gameObject.SetActive(true);
        }

        #endregion
        /// <summary>
        /// 홈 버튼을 누르면 호출되는 함수 
        /// </summary>
        public void Set_Avatar_Equip(int index)
        {
            equipped_on = true;
            avatar_index = index;
        }

        /// <summary>
        /// 젬 부족시 젬 상점으로 이동시켜주는 함수 
        /// </summary>
        /// <param name="is_gem"></param>
        public void Go_To_GemShop()
        {
            ShopMain.OnClick_Goods();// 상점에서 젬 파트가 바로 보이도록 세팅
            click.Play();
            ShopMain.OnClick_Goods();
            ShopMain.OnClick_PanelOn();
        }
        
        public void Click_PackageBtn(int index)
        {
            if (_charaterPackTimer.Get_Char_Target() == index)
                index += 4;
            
            switch (index)
            {
                default:
                    throw new Exception();
                
                case 0: // 파티_노말 패널 띄우기 
                    packagePanel.GetChild(0).gameObject.SetActive(true);
                    break;
                
                case 1: //우주비행사_노말 패널 띄우기
                    packagePanel.GetChild(1).gameObject.SetActive(true);
                    break;
                
                case 2: // 테디 베어_노말 패널 띄우기
                    packagePanel.GetChild(2).gameObject.SetActive(true);
                    break;
                
                case 3: // 과학자_노말 패널 띄우기 
                    packagePanel.GetChild(3).gameObject.SetActive(true);
                    break;
                
                case 4: // 파티_세일 패널 띄우기 
                    packagePanel.GetChild(4).gameObject.SetActive(true);
                    break;
                
                case 5: // 우주비행사_세일 패널 띄우기 
                    packagePanel.GetChild(5).gameObject.SetActive(true);
                    break;
                
                case 6:
                    packagePanel.GetChild(6).gameObject.SetActive(true);
                    break;
                
                case 7:
                    packagePanel.GetChild(7).gameObject.SetActive(true);
                    break;
            }          
            
            
        }
        
    }

}
