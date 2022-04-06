using System.Collections;
using Ad;
using Challenge;
using Data;
using Ingame;
using Log;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Item
{
    
    /// <summary>
    /// 1.5배속 아이템의 스크립트. 광고 혹은 USE버튼을 눌렀을 때의 이벤트를 관리해준다. 
    /// </summary>
    public class BallSpeedAd : MonoBehaviour, IComponent
    {
        [SerializeField] private DataManager _dataManager;
         [SerializeField] private SoundManager _soundManager;
         [SerializeField] private QuestManager _questManager;
         [SerializeField] private LaunchManage _launchManage;
         [SerializeField] private SettingManager _settingManager;

         public Button gem_btn, ad_btn, gem_line, gem_both; 
         public Transform buttonTR;
         public Text timer, timer_lower;
         public Transform item_icon;
         public Text gemText;
         public GameObject activation, permanent_Activate;
         
         private IMediator _mediator;

         private float time, time_const;
         /// <summary>
         /// 1. Ad 버튼 활성 유무
         /// 2. 시간 체크 
         /// </summary>
         public void Start()
         {

             if (Noads_instance.Get_ItemAds()) // 사용자가 아이템 패키지를 샀다면, 자동 적용 시켜주고 UI에 반영시켜줌. 
             {
                 ad_btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
                 gem_btn.gameObject.SetActive(false);
                 permanent_Activate.SetActive(true);
                 _launchManage.Set_BallSpeed_Const(true);// 볼 발사 상수 넣어줌 
                 timer.gameObject.SetActive(false);
                 // 활성화 됐다는 글씨 켜줌 
             }

             else
             {
                 ad_btn = buttonTR.GetChild(0).gameObject.GetComponent<Button>();
                 ad_btn.gameObject.SetActive(true);
                 
                 if (Playerdata_DAO.Player_Gem() < 5) // 사용자가 젬이 부족한 경우
                 {
                     gem_btn.interactable = false; // 버튼 비활성화 
                     gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                         new Color(255f / 255f, 28f / 255f, 26f / 255f);
                 }

                 else
                     gem_btn.interactable = true; 


                 timer.text = 300.ToString(); // 타이머 초기화 
                 timer.gameObject.transform.GetChild(0).GetComponent<Text>().text =
                     "(+" + ((_dataManager.item_duration_const - 1000f)) + ")";
             }
         }

         /// <summary>
         /// 사용자가 광고를 다 보면 호출되는 함수. 
         /// </summary>
         private void UserEarnedReward()
         {
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             Ingame_Log.Speed_Item(false);
             StartCoroutine(Timer());
         }

         /// <summary>
         /// No_ads 아이템을 사용하는 사용자는 use버튼을 누르면 이 함수가 호출됨. 
         /// </summary>
         public void Noads_Show()
         {
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             StartCoroutine(Timer());
         }

         /// <summary>
         /// 광고 보기 버튼을 누르면 호출되는 함수. 광고를 화면에 띄워준다.
         /// </summary>
         public void UserChoseToWatchAd()
         {
             _mediator.Event_Receive(Event_num.SPEED_AD);
         }
         
        /// <summary>
        /// 사용자가 젬을 이용해 아이템을 쓰기로 하면 호출되는 함수 
        /// </summary>
         public void OnClick_Gem()
         {
             // 점수 관련 상수를 변경시켜주는 함수 
             Ingame_Log.Speed_Item(true);
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             StartCoroutine(Timer());
             Playerdata_DAO.Set_Player_Gem(-5);
             gemText.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
             if (Playerdata_DAO.Player_Gem() < 5) // 젬이 5젬 미만일 경우, 다른 아이템의 버튼을도 못누르게 막음. 
             {
                 gem_btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);
                 gem_line.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);

                 gem_both.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color =
                     new Color(255f / 255f, 28f / 255f, 26f / 255f);

                 gem_line.interactable = false;
                 gem_both.interactable = false;
             }
             
             
             gem_btn.interactable = false; // 아이템이 사용중이므로, 중복으로 누르지 못하도록 GEM 버튼과 AD 버튼을 못누르게 막음. 
             ad_btn.interactable = false;
         }

         IEnumerator Timer(bool load_Time = false, float Ltime = 300f)
         {
             if (!load_Time)
             {
                 _settingManager.OnClick_Item_Exit();
                 _questManager.Set_Item();
                 _soundManager.item.Play(); // 사운드 
             }

             _launchManage.Set_BallSpeed_Const(true);// 볼 발사 상수 넣어줌 
             timer.gameObject.transform.GetChild(0).gameObject.SetActive(false); // 옆에 보너스 표시 지워주기 
             time_const = 300f + (_dataManager.item_duration_const - 1000f);
             if (!load_Time)
                 time = 300f + (_dataManager.item_duration_const - 1000f);

             else // 시간을 로드하는 경우 
                 time = Ltime;
             
             Image panel = item_icon.GetChild(1).gameObject.GetComponent<Image>();
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             activation.SetActive(true);
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

             _launchManage.Set_BallSpeed_Const(false);// 상수 풀어줌 
             activation.SetActive(false);
             timer.text = 300.ToString(); // 원래대로 초기화 
             timer.gameObject.transform.GetChild(0).gameObject.SetActive(true); // 옆에 보
             timer.text = ((int) time_const).ToString();
             timer_lower.text = ((int) time_const).ToString();
             item_icon.gameObject.SetActive(false);
             yield break;

         }
         #region Load&Save Data
         public void Save_Data(ref float time, ref bool isActive)
         {
             time =  this.time;
             if (this.time == 0)
                 isActive = false;

             else
                 isActive = true;
         }
        
         #endregion
         
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
