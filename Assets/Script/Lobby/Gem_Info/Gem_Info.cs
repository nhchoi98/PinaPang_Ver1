using System.Collections;
using System.Collections.Generic;
using Attendance;
using Exchange;
using shop;
using UnityEngine;

namespace Lobby
{
    public class Gem_Info : MonoBehaviour
    {
        [SerializeField] private Exchange_Panel _exchangeOn;
        [SerializeField] private GameObject exchangePanel;
        [SerializeField] private Lobby_Main collectionPanel_On;
        [SerializeField] private GameObject collectionPanel;
        [SerializeField] private Attendance_UI attendencePanel;
        [SerializeField] private GameObject skinPanel;
        [SerializeField] private Shop_Main _shopMain;
        public GameObject questPanel;
        
        public void OnClick_Exit()
        {
            this.gameObject.SetActive(false);
        }

        public void Click_Shop()
        {
            exchangePanel.SetActive(false);
            collectionPanel.SetActive(false);
            attendencePanel.onClick_Exit();
            _shopMain.OnClick_PanelOn();
            skinPanel.SetActive(false);
            questPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }


        public void Click_Exchange()
        {
            _exchangeOn.Onclick_PanelOn();
            collectionPanel.SetActive(false);
            attendencePanel.onClick_Exit();
            skinPanel.SetActive(false);
            _shopMain.OnClick_Exit();
            questPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }
        
        public void Click_Collection()
        {
            collectionPanel_On.OnClick_Collection();
            exchangePanel.SetActive(false);
            attendencePanel.onClick_Exit();
            skinPanel.SetActive(false);
            _shopMain.OnClick_Exit();
            questPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }
        
        public void Click_Quest()
        {
            collectionPanel.SetActive(false);
            exchangePanel.SetActive(false);
            attendencePanel.onClick_Exit();
            skinPanel.SetActive(false);
            _shopMain.OnClick_Exit();
            questPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
        
    }
}