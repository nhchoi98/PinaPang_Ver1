using System.Collections;
using System.Collections.Generic;
using Setting;
using UnityEngine;

namespace Warn
{
    public class Ingame_Warn : MonoBehaviour
    {
        private bool flagWarn;
        [SerializeField] private Animator warnImg;
        [SerializeField] private Transform boxTR;

        private const float lowerBound = -410f;
        private const float upperBound = -540f;
        /// <summary>
        /// 조건에 부합하지 않을 경우, 경고를 꺼줌. 
        /// </summary>
        public void TurnOff_Warn()
        {
            if (flagWarn)
            {
                warnImg.SetTrigger("Off");
                flagWarn = false;
            }
        }

        public bool Get_Flag()
        {
            return flagWarn;
        }

        public void Set_Flag(bool flaged)
        {
            flagWarn = flaged;
        }

        public void Set_Animation()
        {
            if (flagWarn)
                warnImg.SetTrigger("On");
            
            
            else
                warnImg.SetTrigger("Off");
            
        }

        public void Determine_On()
        {
            StartCoroutine(Determine_Sign_On());
        }

        IEnumerator Determine_Sign_On()
        {
            Vector2 pos;
            Transform tr;
            for (int i = boxTR.childCount- 1; i >= 0; i--)
            {
                try
                {
                    tr = boxTR.GetChild(i);
                    pos = tr.position;
                    if (pos.y < upperBound && pos.y > lowerBound)
                    {
                        yield break;
                    }
                }

                catch
                {
                    continue;
                }
            }

            if (flagWarn)
            {
                flagWarn = false;
                TurnOff_Warn();
            }
        }

    }
}
