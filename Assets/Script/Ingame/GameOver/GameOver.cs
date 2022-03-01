
using System;
using System.Collections;
using Avatar;
using System.Collections.Generic;
using Badge;
using Battery;
using Challenge;
using Collection;
using Daily_Reward;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Data;
using DG.Tweening;
using Ingame;
using Log;
using Progetile;
using Score;
using Toy;
using UnityEngine.EventSystems;

namespace Manager
{
    public class GameOver : MonoBehaviour, IPointerDownHandler
    {
        [Header("OTHER_UI")] 
        public GameObject tapToContinue;
        public GameObject firstPage;
        public GameObject secondPage;
        
        [Header("Score_Panel_Text")] [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private Text Basic_Score;
        [SerializeField] private Text Good_Score;
        [SerializeField] private Text Great_Score;
        [SerializeField] private Text Excellent_Score;
        [SerializeField] private Text Bonus_score;
        
        [Header("Bonus_Panel_Text")]
        [SerializeField] private Text charaterBonus;
        [SerializeField] private Text badgeBonus;

        [Header("USER_EQUIPPMENT")] 
        [SerializeField] private Animator charaterAni;
        [SerializeField] private SpriteRenderer ball;
        public GameObject charater;

        [Header("ScorePanel")] 
        public GameObject scorePanel;

        [Header("Quest")] 
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private CollectionManager _collectionManager;
        [SerializeField] private Determine_BoxType get_turn;
        public GameObject heart_Not_enough;
        private BatteryDAO battery;
        private IMediator _mediator;
        [SerializeField] private  Ad_Mediator ad;
        
        [Header("Score_Info")]
        private int bestPrevious;
        private int bestScore;
        private int totalScore;
        private int bonusScore;
        
        [Header("MAIN_SCORE_TEXT")]
        [SerializeField] private Text bestScore_Text;
        [SerializeField] private Text scoreText;
        //[SerializeField] private Text bonusText;
        
        [Header("Setup_FLAG")]
        private bool over_best, touch_pushed, particle_on;
        private int stageCount = 0; // 몇 번째 화면을 보고있는지 기록 

        [Header("Reward_Obj")] 
        public GameObject rewardObj;
        public Scrollbar scrollbar;
        public Transform content;
        public GameObject rewardTR;

        [Header("Candy_Result")] public GameOver_CandyResult candyResult;
        
        public GameObject best_set_particle;
        public AudioSource best_sound, count_sou;
        private int candy;

        private IEnumerator now_running;
        private int index;
        
        private bool is_open;
        public GameObject forbid_cast;
        
        private void Awake()
        {
            Set_Equip_Animation();
            Set_Infomation();
            Set_BonusPanel();
            Set_ScorePanel();
            forbid_cast.SetActive(false);
        }
        
        #region Info_Init
        IEnumerator Charater_Move()
        {
            Vector2 Target_Pos = new Vector2(-34f, 484f);
            Transform TR = charater.transform;
            TR.GetComponent<Animator>().SetTrigger("Move");
            TR.DOMove(Target_Pos,2.0f)
                .OnStart(() => { StartCoroutine(Ball_Rotate()); })
                    .SetEase(Ease.Linear)
                        .OnComplete((() => { TR.GetComponent<Animator>().SetTrigger("Arrive"); }));
                
            yield return null;
        }

        IEnumerator Ball_Rotate()
        {
            Transform TR = charater.transform;
            TR.GetChild(5).DORotate(new Vector3(0f, 0f, 185f), 2.5f);
            yield return null;
        }
        
        /// <summary>
        /// 게임에 관련된 로그를 서버로 보내고, 보상 정보를 반영해주는 함수 
        /// </summary>
        private void Set_Infomation()
        {
            // Pre. BestScore 반영 
            bonusScore = (int)(_scoreManager.score*((AbilityDAO.Get_Bonus()/1000f)-1f));
            totalScore = _scoreManager.score + bonusScore;
            // Step 1. 로그 반영 
            Ingame_Log.Log_Turn(get_turn.Get_Stage());
            Ingame_Log.Log_Score(totalScore);
            Ingame_Log.Set_Toy(_collectionManager.total_ingame_count);
            Ingame_Log.Log_User_Count();
            
            // Step 2. 보상 및 퀘스트 정보 반영 
            _questManager.Set_Score(bonusScore); // 보너스 스코어도 stat에 반영해주기 
            _questManager.Set_User_Stat_Save();

            candy = totalScore / 1000;
            Playerdata_DAO.Set_Player_Candy(candy);
            Set_CandyConst(candy);
            if (totalScore > Playerdata_DAO.Player_BestScore())
            {
                bestScore_Text.text = string.Format("{0:#,0}", Playerdata_DAO.Player_BestScore());
                bestPrevious = Playerdata_DAO.Player_BestScore();
                Playerdata_DAO.Set_Best_Score(totalScore);
                over_best = true;
                bestScore = bestPrevious;
            }

            else
                bestScore = Playerdata_DAO.Player_BestScore();
            
            
            bestScore_Text.text = string.Format("{0:#,0}", bestScore);
            
        }
        

        public IEnumerator Set_Reward_List()
        {
            Transform tr;
            Tuple<int, int> item;
            SingleToyDAO toyData;
            for (int i = 0; i < _collectionManager.Get_Collection_Num(); i++)
            {
                item = _collectionManager.Get_Collection_List(i);
                tr = Instantiate(rewardObj).transform;
                tr.SetParent(content);
                tr.SetAsLastSibling();
                tr.GetChild(0).gameObject.GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Lobby/Collection/Collection_" +item.Item1);

                tr.GetChild(1).gameObject.GetComponent<Text>().text = item.Item2.ToString();
                toyData = new SingleToyDAO(item.Item1);
                if (toyData.Get_IsNew())
                {
                    tr.GetChild(2).gameObject.SetActive(true);
                    toyData.Set_IsNew();
                }
                scrollbar.value = 0f;
                yield return new WaitForSeconds(0.1f);
            }
            scrollbar.enabled = true;
            yield return StartCoroutine(TaptoContinue());
            yield return null;
        }

        /// <summary>
        /// 캔디 애니메이션을 작동시키기 위한 함수 
        /// </summary>
        /// <param name="candy"></param>
        private void Set_CandyConst(int candy)
        {
            if (candy < 10)
                candyResult.Set_CandyConst(1);

            else
                candyResult.Set_CandyConst(candy/10f);
            
            candyResult.Set_CandyNum(candy);
        }
        
        /// <summary>
        /// 스코어 패널들의 점수 정보를 Set 해주는 함수 
        /// </summary>
        private void Set_ScorePanel()
        {
            Basic_Score.text = String.Format("{0:#,0}", _scoreManager.basic_score);
            Good_Score.text = String.Format("{0:#,0}", _scoreManager.good_socre);
            Great_Score.text = String.Format("{0:#,0}", _scoreManager.great_score);
            Excellent_Score.text = String.Format("{0:#,0}", _scoreManager.excellent_score);
            Bonus_score.text = String.Format("{0:#,0}", bonusScore);
        }
        
        // 사용자가 장착하고 있는 공과 BALL의 종류를 Set 해주는 함수 
        private void Set_Equip_Animation()
        {
            var equipDATA = new EquippedDAO();
            int index;
            Time.timeScale = 1;
            BallDAO data = new BallDAO();
            int ball_num  = data.Get_BallEquipped_Data(); // 장착한 공의 index를 불러옴
            index = equipDATA.Get_Equipped_index();
            battery = new BatteryDAO();
            battery.Loading_Charge();
            charaterAni.runtimeAnimatorController = Set_Avatar_UI.Set_Charater_GameObject(index);// 애니메이션 바꾸어줌. 임시로 원숭이 
            ball.sprite =  Set_Avatar_UI.Set_Ball_Img(ball_num); // #1. ball 이미지 불러옴 
            scrollbar.enabled = false;
            StartCoroutine(Charater_Move());
            // 볼도 여기 추가되어야함.
        }

        /// <summary>
        /// 보너스 획득 정보를 Set 해주는 함수 
        /// </summary>
        private void Set_BonusPanel()
        {
            badgeBonus.text = "+"+ ((AbilityDAO.badge_bonus/10)) + "%";
            charaterBonus.text = "+" + (AbilityDAO.charater_bonus/10) + "%";
        }
        
        #endregion
        
        #region Button
        public void Home_Btn()
        {
            ad.Destroy_Banner();
            ad.Remove_CallBack();
            SceneManager.LoadScene("Loading_Scene_Game");
            PlayerPrefs.SetInt("Ingame",0);
            PlayerPrefs.SetInt("Play_Game",0);
        }

        public void Retry_Btn()
        {
            if (battery.Get_Count() > 7)
            {
                ad.Destroy_Banner();
                ad.Remove_CallBack();
                battery.Ingame_Start();
                SceneManager.LoadScene("Loading_Scene_Game");
                PlayerPrefs.SetInt("Play_Game", 0);
            }

            else
            {
                heart_Not_enough.SetActive(true); 
            }
        }

        public void OnClick_ScorePanel()
        {
            scorePanel.SetActive(true);
        }

        public void OnClick_ScorePanel_False()
        {
            scorePanel.SetActive(false);    
        }
        #endregion
        
        public IEnumerator Set_Score_root()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            now_running = Set_Score(0);
            yield return StartCoroutine(now_running);
            yield return new WaitForSecondsRealtime(0.3f);
            rewardTR.SetActive(true);
        }

