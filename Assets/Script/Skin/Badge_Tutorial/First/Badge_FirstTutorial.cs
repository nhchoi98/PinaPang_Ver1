
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using log;
using Skin;

namespace Tutorial
{
    public class Badge_FirstTutorial : MonoBehaviour, IPointerDownHandler
    {
        
        public AudioSource type_sound;

        private int stage = 0;
        
        [Header("Text_Control")]
        private bool type_touch_pos;
        private bool is_typing;
        private bool jump_text = false;
        [SerializeField] private Text bubbleText;
        public GameObject triangle;

        [SerializeField] private GameObject snackBtn;

        [Header("Snack")] 
        public GameObject snackPanel;
        public GameObject snackTutorial;

        [Header("Charater")] [SerializeField] private Transform charater;
        public Button badgeBtn;
        public GameObject parent;
        public GameObject hand;

        [SerializeField] private Main_Skin _mainSkin;
        
        private void OnEnable()
        {
            type_touch_pos = false;
            is_typing = false;
            StartCoroutine(OnType(0.025f, Get_Type_Data(stage++)));
            charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
            charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
            charater.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
            Tutorial_Log.Skin_Tutorial_Start();
        }

        IEnumerator OnType(float interval, string Say)
        {
            int typecount = 0;
            bool skip = false;
            bubbleText.text = null; // text 초기화 
            Event_Occur(ref skip);
            is_typing = true;
            if (skip)
                yield break;
            
            for (int i = 0; i < Say.Length; i++) 
            { 
                bubbleText.text = Say.Substring(0, i + 1); 
                yield return new WaitForSecondsRealtime(interval); 
                if (jump_text)
                {
                    bubbleText.text = Say.ToString();
                    break;
                }

                if (typecount == 1)
                {
                    typecount = 0;
                    type_sound.Play();
                }

                else
                    typecount++;
            }
            jump_text = false;
            is_typing = false;
            yield return new WaitForSeconds(0.2f);
            if(stage !=2)
                triangle.gameObject.SetActive(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (is_typing)
                jump_text = true;
            
            if (type_touch_pos)
            {
                bubbleText.text = null; // text 초기화 
                triangle.gameObject.SetActive(false);
                type_touch_pos = false;
                StartCoroutine(OnType(0.025f, Get_Type_Data(stage++)));
                
            }
            
        }


        #region Text_Data

        private string Get_Type_Data(int stage)
        {
            switch (stage)
            {
                default:
                    break;
                
                case 0:
                    return "I'm hungry...";
                
                case 1:
                    return "CAN YOU CHECK IF THERE IS ANY SNACKS?";
            }

            return null;
        }

        #endregion
        
        private void Event_Occur(ref bool return_value)
        {
            switch (stage)
            {
                case 1:
                    type_touch_pos = true;
                    break;
                
                case 2:
                    hand.SetActive(true);
                    snackBtn.SetActive(true);
                    type_touch_pos = false;
                    break;
                
                
            }
        }

        #region Button_Action

        public void OnClick_SnackBtn()
        {
            charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
            charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
            charater.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
            snackTutorial.SetActive(true); // 튜토리얼 창 활성화 시켜줌 
            snackPanel.SetActive(true);
            _mainSkin.Set_Tutorial_Reset(); // 다시 버튼들 되돌려놓음 
            parent.SetActive(false);
            
        }
        

        #endregion

  
    }
}
