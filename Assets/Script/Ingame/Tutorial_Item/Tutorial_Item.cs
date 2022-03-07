using System;
using System.Collections;
using Ad;
using Item;
using log;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class Tutorial_Item : MonoBehaviour
    {
        public GameObject mainItemBtn;
        [Header("First_Mention")] 
        public GameObject firstMention;
        public Button tutorial_button;
        public GameObject itemPanel;

        public GameObject crossButton, speedButton;

        [SerializeField] private Line_Ad _lineAd;
        
        
        private void Start()
        {
            mainItemBtn.SetActive(true);
            tutorial_button.interactable = false;
            crossButton.SetActive(false);
            speedButton.SetActive(false);
            Tutorial_Log.Item_Tutorial_Start();
            StartCoroutine(First_Mention());
        }

        IEnumerator First_Mention()
        {
            yield return new WaitForSeconds(1f);
            tutorial_button.interactable = true;
            firstMention.SetActive(true);
        }

        public void OnClick_itemBtn()
        {
            itemPanel.SetActive(true);
            _lineAd.Set_Tutorial();
            Tutorial_Log.Item_Tutorial_End();
            this.gameObject.SetActive(false);
        }
    }
}
