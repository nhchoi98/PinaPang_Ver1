using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial
{
    public class Tutorial_Exchange : MonoBehaviour, IPointerDownHandler
    {
        public GameObject exchangeTutorial;
        public Transform candyInfo;
        public GameObject exchange_outline;
        public GameObject exchange_btn;
        public void OnPointerDown(PointerEventData eventData)
        {
            exchangeTutorial.SetActive(true);
            candyInfo.gameObject.GetComponent<Canvas>().overrideSorting = true;
            exchange_outline.SetActive(true);
            candyInfo.gameObject.GetComponent<Canvas>().sortingLayerName = "Panel_2";
            exchange_btn.gameObject.GetComponent<Canvas>().sortingLayerName = "Panel_2";
            this.gameObject.SetActive(false);
        }
    }
}