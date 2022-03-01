using System;
using Ad;
using Avatar;
using Data;
using Shop;
using Skin;
using Timer;
using UnityEngine;
using UnityEngine.UI;
namespace shop
{
    public class Shop_Main : MonoBehaviour
    {

        [Header("MAIN_COMMODITY")] 
        [SerializeField] private Text mainGem;
        [SerializeField] private Text mainCandy;
        [SerializeField] private Text skinGem;
        [SerializeField] private Text shopGem;
        
        [Header("Panel")] 
        [SerializeField] private GameObject shopPanel;
        
        public GameObject goods_Panel;
        public GameObject package_Panel;
        public Scrollbar package_scroll;

        [Header("Button")] 
        public Button gemBtn;
        public Button packageBtn;

        [Header("Package")]
        public Transform tr;
        public GameObject empty_panel;

        public SkillInfo_Set SkillInfoSet;
        public GameObject Skillpanel;
        
        public AudioSource click;

        [SerializeField] private StarterTimer _timer;
        [SerializeField] private CharaterPack_Timer _package_timer;
        [SerializeField] private Gem_Goods _gemGoods;
        public AudioSource mainSound, shopSound;

        [Header("Restore_Btn")] public GameObject restoreBtn;
        
        private void Awake()
        {
            #if UNITY_IOS
                restoreBtn.SetActive(true);
            #endif
        }

        public void OnClick_PanelOn()
        {
            shopGem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            _gemGoods.Set_Gem();
            if (PlayerPrefs.GetInt("First_Shop", 0) == 0)
            {
                Determine_StarterDAO starterDao = new Determine_StarterDAO();
                
                if(starterDao.Get_is_first())
                    starterDao.Set_is_first();
                
                _timer.First_Start(false);
                PlayerPrefs.SetInt("First_Shop",1);

            }

            mainSound.Stop();
            shopSound.Play();
            shopPanel.SetActive(true);
        }

        public void OnClick_Exit()
        {
            mainSound.Play();
            shopSound.Stop();
            click.Play();
            mainGem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            mainCandy.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            skinGem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            shopPanel.gameObject.SetActive(false);
        }

        public void OnClick_SkillView(int index)
        {
            SkillInfoSet.Set_Panel(index);
            Skillpanel.gameObject.SetActive(true);
        }

        public void Click_Play()
        {
            click.Play();
        }

        /// <summary>
        /// 상품을 구매하거나, 초기화 시 어떤 영역을 남기고 안남길지를 결정해주는 Region
        /// </summary>
        
        #region Button_Action
        public void OnClick_Goods()
        {
            if (!goods_Panel.activeSelf)
            {
                gemBtn.interactable = false;
                packageBtn.interactable = true;
                click.Play();
                goods_Panel.SetActive(true);
                package_Panel.SetActive(false);
            }
            
        }
        public void OnClick_Package()
        {
            if (!package_Panel.activeSelf)
            {
                gemBtn.interactable = true;
                packageBtn.interactable = false;
                click.Play();
                package_Panel.SetActive(true);
                goods_Panel.SetActive(false);
            }
        }
        public void Set_Package_value()
        {
            package_scroll.value = 0.116f;
        }
        public void OnClick_Starter()
        {
            package_scroll.value = 0.649f;
        }
        public void OnClick_Shop_Package()
        {
            gemBtn.interactable = true;
            packageBtn.interactable = false;
            package_Panel.SetActive(true);
            goods_Panel.SetActive(false);
            shopPanel.SetActive(true);
        }
        #endregion
    }
}