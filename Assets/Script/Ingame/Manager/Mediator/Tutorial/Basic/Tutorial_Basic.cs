using System;
using System.Collections;
using System.Collections.Generic;
using Collider;
using Ingame;
using log;
using Log;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Tutorial
{
    public enum Tutorial_enum
    {
        START_BASIC,
        SKIP,
        TEXT_TOUCH, // 이거 발생하면 터치시 자동으로 텍스트 박스가 완성됨
        LAUNCH_TUTORIAL,
        LAUNCH_TUTORIAL_DONE,
        METHOD_TUTORIAL,
        METHOD_TUTORIAL_DONE,
        ALL_TUTORIAL_DONE
    }
    
    public class Tutorial_Basic : MonoBehaviour, IPointerDownHandler
    {
        private Tutorial_Basic_Type type_data;
        private IMediator _mediator;

        public Ground ground;
        public Transform charater;

        public AudioSource type_sound;
        
        [Header("Text_Control")]
        private bool type_touch_pos;
        private bool is_typing;
        private bool jump_text = false;
        
        
        [Header("Tutorial_")]
        private int stage = 1;
        
        [Header("UI_textBox")] 
        public GameObject textBox;
        public Text bubble_Text;
        public GameObject triangle;
        public GameObject background;
        public Button pauseBtn,  skipBtn;
        public GameObject launch_Tutorial;
        public GameObject launch_Panel;
        public GameObject parent;
        public GameObject tapBtn;
        [Header("Skip_Panel")] public GameObject skipPanel;


        #region Skip_Panel
        public void OnClick_Skip()
        {
            skipPanel.SetActive(true);
        }

        public void OnClick_NoSkip()
        {
            skipPanel.SetActive(false);
        }
        #endregion

        
        private void Awake()
        {
            pauseBtn.gameObject.SetActive(false);
            skipBtn.gameObject.SetActive(false);
            charater.position = new Vector3(-626f, -655f, 0f);
            type_data = new Tutorial_Basic_Type();
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            Tutorial_Log.Basic_Tutorial_Start();
            // 처음 튜토리얼에 들어오면, Panel 보다 위에 위치해야 하므로.
            charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel";
            charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel";
        }

        private void OnEnable()
        {
            type_touch_pos = false;
            is_typing = false;
            StartCoroutine(_Charater_Move(Vector2.zero));
        }

        public void Set_Skip()
        {
            pauseBtn.gameObject.SetActive(true);
            _mediator.Event_Receive(Event_num.Tutorial_Basic_Done);
            Tutorial_Log.Tutorial_Skipped();
            ground.tutorial = false;
            textBox.SetActive(false);
            background.SetActive(false);
            charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
            charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
            skipPanel.SetActive(false);
            parent.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
        }
        
        IEnumerator OnType(float interval, string Say)
        {
            int typecount = 0;
            bool skip = false;
            bubble_Text.text = null; // text 초기화 
            Event_Occur(ref skip);
            is_typing = true;
            if (skip)
                yield break;
            
            for (int i = 0; i < Say.Length; i++) 
            { 
                bubble_Text.text = Say.Substring(0, i + 1); 
                yield return new WaitForSecondsRealtime(interval); 
                if (jump_text)
                {
                    bubble_Text.text = Say.ToString();
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
            triangle.gameObject.SetActive(true);
            Type_touch_pos_set();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (is_typing)
                jump_text = true;
            
            if (type_touch_pos)
            {
                bubble_Text.text = null; // text 초기화 
                triangle.gameObject.SetActive(false);
                type_touch_pos = false;
                StartCoroutine(OnType(0.025f, type_data.Get_Target_Text(stage++)));
                
            }
            
        }

        #region Event
        private void Init_Text_Start()
        {
            textBox.SetActive(true);
            triangle.gameObject.SetActive(false);
            StartCoroutine(OnType(0.025f, type_data.Get_Target_Text(stage++)));
        }
        
        public IEnumerator Launch_Done()
        {
            background.SetActive(true);
            triangle.SetActive(false);
            bubble_Text.text = null; // text 초기화 
            textBox.SetActive(true);
            tapBtn.SetActive(false);
            StartCoroutine(OnType(0.025f, type_data.Get_Target_Text(stage++)));
            type_touch_pos = true;
            launch_Panel.SetActive(false);
            
            yield return null;
        }
        
        public IEnumerator Tutorial_end()
        {
            yield return new WaitForSeconds(0.5f);
            pauseBtn.gameObject.SetActive(true);
            _mediator.Event_Receive(Event_num.Tutorial_Basic_Done);
            ground.tutorial = false;
            background.SetActive(false);
            textBox.SetActive(false);
            charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
            charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
            parent.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
            yield return null;
        }
        #endregion

        private void Event_Occur(ref bool return_value)
        {
            switch (stage)
            {
                case 4:
                    charater.GetComponent<Animator>().SetTrigger("Happy");
                    break;

                case 5:
                    type_touch_pos = false;
                    textBox.SetActive(false);
                    launch_Tutorial.SetActive(true);
                    break;
                
                case 8:
                    return_value = true;
                    textBox.SetActive(false);
                    type_touch_pos = false;
                    background.SetActive(false);
                    Ingame_Log.Tutorial_End();
                    Tutorial_Log.Basic_Tutorial_Complete();
                    StartCoroutine(Tutorial_end());
                    break;
            }
        }

        private void Type_touch_pos_set()
        {
            switch (stage)
            {
                default:
                    type_touch_pos = true;
                    break;
                
                case 5:
                    type_touch_pos = false;
                    break;
            }
        }
        
        #region CharaterMove
        private Vector2 Determine_Charater_Pos(Vector2 Pos)
        {
            float X_offset = 135f;
            Vector2 Target_Pos;

            if (Pos.x < 0f)
            {
                Target_Pos = new Vector2(Pos.x + X_offset, charater.position.y);
                charater.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                charater.GetChild(1).GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                Target_Pos = new Vector2(Pos.x - X_offset, charater.position.y);
                charater.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                charater.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
            }
            return Target_Pos;
        }
        
        private IEnumerator _Charater_Move(Vector2 pos)
        {
            Vector2 Target_Pos = Determine_Charater_Pos(pos);
            Transform TR = charater;
            float speed = 500f;
            charater.gameObject.GetComponent<Animator>().SetTrigger("Move"); 
            while (true)
            {
                TR.position = Vector3.MoveTowards(TR.position, Target_Pos, speed * Time.deltaTime);
                if ((Vector2)TR.position == Target_Pos)
                    break;


                yield return null;
            }
            charater.gameObject.GetComponent<Animator>().SetTrigger("Happy");
            // # 2. 타이핑이 쳐지기 시작함
            skipBtn.gameObject.SetActive(true);
            Init_Text_Start(); // 첫 타이핑이 필요함 
            yield return null;

        }
        

        #endregion
        // 이벤트 발생하면, 여기로 메시지를 전달해 다음으로 넘어가도록 해야함 
    }
}
