using System;
using System.Collections;
using camera;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Avatar_3001_Skill : MonoBehaviour, IAvatarSkill
    {
        [Header("Data")]
        private Camera_Shake _cameraShake;
        //[SerializeField] private Transform boxGroup;
        private Transform char_tr;

        [Header("mediator")] private IMediator _mediator;

        [Header("UI")] public GameObject sheep;
        public GameObject start_ani;
        private bool fliped_x_true, fliped_x_false;
        public GameObject raycast_forbid;
        public Animator background_ani;
        public GameObject background;

        [Header("Skill_Btn")] public Transform skillBtn;
        private const int target_num = 30; 
        private int count = 11; //11
        private bool pos_activate = true;
        private bool firstCount = true;
        private bool sheep_move = false;

        [Header("Sound")] 
        public AudioSource mainBgm;
        public AudioSource skillBgm;
        
        private event EventHandler sheep_event;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            //boxGroup = GameObject.FindWithTag("BoxGroup").transform;
            _cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Shake>();
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            char_tr = GameObject.FindWithTag("Player").transform;
            sheep_event += Target_Arrive; // 이벤트 등록 
            mainBgm = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
            sheep.GetComponent<Sheep>().sheep_event += sheep_event;
            pos_activate = true;
        }
        
        public void OnClick_Skill_Activate()
        {
            raycast_forbid.SetActive(true);
            _mediator.Event_Receive(Event_num.Launch_Red);
            Init_SkillBtn();
            StartCoroutine(Sound_Start());
            Set_Animation();
        }

        public void Set_Animation()
        {
            Determine_flip();
            if (fliped_x_true)
            {
                char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                char_tr.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            else if (fliped_x_false)
            {
            
                char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                char_tr.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            char_tr.gameObject.GetComponent<Animator>().SetTrigger("Sleep");
            start_ani.SetActive(true);
            background.SetActive(true);
        }

        public void Activate()
        {
            sheep_move = true;
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(15f, 2.0f, false, false)); // 카메라 흔들기 스타트 
            sheep.SetActive(true);
            StartCoroutine(Sheep_move());
        }

        IEnumerator Sheep_move()
        {
            for (int i = 0; i < sheep.transform.childCount-1; i++)
                sheep.transform.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("start");
            
            while (true)
            {
                if (!sheep_move)
                    break;

                yield return null;
            }
            sheep.SetActive(false);
            background_ani.SetTrigger("wake_up");
            char_tr.gameObject.GetComponent<Animator>().SetTrigger("wake_up");
            if (fliped_x_false)
            {
                char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                char_tr.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            else if(fliped_x_true)
            {
                char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                char_tr.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            // 뒤집을지 안뒤집을지 결정 
            StartCoroutine(Sound_End());
            yield return new WaitForSeconds(0.3f);
            background.gameObject.SetActive(false);
            raycast_forbid.SetActive(false);
            _mediator.Event_Receive(Event_num.Launch_Green);
        }

        public void Cancle()
        {
            
        }

        public void Set_count()
        {
            if (!pos_activate)
            {
                --count;
                if (count != 0)
                {
                    skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = count / (float) target_num;
                    skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = count.ToString();
                }

                else
                {
                    pos_activate = true;
                    skillBtn.GetChild(0).gameObject.SetActive(false);
                    skillBtn.GetChild(1).gameObject.SetActive(false);
                    skillBtn.gameObject.GetComponent<Button>().interactable = true;
                    skillBtn.gameObject.GetComponent<Animator>().SetTrigger("interactable");
                }
            }
            
            else if (firstCount)
            {
                --count;
                if (count != 0)
                {
                    skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = count / (float) 10;
                    skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = count.ToString();
                }

                else
                {
                    firstCount = false;
                    skillBtn.GetChild(0).gameObject.SetActive(false);
                    skillBtn.GetChild(1).gameObject.SetActive(false);
                    skillBtn.gameObject.GetComponent<Button>().interactable = true;
                    skillBtn.gameObject.GetComponent<Animator>().SetTrigger("interactable");
                }
            }
            
        }

        private void Init_SkillBtn()
        {
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = 30.ToString();
            skillBtn.GetChild(0).gameObject.SetActive(true);
            skillBtn.GetChild(1).gameObject.SetActive(true);
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            count = 30;
            pos_activate = false;
            
        }
        
        public void Set_SkillBtn(Transform tr)
        {
            skillBtn = tr;
            skillBtn.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Pajama");
            skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = 10.ToString();
            skillBtn.GetChild(0).gameObject.SetActive(true);
            skillBtn.GetChild(1).gameObject.SetActive(true);
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            tr.gameObject.SetActive(true);
        }

        #region Event
        private void Target_Arrive(object obj, EventArgs args)
        {
            sheep_move = false;
        }
        
        #endregion

        private void Determine_flip()
        {
            if (char_tr.position.x > 437 && char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX)
                fliped_x_true = true;

            
            if(char_tr.position.x<-256 && !char_tr.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX )
                fliped_x_false = false;
        }

        #region Sound_Control

        IEnumerator Sound_Start()
        {
            float F_time = 1f;
            float time = 0f;
            float speed = 1.6f;
            float volume = 1f;

 
            while (true)
            {
                time += Time.unscaledDeltaTime * speed;
                mainBgm.volume -= Time.unscaledDeltaTime * speed;

                if (time >= F_time)
                {
                    mainBgm.Stop();
                    break;
                }

                // 볼륨 조절. 
                yield return null;
            }
            
            skillBgm.Play();
            while (true)
            {
                skillBgm.volume += Time.unscaledDeltaTime * speed;
                if (skillBgm.volume>=volume)
                    break;
                
                // 볼륨 조절. 
                yield return null;
            }
            
            
        }
        
        IEnumerator Sound_End()
        {
            float F_time = 1f;
            float time = 0f;
            float speed = 1.6f;
            float volume = 1f;
            
            while (true)
            {
                time += Time.unscaledDeltaTime * speed;
                skillBgm.volume -= Time.unscaledDeltaTime * speed;

                if (time >= F_time)
                {
                    skillBgm.Stop();
                    break;
                }

                // 볼륨 조절. 
                yield return null;
            }
            
            mainBgm.Play();
            while (true)
            {
                mainBgm.volume += Time.unscaledDeltaTime * speed;
                if (mainBgm.volume>=volume)
                    break;
                
                // 볼륨 조절. 
                yield return null;
            }
        }
            
        
        

        #endregion
    }
}
