using System.Collections;
using System;
using System.Collections.Generic;
using Avatar;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Timer
{
    public class Charater_Package_Panel : MonoBehaviour, IPackagePanel, IPointerDownHandler
    {
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject parent_Obj;
        [SerializeField] private GameObject tapToContinue;

        void OnEnable()
        {
            Timer_Set();
            StartCoroutine(Popup_Action());
        }

        public IEnumerator Popup_Action()
        {
            yield return new WaitForSeconds(1f);
            tapToContinue.SetActive(true);
        }

        public IEnumerator Timer()
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

        public void Timer_Set()
        {
            StartCoroutine(Timer());
        }

        public void OnClick_BuyBtn()
        {
            
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            parent_Obj.gameObject.SetActive(false);
        }


    }
}
