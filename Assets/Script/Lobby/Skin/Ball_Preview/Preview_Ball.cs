
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ball
{
    public class Preview_Ball : MonoBehaviour
    {
        public Vector3 target_rotation = new Vector3(0f, 0f, 0f);
        public Vector2 target_pos, target_vec;
        public Image ball_img;
        public Rigidbody2D rigid;
        private GameObject particle;
        GameObject obj;
        private IEnumerator go_ienumerator;
        private bool effect_active;

        public CircleCollider2D Collider2D;
        public GameObject celling;
        void Start()
        {
            target_pos = new Vector2(0f, 0f);
            target_vec = new Vector2(0f, 350f);
        }

        public void OnClick_Ball(int index)
        {
            Collider2D.enabled = false;
            if (go_ienumerator != null)
            {
                StopCoroutine(go_ienumerator);
            }
            rigid.freezeRotation = true;
            transform.localPosition = target_pos; // 출발 지점으로 이동함 
            transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
            go_ienumerator = box_preview();
            if (Determine_Effect(index))
            {
                Destroy(obj);
                particle = Resources.Load("Ball/particle/" + Calc_Index.Get_Ball_Num(index).ToString()) as GameObject; // 파티클 파일 불러오기 
                obj = Instantiate(particle,target_pos,Quaternion.identity);
                effect_active = true;
            }

            else
                effect_active = false;
            
            StartCoroutine(go_ienumerator);


        }
        /// <summary>
        /// 박스 충돌시 호출되는 함수 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private IEnumerator OnCollisionEnter2D(Collision2D other)
        {
            go_ienumerator = null;
            rigid.velocity = Vector2.zero;
            Collider2D.enabled = false;
            rigid.freezeRotation = true;
            transform.rotation = Quaternion.Euler(target_rotation);
            if (effect_active)
            {
                obj.transform.position = other.contacts[0].point;
                obj.SetActive(true);
            }
            go_ienumerator = box_preview();
            ball_img.enabled = false;
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(go_ienumerator);

        }

        private IEnumerator box_preview()
        {
            transform.localPosition = target_pos; // 출발 지점으로 이동함 
            ball_img.enabled = true;
            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(1.0f);
            rigid.freezeRotation = false;
            transform.rotation = Quaternion.Euler(target_rotation);
            rigid.AddForce(target_vec);
            rigid.AddTorque(800f);
            Collider2D.enabled = true;
        }
        
        
        private bool Determine_Effect(int index)
        {
            int num_index = Calc_Index.Get_Ball_Num(index);
            bool value;
            switch (num_index)
            {
                default:
                    value = true;
                    break;
                
                case 0:
                case 1000:
                case 1001:
                case 1002:
                case 1003:
                case 1004:
                case 1005:
                case 1006:
                case 1007:
                    value = false;
                    break;
            }

            return value;
        }

    }
}
