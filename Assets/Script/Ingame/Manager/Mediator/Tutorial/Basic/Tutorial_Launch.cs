using System;
using System.Collections;
using System.Collections.Generic;
using Collider;
using UnityEngine;
using UnityEngine.UI;
using Ingame;
using Manager;
using UnityEngine.EventSystems;

namespace Tutorial
{
    public class Tutorial_Launch : MonoBehaviour, IComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private IMediator _mediator;
        [Header("Launch_Info")] private bool launch_possible = true;
        private Vector2 dir, drag_Start_Pos, dragging_Pos;
        private bool cancleDrag, startDrag;
        public bool Launch_Method; // 발사 방법 관련 flag
        private Vector2 progetilePos;
        private int ball_count;

        private bool abort_launch, is_launching;
        [SerializeField] private Transform charater;
        private IEnumerator launch_ienum;

        [Header("Launch_Power")] public float POWER;
        private const float Ground_Y = -693.7f;
        public Transform _Launch_Preview_Ball;

        [Header("Aim")] RaycastHit2D hit, reflect_hit;
        private Vector2 reflect_Vec, incomingVec, Determine_vector;
        public Text damage_text;
        public Line_Animation _LineAnimation;

        [Header("Object_TR")] [SerializeField] private Transform progetile_group;
        [SerializeField] private Transform flight_group;

        [Header("Mediator")] [SerializeField] private Tutorial_Basic tutorial_controller;
        public GameObject background_tutorial;
        public GameObject textbox;
        public GameObject launch_preview;
        public GameObject double_touch_preview;
        
        public Animator first_stage_animation;
        [SerializeField] private Ground ground;
        public Transform triangle;
        private int stage;
        private bool launch_action = false;
        int count = 0;

        public GameObject tapBtn;

        void Awake()
        {
            progetilePos = new Vector2(0f, -693.7f);
            POWER = 3300f; // Ball speed 초기화 
        }

