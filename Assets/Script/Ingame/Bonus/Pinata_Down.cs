using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using Block;
using Ingame;

namespace Pinata
{
    public class Pinata_Down : MonoBehaviour
    {
        [SerializeField]
        private Text Hp_text;
        [SerializeField]
        private Animator animator, pinata_ani;
        public int Original_Hp;
        public GameObject Particle_hit, Damage_Obj, Destroy_particle, Pinata_img;
        public int HP { get; private set; }
        [SerializeField]
        private Momentum_calc MC;

        private int attack = 1;
        private AudioSource Hit_Sound, Destroy_Sound;
        public pinata_down pd;

        private bool Ishit;
    

        void Start()
        {
            HP = Original_Hp;
            Hp_text.text = HP.ToString();
            Manager.SoundManager SM = GameObject.FindWithTag("SM").GetComponent<Manager.SoundManager>();
            Hit_Sound = SM.Pinata_hit;
        }
        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.layer)
            {

                default:
                    yield break;
                    break;

                case 3:
                    attack = 1;
                    break;
            }

            Instantiate(Particle_hit, collision.contacts[0].point, Quaternion.identity);
            Hit();
            // 사운드 재생
            yield return null;
        }

        public void Item_Hit()
        {
            Instantiate(Particle_hit, transform.position, Quaternion.identity);
            Hit();
        }
        
        private void Hit()
        {
            Instantiate(Damage_Obj, transform.position, Quaternion.identity);
            if (HP - attack > 0)
            {
                Hit_Sound.Play();
                HP -= attack;
                Hp_text.text = HP.ToString();
                pinata_ani.SetTrigger("hit");
                pinata_body();
            }

            else
            {
                if (!Ishit)
                {
                    Ishit = true;
                    Hp_text.gameObject.SetActive(false);
                    pd.active_Destroy();
                    Destroy_particle.SetActive(true);
                }
            }

            
            
        }
        public void for_revive()
        {
            if (!Ishit)
            {
                GameManage gameManage = GameObject.FindWithTag("GameController").GetComponent<GameManage>();
                gameManage.Event_Receive(Event_num.Launch_Red);
                Hp_text.gameObject.SetActive(false);
                animator.SetTrigger("Down");
                Ishit = true;
                Hp_text.gameObject.SetActive(false);
                pd.active_Destroy();
                Destroy_particle.SetActive(true);
            }

            return;
        }

        //피냐타의 체력별로 몸의 이미지 형태를 바꿔줌 
        private void pinata_body()
        {
            float percent = ((float) HP / (float) Original_Hp) * 100;
            if (percent <= 75 && percent>74 )
                pinata_ani.SetInteger("hpCount", 1);
            
            else if (percent <= 50 && percent >49)
                pinata_ani.SetInteger("hpCount", 2);
            
            else if (percent <= 25 && percent>24)
                pinata_ani.SetInteger("hpCount", 3);

            else
                return;

        }

    }
}
