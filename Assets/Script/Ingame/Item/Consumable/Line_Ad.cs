 using Manager;
 using Data;
using UnityEngine;
using System.Collections;
 using Ad;
 using Challenge;
 using Ingame;
 using Log;
 using UnityEngine.UI;

 namespace Item
 {
     public class Line_Ad : MonoBehaviour, IComponent
     {
         [SerializeField] private DataManager _dataManager;
         [SerializeField] private SoundManager _soundManager;
         [SerializeField] private QuestManager _questManager;
         [SerializeField] private LaunchManage _launchManage;
         [SerializeField] private SettingManager _settingManager;
         [SerializeField] private Line_Animation _lineAnimation;
         
         public Button gem_btn, ad_btn, gem_speed, gem_both; 
         public Transform buttonTR;
         public Text timer, timer_lower;
         public Transform item_icon;
         public Text gemText;
         public GameObject activation, permanent_Activate;
         
         private IMediator _mediator;
         
         [Header("Tutorial")] 
         public GameObject tutorial_part;
         public GameObject exitBtn;
         public Button pauseBtn;
         public GameObject crossButton,speedButton;
         /// <summary>
         /// 1. Ad 버튼 활성 유무
         /// 2. 시간 체크 
         /// </summary>
         public void Start()
         {
             
             if (Noads_instance.Get_ItemAds())
             {
                 timer.gameObject.SetActive(false); // 타이머 꺼줌
                 permanent_Activate.SetActive(true);// 영구 활성화 글씨 띄워줌 
                 // 기능 영구 활성화 
                 _lineAnimation.Set_SecondLine(true);
                 _launchManage.Set_Item();
                 gem_btn.gameObject.SetActive(false);
             }

             else
             {
                 ad_btn = buttonTR.GetChild(0).gameObject.GetComponent<Button>();
                 ad_btn.gameObject.SetActive(true);
                 
                 if (Playerdata_DAO.Player_Gem() < 5)
                 {
                     gem_btn.interactable = false;
                     gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                         new Color(255f / 255f, 28f / 255f, 26f / 255f);
                 }

                 else
                     gem_btn.interactable = true;
             
                 
                 timer.text = 300.ToString();
                 timer.gameObject.transform.GetChild(0).GetComponent<Text>().text =
                     "(+" + ((_dataManager.item_duration_const - 1000f)) + ")";
             }

            
         }

         public void Set_Tutorial()
         {
             ad_btn.gameObject.SetActive(false);
             gem_btn.gameObject.SetActive(false);
             exitBtn.SetActive(false);
             tutorial_part.SetActive(true);
         }
         
         public void OnClick_Tutorial_Btn()
         {
             if (PlayerPrefs.GetInt("Tutorial_Item_End", 0) == 0)
             {
                 PlayerPrefs.SetInt("Tutorial_Item_End", 1);
                 crossButton.SetActive(true);
                 speedButton.SetActive(true);
             }

             tutorial_part.SetActive(false);
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             pauseBtn.interactable = true;
             exitBtn.SetActive(true);
             if (Noads_instance.Get_Is_Noads())
             {
                 ad_btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
                 gem_btn.gameObject.SetActive(false);
             }

             else
             {
                 ad_btn = buttonTR.GetChild(0).gameObject.GetComponent<Button>();
                 gem_btn.gameObject.SetActive(true);
             }

             ad_btn.gameObject.SetActive(true);
             ad_btn.interactable = false;
             
             if (Playerdata_DAO.Player_Gem() < 5)
             {
                 gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);
             }

             gem_btn.interactable = false;
             StartCoroutine(Timer());
             
         }
         
         
         
         private void UserEarnedReward()
         {
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             Ingame_Log.Line_Item(false);
             StartCoroutine(Timer());
         }

         public void Noads_Show()
         {
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             StartCoroutine(Timer());
         }

         public void UserChoseToWatchAd()
         {
             _mediator.Event_Receive(Event_num.LINE_AD);
         }

         public void Set_BallSpeed_Const(bool is_Activating)
         {
             
             
         }

         public void OnClick_Gem()
         {
             // 점수 관련 상수를 변경시켜주는 함수 
             Ingame_Log.Line_Item(true);
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             StartCoroutine(Timer());
             Playerdata_DAO.Set_Player_Gem(-5);
             gemText.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
             if (Playerdata_DAO.Player_Gem() < 5)
             {
                 gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);
                 gem_speed.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);

                 gem_both.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);

                 gem_speed.interactable = false;
                 gem_both.interactable = false;
             }
             
             
             gem_btn.interactable = false;
             ad_btn.interactable = false;
         }

         IEnumerator Timer()
         {
             _lineAnimation.Set_SecondLine(true);
             _settingManager.OnClick_Item_Exit();
             _questManager.Set_Item();
             _soundManager.item.Play(); // 사운드 
             timer.gameObject.transform.GetChild(0).gameObject.SetActive(false); // 옆에 보너스 표시 지워주기 
             float time = 300f  + (_dataManager.item_duration_const-1000f);
             float time_const = 300f + (_dataManager.item_duration_const-1000f);
             Image panel = item_icon.GetChild(1).gameObject.GetComponent<Image>();
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             activation.SetActive(true);
             timer.text = ((int) time).ToString();
             timer_lower.text = ((int) time).ToString();
             _launchManage.Set_Item();
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
             _launchManage.Set_Item();
             ad_btn.interactable = true;
             if (Playerdata_DAO.Player_Gem() > 4)
             {
                 gem_btn.interactable = true;

             }
             _lineAnimation.Set_SecondLine(false);
             activation.SetActive(false);
             timer.text = 300.ToString(); // 원래대로 초기화 
             timer.gameObject.transform.GetChild(0).gameObject.SetActive(true); // 옆에 보
             timer.text = ((int) time_const).ToString();
             timer_lower.text = ((int) time_const).ToString();
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
    

