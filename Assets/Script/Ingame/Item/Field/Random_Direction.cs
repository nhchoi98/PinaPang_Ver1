using System;
using System.Collections;
using System.Collections.Generic;
using Avatar;
using UnityEngine;

namespace Item
{
    public class Random_Direction : MonoBehaviour
    {
        private Vector2 dir;
        private float x, y;
        [SerializeField] private Determine_Destroy _determineDestroy;
        public AudioSource[] sound;
        public Animator Animator;
        private void Start() => Set_Dir();
        /// <summary>
        /// 난반사 아이템이 들어오면, 어느 방향으로 튕겨낼 지를 결정해주는 변수 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 3)
            {
                Sound();
                _determineDestroy.Activated();
                Rigidbody2D obj_rigid = other.gameObject.GetComponent<Rigidbody2D>();
                float scale = obj_rigid.velocity.magnitude;
                Set_Dir();
                obj_rigid.velocity = dir * scale;
                Animator.SetTrigger("shock");
            }
            yield return null;
        }

        private void Set_Dir()
        {
            float which_degree = UnityEngine.Random.Range(40f, 140f);
            which_degree = which_degree * Mathf.PI / 180; // 라디안 값으로 변환 
            x = 10f * Mathf.Cos(which_degree);
            y = 10f * Mathf.Sin(which_degree);
            dir = new Vector2(x, y);
            dir = dir.normalized;
        }
        
        private void Sound()
        {
            for (int i = 0; i < 5; i++)
            {
                if (!sound[i].isPlaying)
                {
                    sound[i].Play();
                    return;
                }
            }
        }
        
        public void Set_Animation_Down()
        {
            Destroy(this.gameObject);
        }

    }
}
