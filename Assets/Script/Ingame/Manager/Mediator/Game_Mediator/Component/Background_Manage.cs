
using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Skin;
using Theme;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Background_Manage : MonoBehaviour, IComponent
    {
        [Header("Flag")] private bool firstOpen = true;
        public Button pauseBtn;
        
        [Header("Theme")]
        private int theme_name;
        public List<Sprite> BG_Sprite;
        public List<int> BG_INDEX;
        public Image Background, Changed_img;
        private int Which_Theme, bgIndex; //
        public AudioSource BGM;
        public Animator animator;
        private int themeNum;

        private void Awake()
        {
            List<int> index_rand = new List<int>();
            int rand;
            int rand_scale;
            BG_INDEX = new List<int>();

            for (int i = 0; i < 4; i++)
                index_rand.Add(i);  

            rand_scale = index_rand.Count;
            for (int i = 0; i < rand_scale; i++)
            {
                rand = UnityEngine.Random.Range(0, index_rand.Count);
                BG_INDEX.Add(index_rand[rand]);
                index_rand.RemoveAt(rand);
            }

            Set_Theme();
        }
        

        #region Action

        private void BGM_Set_Start()
        {
            pauseBtn.interactable = false;
            
            if (!firstOpen)
            {
                StartCoroutine(Change_Sound());
                Background.sprite = BG_Sprite[BG_INDEX[bgIndex]];
                firstOpen = true;
                ++bgIndex;
            }

            else
                Set_BG_with_music();
            
            
        }

        public void Transition_End()
        {
            pauseBtn.interactable = true;
            
        }
        private void Set_Theme()
        {
            SkinDAO DATA = new SkinDAO();
            BG_Sprite= new List<Sprite>();
            List<int> rand_List = new List<int>();
            DATA.Set_Theme_Data(); // Theme 번호 부여받음 
            themeNum = DATA.Get_Theme_Data();
            for(int i =1; i<5; i++)
                rand_List.Add(i);

            for (int i = 0; i < 4; i++)
            {
                int randNum = UnityEngine.Random.Range(0, rand_List.Count);
                BG_Sprite.Add(DATA.Get_Background(rand_List[randNum]));
                rand_List.RemoveAt(randNum);
            }
            theme_name = DATA.Themename();
        }
        


        /// <summary>
        /// 배경과 음악을 자연스럽게 바꾸어 주는 CLASS 
        /// </summary>
        public void Set_BG_with_music()
        {
            Changed_img.sprite = BG_Sprite[BG_INDEX[bgIndex]];
            StartCoroutine(Change_BG());
            StartCoroutine(Change_Sound());
            if (bgIndex == 3)
                bgIndex = 0;
            ++bgIndex;
        }

        IEnumerator Change_BG()
        {
            float F_time = 1f;
            float time = 0f;
            float alpha = 1f;
            float speed = 0.8f;
            Changed_img.gameObject.SetActive(true);
            while (true)
            {
                time += Time.unscaledDeltaTime * speed;
                alpha -= Time.unscaledDeltaTime * speed;
                if (time >= F_time)
                    break;
                
                Background.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }
            
            Background.sprite = Changed_img.sprite;
            Background.color = new Color(1f, 1f, 1f, 1f);
            Changed_img.gameObject.SetActive(false);
        }

        IEnumerator Change_Sound()
        {
            string path = Determine_Sound.Sound_name(theme_name);
            AudioClip sound = Resources.Load<AudioClip>(path  + (BG_INDEX[bgIndex]+1));
            float F_time = 1f;
            float time = 0f;
            float speed = 1.6f;
            float volume = BGM.volume;

            if (BGM.volume != 0)
            {
                while (true)
                {
                    time += Time.unscaledDeltaTime * speed;
                    BGM.volume -= Time.unscaledDeltaTime * speed;

                    if (time >= F_time)
                    {
                        BGM.Stop();
                        break;
                    }

                    // 볼륨 조절. 
                    yield return null;
                }
                
                BGM.clip = sound;
                BGM.Play();
                while (true)
                {
                    BGM.volume += Time.unscaledDeltaTime * speed;
                    if (BGM.volume>=volume)
                        break;
                    
                    // 볼륨 조절. 
                    yield return null;
                }
            }

            else
                BGM.clip = sound;

            BGM.volume = volume;
            pauseBtn.interactable = true;
            // 볼륨 바꾸고, 다시 볼륨 올리기 

        }
        #endregion


        #region Component
        /// <summary>
        /// 중재자 등록하기 
        /// </summary>
        /// <param name="mediator"></param>
        public void Set_Mediator(IMediator mediator)
        {
        }

        /// <summary>
        /// 이벤트가 발생하면, 이에 맞게 action을 취해주는 함수 
        /// </summary>
        /// <param name="event_num"></param>
        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.BGM_SET_START:
                    BGM_Set_Start();
                    break;
            }
        }
        
        #endregion
    }
    
}
