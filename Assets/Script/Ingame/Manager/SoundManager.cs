
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Block Sound")] public AudioSource Block_hit, Block_destroy, x2_hit, x2_explode;
        [Header("Object_Get")]
        public AudioSource PlusBall;
        [Header("Pinata")]
        public AudioSource Pinata_hit, Pinata_exploce, Pinata_expansion, Pinata_showup;
        [Header("BGM")]
        public AudioSource item;
        public AudioSource Click;

        [Header("Mixer")] [SerializeField] private AudioMixer mixer;


        [Header("Array_Sound")] private AudioSource[] hit_sound;
        private AudioSource[] x2_hit_sound; 
        private void Awake()
        {
            int Master_Volume = PlayerPrefs.GetInt("Master_Volume", -17);
            int BGM_Volume = PlayerPrefs.GetInt("Background_Volume", -17);
            Time.timeScale = 1f;
            if(Master_Volume <=-35f)
                mixer.SetFloat("SFX", -80f);
            
            else
                mixer.SetFloat("SFX", Master_Volume);

            
            if(BGM_Volume <=-35f)
                mixer.SetFloat("BGM", -80f);
            
            else
                mixer.SetFloat("BGM", BGM_Volume);


            hit_sound = new AudioSource[10];
            x2_hit_sound = new AudioSource[10];

            for (int i = 0; i < hit_sound.Length; i++)
                hit_sound[i] = Block_hit;
            
            for (int i = 0; i < x2_hit_sound.Length; i++)
                x2_hit_sound[i] = x2_hit;

        }

        public void Click_Play()
        {
            Click.Play();
        }

        public void Play_Hitsound()
        {
            for (int i = 0; i < hit_sound.Length; i++)
            {
                if (!hit_sound[i].isPlaying)
                {
                    hit_sound[i].Play();
                    return;
                }
            }

        }

        public void Play_x2_Hit()
        {
            for (int i = 0; i < x2_hit_sound.Length; i++)
            {
                if (!x2_hit_sound[i].isPlaying)
                {
                    x2_hit_sound[i].Play();
                    return;
                }
            }
            
            
        }

    }
}
