
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Skill
{
    public class Avatar_3002_Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        
        [SerializeField] private Avatar_3002_Skill skill;
        [Header("Sensivity_Set")]
        private const float inchToCm = 2.54f;
        private EventSystem eventSystem = null;
        private readonly float dragThresholdCM = 0.4f;
        private Vector2 before_pos, now_pos;

        [Header("Data")] private bool select_exit;

        [Header("UI")]
        public Transform selectTR;
        private bool is_left;
        public Animator grid_animator;
        private Vector2 touchPos;
        private int row, col;
        private void Awake()
        {
            SetDragThreshold();
        }
        
        private void SetDragThreshold()
        {
            if (eventSystem == null)
            {
                eventSystem = GetComponent<EventSystem>();
            }
            if (eventSystem != null)
            {
                eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!select_exit)
            {
                if (eventData.pointerCurrentRaycast.worldPosition.y < -659f)
                    return;
            }

            touchPos = new Vector2(eventData.pointerCurrentRaycast.worldPosition.x,
                eventData.pointerCurrentRaycast.worldPosition.y - 60f);
            selectTR.position = touchPos;
            Set_Grid_TR_Pos (touchPos);
            before_pos = touchPos;
            select_exit = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!select_exit)
                return;
            
            touchPos = new Vector2(eventData.pointerCurrentRaycast.worldPosition.x,
                eventData.pointerCurrentRaycast.worldPosition.y + 200f);
            now_pos = touchPos;
            selectTR.position = touchPos;
            before_pos = now_pos;
            Set_Grid_TR_Pos(touchPos);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!select_exit)
            {
                grid_animator.SetTrigger("idle");
            }
            skill.Set_Dir(is_left);
            skill.Set_Animation();
            select_exit = false;
        }


        private void Set_Grid_TR_Pos(Vector2 pos)
        {
            if (pos.y < -630f)
            {
                select_exit = false;
                return;
            }

            _Determine_Pos. Calc_Which_Grid_Skill(ref row, ref col, pos);
            if (col < 3)
            {
                if (!is_left)
                {
                    grid_animator.SetTrigger("left");
                    is_left = true;
                }
            }
                
            else
            {
                if (is_left)
                {
                    grid_animator.SetTrigger("right");
                    is_left = false;
                }
            }
        }
    }
}
