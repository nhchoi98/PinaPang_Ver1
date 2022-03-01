using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battery
{
    public class HeartFlying : MonoBehaviour
    {
        public Vector2 Target_Pos;
        public Vector2 start_pos;
        public Text heart_text;
        public event EventHandler arrive;
        private int heart;
        
        private void Start()
        {
            StartCoroutine(Random_Pop());
            // 처음 날라가는 이뉴머레이터 실행 
        }
        
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
            while (true)
            {
                if (Mathf.Abs(Target.x - TR.position.x )<1)
                    break;
                
                if(Time.timeScale!=0)
                    TR.position = Vector2.MoveTowards(TR.position, Target, speed*Time.unscaledDeltaTime);
                speed += Time.unscaledDeltaTime * 7000;
                yield return null;
            }

            StartCoroutine(Move_To_Text());
            yield return null;
        }
        
        private IEnumerator Move_To_Text()
        {
            Transform TR = this.transform;
            float speed = 350f;
            object e = new object();
            EventArgs s = new EventArgs();
            while (true)
            {
                if (Mathf.Abs(Target_Pos.x - TR.position.x) < 1)
                    break;

                TR.position = Vector2.MoveTowards(TR.position, Target_Pos, speed * Time.unscaledDeltaTime);
                speed += Time.unscaledDeltaTime * 1500;
                yield return null;
            }
            
            arrive(e,s);
            Destroy(this.gameObject);
        }
 
    }
}
