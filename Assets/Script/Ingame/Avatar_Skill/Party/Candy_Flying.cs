using System;
using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Candy_Flying : MonoBehaviour
    {
        public Vector2 Target_Pos; // 날아갈 위치를 지정하는 변수 
        public Transform boxGroup;
        public Animator candy_animator;
        public Animator obj_animator;
        public Text gemText;
        private int candy;
        private bool is_collide;
        public AudioSource coin;
        public CircleCollider2D Collider2D;
        
        public event EventHandler arrive;
        void Start()
        {
            StartCoroutine(Vertical_Pop());
        }

        IEnumerator Vertical_Pop()
        {
            Transform TR = this.transform;
            Vector2 dir;
            Vector2 Target = new Vector2(TR.position.x ,TR.position.y + 150f);
            float speed = 1300f;
            while (true)
            {

                if(Time.timeScale!=0)
                    TR.position = Vector2.MoveTowards(TR.position, Target, speed*Time.unscaledDeltaTime);

                if (Mathf.Abs(TR.position.y - Target.y) < 1)
                    break;
                
                yield return null;
            }

            StartCoroutine(Move_To_Text());
            yield return null;
        }
        
        private IEnumerator Move_To_Text()
        {
            Transform TR = this.transform;
            float speed = 1300f;
            object e = new object();
            EventArgs s = new EventArgs();
            while (true)
            {
                if (Mathf.Abs(Target_Pos.x - TR.position.x) < 1)
                    break;

                TR.position = Vector2.MoveTowards(TR.position, Target_Pos, speed * Time.unscaledDeltaTime);
                yield return null;
            }

            Collider2D.enabled = true;
        }
        
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 3) // 공이 들어온 경우
            {
                if (!is_collide)
                {
                    is_collide = true;
                    Collider2D.enabled = false;
                    coin.Play();
                    StartCoroutine(Target_To_Bar());
                }
            }

            yield return null;
        }

        private IEnumerator Target_To_Bar()
        {
            Transform TR = this.transform;
            Target_Pos = new Vector2(362f, 405f);
            float speed = 1000f;
            object e = new object();
            EventArgs s = new EventArgs();
            candy_animator.SetTrigger("Start");
            Playerdata_DAO.Set_Player_Gem(1);
            while (true)
            {
                if (Mathf.Abs(Target_Pos.x - TR.position.x) < 0.5)
                    break;

                TR.position = Vector2.MoveTowards(TR.position, Target_Pos, speed * Time.unscaledDeltaTime);
                speed += Time.unscaledDeltaTime * 1500;
                yield return null;
            }
            candy_animator.SetTrigger("Arrive");
            arrive(e,s);
            Destroy(this.gameObject);
        }

    }
}
