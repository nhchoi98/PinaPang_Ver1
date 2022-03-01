using System;
using UnityEngine.UI;
using UnityEngine;
using Avatar;
using Collider;
using Ingame;
using Progetile;

namespace Manager
{
    public class Charater_Contorller : MonoBehaviour, IComponent 
    {
        private IMediator _mediator;
        public Animator chara_ani;
        public AudioSource launch_sound;
        public Animator groundEffect;
        public GameObject groundObj;

        public Ground ground;
        private bool is_fliped_false;
        private bool is_fliped_true;
        private void Awake()
        {
            var equipDATA = new EquippedDAO();
            int index;
            index = equipDATA.Get_Equipped_index();
            chara_ani.runtimeAnimatorController = Set_Avatar_UI.Set_Charater_GameObject(index);// 애니메이션 바꾸어줌. 임시로 원숭이 
            SpriteRenderer shadow = chara_ani.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            switch (index)
            {
                default:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow");
                    break;
                
                case 19:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_M");
                    break;
                
                case 1000:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_L");
                    break;

            }

            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            ground.Set_Charater_Index(index);
        }
        

        public void Set_Launch()
        {
            launch_sound.Play();
            _mediator.Event_Receive(Event_num.Launch_MOTION);
        }

        #region Teddy

        private void Set_Fliped()
        {
            var x_offset = 67f;
            bool X_FLIPED = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX;
            groundObj = groundEffect.gameObject;
            is_fliped_false = false;
            is_fliped_true = false;
            if (transform.position.x > 353f && X_FLIPED)
            {
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipY = false;
                is_fliped_false = true;
            }
            
            if (transform.position.x < -353f && !X_FLIPED)
            {
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipY = true;
                is_fliped_true = true;
            }

            if (X_FLIPED)
            {
                groundObj.transform.position =
                    new Vector3(this.transform.position.x + x_offset, groundObj.transform.position.y);
            }

            else
            {
                groundObj.transform.position =
                    new Vector3(this.transform.position.x - x_offset, groundObj.transform.position.y);
            }
            
            
        }

        public void Angry_Down()
        {
            Set_Fliped();
            _mediator.Event_Receive(Event_num.BEAR_SKILL);
        }

        public void Ground_Effect()
        {
            // 위치 조정해주기 
            groundEffect.SetTrigger("start");
        }


        public void End()
        {
            if (is_fliped_false)
            {
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipY = false;
            }

            if (is_fliped_true)
            {
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipY = true;
            }
        }
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            
        }
        
        #endregion
    }
}