        #region Mediate_Action

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
        }

        public void Event_Occur(Event_num eventNum)
        {

        }

        #endregion
        
        #region Dragging
        // 드래그를 시작했을 때, 처음 터치한 지점의 좌표를 저장해줌 
        public void OnBeginDrag(PointerEventData eventData)
        {
            drag_Start_Pos = progetilePos;
            _LineAnimation.Set_Start_Pos(progetilePos);
            first_stage_animation.enabled = false;  
            launch_preview.SetActive(false);
            background_tutorial.SetActive(false);
            launch_action = true;
        }


        // 드래그를 계속 하면 LineRenderer의 좌표를 바꾸어줌 
        public void OnDrag(PointerEventData eventData)
        {
            
                if (!launch_possible)
                    return;
                
                startDrag = true;
                
                dragging_Pos = eventData.pointerCurrentRaycast.worldPosition;
                Determine_vector = dragging_Pos - drag_Start_Pos;
                
                
                
                if ( Determine_vector.normalized.y < -0.20)
                {
                    cancleDrag = true;
                    // 선이 사라지는 애니메이션 
                    _LineAnimation.Remove_Line();
                    return;
                }

                else
                {
                    cancleDrag = false;
                    if (Determine_vector.normalized.y < 0.20)
                        return;
                    
                    hit = Physics2D.CircleCast(progetilePos, 22.51f,Determine_vector, 10000,
                        1 << LayerMask.NameToLayer("Wall") | 1<< LayerMask.NameToLayer("Object")|1<< LayerMask.NameToLayer("Pinata"));
            
                    _Launch_Preview_Ball.position = hit.centroid;
                    _LineAnimation.Set_Line_Pos(hit.centroid,hit.centroid);
                    
                    if(!_Launch_Preview_Ball.gameObject.activeSelf)
                        _Launch_Preview_Ball.gameObject.SetActive(true);
                }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (cancleDrag || !launch_possible)
                return;

            else
            {
                dir = (dragging_Pos - drag_Start_Pos).normalized;
                    
                if (dir.y < 0.20f)
                    dir = new Vector2(dir.x, 0.20f);
                
                Launch_Progetile();// GameManager의 Launch 함수 호출 
            }
        }
        
        
        /// <summary>
        /// 투사체를 발사시키고, State를 투사체 비행상채로 바꾸어주는 action
        /// </summary>
        private void Launch_Progetile()
        {
            _LineAnimation.Remove_Line();
            launch_possible = false; // 발사 가능 조건 판단 false
            _Launch_Preview_Ball.gameObject.SetActive(false); // 숨겨진 버튼 활성화 
            charater.gameObject.GetComponent<Animator>().SetTrigger("Launch");
            background_tutorial.SetActive(false);
            textbox.gameObject.SetActive(false);
            launch_possible = false;
            launch_preview.SetActive(false);
            Launch_Method = true; // 밀어서 발사 
            launch_action = false;
            StartCoroutine(Launch());
            // 발사 취소 활성화 
        }
        private IEnumerator Launch()
        {
            int ball_num = progetile_group.childCount;
            int sum = ball_num;
            Transform TR;
            is_launching = true;
            yield return new WaitForSeconds(0.2f);
            tapBtn.SetActive(true);
            for (int i = 0; i < ball_num; i++)
            {
                TR = progetile_group.GetChild(0);
                TR.GetComponent<Rigidbody2D>().AddForce(dir * POWER);
                TR.GetComponent<CircleCollider2D>().enabled = true;
                TR.SetParent(flight_group);
                --sum;
                damage_text.text = string.Format("{0:#,0}", sum);
                yield return new WaitForSeconds(0.08f);
            }

            damage_text.gameObject.SetActive(false);
            is_launching = false;
            launch_ienum = null;
            yield return null;
        }
        #endregion

        #region Action
        public void Abort_Launch()
        {
            if (is_launching)
            {
                abort_launch = true;
            }

            else
            {
                Set_ALL_GROUND();
            }
        }

        private void Set_ALL_GROUND()
        {
            int target_num;
            target_num = flight_group.childCount;
            
            tapBtn.SetActive(false);
            StartCoroutine(Down_Ball());
            
        }
        
        IEnumerator Down_Ball()
        {
            Transform tr;
            Vector3 target_rotation = new Vector3(0f, 0f, 0f);
            int target_num = flight_group.childCount;
            for (int i = 0; i < target_num; i++)
            {
                tr = flight_group.GetChild(0);
                tr.SetParent(progetile_group);
                CircleCollider2D collider2D = tr.gameObject.GetComponent<CircleCollider2D>();
                collider2D.enabled = false;
                Rigidbody2D rigid = tr.gameObject.GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
                rigid.freezeRotation = true;
                tr.rotation = Quaternion.Euler(target_rotation);
                StartCoroutine(Down_Ball_Single(tr));
            }

            while (true)
            {
                if (count == target_num)
                {
                    count = 0;
                    break;
                }

                yield return null;
            }
            
            for (int i = 0; i < progetile_group.childCount; i++)
            {
                tr = progetile_group.GetChild(i);
                tr.position = progetilePos;
            }
            ground.Tutorial_Down();
            abort_launch = false;
        }
        
        IEnumerator Down_Ball_Single(Transform TR)
        {
            int speed = 3000;
            Vector2 target_pos = new Vector2(TR.position.x, progetilePos.y);
            while (true)
            {
                TR.position = Vector3.MoveTowards(TR.position, target_pos, speed * Time.deltaTime);
                if (TR.position == (Vector3)target_pos)
                    break;
                
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            while (true)
            {
                TR.position = Vector3.MoveTowards(TR.position, progetilePos, speed * Time.deltaTime);
                if (TR.position == (Vector3)progetilePos)
                    break;
                
                yield return null;
            }

            count++;
        }
        #endregion

    }
}
