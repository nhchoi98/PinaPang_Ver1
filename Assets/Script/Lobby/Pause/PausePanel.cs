using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Setting
{
    public class PausePanel : MonoBehaviour
    {
        public GameObject Master_MuteBtn, Master_SoundBtn, BGM_MuteBtn, BGM_SoundBtn;
        public GameObject Vibration_Btn, Vibration_Off;
        public Scrollbar vibe_bar;
        public AudioMixer mixer;
        public Slider Master_Volume_OBJ, BGM_Volume_OBJ;
        private float Timescale_const;
        private bool Launch_Method, vibe_on;
        public GameObject restorePopup, waitPanel;
        public AudioSource click;
        private bool init_vibe;
        /// <summary>
        /// 1. 볼륨
        /// 2. 배경음악 볼륨
        /// 3. 발사방법 의 설정을 불러옴. 
        /// 4. 홈으로 이동
        /// 5. 재시작 의 버튼 동작도 여기서 관리하도록함
        /// 6. 게임으로 이동 
        /// 도 여기서 관리하도록 함. 
        /// </summary>

        private void Awake()
        {
            int Master_Volume = PlayerPrefs.GetInt("Master_Volume", -17);
            int BGM_Volume = PlayerPrefs.GetInt("Background_Volume", -17);
            Master_Volume_OBJ.value = (float) Master_Volume;
            BGM_Volume_OBJ.value = (float) BGM_Volume;
            if (Master_Volume <= -35)
                Master_Volume_Zero();
            
            else
                Master_Volume_Nonzero();

            if (BGM_Volume <= -35)
                BGM_Volume_Zero();

            else
                BGM_Volume_nonzero();
            

            if (Vibration.Get_IsOn()) // 진동이 켜져 있는 경우 
            {
                Vibration_Btn.SetActive(false);
                Vibration_Off.SetActive(true);
                vibe_bar.value = 1;
                vibe_on = true;
            }

            else
            {
                Vibration_Btn.SetActive(true);
                Vibration_Off.SetActive(false);
                vibe_bar.value = 0;
                vibe_on = false;
            }

        }
        
        #region Master_Volume

        /// <summary>
        /// 마스터 볼륨을 설정하는 슬라이더가 움직이면 호출되는 함수 
        /// </summary>
        /// <param name="Vol"></param>
        public void Master_Volume_Control()
        {
            PlayerPrefs.SetInt("Master_Volume", (int) (Master_Volume_OBJ.value));
            if (Master_Volume_OBJ.value == -35)
            {
                mixer.SetFloat("SFX", -80f);
                Master_Volume_Zero();
            }
            else
            {
                mixer.SetFloat("SFX", Master_Volume_OBJ.value);
                Master_Volume_Nonzero();
            }
        }

        private void Master_Volume_Zero()
        {
            Master_MuteBtn.gameObject.SetActive(true);
            Master_SoundBtn.gameObject.SetActive(false);
        }

        private void Master_Volume_Nonzero()
        {
            Master_MuteBtn.gameObject.SetActive(false);
            Master_SoundBtn.gameObject.SetActive(true);
        }

        // 어디에 클릭음 넣지? 
        public void Mute_Click_Sound()
        { 
            if (Master_Volume_OBJ.value <= -35)
            {
                mixer.SetFloat("SFX", 0);
                Master_Volume_OBJ.value = 0;
                PlayerPrefs.SetInt("Master_Volume", 0);
                Master_Volume_Nonzero();
            }
            else
            {
                mixer.SetFloat("SFX", -80f);
                Master_Volume_OBJ.value = -35;
                PlayerPrefs.SetInt("Master_Volume", (int) -80);
                Master_Volume_Zero();
            }
        }

        public void Mute_Click_Music()
        {

            if (BGM_Volume_OBJ.value <=-35)
            {
                mixer.SetFloat("BGM", 0);
                BGM_Volume_OBJ.value = 0;
                PlayerPrefs.SetInt("Background_Volume", (int) 0);
                BGM_Volume_nonzero();
            }
            else
            {
                mixer.SetFloat("BGM", -80f);
                BGM_Volume_OBJ.value = -35;
                PlayerPrefs.SetInt("Background_Volume", (int) -80);
                BGM_Volume_Zero();
            }

        }


        #endregion

        #region BGM_Volume

        public void BGM_Control()
        {
            PlayerPrefs.SetInt("Background_Volume", (int) (BGM_Volume_OBJ.value));
            if (BGM_Volume_OBJ.value <= -35)
            {
                BGM_Volume_OBJ.value = -35;
                mixer.SetFloat("BGM", -80);
                BGM_Volume_Zero();
            }
            else
            {
                mixer.SetFloat("BGM", BGM_Volume_OBJ.value);
                BGM_Volume_nonzero();
            }

        }

        private void BGM_Volume_Zero()
        {
            BGM_MuteBtn.gameObject.SetActive(true);
            BGM_SoundBtn.gameObject.SetActive(false);
        }

        private void BGM_Volume_nonzero()
        {
            BGM_MuteBtn.gameObject.SetActive(false);
            BGM_SoundBtn.gameObject.SetActive(true);

        }

        #endregion

        #region Button


        public void Exit_Btn()
        {
            click.Play();
            this.gameObject.SetActive(false);
        }
        
        #endregion

        #region Panel
        public void Open_Restore_Panel()
        {
            restorePopup.SetActive(true);
        }

        public void Close_Restore_Panel()
        {
            restorePopup.SetActive(false);
        }

        public void Yes_Restore()
        {
            waitPanel.SetActive(true);
        }

        #endregion

        #region Vibration

        public void On_Vibration()
        {
            Vibration_Btn.SetActive(false);
            Vibration_Off.SetActive(true);
            vibe_bar.value = 1;
            vibe_on = true;
            click.Play();
            Vibration.Set_IsOn(true);
            Vibration.Vibrate(500);
        }

        public void Off_Vibration()
        {
            Vibration_Btn.SetActive(true);
            Vibration_Off.SetActive(false);
            vibe_bar.value = 0;
            vibe_on = false;
            click.Play();
            Vibration.Set_IsOn(false);
        }

        public void Click_Vibe_Scroll()
        {
            if (init_vibe)
            {
                click.Play();
                if (vibe_on)
                {
                    Vibration_Btn.SetActive(true);
                    Vibration_Off.SetActive(false);
                    vibe_bar.value = 0;
                    vibe_on = false;
                    Vibration.Set_IsOn(false);
                }

                else
                {
                    Vibration_Btn.SetActive(false);
                    Vibration_Off.SetActive(true);
                    vibe_bar.value = 1;
                    vibe_on = true;
                    Vibration.Set_IsOn(true);
                    Vibration.Vibrate(500);
                }
            }

            else
            {
                init_vibe = true;
            }

        }
        
        #endregion
    }
}


