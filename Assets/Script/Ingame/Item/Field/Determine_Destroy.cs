using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Determine_Destroy : MonoBehaviour
    {
        public bool _Is_activated;
        public Animator animator;
        
        public bool Set_Destroy()
        {
            if (!_Is_activated)
                return false;

            else
            {
                return true;
            }
        }

        public void Set_Animation()
        {
            animator.enabled = true;
            animator.SetTrigger("down");
        }
        
        public void Activated()
        {
            if (_Is_activated)
                return;

            _Is_activated = true;
        }
    }
}
