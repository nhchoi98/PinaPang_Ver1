using System;
using System.Collections;
using Challenge;
using UnityEngine;
using UnityEngine.UI;
using Ad;
using Data;
using Ingame;
using Log;

namespace Manager
{
    public class Revive : MonoBehaviour ,IComponent
    {
        [SerializeField]
        private GameObject GameOverPanel;
        
        [SerializeField] 
        private QuestManager _questManager;
        
        [SerializeField] private SoundManager sm;
        public Slider Timer_Slider;
        public bool yes_pushed = false;
        public Text Timer_text;
        public Button reviveGem;
        public GameObject noadsBtn;
        public Transform btn;
        
        private IMediator _mediator;
        public Transform gm;
        private void Awake()
        {
            _mediator = gm.gameObject.GetComponent<IMediator>();
            if (Noads_instance.Get_Is_Noads())
            {
                noadsBtn.SetActive(true);
                btn.gameObject.SetActive(false); // 일반 버튼들 전부 날리기 
            }

        }

        void OnEnable()
        {
            if (Playerdata_DAO.Player_Gem() < 5)
            {
                reviveGem.interactable = false;
                reviveGem.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(255f / 255f, 28f / 255f, 26f / 255f);
                
            }

            StartCoroutine(Timer());
            // 코인을 쓸 수 있는지 사용자 재화를 따져보고, 부족하면 코인으로 부활 가능한 버튼은 비활성화 하기 .
        }

        IEnumerator Timer()
        {
            float Target_Time = 10f;
            float F_time = 0;
            while(true)
            {
                if (Target_Time <= F_time)
                    break;

                F_time += Time.unscaledDeltaTime;
                Timer_Slider.value = 1 - (float)(F_time / Target_Time);
                Timer_text.text = ((int)(Target_Time - F_time)).ToString();
                
                if (yes_pushed)
                    yield break;

                yield return null;
            }

            yes_pushed = true;
            if (!Noads_instance.Get_Is_Noads() && !Noads_instance.Get_Is_Noads_New())
            {
                IMediator _mediator = GameObject.FindWithTag("adcontrol").GetComponent<IMediator>();
                _mediator.Event_Receive(Event_num.USER_DIE);
            }

            else
            {
                GameOverPanel.SetActive(true);
            }
            gameObject.SetActive(false);
        }

        public void Noads_Click()
        {
            sm.Click.Play();
            Time.timeScale = 1f;
            _mediator.Event_Receive(Event_num.BOX_REMOVE);
            _questManager.Set_Revive();
            this.gameObject.SetActive(false);
        }

        public void OnClick_Ad_Revive() 
        {
            _questManager.Set_Revive();
            Time.timeScale = 1f;
            _mediator.Event_Receive(Event_num.BOX_REMOVE);
            this.gameObject.SetActive(false);
        }

        public void Gem_Revive()
        {
            sm.Click.Play();
            yes_pushed = true;
            Ingame_Log.Revive(true);
            Revive_Coin();
        }

        void Revive_Coin()
        {
            Playerdata_DAO.Set_Player_Gem(-5);
            sm.Click.Play();
            Time.timeScale = 1f;
            _mediator.Event_Receive(Event_num.BOX_REMOVE);
            _questManager.Set_Revive();
            this.gameObject.SetActive(false);
        }

        public void Exit_click()
        {
            sm.Click.Play();
            yes_pushed = true;
            if (!Noads_instance.Get_Is_Noads() && !Noads_instance.Get_Is_Noads_New())
            {
                IMediator _mediator = GameObject.FindWithTag("adcontrol").GetComponent<IMediator>();
                _mediator.Event_Receive(Event_num.USER_DIE);
            }

            else
            {
                GameOverPanel.SetActive(true);
            }
            gameObject.SetActive(false);
        }


        public void Set_Mediator(IMediator mediator)
        {
            
        }

        public void Event_Occur(Event_num eventNum)
        {
            
        }
    }
}
