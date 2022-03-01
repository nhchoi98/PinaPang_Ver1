using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace camera
{
    public class Camera_Shake : MonoBehaviour
    {

        [SerializeField]
        private Transform Camera;
        Vector3 originPos;
        
        void Start()
        {
            originPos = transform.localPosition;
        }

        /// <summary>
        /// 카메라를 흔들어주는 변수. 
        /// </summary>
        /// <param name="_amount"></param> 해당 변수는 흔들림의 상한 or 하한을 을 지정해줌 
        /// <param name="_duration"></param>
        /// <param name="up"></param> 카메라를 흔들 때 강도를 세게 할지, 약하게 할 지를 정해주는 변수이다. 
        /// <returns></returns>
        public IEnumerator Shake_Cam(float _amount, float _duration, bool up, bool dynamic)
        {
            transform.localPosition = originPos;
            originPos = Camera.transform.localPosition;
            float timer = 0;
            float amount;

            if (dynamic)
            {
                if (!up)
                    amount = _amount;

                else
                    amount = 0;
            }

            else
                amount = _amount;


            while (timer <= _duration)
            {
                transform.localPosition = (Vector3)Random.insideUnitCircle * amount + originPos;
                if (dynamic)
                {
                    if (up)
                    {
                        if (amount <= _amount)
                            amount += (_amount / _duration) * Time.unscaledDeltaTime;
                        timer += Time.unscaledDeltaTime;
                    }

                    else
                    {
                        if (amount >= 0)
                            amount -= (_amount / _duration) * Time.unscaledDeltaTime;

                        timer += 3f*Time.unscaledDeltaTime;
                    }
                }

                else
                    timer += Time.unscaledDeltaTime;

                yield return null;
            }

            transform.localPosition = originPos;

            yield return null;
        }

    }
}


