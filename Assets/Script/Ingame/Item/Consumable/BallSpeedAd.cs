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
         public GameObject activation;
         
         private IMediator _mediator;

         /// <summary>
         /// 1. Ad 버튼 활성 유무
         /// 2. 시간 체크 
         /// </summary>
         public void Start()
         {

             if (Noads_instance.Get_ItemAds())
             {
                 ad_btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
                 gem_btn.gameObject.SetActive(false);
                 _launchManage.Set_BallSpeed_Const(true);// 볼 발사 상수 넣어줌 
                 timer.gameObject.SetActive(false);
                 // 활성화 됐다는 글씨 켜줌 
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

         private void UserEarnedReward()
         {
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             gem_btn.interactable = false;
             ad_btn.interactable = false;
             Ingame_Log.Speed_Item(false);
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
             _mediator.Event_Receive(Event_num.SPEED_AD);
         }
         

         public void OnClick_Gem()
         {
             // 점수 관련 상수를 변경시켜주는 함수 
             Ingame_Log.Speed_Item(true);
             item_icon.SetAsLastSibling();
             item_icon.gameObject.SetActive(true);
             StartCoroutine(Timer());
             Playerdata_DAO.Set_Player_Gem(-5);
             gemText.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
             if (Playerdata_DAO.Player_Gem() < 5)
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
             
             
             gem_btn.interactable = false;
             ad_btn.interactable = false;
         }

         IEnumerator Timer()
         {
             _settingManager.OnClick_Item_Exit();
             _questManager.Set_Item();
             _soundManager.item.Play(); // 사운드 
             _launchManage.Set_BallSpeed_Const(true);// 볼 발사 상수 넣어줌 
             timer.gameObject.transform.GetChild(0).gameObject.SetActive(false); // 옆에 보너스 표시 지워주기 
             float time = 300f  + (_dataManager.item_duration_const-1000f);
             float time_const = 300f + (_dataManager.item_duration_const-1000f);
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
