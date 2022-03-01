using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Ingame
{
    public class Rocket : MonoBehaviour
    {
        public IDestroy_Action target;
        public Transform targetPos;
        public event EventHandler rocket_event;

        private bool is_destroyed = false;

        [Header("Projetile_Data")] 
        private float y0;
        private float y1;
        private float distance;
        private float nextY;
        private float baseX;
        private float arc;
        public float m_Speed;
        public float m_HeightArc;
        public float arc_const;
        private Vector3 m_StartPosition;
        private Vector3 nextPosition;
        
        public GameObject destroy_particle;
        
        void OnEnable()
        {
            target = targetPos.gameObject.GetComponent<IDestroy_Action>();
            m_StartPosition = transform.position;
            StartCoroutine(Flight());
        }

        IEnumerator Flight()
        {
            object o = new object();
            EventArgs arge = new EventArgs();
            // Step 2. 위치와 target pos 계속 검사  --> 도달하면 펑!  + 이벤트 전달 
            while (true)
            {
                if ((targetPos.position.y - transform.position.y) < 0.05f)
                {
                    Instantiate(destroy_particle, transform.position, Quaternion.identity);
                    break;

                }

                yield return null;
            }
            
            target.Destroy_Action();
            rocket_event.Invoke(o,arge);
            Destroy(this.gameObject);
        }

        private void Update()
        {
            if (is_destroyed)
                return;
            
            
            y0 = m_StartPosition.y;
            y1 = targetPos.position.y;
            distance = y1 - y0;
            nextY = Mathf.MoveTowards(transform.position.y, y1, m_Speed * Time.unscaledDeltaTime);
            baseX = Mathf.Lerp(m_StartPosition.x, targetPos.position.x, (nextY - y0) / distance);
            arc = m_HeightArc * (nextY - y0) * (nextY - y1) / (arc_const * distance * distance);
            nextPosition = new Vector3(baseX+arc, nextY, transform.position.z);
            transform.rotation = LookAt2D(transform.position-nextPosition);
            transform.position = nextPosition;
        }
        
        Quaternion LookAt2D(Vector2 forward)
        {
            return Quaternion.Euler(0, 0, (Mathf.Atan2(forward.y, forward.x)+(Mathf.PI/2)) * Mathf.Rad2Deg);
        }
    }
}
