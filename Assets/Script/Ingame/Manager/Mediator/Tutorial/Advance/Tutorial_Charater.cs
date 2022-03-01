using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using log;

namespace Tutorial
{
    /// <summary>
    /// 글자가 날아갈 때 메시지를 띄워주는 함수 
    /// </summary>
    public class Tutorial_Charater : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Button pauseBtn, itemBtn;
        [SerializeField] private Text descText;
        private Transform targetBox; // 레이어를 띄울 박스 지정 
        private bool canGoNext = false;
        private bool jump_text = false;
        private float target_timescale;
        
        [Header("Text")]
        [SerializeField] private GameObject triangle;

        [Header("Charater_Transform")] 
        [SerializeField]
        private Transform charaterTR;
        public GameObject parentObj;
        public void Awake()
        {
            Time.timeScale = 0f;
            pauseBtn.interactable = false;
            itemBtn.interactable = false;
            StartCoroutine(Type_Desc());
            charaterTR.gameObject.GetComponent<Canvas>().sortingLayerID = 4;
            // # 1. 시간을 멈춤
            // 캐릭터 글자를 띄워줌 

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (canGoNext)
            {
                triangle.gameObject.SetActive(false);
                StartCoroutine(Type_Desc2());
            }

            if (jump_text)
            {
                pauseBtn.interactable = true;
                itemBtn.interactable = true;
                charaterTR.gameObject.GetComponent<Canvas>().sortingLayerID = 0;
                Time.timeScale = 1f;
                PlayerPrefs.SetInt("Tutorial_Advance_Candle_Charater", 1); // 문자 튜토리얼 종료 
                Tutorial_Log.Candle_Tutorial_End();
                parentObj.gameObject.SetActive(false);
            }
        }

        IEnumerator Type_Desc()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            triangle.gameObject.SetActive(true);
            canGoNext = true;
        }

        IEnumerator Type_Desc2()
        {
            descText.text = "LET'S COLLECT ALL THE  <color=#DCA90A>CANDLES</color>!";
            yield return new WaitForSecondsRealtime(0.5f);
            triangle.gameObject.SetActive(true);
            jump_text  = true;
        }
    }
}
