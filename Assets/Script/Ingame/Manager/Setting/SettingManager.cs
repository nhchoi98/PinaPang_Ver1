using System;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Manager
{
    public class SettingManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject PausePanel, Info_Panel, item_panel;
        
        public GameObject notshow_btn;
        public GameObject skill_notshow;
        public Animator Item_Now_Show;
        public AudioSource not_show_sound;
        [SerializeField] private Text gemText;
        public Animator panel_animator, pauseAnimator;
        
        private bool itemAlarm_flag;
        
        public void OnClick_PauseBtn()
        {
            pauseAnimator.enabled = true;
            PausePanel.SetActive(true);
        }

        public void OnClick_Item_Nowshow()
        {
            not_show_sound.Play();
            Item_Now_Show.SetTrigger("show");
        }

        public void Onclick_ItemPanel()
        {
            gemText.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            panel_animator.enabled = true;
            item_panel.SetActive(true);
            Time.timeScale = 0f;
            // 발사 막기 
        }

        public void OnClick_Item_Exit()
        {
            item_panel.SetActive(false);
            panel_animator.enabled = false;
            Time.timeScale= 1f;
        }

        public void ItemBtn_LaunchSet(bool green)
        {
            if (green)
            {
                notshow_btn.SetActive(false);
                skill_notshow.SetActive(false);
            }
            
            else
            {
                notshow_btn.SetActive(true);
                skill_notshow.SetActive(true);
            }
        }
        
        public void OnClick_Info()
        {
            Info_Panel.SetActive(true);
        }

        

    }
}
