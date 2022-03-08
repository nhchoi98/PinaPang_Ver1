using System;
using UnityEngine.UI;
using UnityEngine;
using Setting;
using System.Collections;
using Manager;
using Progetile;
using Score;
using DG.Tweening;

namespace Block
{
    public class Classy_Box : MonoBehaviour, IBox, IDestroy_Action
    {
        [Header("Text")] [SerializeField] private Text Hp_text;

        [Header("Animation")] public Animator animator;

        [Header("Sound")] public AudioSource Explode_sound;
        private Manager.SoundManager SM;

        [Header("Box_Info")] public bool destroy_hit, candle;
        public int HP { get; private set; }
        private int colorType;
        public int Original_Hp, candle_type;
        private blocktype type;
        public Quaternion QI = Quaternion.identity;

        [Header("Particle")] 
        Gradient ourGradient;
        private GameObject particle; // 박스 파괴시 생성되는 파티클 
        private Transform diePool;
        private GameObject[] ball_particle;
        private bool is_effective;
        private Transform particle_pool;
        
        [Header("X2_Info")] private int first_target = 0 ;
        private int second_target = 0;
        private AudioSource explode;
        public IBox.event_delegate args;
        
        [Header("Animator")]
        private Tween hitTween;
        private Sequence hitSequence;
        public Image hit;
        public Sprite first_reach;
        public SpriteRenderer wrap;
        private Color targetColor;
        private bool isHit;

        private int row;

        private void Awake()
        {
            SM = GameObject.FindWithTag("SM").GetComponent<Manager.SoundManager>();
            diePool = GameObject.FindWithTag("Die_Pool").transform;
            Explode_sound = SM.Block_destroy;
            explode = SM.x2_explode;
            transform.DOScale( new Vector2(1f, 1f) , 0.5f)
                .OnComplete(() => { transform.DOKill();});
            targetColor = new Color (hit.color.r/255f, hit.color.g/255f, hit.color.b/255f, 0f);
            hitSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(()=>
                {
                    isHit = true;
                })
                .Append(DOTween.ToAlpha(() => hit.color, alpha => hit.color = alpha, 0.8f, 0.15f))
                .Append(DOTween.ToAlpha(() => hit.color, alpha => hit.color = alpha, 0f, 0.15f))
                .OnComplete(()=>
                {
                    isHit = false;
                });
        }
        #region Animation
        IEnumerator Attack_Animation()
        {
            if(!isHit)
                hitSequence.Restart();
            yield return null;
        }

        #endregion
        
        public int whichRow()
        {
            return row;
        }

        public void Set_Row(int value)
        {
            row += value;
        }
        #region Attack

        // 박스 피격시의 액션을 결정지어주는 함수 
        public void Attack(Vector2 pos, bool is_item = false)
        {
            switch (HP-1)
            {
                default:
                    if (is_effective && !is_item)
                        StartCoroutine(Set_BallParticle(pos));
                    HP -= 1;
                    Hp_text.text = HP.ToString();
                    StartCoroutine(Attack_Animation());
                    if (HP >= second_target)
                    {
                        SM.Play_x2_Hit();
                        if (HP == first_target)
                            wrap.sprite = first_reach;

                        else if (HP == second_target)
                        {
                            animator.enabled = true;
                            explode.Play();
                        }
                    }

                    else
                        SM.Play_Hitsound();

                    break;
                
                case 0:
                    if (!destroy_hit)
                    {
                        destroy_hit = true;
                        Destroy_Action();
                    }

                    break;
            }
        }

        public void Item_Attack()
        {
            Attack(Vector2.zero, true);
        }

        /// <summary>
        /// 박스 생성시 HP와 양초 박스 유무를 결정지어줌 
        /// </summary>
        /// <param name="HP"></param>
        /// <param name="candle"></param>
        public void Set_HP(int HP)
        {
            //박스의 HP 초기화 해주기 
            Original_Hp = HP;
            this.HP = Original_Hp;
            Hp_text.text = String.Format("{0:#,0}", HP);
            first_target = (int)((float)Original_Hp* (3f / 4f));
            second_target = Original_Hp/2;
        }

