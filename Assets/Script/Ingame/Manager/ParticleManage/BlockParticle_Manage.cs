using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Particle
{
    /// <summary>
    /// 블록 파티클의 particle pooling을 담당하는 class 
    /// </summary>
    public class BlockParticle_Manage: MonoBehaviour
    {
        [SerializeField] private Transform particle_pool;
        [SerializeField] private Transform returnPool;
        [SerializeField] private Transform waitPool;
        [SerializeField] private GameObject particle;
        
        /// <summary>
        /// Particle 요청이 들어오면 이를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public Transform Set_Particle()
        {
            Transform tr;
            if (particle_pool.childCount == 0)
            {
                Destroy_Particle_Pooling info;
                tr = Instantiate(particle).transform;
                info = tr.gameObject.GetComponent<Destroy_Particle_Pooling>();
                info.OnlyDeactivate = true;
                info.Activating_pool = returnPool;
                info.Object_Pool = particle_pool;
                tr.SetParent(waitPool);
            }

            else
                tr = particle_pool.GetChild(0);

            
            tr.SetParent(waitPool);
            return tr;
        }

    }
}
