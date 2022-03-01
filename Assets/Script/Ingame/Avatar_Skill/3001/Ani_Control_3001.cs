
using System;
using UnityEngine;

namespace Ingame
{
    public class Ani_Control_3001 : MonoBehaviour
    {
        [SerializeField] private Avatar_3001_Skill skill;
        public AudioSource sleep_sound;
        

        public void end()
        {
            skill.Activate(); // 스킬 활성화 
            this.gameObject.SetActive(false);
        }
    }
}
