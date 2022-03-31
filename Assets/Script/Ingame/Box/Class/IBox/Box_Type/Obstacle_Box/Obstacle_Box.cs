using System;
using UnityEngine;
using System.Collections;
using Progetile;
using Score;
using DG.Tweening;

namespace Block
{
    public class Obstacle_Box : MonoBehaviour, IBox, IDestroy_Action
    {

        [Header("Sound")] public AudioSource Explode_sound;
        private Manager.SoundManager SM;

        [Header("Particle_Gradient")] 
        Gradient ourGradient;
        private GameObject particle;
        private Transform diePool;

        private blocktype type;
        private GameObject[] ball_particle;
        private bool is_effective;
        private Transform particle_pool;
        public bool destroy_hit;
        private int row;
        
        private void Awake()
        {
            SM = GameObject.FindWithTag("SM").GetComponent<Manager.SoundManager>();
            diePool = GameObject.FindWithTag("Die_Pool").gameObject.transform;
            transform.DOScale( new Vector2(1f, 1f) , 0.5f);
            Explode_sound = SM.Block_destroy;
            Set_Color();
        }

        public int Get_Candle()
        {
            return -1;
        }

        public int Get_HP()
        {
            return 0;
        }

        public blocktype Get_Type()
        {
            return type;
        }

        public Vector2 Get_Position()
        {
            return this.transform.position;
        }
        
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
            if (is_effective)
                StartCoroutine(Set_BallParticle(pos));
        }

        public void Item_Attack()
        {
            return;
        }

        /// <summary>
        /// 박스 생성시 HP와 양초 박스 유무를 결정지어줌 
        /// </summary>
        /// <param name="HP"></param>
        /// <param name="candle"></param>
        public void Set_HP(int HP)
        {
            return;
        }

        /// <summary>
        /// 점수를 메기기 위해 type을 Set 함
        /// </summary>
        /// <param name="type"></param>
        public void Set_Type(blocktype type)
        {
            this.type = type;
        }

        public void Set_ColorType(int colorType,Transform particle)
        {
            this.particle = particle.gameObject;
        }

        public void Set_Event(IBox.event_delegate _event, ref Progetile_Particle script)
        {
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

        }


        public void Set_Event(EventHandler<DestroyArgs> _event)
        {
            
            
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
                    SM.Play_Hitsound();
                    Attack(collision.contacts[0].point);
                    break;
                
                case 8:
                case 10:
                    if (!destroy_hit)
                    {
                        destroy_hit = true;
                        Destroy_Action();
                    }

                    break;
                    
            }

            yield return null;
        }


        #endregion

        #region Destroy_Action

        public void Destroy_Action()
        {
            Explode_sound.Play();
            this.transform.SetParent(diePool);
            particle.transform.position = this.transform.position;
            particle.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void Set_Color()
        {
            ParticleSystem.MinMaxGradient minMaxGradient;
            Gradient ourGradient;
            ourGradient = new Gradient();
            ourGradient.mode = GradientMode.Fixed;
            ourGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(255f/255f, 80f/255f, 33f/255f), 0.2f), new GradientColorKey(new Color(255f/255f,227f/255f,96f/255f), 0.4f), 
                    new GradientColorKey(new Color(114f/255f,230f/255f,255f/255f), 0.6f),
                    new GradientColorKey(new Color (147f/255f,120f/255f,255f/255f), 0.8f), new GradientColorKey(new Color (255f/255f,95f/255f,207f/255f), 1.0f)
                
                },
                new GradientAlphaKey[]
                    {new GradientAlphaKey(1f, 1.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(1f, 0.0f)}
            );

            particle = particle_pool.GetChild(0).gameObject;
            var ps = particle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient (ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor= minMaxGradient;
        
            
            ps  = particle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
            minMaxGradient = new ParticleSystem.MinMaxGradient (ourGradient);
            minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;
            ps.startColor= minMaxGradient;

        }


        #endregion
    }
}
