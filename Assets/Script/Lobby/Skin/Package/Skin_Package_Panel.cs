using System;
using System.Collections;
using System.Collections.Generic;
using Timer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skin
{
    public class Skin_Package_Panel : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject tapToContinue;
        public GameObject parentObj;
        
        private void OnEnable()
        {
            if(timerText!=null)
                Timer_Set();
            
            StartCoroutine(Popup_Action());
        }
        
        private IEnumerator Popup_Action()
        {
            yield return new WaitForSeconds(1f);
            tapToContinue.SetActive(true);
        }
        
        private void Timer_Set()
        {
            StartCoroutine(Timer());
        }
        
        private IEnumerator Timer()
        {
            CharaterPack_DAO _starterDao = new CharaterPack_DAO();
            DateTime targetTime = _starterDao.Get_TargetTime();
            while (true)
            {
                var delta_target = targetTime.Subtract(DateTime.UtcNow);
                timerText.text = (int) delta_target.TotalHours + delta_target.ToString(@"\:mm\:ss");
                if (delta_target < TimeSpan.FromSeconds(1))
                    break;

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }
        
        public void OnClick_BuyBtn()
        {
            // 바로 결제창 띄우기 
            tapToContinue.SetActive(false);
            parentObj.SetActive(false);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            tapToContinue.SetActive(false);
            parentObj.SetActive(false);
        }
    }
}
