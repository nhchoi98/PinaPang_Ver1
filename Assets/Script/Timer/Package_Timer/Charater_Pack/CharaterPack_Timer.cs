using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using Shop;

namespace Timer
{
    public class CharaterPack_Timer : MonoBehaviour 
    {
        [Header("Object")]
        [SerializeField] private Text superText;

        private DateTime targetTime;
        private CharaterPack_DAO _charaterPackDao;

        [Header("Popup")] [SerializeField] private GameObject popup_Panel;
        [SerializeField] private Transform package_TR;
        [SerializeField] private Transform package_lower_TR;
        
        [Header("Shop_TR")] [SerializeField] private Transform shopTR;
        private int which_charater;
        [SerializeField] private Panel_Control _panelControl;

        [Header("Lobby_Timer")] public Transform lobbyTimer;
        private Text lobbyTimer_Text;
        
        // 그냥 시작 하는 경우에는, shop의 패널만 띄워줌. 코드를 구분해야겠네.
        private void Awake()
        {
            _charaterPackDao = new CharaterPack_DAO();
            if (_charaterPackDao.Get_is_first())
                return;

            else
            {
                Set_Lobby_Timer();
                Set_Panel();
            }
        }

        /// <summary>
        ///  로비의 캐릭터 타이머를 만들어주는 함수 
        /// </summary>
        private void Set_Lobby_Timer()
        {
            which_charater = _charaterPackDao.Get_Which_Charater();
            if (which_charater == 99)
                return;
            
            switch (which_charater)
            {
                default:
                    return;
                
                case 0:
                    lobbyTimer.GetChild(0).gameObject.SetActive(true);
                    lobbyTimer.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    lobbyTimer_Text = lobbyTimer.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
                    break;

                case 1: // 우주비행사일 경우 
                    lobbyTimer.GetChild(1).gameObject.SetActive(true);
                    lobbyTimer.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    lobbyTimer_Text = lobbyTimer.GetChild(1).GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
                    break;

                case 2: // 곰일 경우 
                    lobbyTimer.GetChild(2).gameObject.SetActive(true);
                    lobbyTimer.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    lobbyTimer_Text = lobbyTimer.GetChild(2).GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
                    break;

                case 3: // 과학자일 경우 
                    lobbyTimer.GetChild(3).gameObject.SetActive(true);
                    lobbyTimer.GetChild(3).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    lobbyTimer_Text = lobbyTimer.GetChild(3).GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
                    break;
            }
            
        }

        public void OnClick_LobbyTimer()
        {
            switch (which_charater) // 하단의 상품을 꺼줌 
            {
                case 0: // 파티광일 경우 
                    package_TR.GetChild(0).gameObject.SetActive(true);
                    break;

                case 1: // 우주비행사일 경우 
                    package_TR.GetChild(1).gameObject.SetActive(true);
                    break;

                case 2: // 곰일 경우 
                    package_TR.GetChild(2).gameObject.SetActive(true);
                    break;

                case 3: // 과학자일 경우 
                    package_TR.GetChild(3).gameObject.SetActive(true);
                    break;
            }
            popup_Panel.SetActive(true);
        }
        
        void Set_Panel()
        {
            
            StartCoroutine(Timer());
            switch (which_charater) // 하단의 상품을 꺼줌 
            {
                case 0: // 파티광일 경우 
                    package_lower_TR.GetChild(7).gameObject.SetActive(false);
                    package_TR.GetChild(0).gameObject.SetActive(true);
                    break;

                case 1: // 우주비행사일 경우 
                    package_lower_TR.GetChild(8).gameObject.SetActive(false);
                    package_TR.GetChild(1).gameObject.SetActive(true);
                    break;

                case 2: // 곰일 경우 
                    package_lower_TR.GetChild(9).gameObject.SetActive(false);
                    package_TR.GetChild(2).gameObject.SetActive(true);
                    break;

                case 3: // 과학자일 경우 
                    package_lower_TR.GetChild(6).gameObject.SetActive(false);
                    package_TR.GetChild(3).gameObject.SetActive(true);
                    break;
            }
            _panelControl.Init_SuperSale(which_charater);
        }
        
        public IEnumerator Timer()
        {
            targetTime = _charaterPackDao.Get_TargetTime();
            while (true)
            {
                var delta_target = targetTime.Subtract(DateTime.UtcNow);
                superText.text = (int) delta_target.TotalHours + delta_target.ToString(@"\:mm\:ss");
                lobbyTimer_Text.text = (int) delta_target.TotalHours + delta_target.ToString(@"\:mm\:ss");
                if (delta_target < TimeSpan.FromSeconds(1))
                    break;
                
                yield return new WaitForSecondsRealtime(1.0f);
            }
            Action();
        }

        public void Action()
        {
            _charaterPackDao = new CharaterPack_DAO();
            _charaterPackDao.Determine_DateTime();
            which_charater = _charaterPackDao.Get_Which_Charater();
            _panelControl.Set_SuperSale();
            // 타이머 끄고, 할인가 지워야함 
        }
        
        public void Open_Lobby_Popup()
        {
            if (_charaterPackDao.Get_is_first())
            {
                _charaterPackDao.Set_is_first();
                Set_Lobby_Timer();
                Set_Panel();
            }
            
            if(which_charater!=99)
                popup_Panel.SetActive(true);
            
        }

        public int Get_Char_Target()
        {
            return _charaterPackDao.Get_Which_Charater();
        }

        public void Reset_SuperSale()
        {
            _charaterPackDao.Determine_DateTime(true);
            lobbyTimer.GetChild(0).gameObject.SetActive(false);
            lobbyTimer.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().enabled = false; // 우주비행사일 경우 
            lobbyTimer.GetChild(1).gameObject.SetActive(false);
            lobbyTimer.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>().enabled = false; 
            lobbyTimer.GetChild(2).gameObject.SetActive(false);
            lobbyTimer.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
            lobbyTimer.GetChild(3).gameObject.SetActive(false);
            lobbyTimer.GetChild(3).GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
            which_charater = _charaterPackDao.Get_Which_Charater();
            if (which_charater != 99)
            {
                StartCoroutine(Timer());
                shopTR.GetChild(which_charater).gameObject.SetActive(true);
                // 상점 아래 꺼줌 
                switch (which_charater)
                {
                    case 0: // 파티광일 경우 
                        package_lower_TR.GetChild(7).gameObject.SetActive(false);
                        break;

                    case 1: // 우주비행사일 경우 
                        package_lower_TR.GetChild(8).gameObject.SetActive(false);
                        break;

                    case 2: // 곰일 경우 
                        package_lower_TR.GetChild(9).gameObject.SetActive(false);
                        break;

                    case 3: // 과학자일 경우 
                        package_lower_TR.GetChild(6).gameObject.SetActive(false);
                        break;
                }

                _panelControl.Init_SuperSale(which_charater);
            }

            else
            {
                package_TR.GetChild(0).gameObject.SetActive(false);
            }
            Set_Lobby_Timer();
            
        }
    }
}
