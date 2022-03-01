
using System;
using System.Collections;
using Battery;
using Data;
using Log;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class PlayerBtn_Set : MonoBehaviour
    {
        private BatteryDAO _batteryDao;
        public Button playBtn;
        public GameObject chargeBtn;
        
        [Header("Charge_Btn")] 
        public Button gemCharge;
        public Button adCharge;

        [Header("Charge_Panel")] 
        public GameObject chargePanel;

        [Header("Charge_Info")] 
        public Text txt_charge_info;
        public Slider charge_slider;
        public Text timer;

        [Header("Charge_UI")] 
        public Slider panel_charge_slider;
        public Text timer_panel;
        public Text txt_charge_info_panel;
        
        private int heartnum;
        public GameObject heartObj;
        public AudioSource chargeSound;

        public Text mainGem;
        private void Awake()
        {
            _batteryDao = new BatteryDAO();
            Set_Charge_Info();
            // ad_charge는 instance 할 때 부터 판단 
            if (_batteryDao.Get_Ad_Count() < 1)
            {
                adCharge.interactable = false;
                adCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =  "<color='red'>" + _batteryDao.Get_Ad_Count().ToString() + "/5</color>";
            }

            else
            {
                adCharge.interactable = true;
                adCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =   _batteryDao.Get_Ad_Count().ToString() + "/5";
            }

            // 차지 이뉴머레이터 시작시키기 
        }

        private void Determine_Btn()
        {
            if (_batteryDao.Get_GemCount() < 1)
            {
                gemCharge.interactable = false;
                gemCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "<color='red'>" + _batteryDao.Get_GemCount().ToString() + "/10</color>";
            }
            
            else if (Playerdata_DAO.Player_Gem() < 30)
            {
                gemCharge.interactable = false;
                gemCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = _batteryDao.Get_GemCount().ToString() + "/10";
            }

            else
            {
                gemCharge.interactable = true;
                gemCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =  _batteryDao.Get_GemCount().ToString() + "/10";
            }
            
            if (_batteryDao.Is_Playable())
            {
                playBtn.interactable = true;
                chargeBtn.SetActive(false);
            }

            else
            {
                playBtn.interactable = false;
                chargeBtn.SetActive(true);
            }
        }

        private void OnEnable()
        {
            Determine_Btn();

        }
        // 여기서 부터는 상단 UI 관리 

        public void OnClick_GemCharge()
        {
            // # 1. 번개 날아가는 애니메이션 
            // # 2. 배터리 충전 
            Ingame_Log.Heart_Charge(true);
            _batteryDao.Gem_Charge();
            StartCoroutine(Get_Heart(30, false));
            if (_batteryDao.Get_GemCount() < 1)
            {
                gemCharge.interactable = false;
                gemCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "<color='red'>" + _batteryDao.Get_GemCount().ToString() + "/10</color>";
            }
            
            else if(Playerdata_DAO.Player_Gem()<30)
                gemCharge.interactable = false;

            else
            {
                gemCharge.interactable = true;
                gemCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =  _batteryDao.Get_GemCount().ToString() + "/10";
            }

            mainGem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            Set_Charge_Info();
        }

        public void Set_Ad_Btn()
        {
            StartCoroutine(Get_Heart(10, true));
            if (_batteryDao.Get_Ad_Count() < 1)
            {
                adCharge.interactable = false;
                adCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =  "<color='red'>" + _batteryDao.Get_Ad_Count().ToString() + "/5</color>";
            }

            else
            {
                adCharge.interactable = true;
                adCharge.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text =   _batteryDao.Get_Ad_Count().ToString() + "/5";
            }
        }

        public void Ad_Btn_Clicked()
        {
            // 배터리 애니메이션 
            Ingame_Log.Heart_Charge(false);
            _batteryDao.Ad_Charge();
            Set_Charge_Info();
        }

        public void OnClick_PanelOpen()
        {
            Determine_Btn();
            chargePanel.SetActive(true);
            heartnum = _batteryDao.Get_Count();
            txt_charge_info_panel.text = _batteryDao.Get_Count().ToString() + "/30";
            panel_charge_slider.value = (float) _batteryDao.Get_Count() / 30f;
            if(_batteryDao.Get_Count()>29)
                timer_panel.gameObject.SetActive(false);
            
            else
                timer_panel.gameObject.SetActive(true);
                
        }

        public void Exit_Panel()
        {
            chargePanel.SetActive(false);
        }

        public void Set_Charge_Info()
        {
            txt_charge_info.text = _batteryDao.Get_Count().ToString() + "/30";
            charge_slider.value = (float) _batteryDao.Get_Count() / 30f;

            if (_batteryDao.Get_Count() > 29)
            {
                timer.gameObject.SetActive(false);
                timer_panel.gameObject.SetActive(false);
                chargeBtn.SetActive(false);
                playBtn.interactable = true;
            }

            if (_batteryDao.Get_Count() > 7)
            {
                chargeBtn.SetActive(false);
                playBtn.interactable = true;
            }
            
            else
            {
                chargeBtn.SetActive(true);
                playBtn.interactable = false;
            }
        }

        public void Set_Lobby_Charge()
        {
            _batteryDao.Lobby_Charge();
            Set_Charge_Info();
        }

        public TimeSpan Get_Target_Time()
        {
            return _batteryDao.Get_TargetTime();
        }

        public int Get_Count()
        {
            return _batteryDao.Get_Count();
        }
        
        public void OnClick_Play()
        {
            _batteryDao.Ingame_Start();
        }

        #region Heard_Animation

        IEnumerator Get_Heart(int quantity, bool is_ad)
        {
            Vector2 startPos;
            HeartFlying script;
            if (is_ad)
                startPos = new Vector2(168f, 52f);

            else
                startPos = new Vector2(-177f, 52f);

            chargeSound.Play();
            for (int i = 0; i < quantity; i++)
            {
                GameObject obj = Instantiate(heartObj, startPos,Quaternion.identity); // 잼 획득 연출 넣기 
                script = obj.GetComponent<HeartFlying>();
                script.start_pos = startPos;
                script.heart_text = this.txt_charge_info_panel;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(155f, 857f);
            }

            yield return null;
        }
        
        private void set_text(object sender, EventArgs e)
        {
            heartnum += 1;
            txt_charge_info_panel.text = string.Format("{0:#,0}", heartnum)+ "/30";
            panel_charge_slider.value = (float) heartnum / 30f;
        }
        #endregion
    }
}
