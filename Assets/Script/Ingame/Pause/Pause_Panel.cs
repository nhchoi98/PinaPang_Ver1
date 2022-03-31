using Challenge;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Manager;
using Data;
using Ingame;
using UnityEngine.Audio;

/// <summary>
/// 이 스크립트에서는 다음과 같은 설정을 관리한다. 
/// </summary>

namespace Setting
{
    public class Pause_Panel : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private QuestManager _questManager;
        public GameObject Master_MuteBtn, Master_SoundBtn, BGM_MuteBtn, BGM_SoundBtn;
        public GameObject Vibration_Btn, Vibration_Off;
        public Slider Master_Volume_OBJ, BGM_Volume_OBJ;
        public Scrollbar vibe_bar;
        private bool vibe_on;
        private float Timescale_const;
        public Animator animator;

        public GameObject confirm_Panel;

        [SerializeField] private  Ad_Mediator ad;
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

            
            // 진동 설정 불러오기 

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


        private void OnEnable()
        {
            Timescale_const = Time.timeScale;
            if (Timescale_const == 0)
                Timescale_const = 1;
            
            Time.timeScale = 0f;
        }

        #region Master_Volume

        public void Master_Volume_Control()
        {
            if (Master_Volume_OBJ.value <= -35)
            {
                PlayerPrefs.SetInt("Master_Volume", (int) (-80));
                mixer.SetFloat("SFX", -80f);
                Master_Volume_Zero();
            }
            else
            {
                PlayerPrefs.SetInt("Master_Volume", (int) (Master_Volume_OBJ.value));
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


        public void Handle_Touch()
        {
            
        }

        #endregion

        #region BGM_Volume

        public void BGM_Control()
        {
            if (BGM_Volume_OBJ.value <= -35)
            {
                PlayerPrefs.SetInt("Background_Volume", (int) (-80));
                BGM_Volume_OBJ.value = -35;
                mixer.SetFloat("BGM", -80);
                BGM_Volume_Zero();
            }
            else
            {
                PlayerPrefs.SetInt("Background_Volume", (int) (BGM_Volume_OBJ.value));
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

        public void HomeBtn()
        {
            ad.Destroy_Banner();
            ad.Remove_CallBack();
            _questManager.Set_User_Stat_Save();
            SceneManager.LoadScene("Loading_Scene_Game");
            PlayerPrefs.SetInt("Still_Game", 0); // 이어하기 기능 중지 시키기 
            PlayerPrefs.SetInt("Ingame", 0);
            PlayerPrefs.SetInt("Play_Game",0);
        }

        public void PausePanel_HomeBtn()
        {
            confirm_Panel.SetActive(true);
        }

        public void HomeComfirm_noBtn()
        {
            confirm_Panel.SetActive(false);
        }

        public void Off_Animator()
        {
            animator.enabled = false;
        }
        
        public void Exit_Btn()
        {
            Time.timeScale = Timescale_const;
            this.gameObject.SetActive(false);
        }

        #endregion

        public void On_Vibration()
        {
            Vibration_Btn.SetActive(false);
            Vibration_Off.SetActive(true);
            vibe_bar.value = 1;
            vibe_on = true;
            Vibration.Set_IsOn(true);
            Vibration.Vibrate(500);
        }

        public void Off_Vibration()
        {
            Vibration_Btn.SetActive(true);
            Vibration_Off.SetActive(false);
            vibe_bar.value = 0;
            vibe_on = false;
            Vibration.Set_IsOn(false);
        }

        public void Click_Vibe_Scroll()
        {
            if (init_vibe)
            {
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
                    Vibration.Set_IsOn(true);
                }
            }

            else
            {
                init_vibe = true;
            }

        }
        
    }
}
