using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Manager;
using Progetile;
using Score;
using Setting;


namespace Block
{
    /// <summary>
    /// 노말 타입의 박스들은 이 스크립트를 가지고 있음
    /// 1. IBox: 박스의 아이템 공격 당할 시, 공에게 공격당할 시, 박스 파괴시 파티클 및 파티클 색상 지정, 캔들 박스 여부를 IBox 를 통해 설정하고 값을 리턴해줌.
    /// 2. IDestroy_Action: 박스 파괴시의 액션을 구체화 할 수 있는 Interface. 
    /// </summary>
    public class Normal_Box : MonoBehaviour, IBox, IDestroy_Action
    {
        [Header("Text")]
        [SerializeField]
        private Text hpText;
        
        [Header("Sound")]
        public AudioSource  Explode_sound;
        private Manager.SoundManager SM;
        
        [Header("Box_Info")]
        public bool destroy_hit, candle; // destroy hit: 박스가 부셔지는 함수가 호출 되면 또 호출되지 못하도록 막는 함수. 
        private int HP;
        private int Original_Hp, candle_type;
        private blocktype type;
        private int colorType;
        public Quaternion QI = Quaternion.identity;
        private Transform diePool;
        
        [Header("Particle_Gradient")]
        Gradient ourGradient;
        private GameObject particle; // 박스 파괴시 파티클을 담고있는 var
        private GameObject[] ball_particle; // 박스 피격시 볼이 파티클 있는 공이라면, 볼 파티클을 저장해두는 var;
        private bool is_effective;
        private Transform particle_pool;
        public Image hit;
        public IBox.event_delegate args;
        private Color targetColor;
        
        [Header("Animator")]
        private Tween hitTween; 
        private Sequence hitSequence; // 박스 피격시 애니메이션 과정을 담고있는 Sequence.
        private bool isHit;

        [Header("Pos_Item")] 
        private int row;
        private void Awake()
        {
            // 크기가 변하는 TWEEN 
            transform.DOScale(new Vector2(1f, 1f), 0.5f)
                .OnComplete(() => { transform.DOKill();});
            SM = GameObject.FindWithTag("SM").GetComponent<Manager.SoundManager>();
            diePool = GameObject.FindWithTag("Die_Pool").transform;
            Explode_sound = SM.Block_destroy;
            targetColor = new Color (hit.color.r/255f, hit.color.g/255f, hit.color.b/255f, 0f);
            hitSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(()=>
                {
                    isHit = true; // 애니메이션이 시작되면, 다른 공이 날아와서 박스를 피격하더라도 애니메이션을 실행하지 않음.
                })
                .Append(DOTween.ToAlpha(() => hit.color, alpha => hit.color = alpha, 0.8f, 0.15f)) 
                .Append(DOTween.ToAlpha(() => hit.color, alpha => hit.color = alpha, 0f, 0.15f)) // 피격 시 나타나는 빨간색 이미지의 alpha값을 바꿔줌. 
                .OnComplete(()=>
                {
                    isHit = false;// 애니메이션이 끝나면, 다시 애니메션을 시작할 수 있음을 flag를 통해 설정해줌.
                });
        }

        /// <summary>
        /// 캔들박스인지 아닌지 여부를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public bool Get_Has_Candle()
        {
            return candle;
        }

        #region  Attack
        // 박스 피격시의 액션을 결정지어주는 함수 
        public void Attack(Vector2 pos, bool is_item = false)
        {
            switch (HP - 1)
            {
                default:
                    if (is_effective && !is_item)
                        StartCoroutine(Set_BallParticle(pos));
                    HP -= 1;
                    hpText.text = String.Format("{0:#,0}", HP);
                    StartCoroutine(Attack_Animation()); //애니메이션 재생 
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

        #region Animation
        

        IEnumerator Attack_Animation()
        {
            if(!isHit)
                hitSequence.Restart();
            yield return null;
        }

        #endregion

        /// <summary>
        /// 필드 아이템이 박스를 공격시 호출되는 함수  
        /// </summary>
        public void Item_Attack()
        {
            if (destroy_hit)
                return;
            
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
            hpText.text = String.Format("{0:#,0}", HP);
        }
        
        /// <summary>
        /// 이 박스가 삼각형인지, 사각형인지, 원인지를 설정해주는 함수. Respawn_Box에서 호출됨.
        /// </summary>
        /// <param name="type"></param>
        public void Set_Type(blocktype type)
        {
            this.type = type;
        }

        public void Set_Candle(int candle_type)
        {
            this.candle = true; // 양초 박스인지 아닌지를 결정
            this.candle_type = candle_type;
            //캔들박스 처럼 보이기 위해 UI를 바꾸어주는 함수 
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
        
        public void Set_ColorType(int colorType,Transform particle)
        {
            this.colorType = colorType;
            this.particle = particle.gameObject;
        }

        /// <summary>
        /// 볼 피격시 파티클을 가지고 있는 공이라면 공파티클을 뿌리게 만들어주는 함수. 
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="script"></param>
        public void Set_Event(IBox.event_delegate _event, ref Progetile_Particle script)
        {
            is_effective = script.Get_Is_Effective();
            if (is_effective)
            {
                particle_pool = script.Particle_Pool;
                ball_particle = new GameObject[8];
                script.Make_Particle(ref ball_particle);
            }
            this.args = _event;
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
                case 10: // 양 or 피냐타 
                    destroy_hit = true;
                    Destroy_Action();
                    break;
                
                
            }
            yield return null;
        }
        

        #endregion

        #region Destroy_Action
        /// <summary>
        /// 박스 파괴시 호출되는 함수. 파괴 시 액션을 정의한다. 
        /// </summary>
        public void Destroy_Action()
        {
            Vibration.Vibrate(100);
            object o = new object();
            DestroyArgs args = new DestroyArgs(transform.position, type);
            this.args(o, args);
            ParticleSystem.MinMaxGradient minMaxGradient;
            this.transform.SetParent(diePool);
            particle.transform.position = this.transform.position; 
            Explode_sound.Play();
            Set_Color();

            var ps = particle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient (ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor= minMaxGradient;
        
            
            ps  = particle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient (ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor= minMaxGradient;
            
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
        
        public bool Get_Candle()
        {
            return candle;
        }

        public int Get_HP()
        {
            return HP;
        }

        public int whichRow()
        {
            return row;
        }

        public void Set_Row(int value)
        {
            row += value;
        }

        public blocktype Get_Type()
        {
            return type;
        }

        public Vector2 Get_Position()
        {
            return this.transform.position;
        }

        /// <summary>
        /// 파티클의 색상을 지정해주는 함수. 
        /// </summary>
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
