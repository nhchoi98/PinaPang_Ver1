using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class Ani_Control_3002 : MonoBehaviour
    {
        [SerializeField] private Avatar_3002_Skill skill;
        public AudioSource houlSound;
        private Animator charater_ani;
        public void end()
        {
            skill.Activate();
            this.gameObject.SetActive(false);
        }

        public void Set_Angry()
        {
            skill.charater_ani.SetTrigger("Angry"); // 화나게 만듬 
        }
    }
}
