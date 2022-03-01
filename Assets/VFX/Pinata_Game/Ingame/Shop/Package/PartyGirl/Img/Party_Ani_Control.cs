using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class Party_Ani_Control : MonoBehaviour
    {
        public Transform ani_TR;

        void Start()
        {
            StartCoroutine(Start_Animation());
        }

        IEnumerator Start_Animation()
        {
            Animator animator;
            for (int i = 0; i < ani_TR.childCount; i++)
            {
                animator = ani_TR.GetChild(i).gameObject.GetComponent<Animator>();
                animator.SetTrigger("go");
                yield return new WaitForSeconds(0.3f);
            }

            yield return null;
        }
        
    }
}
