using System;
using System.Collections;
using System.Collections.Generic;
using Avatar;
using Ingame;
using Manager;
using UnityEngine;


namespace Progetile
{
    public class Progetile_Particle : MonoBehaviour
    {
        [SerializeField]
        private GameObject particle;
        
        private bool effect_active;
        public Transform Particle_Pool, Particle_Activate, particle_assign;
        private void Awake()
        {
            //Step 1. 공의 인덱스를 불러옴
            var ballData = new BallDAO();
            var index = ballData.Get_BallEquipped_Data();
            if (Determine_Effect(index))
            {
                particle = Resources.Load("Ball/particle/" + Calc_Index.Get_Ball_Num(index).ToString()) as GameObject;   // 파티클 파일 불러오기 
                effect_active = true;
                Set_Particle_Pool();
            }
        }

        public void Set_Particle_Pool()
        {
            GameObject obj;
            for (int i = 0; i < 100; i++)
            {
                obj = Instantiate(particle);
                obj.transform.SetParent(Particle_Pool);
                obj.GetComponent<Destroy_Particle_Pooling>().Object_Pool = particle_assign;
                obj.GetComponent<Destroy_Particle_Pooling>().Activating_pool = Particle_Activate;
            }
        }

        public bool Get_Is_Effective()
        {
            return effect_active;
        }
        
        /// <summary>
        /// 피격시 파티클을 오브젝트 풀링 해주는 함수 
        /// </summary>
        public void Make_Particle( ref GameObject[] particle_pool)
        {
            GameObject Obj;
            if (!effect_active)
                return;

            else
            {
                for (int i = 0; i < particle_pool.Length; i++)
                {
                    switch (Particle_Pool.childCount)
                    {
                        default:
                            Obj = Particle_Pool.GetChild(0).gameObject;
                            break;
                    
                        case 0:
                            Obj = Instantiate(particle);
                            Obj.transform.SetParent(Particle_Pool);
                            Obj.GetComponent<Destroy_Particle_Pooling>().Object_Pool = particle_assign;
                            Obj.GetComponent<Destroy_Particle_Pooling>().Activating_pool = Particle_Activate;
                            break;
                    }
                    
                    Obj.transform.SetParent(particle_assign);
                    particle_pool[i] = Obj;
                }

            }
            
        }

        /// <summary>
        /// 특수 효과가 들어가는 공인지 판단하는 함수 
        /// </summary>
        /// <returns></returns>
        private bool Determine_Effect(int index)
        {
            int num_index = Calc_Index.Get_Ball_Num(index);
            bool value;
            switch (num_index)
            {

                default:
                    value = true;
                    break;
                
                case 0:
                case 1000:
                case 1001:
                case 1002:
                case 1003:
                case 1004:
                case 1005:
                case 1006:
                case 1007:
                    value = false;
                    break;
            }

            return value;
        }



    }
}