        /// <summary>
        /// 점수를 메기기 위해 type을 Set 함
        /// </summary>
        /// <param name="type"></param>
        public void Set_Type(blocktype type)
        {
            this.type = type;
        }

        public void Set_ColorType(int colorType, Transform particle)
        {
            this.colorType = colorType;
            this.particle = particle.gameObject;
        }

        public void Set_Event(IBox.event_delegate _event, ref Progetile_Particle script)
        {
            this.args = _event;
            is_effective = script.Get_Is_Effective();
            if (is_effective)
            {
                particle_pool = script.Particle_Pool;
                ball_particle = new GameObject[8];
                script.Make_Particle(ref ball_particle);
            }
        }

        public void Set_Candle(int candle_type)
        {
            this.candle = true; // 양초 박스인지 아닌지를 결정
            this.candle_type = candle_type;
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }

        #endregion

        #region Collider

        private IEnumerator Set_BallParticle(Vector2 pos)
        {
            for (int i = 0; i < ball_particle.Length; i++)
            {
                if (!ball_particle[i].activeSelf)
                {
                    ball_particle[i].transform.position = pos;
                    ball_particle[i].SetActive(true);
                    yield break;
                }

            }
        }

        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.layer)
            {
                default:
                    Attack(collision.contacts[0].point);
                    break;
                
                case 8:
                case 10:
                //양 or 피냐타 
                    destroy_hit = true;
                    Destroy_Action();
                    break;
                
            }

            yield return null;
        }
        public bool Get_Candle()
        {
            return candle;
        }

        public int Get_HP()
        {
            return HP;
        }

        public blocktype Get_Type()
        {
            return type;
        }

        public Vector2 Get_Position()
        {
            return this.transform.position;
        }
        
        #endregion

        #region Destroy_Action


        
        public void Destroy_Action()
        {
            object o = new object();
            DestroyArgs args = new DestroyArgs(transform.position, type);
            this.args(o, args);

            Vibration.Vibrate(100);
            ParticleSystem.MinMaxGradient minMaxGradient;
            Explode_sound.Play();
            Set_Color();
            this.transform.SetParent(diePool);
            particle.transform.position = transform.position;

            var ps = particle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient(ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor = minMaxGradient;


            ps = particle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient(ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor = minMaxGradient;

            if (candle)
            {
                BonusCharater bonus  = GameObject.FindWithTag("bonuscharater").GetComponent<BonusCharater>();
                bonus.Set_Bonus_Charater(candle_type, this.transform.position, true); // 캔디를 비행하게 만들어 습득여부를 바꾸어주는 함수 
            }
            
            particle.SetActive(true);
            if (is_effective)
            {
                for (int i = 0; i < ball_particle.Length; i++)
                    ball_particle[i].transform.SetParent(particle_pool);
                    
            }
            hitSequence.Kill();
            this.gameObject.SetActive(false);
        }

        private void Set_Color()
        {
            Color[] color = new Color[2];
            if (!candle)
            {
                // 일반 상자 
                color[0] = new Color(255f / 255f, 118f / 255f, 165f / 255f);
                color[1] = new Color(255f / 255f, 165f / 255f, 41f / 255f);
            }

            else
            {

                switch (colorType)
                {
                    default:
                        color[0] = new Color(223f / 255f, 211f / 255f, 255f / 255f);
                        color[1] = new Color(254f / 255f, 128f / 255f, 147f / 255f);
                        break;
                    
                    case 1: // 삼각형, 반반박스
                        color[0] = new Color(255f / 255f, 207f / 255f, 2f / 255f);
                        color[1] = new Color(187f / 255f, 92f / 255f, 214f / 255f);
                        break;

                    case 2: // 원형 블록 
                        color[0] = new Color(121f / 255f, 227f / 255f, 255f / 255f);
                        color[1] = new Color(0f / 255f, 144f / 255f, 192f / 255f);
                        break;
                }
                
            }
            
            ourGradient = new Gradient();
            ourGradient.mode = GradientMode.Fixed;
            ourGradient.SetKeys(
                new GradientColorKey[] {new GradientColorKey(color[0], 0.5f), new GradientColorKey(color[1], 1f)},
                new GradientAlphaKey[]
                    {new GradientAlphaKey(1f, 1.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(1f, 0.0f)}
            );
        }


        #endregion
    }
}
