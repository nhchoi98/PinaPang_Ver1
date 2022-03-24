using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Ingame
{
    /// <summary>
    /// 공 발사시 배속상태를 계산해주는 함수 
    /// </summary>
    public class Speed_Control : MonoBehaviour, IComponent//, IPointerDownHandler
    {
        [Header("Game_Condition")]
        private IMediator _mediator;
        private IEnumerator double_speed;
        [SerializeField] private Determine_BoxType stage_info;
        
        #region DoubleSpeed
        /// <summary>
        /// 배속이 걸릴 때 까지의 시간을 stage를 기준으로 계산해 실행해주는 반복자. 
        /// </summary>
        /// <returns></returns>
        IEnumerator Determine_Double_Speed()
        {
            float target_time = this.target_time();
            yield return new WaitForSeconds(target_time);
            Time.timeScale = 2f;
            double_speed = null;

        }
        
        /// <summary>
        /// 배속이 걸릴 때 까지의 시간을 수식에 맞추어 계산해 값을 리턴해주는 함수. 
        /// </summary>
        /// <returns></returns>
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
                // 발사를 했다면, 2배속 관리를 시작함
                case Event_num.Launch_MOTION:
                    double_speed = Determine_Double_Speed();
                    StartCoroutine(double_speed);
                    break;
                
                // 박스를 생성하는 과정일 경우, 배속을 풀어줌 
                case Event_num.BOX_SPAWN:
                    Time.timeScale = 1f;
                    if (double_speed != null)
                        StopCoroutine(double_speed);

                    double_speed = null;
                    // 이뉴머레이터 전부 비활성화, 정보 초기화 
                    break;
                
                // 발사 중지시 배속 시스템을 초기화 해주기 위해 호출되는 이벤트. 
                case Event_num.Abort_Launch:
                    Time.timeScale = 1f;
                    if(double_speed!=null)
                        StopCoroutine(double_speed);
                    double_speed = null;
                    break;
                
                // 볼을 내리는 이벤트가 발생했을 경우, 배속을 풀어줌 
                case Event_num.BALL_DOWN:
                    Time.timeScale = 1f;
                    if(double_speed!=null)
                        StopCoroutine(double_speed);
                    double_speed = null;
                    break;
                
                // 피냐타가 죽으면. 배속을 풀어줌 
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

