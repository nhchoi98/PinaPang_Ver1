
using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Challenge;
using Ingame;
using Item;
using Log;

namespace Ad
{
    public class Both_Ad : MonoBehaviour, IComponent
    {
        
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private SettingManager _settingManager;
        [SerializeField] private GameManage _manage;
        [SerializeField] private QuestManager _questManager;
        public Button gem_btn, ad_btn, gem_line, gem_speed; 
        public Text timer, timer_lower;
        public Transform item_icon;

        public Text gemText;
        public GameObject activation;
        private IMediator _mediator;
        public Transform buttonTR;

        public GameObject crossObj;
        public Transform itemTR;
        public Transform diePool;
        [SerializeField] private LocateBox _locateBox;

        void Start()
        {
            if (Noads_instance.Get_ItemAds())
            {
                ad_btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
                gem_btn.gameObject.SetActive(false);
                // 영구 활성화 글씨 띄워줌
                // 기능 활성화 
                Change_Item_To_CrossRazer();
                _manage.Event_Receive(Event_num.CROSS_ITEM);
                timer.gameObject.SetActive(false);
            }

            else
            {
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

        }

        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.CROSS_AD);
        }

        public void OnClick_Gem()
        {
            // 점수 관련 상수를 변경시켜주는 함수 
            Ingame_Log.Cross_Razer(true);
            StartCoroutine(Timer());
            Playerdata_DAO.Set_Player_Gem(-5);
            gemText.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            if (Playerdata_DAO.Player_Gem() < 5)
            {

                gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_speed.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.interactable = false;
                gem_speed.interactable = false;
            }
            gem_btn.interactable = false;
            ad_btn.interactable = false;
        }

        private void HandleUserEarnedReward()
        {
            Ingame_Log.Cross_Razer(false);
            gem_btn.interactable = false;
            ad_btn.interactable = false;
            StartCoroutine(Timer());
        }

        public void OnClick_NOADS()
        {
            gem_btn.interactable = false;
            ad_btn.interactable = false;
            StartCoroutine(Timer());
        }

        private void Change_Item_To_CrossRazer()
        {
            Transform tr;
            List<Transform> fieldItem = new List<Transform>();
            List<Vector2> position = new List<Vector2>();

            var targetNum = itemTR.childCount;
            for (int i = 0; i < targetNum; i++)
            {
                if (itemTR.GetChild(0).gameObject.CompareTag("random_item")) 
                    continue;
                
                position.Add(itemTR.GetChild(0).position);
                tr = itemTR.GetChild(0);
                tr.SetParent(diePool);
                tr.gameObject.SetActive(false);
            }
            for (int i = 0; i < position.Count; i++)
            {
                tr = Instantiate(crossObj, position[i], Quaternion.identity).transform;
                tr.GetComponent<Raw_Item>().locateBox = _locateBox;
                tr.GetComponent<Col_Item>().locateBox = _locateBox;
                fieldItem.Add(tr);
            }

            for (int i = 0; i < fieldItem.Count; i++)
            {
                fieldItem[i].SetParent(itemTR);
            }
        }

        IEnumerator Timer()
        {
            // Step 1. 필드상의 아이템 전부 바꾸어줌 . 
            Change_Item_To_CrossRazer();
            // Step 2. 기타 설정을 해줌 
            _settingManager.OnClick_Item_Exit();
            _mediator.Event_Receive(Event_num.SET_ITEM);
            _questManager.Set_Item();
            _soundManager.item.Play(); // 사운드 
            timer.gameObject.transform.GetChild(0).gameObject.SetActive(false); // 옆에 보너스 표시 지워주기 
            _manage.Event_Receive(Event_num.CROSS_ITEM);
            Image panel = item_icon.GetChild(1).gameObject.GetComponent<Image>();
            item_icon.SetAsLastSibling();
            item_icon.gameObject.SetActive(true);
            activation.SetActive(true);
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
                    panel.fillAmount = (1 - time / time_const);
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
            
            activation.SetActive(false);
            timer.text = 300.ToString(); // 원래대로 초기화 
            timer.gameObject.transform.GetChild(0).gameObject.SetActive(true); // 옆에 보너스 표시 지워주기
            timer_lower.text = ((int) time_const).ToString();
            item_icon.gameObject.SetActive(false);
            _manage.Event_Receive(Event_num.CROSS_ITEM);
            yield break;

        }


        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            HandleUserEarnedReward();
        }
    }
}
