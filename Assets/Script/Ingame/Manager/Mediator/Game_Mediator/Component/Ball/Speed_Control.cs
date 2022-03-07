using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Ingame
{
    public class Speed_Control : MonoBehaviour, IComponent//, IPointerDownHandler
    {
        [Header("Game_Condition")]
        private IMediator _mediator;
        private IEnumerator double_speed;
        [SerializeField] private Determine_BoxType stage_info;
        
        #region DoubleSpeed
        IEnumerator Determine_Double_Speed()
        {
            float target_time = this.target_time();
            yield return new WaitForSeconds(target_time);
            Time.timeScale = 2f;
            double_speed = null;

        }
        
        private float target_time()
        {
            float stage_value = (float)stage_info.Get_Stage();
            float value;
            if (stage_value<146f)
                value = ((stage_value - 50f) * (stage_value - 200f) * 7f / 9751f) +
                        ((stage_value - 200f) * (stage_value - 1f) * 5.5f / -7350f)
                        + ((stage_value - 1f) * (stage_value - 50f) * 4f / 29850f);

            else
                return 4.0f;
            
            return value;

        }
        #endregion
        
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.Launch_MOTION:
                    double_speed = Determine_Double_Speed();
                    StartCoroutine(double_speed);
                    break;
                
                case Event_num.BOX_SPAWN:
                    Time.timeScale = 1f;
                    if (double_speed != null)
                        StopCoroutine(double_speed);

                    double_speed = null;
                    // 이뉴머레이터 전부 비활성화, 정보 초기화 
                    break;
                
                case Event_num.Abort_Launch:
                    Time.timeScale = 1f;
                    if(double_speed!=null)
                        StopCoroutine(double_speed);
                    double_speed = null;
                    break;
                
                case Event_num.BALL_DOWN:
                    Time.timeScale = 1f;
                    if(double_speed!=null)
                        StopCoroutine(double_speed);
                    double_speed = null;
                    break;
                
                case Event_num.PINATA_DIE:
                    Time.timeScale = 1f;
                    if(double_speed!=null)
                        StopCoroutine(double_speed);
                    
                    double_speed = null;
                    break;
            }
        }
        
        
    }
}

