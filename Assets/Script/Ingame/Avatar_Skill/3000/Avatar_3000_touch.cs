using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ingame
{
    public class Avatar_3000_touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private bool grid_set;
        [Header("Grid_UI")] 
        public Transform gridTR;
        public Transform selectTR;
        public Animator grid_animator;
        
        [Header("Sensivity_Set")]
        private const float inchToCm = 2.54f;
        private EventSystem eventSystem = null;
        private readonly float dragThresholdCM = 0.4f;
        private Vector2 before_pos, now_pos;
        [SerializeField] private Avatar_3000_Skill skill;

        private int row, col;
        private void Awake()
        {
            SetDragThreshold();
        }

        private void OnEnable()
        {
            grid_set = false;
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
            if (!grid_set)
            {
                _Determine_Pos.Calc_Which_Grid(ref row, ref col, eventData.pointerCurrentRaycast.worldPosition, true);
                if (row-1 < 1 && row-1 >7)
                    return;
                
                grid_set = true;
                gridTR.gameObject.SetActive(true);
                if (skill.Get_Target_List(row, col))
                    grid_animator.SetBool("activating",true);
            
            
                else
                    grid_animator.SetBool("activating",false);

                before_pos = eventData.position;
                gridTR.position = _Determine_Pos.Which_Pos(row-1, col);
                selectTR.position = eventData.pointerCurrentRaycast.worldPosition;
                
                if (skill.Get_Target_List(row, col))
                {

                    grid_animator.SetBool("activating", true);
                    grid_animator.SetTrigger("activating_tri");
                
                }

                else
                {
                    grid_animator.SetBool("activating", false);
                    grid_animator.SetTrigger("deactivating_tri");
                    
                }
            }
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!grid_set)
                return;

            now_pos = eventData.position;
            _Determine_Pos.Calc_Which_Grid(ref row, ref col, eventData.pointerCurrentRaycast.worldPosition,true);
            Debug.Log("row:" + row + ", col: " + col);
            
            before_pos = now_pos;
            
            if (row-1 < 1 || row-1 > 7)
                return;

            if (skill.Get_Target_List(row-1, col))
            {
                if (!grid_animator.GetBool("activating"))
                {
                    grid_animator.SetBool("activating", true);
                    grid_animator.SetTrigger("activating_tri");
                }
            }

            else
            {
                if (grid_animator.GetBool("activating"))
                {
                    grid_animator.SetBool("activating", false);
                    grid_animator.SetTrigger("deactivating_tri");
                }
            }

            gridTR.position = _Determine_Pos.Which_Pos(row-1, col);
            selectTR.position = eventData.pointerCurrentRaycast.worldPosition;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (row-1 < 1 || row-1 > 7)
            {
                grid_set = false;
                gridTR.gameObject.SetActive(false);
                return;

            }


            if (skill.Get_Target_List(row-1,col)) // 놓은 위치를 따라서 TRUE, FALSE여부로 스킬 발동 결정 
            {
                gridTR.gameObject.SetActive(false);
                skill.Set_rowcol_data(row-1,col);
                skill.Activate();
            }

            else
            {
                grid_set = false;
                gridTR.gameObject.SetActive(false);
            }
        }
    }
}