        private IEnumerator Set_Score(int value)
        {
            Text target_text;
            int target_score;
            float bestTargetConst;
            float count_score = 0;
            float targetConst;

            target_text = scoreText;
            target_score = totalScore;
            target_text.text = "0";
            targetConst = totalScore / 100f;
            count_sou.Play();
            // Step 3. 카운트 숫자 지정해주기 
            while (true)
            {
                if (count_score >= target_score)
                {
                    target_text.text = string.Format("{0:#,0}", target_score);
                    if(over_best)
                        bestScore_Text.text =string.Format("{0:#,0}", target_score);
                    
                    break;
                }

                count_score += (targetConst);
                target_text.text = string.Format("{0:#,0}", (int) count_score);
                if (over_best && count_score > bestPrevious)
                {
                    if (!particle_on)
                        StartCoroutine(Best_Particle());
                    
                    bestScore_Text.text = string.Format("{0:#,0}",(int)count_score);
                }
                
                yield return new WaitForSecondsRealtime(0.0001f);
                if (!count_sou.isPlaying)
                    count_sou.Play();
                
                target_text.text = string.Format("{0:#,0}", (int) target_score);
                
            }
        }

        IEnumerator Best_Particle()
        {
            if (particle_on)
                yield break;
            
            best_set_particle.SetActive(true);
            best_set_particle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            best_set_particle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            best_set_particle.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
            best_sound.Play();
            particle_on = true;
            yield return new WaitForSecondsRealtime(0.4f);
            best_sound.Play();
            yield return new WaitForSecondsRealtime(0.4f);
            best_sound.Play();
            yield return null;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (stageCount == 1)
            {
                firstPage.SetActive(false);
                secondPage.SetActive(true);
                candyResult.Start_SecondPage();
                touch_pushed = true;
                stageCount++;
            }
        }
        
        IEnumerator TaptoContinue()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            tapToContinue.SetActive(true);
            stageCount++;
            touch_pushed = false;
        }

        
    }
}
