
using System.Collections;
using Daily_Reward;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Badge;
using Battery;
using Timer;
using DG.Tweening;

namespace Loading
{
    public class Loading_Ingame : MonoBehaviour
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
        // Start is called before the first frame update
        private void Awake()
        {
            rand = UnityEngine.Random.Range(0, 4);
            F_time = 1.0f;
            F_time2 = 2.5f;
            Time.timeScale = 1f;
            Set_Tip_Text();
            StartCoroutine(MainSplash());
            BatteryDAO batteryDao = new BatteryDAO();
            batteryDao.Loading_Charge();
            Package_Update(); // 패키지 상품 구매 여부 업데이트 
            DOTween.Clear();
            
        }
        void Start()
        {
            
            //StartCoroutine(LogIn());
            /*
            if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
                nextScene = "Tutorial";
            */
            
            // 인게임에 입장할 때 
            if (PlayerPrefs.GetInt("Ingame", 0) == 1)
            {
                nextScene = "Ingame";
                DailyDAO timedata = new DailyDAO();
                timedata.Get_reward_time();
                AbilityDAO.Set_Ability_Data(0);
            }

            else
            {
                nextScene = "Lobby_1";
            }

            op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            StartCoroutine(LoadScene());
        }

        /*
        // Update is called once per frame
        IEnumerator LogIn()
        {
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success) LogText.text = Social.localUser.id + "\n" + Social.localUser.userName + ((PlayGamesLocalUser)Social.localUser).Email;
                else LogText.text = "구글 로그인 실패";

            });
            yield return null;
        }
        */

        private void FixedUpdate()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void OnApplicationQuit()
        {
            FirstPurchase_Timer.Save_Data();
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

        private void Set_Tip_Text()
        {
            int rand = UnityEngine.Random.Range(0, 13);

            switch (rand)
            {
                default:
                    TipText.text = "Kevin insists his birthday is all year around.";
                    break;
                
                case 1:
                    TipText.text = "Believe it or not, but kevin is 5 years old.";
                    break;  
                
                case 2:
                    TipText.text = "Pinata will be shaken by balls you shooting. Other boxes can be broke by the pinata.";
                    break; 
                
                case 3:
                    TipText.text = "Kevin has GF named 'Minx'.";
                    break;

                case 4:
                    TipText.text = "Breaking a pinata makes all the boxes break which are on the field.";
                    break;  
                
                case 5:
                    TipText.text = "asdfawqded12 das1qw,,, Kevin pressed the keyboard and gone.";
                    break;  
                
                case 6:
                    TipText.text = "You can control the shooting angle by touching anywhere on the screen.";
                    break;

                case 7:
                    TipText.text = "There are so many toys in the pinata.";
                    break; 
                
                case 8:
                    TipText.text = "[Extra Line] shows the direction in which the ball bounces once.";
                    break;
                
                case 9:
                    TipText.text = "You can turn vibration on and off in the Settings window.";
                    break;
                
                case 10:
                    TipText.text = "You can exchange 5 candies for 1 Gem at the exchange.";
                    break;     
                
                case 11:
                    TipText.text = "Collect many toys and exchange them with Gem.";
                    break;
                
                case 12:
                    TipText.text = "Teddy, who exchanges candies to gems, is actually Kevin's...";
                    break;
            }


        }
        
        private void Package_Update()
        {
            Determine_StarterDAO starterData = new Determine_StarterDAO();
            CharaterPack_DAO charPackData = new CharaterPack_DAO();
            starterData.Set_Purchasable();// # 1. 스타터 팩의 결제 가능 여부  업데이트  
            charPackData.Determine_DateTime();
        }
    }
}
