using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Collection
{
    public class Gem_Flying : MonoBehaviour
    {
        public Vector2 Target_Pos; // 날아갈 위치를 지정하는 변수 
        public Animator gem_animator; // 젬 텍스트 애니메이션을 지정하는 변수 
        public Vector2 start_pos;
        public Text gem_text;
        private int gem;
        public event EventHandler arrive;
        private void Start()
        {
            StartCoroutine(Random_Pop());
            // 처음 날라가는 이뉴머레이터 실행 
        }

        /// <summary>
        /// 처음 버튼에서 튀어나오는 액션을 취해주는 IE
        /// </summary>
        /// <returns></returns>
        private IEnumerator Random_Pop()
        {
            Transform TR = this.transform;
            Vector2 dir;
            float speed = 350f;
            float rand_x = UnityEngine.Random.Range(-10, 11) / (float) 10f;
            float rand_y = UnityEngine.Random.Range(-10, 11) / (float) 10f;
            dir = new Vector2(rand_x, rand_y);
            
            Vector2 Target = new Vector2(TR.position.x + (dir.x) * 100f,
                TR.position.y + (dir.y) * 100f);

            TR.DOMove(Target, 0.4f)
                .SetEase(Ease.Linear)
                .OnComplete(()=>{StartCoroutine(Move_To_Text());});
            
            yield return null;
        }

        private IEnumerator Move_To_Text()
        {
            Transform TR = this.transform;
            object e = new object();
            EventArgs s = new EventArgs();
            TR.DOMove(Target_Pos, 1f)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    gem_animator.SetTrigger("get");
                    arrive(e,s);
                    Destroy(this.gameObject);
                });
            
            yield return null;
        }
    }
}

