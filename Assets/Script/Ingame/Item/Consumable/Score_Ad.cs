
using System.Collections;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Challenge;
using Ingame;
using Log;
using Score;

namespace Ad
{
    public class Score_Ad : MonoBehaviour, IComponent
    {
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private SettingManager _settingManager;
        public GameObject locked_obj;
        public Button gem_btn, ad_btn, gem_line, gem_both; //gem_mommy;
        public GameObject itemPanel;
        public Text timer, timer_lower;
        public Transform item_icon;
        public GameObject activation;
        private IMediator _mediator; 

        public Text gemText;
        public Transform buttonTR;

        public void Start()
        {
            EXP_DAO level_data = new EXP_DAO();

            if (Noads_instance.Get_Is_Noads())
            {
                ad_btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
                gem_btn.gameObject.SetActive(false);
            }

            else
                ad_btn = buttonTR.GetChild(0).gameObject.GetComponent<Button>();

            ad_btn.gameObject.SetActive(true);


            if (Playerdata_DAO.Player_Gem() < 5)
            {
                gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_btn.interactable = false;
            }

            else
                gem_btn.interactable = true;



            timer.text = 300.ToString();
            timer.gameObject.transform.GetChild(0).GetComponent<Text>().text =
                "(+" + ((_dataManager.item_duration_const - 1000f)) + ")";

        }

        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.SCORE_AD);
        }

        public void OnClick_Gem()
        {
            // 점수 관련 상수를 변경시켜주는 함수 
            item_icon.SetAsLastSibling();
            item_icon.gameObject.SetActive(true);
            StartCoroutine(Timer());
            Playerdata_DAO.Set_Player_Gem(-5);
            gemText.text = string.Format("{0:#,0}",Playerdata_DAO.Player_Gem());
            if (Playerdata_DAO.Player_Gem() < 5)
            {
                /*
                if (gem_mommy.interactable)
                {
                    gem_mommy.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color =
                        new Color(255f / 255f, 28f / 255f, 26f / 255f);
                    gem_mommy.interactable = false;
                }
                */
                
                gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_both.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.interactable = false;
                gem_both.interactable = false;
            }
            Ingame_Log.Extra_Score(true);
            gem_btn.interactable = false;
            ad_btn.interactable = false;
        }

        private void UserEarnedReward()
        {
            item_icon.SetAsLastSibling();
            item_icon.gameObject.SetActive(true);
            gem_btn.interactable = false;
            ad_btn.interactable = false;
            StartCoroutine(Timer());
            Ingame_Log.Extra_Score(false);
        }

        public void Noads_Click()
        {
            item_icon.SetAsLastSibling();
            item_icon.gameObject.SetActive(true);
            gem_btn.interactable = false;
            ad_btn.interactable = false;
            StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            _settingManager.OnClick_Item_Exit();
            _questManager.Set_Item();
            _soundManager.item.Play();
            
            timer.gameObject.transform.GetChild(0).gameObject.SetActive(false); // 옆에 보너스 표시 지워주기 
            item_icon.SetAsLastSibling();
            item_icon.gameObject.SetActive(true);
            scoreManager.Set_Item_Const(true);
            activation.SetActive(true);
            Image panel = item_icon.GetChild(1).gameObject.GetComponent<Image>();
            float time = 300f  + (_dataManager.item_duration_const-1000f);
            float time_const = 300f + (_dataManager.item_duration_const-1000f);
            timer.text = ((int) time).ToString();
            timer_lower.text = ((int) time).ToString();
            while (true)
            {
                if (Time.timeScale != 0)
                {
                    time -= Time.deltaTime / Time.timeScale;
                    timer.text = ((int) time).ToString();
                    timer_lower.text = ((int) time).ToString();
                    panel.fillAmount = (1- time / time_const);
                    if (time < 1)
                        break;

                }
                yield return null;
            }

            yield return new WaitForSeconds(1f); 
            ad_btn.interactable = true;
            if (Playerdata_DAO.Player_Gem() > 4)
            {
                gem_btn.interactable = true;
            }
            
            timer.text = 300.ToString(); // 원래대로 초기화 
            timer.gameObject.transform.GetChild(0).gameObject.SetActive(true); // 옆에 보
            timer.text = ((int)time_const).ToString();
            timer_lower.text = ((int)time_const).ToString();
            activation.SetActive(false);
            scoreManager.Set_Item_Const(false);
            item_icon.gameObject.SetActive(false);
            yield break;

        }

        public void Set_Mediator(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
        }
    }
}
