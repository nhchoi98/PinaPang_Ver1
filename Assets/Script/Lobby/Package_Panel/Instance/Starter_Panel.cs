using System.Collections;
using System;
using Timer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Shop
{
    public class Starter_Panel : MonoBehaviour, IPackagePanel, IPointerDownHandler
    {
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject parent_Obj;
        [SerializeField] private GameObject tapToContinue;
        // Start is called before the first frame update
        void Start()
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
            Determine_StarterDAO _starterDao = new Determine_StarterDAO();
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
            parent_Obj.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {

            parent_Obj.SetActive(false);
        }
    }
}
