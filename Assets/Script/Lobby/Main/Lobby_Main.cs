
using System;
using Ad;
using Data;
using Timer;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby
{
    /// <summary>
    /// 로비에서 버튼들의 기능들을 담당하고, 화면 이동에 대한 기능을 총괄하는 스크립트.
    ///1. 플레이 버튼
// 2. 스킨 버튼
// 3. 상점 버튼
// 4. 컬렉션 버튼
// 5. 뽑기 버튼
// 6. 설정 버튼
// 7. 랭킹 버튼
// 8. 캔디 충전 버튼
    /// </summary>
    public class Lobby_Main : MonoBehaviour
    {
        [SerializeField] private GameObject  Skin_Panel, settingPanel;
        [SerializeField] private GameObject Challenge;
        [SerializeField] private Lobby_Ad_Mediator _adMediator;
        public SpriteRenderer Background; // 배경 이미지를 관리해주는 변수 
        public Lobby_Playerinfo lp;
        private bool hamburger;
        public AudioSource click;

        public AudioMixer mixer;

        [Header("Timer")] [SerializeField] private StarterTimer _starterTimer;
        [SerializeField] private CharaterPack_Timer pack_timer;
        public GameObject collection_panel;

        public AudioSource gem_flying_sound;

        public GameObject tutorial;

        public GameObject gemInfo;
        public GameObject exitPanel;
        [SerializeField] private Text bestScore;
        private void Awake()
        {            
            int Master_Volume = PlayerPrefs.GetInt("Master_Volume", -17);
            int BGM_Volume = PlayerPrefs.GetInt("Background_Volume", -17);
            Time.timeScale = 1f;

             if(Master_Volume <=-35f)
                 mixer.SetFloat("SFX", -80f);
             
             else
                 mixer.SetFloat("SFX", Master_Volume);
 
             
             if(BGM_Volume <=-35f)
                 mixer.SetFloat("BGM", -80f);
             
             else
                 mixer.SetFloat("BGM", BGM_Volume);
             
             if( (PlayerPrefs.GetInt("Tutorial_Exchange",0) == 0)  && (PlayerPrefs.GetInt("Tutorial_Basic_Ingame",0)== 1))
                 tutorial.SetActive(true);

             bestScore.text = String.Format("{0:#,0}", Playerdata_DAO.Player_BestScore());
        }
        

        private void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                exitPanel.SetActive(true);
            }
        }

        public void OnClick_Exit_Yes()
        {
            Application.Quit();
        }

        public void OnClick_Exit_No()
        {
            exitPanel.SetActive(false);
        }
        
        #region Button_Func

        public void OnClick_Collection()
        {
            collection_panel.SetActive(true);
        }
        
        public void Click_Setting()
        {
            click.Play();
            settingPanel.SetActive(true);
        }
        
        /// <summary>
        /// 플레이버튼. 위에 스테이지 점핑 패널을 띄운다. 
        /// </summary>
        public void Click_Play_Btn()
        {
            // 로딩 화면 로드해야함 
            click.Play();
            _adMediator.Remove_CallBack(); // 광고 이벤트 제거 
            PlayerPrefs.SetInt("Ingame",1);
            SceneManager.LoadScene("Loading_Scene_Game");
            // 인게임으로 들어가는 변수를 설정해주어야함 
        }

        // 2. 스킨 버튼
        public void Click_Skin_Btn()
        {
            click.Play();
            Skin_Panel.SetActive(true);
        }

        public void Close_Skin()
        {
            if (PlayerPrefs.GetInt("First_Skin", 0) == 0)
            {
                PlayerPrefs.SetInt("First_Skin",1);
                Determine_StarterDAO starterDao = new Determine_StarterDAO();
                if(starterDao.Get_is_first())
                    starterDao.Set_is_first();

                _starterTimer.First_Start(true);
            }
        }

        /// <summary>
        /// 햄버거 버튼을 누르면 실행되는 함수. 자연스러운 연출을 위해 계속 눌러도 애니메이션 끝나면 켜지게 만듬 
        /// </summary>
        public void Challenge_Btn()
        {
            click.Play();
            Challenge.SetActive(true);
        }

        public void Play_Click()
        {
            click.Play();
        }

        public void Play_Gem_Sound()
        {
            gem_flying_sound.Play();
        }
        #endregion

        public void OnClick_GemBar()
        {
            click.Play();
            gemInfo.SetActive(true);
        }

        private void OnApplicationQuit()
        {
            FirstPurchase_Timer.Save_Data();
        }
    }
}
