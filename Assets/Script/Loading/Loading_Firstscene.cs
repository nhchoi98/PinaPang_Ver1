
using System;
using System.Collections;
using Ad;
using Badge;
using Battery;
using Daily_Reward;
using Log;
using Timer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;     
#endif

namespace Loading
{
    public class Loading_Firstscene : MonoBehaviour
    {
        public static string nextScene;
        int TimeCount ,Point_Num;
        public Text LogText;
        public Text PointText, TipText, Loading, Loading_percent;
        public Image  BackGroundImg;
        public Slider Loading_Slider;
        private float time1, time2, time3 ,F_time, F_time2;
        int rand;
        AsyncOperation op;
        private float time;
        string Loading_Text = "Packing a gift" ;
        

        [Header("Data_Set")] private Init_Setting _initSetting;
        // Start is called before the first frame update

        [Header("Term_Panel")] [SerializeField] private GameObject termPanel;
        private bool is_privacy_clicked = false;
        private bool is_terms_clicked = false;
        [SerializeField] private Button acceptBtn;
        
        [SerializeField] private AudioSource clickSound;
        public event Action sentTrackingAuthorizationRequest;

        #region Term

        public void OnClick_Privacy_View()
        {
            Application.OpenURL("http://www.anydog.co.kr/assets/privacyPolicy.html");
        }

        public void OnClick_terms_View()
        {
            Application.OpenURL("http://www.anydog.co.kr/assets/termsofService.html");
        }

        public void OnClick_privacy_click()
        {
            clickSound.Play();
            if (is_privacy_clicked)
            {
                is_privacy_clicked = false;
                acceptBtn.interactable = false;
            }
            else
            {
                is_privacy_clicked = true;
                if (is_terms_clicked)
                    acceptBtn.interactable = true;
            }
        }

        public void OnClick_Terms_Clicked()
        {
            clickSound.Play();
            if (is_terms_clicked)
            {
                is_terms_clicked = false;
                acceptBtn.interactable = false;
            }
            else
            {
                is_terms_clicked = true;
                if(is_privacy_clicked)
                    acceptBtn.interactable = true;
            }
        }

        /// <summary>
        /// clicksound 호출 x 
        /// </summary>
        public void OnClick_Accept()
        {
            clickSound.Play();
            if (is_privacy_clicked && is_terms_clicked)
            {
                PlayerPrefs.SetInt("Tutorial_Terms", 1);
                Next_Scene();
                termPanel.SetActive(false);
            }

            else
                return;
        }
        
        #endregion
        private void Awake()
        {
            Application.targetFrameRate = 60;
            rand = UnityEngine.Random.Range(0, 4);
            F_time = 1.0f;
            F_time2 = 2.5f;
            _initSetting = new Init_Setting();
            BatteryDAO batteryDao = new BatteryDAO();
            Noads_instance.Init_Data();
            Ad_Init.Init_Ad();
            batteryDao.Loading_Charge(); // 차지 
            Package_Update();
            if (PlayerPrefs.GetInt("Tutorial_Terms", 0) == 0)
                termPanel.SetActive(true);
            
            #if UNITY_IOS
            ATTrackingStatusBinding.RequestAuthorizationTracking(AuthorizationTrackingReceived);
            sentTrackingAuthorizationRequest?.Invoke();
            #endif
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Ingame_Log.LogIn();
            FirstPurchase_Timer.Init_Data();
        }

        private void AuthorizationTrackingReceived(int status) {
            Debug.LogFormat("Tracking status received: {0}", status);

        }
        
        void Start()
        {
            if (PlayerPrefs.GetInt("Tutorial_Terms", 0) != 0){
                if (PlayerPrefs.GetInt("Tutorial_Basic_Ingame", 0) == 0)
                {
                    PlayerPrefs.SetInt("Ingame", 1);
                    nextScene = "Ingame";
                    AbilityDAO.Set_Ability_Data(0);
                }

                else
                    nextScene = "Lobby_1";
                
                op = SceneManager.LoadSceneAsync(nextScene);
                op.allowSceneActivation = false;
                StartCoroutine(LoadScene());
            }

            StartCoroutine(MainSplash());
        }

        private void Next_Scene()
        {
            if (PlayerPrefs.GetInt("Tutorial_Basic_Ingame", 0) == 0)
            {
                PlayerPrefs.SetInt("Ingame",1);
                nextScene = "Ingame";
                DailyDAO timedata = new DailyDAO();
                timedata.Get_reward_time();
                AbilityDAO.Set_Ability_Data(0);
            }
            
            else 
                nextScene = "Lobby_1";
            
            op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            StartCoroutine(LoadScene());
        }

        private void FixedUpdate()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (++TimeCount == 30) {
                if(Point_Num == 0) 
                {
                    Loading.text = Loading.text.ToString() + " .";
                    TimeCount = 0;
                    ++Point_Num;
                }


                else if (Point_Num == 1)
                {
                    Loading.text = Loading.text.ToString() + " .";
                    TimeCount = 0;
                    ++Point_Num;
                }


                else if (Point_Num == 2)
                {
                    Loading.text = Loading.text.ToString() + " .";
                    TimeCount = 0;
                    ++Point_Num;
                }


                else if (Point_Num == 3)
                {
                    Loading.text = "Packing a gift";
                    TimeCount = 0;
                    Point_Num = 0;
                }


                TimeCount = 0;
            }
        }
        
        IEnumerator MainSplash()
        {
            Color color = BackGroundImg.color;                            //color 에 판넬 이미지 참조
            while (BackGroundImg.color.a > 0f)
            {
                time1 += Time.deltaTime / F_time;
                color.a = Mathf.Lerp(1f, 0f, time1);
                BackGroundImg.color = color;
                yield return null;
            }

            yield return null;
        }
        
        IEnumerator LoadScene()
        {

            Color color = BackGroundImg.color;
            while(time3<F_time2)
            {
                time3 += Time.deltaTime;
                Loading_Slider.value = Mathf.Lerp(0f, 1f, time3);
                Loading_percent.text = Mathf.RoundToInt((Loading_Slider.value * 100f)).ToString()+"%";
                yield return null;
            }
            
            // Fade -out 
            while (color.a <= 0.999f)
            {
                time2 += Time.deltaTime / F_time;
                color.a = Mathf.Lerp(0f, 1f, time2);
                BackGroundImg.color = color;
                yield return null;

            }
            op.allowSceneActivation = true;
            yield return null;
        }
        
        #region Package_Determine
        

        /// <summary>
        /// 구매 가능 여부 + 할인 가능 여부를 업데이트 해줌 
        /// </summary>
        private void Package_Update()
        {
            if (PlayerPrefs.GetInt("Tutorial_Basic_Reward", 0) == 1)
            {
                Determine_StarterDAO starterData = new Determine_StarterDAO();
                CharaterPack_DAO charPackData = new CharaterPack_DAO();
                starterData.Set_Purchasable(); // # 1. 스타터 팩의 결제 가능 여부  업데이트  
                charPackData.Determine_DateTime();
                PlayerPrefs.SetInt("First_Show_Popup",1);
            }
        }
        #endregion

        private void OnApplicationQuit()
        {
            FirstPurchase_Timer.Save_Data();
        }
    }
}
