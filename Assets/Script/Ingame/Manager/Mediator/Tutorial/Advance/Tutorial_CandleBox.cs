using System;
using System.Collections;
using System.Collections.Generic;
using Block;
using Box;
using Ingame;
using log;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tutorial
{
    public class Tutorial_CandleBox : MonoBehaviour ,IPointerDownHandler
    {
        [SerializeField] private Button pauseBtn, itemBtn;

        [SerializeField] private GameManage _gameManage;

        [SerializeField] private Transform boxGroup;
        private Transform targetBox; // 레이어를 띄울 박스 지정 
        private bool is_typing = false, canGoNext = false;
        private bool jump_text = false;
        

        [Header("Text")]
        [SerializeField] private GameObject triangle;

        public GameObject parentObj;
        public void Awake()
        {
            pauseBtn.interactable = false;
            itemBtn.interactable = false;
            _gameManage.Event_Receive(Event_num.Launch_Red);
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                if (boxGroup.GetChild(i).gameObject.CompareTag("Box"))
                {
                    if (boxGroup.GetChild(i).gameObject.GetComponent<Normal_Box>().Get_Has_Candle())
                        targetBox = boxGroup.GetChild(i);
                }
            }
            targetBox.gameObject.GetComponent<Boxselect>().Activate_Grid();
            Tutorial_Log.Candle_Tutorial_Begin();
            StartCoroutine(Wait_Until_ScaleUp(targetBox));
            // # 1. 시간을 멈춤
            // # 2. 버튼들 안눌리게 만듬 (Pause, Item)
            // # 3. 발사 안되게 만듬
            // # 4. 캔들 박스 찾음 
            // # 5. 캔들 박스의 레이어를 띄워줌 
        }

        IEnumerator Wait_Until_ScaleUp(Transform TR)
        {
            Vector3 targetScale = new Vector3(1f, 1f,0f);
            while (true)
            {
                if ((targetScale.x)-TR.localScale.x<0.01)
                    break;

                yield return null;
            }
            Time.timeScale = 0f;
            StartCoroutine(Type_Desc());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (canGoNext)
            {
                Time.timeScale = 1f;
                _gameManage.Event_Receive(Event_num.Launch_Green);
                PlayerPrefs.SetInt("Tutorial_Advance_Candle_Show", 1);
                PlayerPrefs.SetInt("Tutorial_Advance_Candle_Respawn", 0);
                pauseBtn.interactable = true;
                itemBtn.interactable = true;
                parentObj.SetActive(false);
            }
        }

        IEnumerator Type_Desc()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            triangle.gameObject.SetActive(true);
            canGoNext = true;
        }
    }
}
