
using UnityEngine;

namespace Ingame
{
    public class StartAni_Control : MonoBehaviour
    {
        [SerializeField] private Avatar_3000_Skill skill;
        public AudioSource launch_Sound;
        public void end()
        {
            skill.StartCoroutine(skill.Launch_Rocket());
            this.gameObject.SetActive(false);
        }

        public void Play_Launch()
        {
            launch_Sound.Play();
        }
    }
}
