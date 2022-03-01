using System.Collections;
using System.Collections.Generic;
using Badge;
using Ingame;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class LaunchManage : MonoBehaviour, IComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
{
        private IMediator _mediator;
        [Header("Launch_Info")]
        private bool launch_possible;
        private Vector2 dir, drag_Start_Pos, dragging_Pos;
        private bool cancleDrag, startDrag;
        private Vector2 progetilePos;
        private IEnumerator launch_ienum;
        private int ball_count;

        private bool abort_launch, is_launching;
        
        [Header("Launch_Power")] 
        public float POWER;
        private const float Ground_Y = -701.02f;

        [Header("Item")] 
        private bool _Item;
        public Button item_inactive_btn;
    
        [SerializeField] private Transform Charater;
        public Transform _Launch_Preview_Ball, plusActive;
        
        [Header("Aim")]
        RaycastHit2D hit, reflect_hit;
        private Vector2 reflect_Vec, incomingVec, Determine_vector;
        public Text damage_text;
        public Line_Animation _LineAnimation;
        public Image drag_img;
        private int count = 0;

        [Header("Object_TR")] 
        [SerializeField] private Transform progetile_group;
        [SerializeField] private Transform flight_group;

        public GameObject tap_Btn;
        
        private const float inchToCm = 2.54f;
        private EventSystem eventSystem = null;
        private readonly float dragThresholdCM = 0.4f;
        private int launchCount = 1;
        private bool isPinata;
        
        void Awake()
        {
            POWER =  (1f+((AbilityDAO.Get_Ball_Speed())/100f))*3600f ; // Ball speed 초기화 

            if (eventSystem == null)
            {
                eventSystem = GetComponent<EventSystem>();

            }

            SetDragThreshold();
        }
        
        private void SetDragThreshold()
        {
            if (eventSystem != null)
            {
                eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
            }
        }
        
        #region Mediate_Action
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.Launch_Green:
                    launch_possible = true;
                    damage_text.text = string.Format("{0:#,0}", progetile_group.childCount);
                    damage_text.gameObject.SetActive(true);
                    drag_img.enabled = true;
                    break;
                
                case Event_num.Launch_Red:
                    launch_possible = false;
                    damage_text.gameObject.SetActive(false);
                    break;
                
                case Event_num.INIT_DATA:
                    progetilePos = new Vector3(0f, Ground_Y, 0f);
                    break;
                
                case Event_num.Abort_Launch:
                    Abort_Launch();
                    break;

                case Event_num.Launch_MOTION:
                    drag_img.enabled = false;
                    StartCoroutine(launch_ienum);
                    break;
                
                case Event_num.Abort_Launch_PINATA:
                    isPinata = true;
                    Abort_Launch();
                    break;
                
               case  Event_num.Tutorial_Basic_Done:
                   progetilePos = new Vector3(0f, Ground_Y, 0f);
                   damage_text.transform.position =  new Vector3((Mathf.Abs(progetilePos.x)>463f ?  (progetilePos.x<-463f ? -463f:463f):progetilePos.x),progetilePos.y-70f, 0f);
                   break;
            }
        }
    
        #endregion
        
        #region Dragging
            

            // 드래그를 시작했을 때, 처음 터치한 지점의 좌표를 저장해줌 
            public void OnBeginDrag(PointerEventData eventData)
            {
                if (!launch_possible)
                    return;

                drag_Start_Pos = progetilePos;
                _LineAnimation.Set_Start_Pos(progetilePos);
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
                    
                    hit = Physics2D.CircleCast(progetilePos, 24.01f,Determine_vector, 10000,
                        1 << LayerMask.NameToLayer("Wall") | 1<< LayerMask.NameToLayer("Object")|1<< LayerMask.NameToLayer("Pinata"));
                    if (!_Item)
                    {
                        _Launch_Preview_Ball.position = hit.centroid;
                        _LineAnimation.Set_Line_Pos(hit.centroid,hit.centroid);
                    }

                    else
                    {
                        incomingVec = hit.centroid - (Vector2)progetilePos;
                        reflect_Vec = Vector2.Reflect(incomingVec,hit.normal);
                        reflect_hit = Physics2D.CircleCast(hit.centroid, 23.99f,reflect_Vec, 10000,
                            1 << LayerMask.NameToLayer("Wall") | 1<< LayerMask.NameToLayer("Object") | 1<< LayerMask.NameToLayer("Pinata"));
                        _LineAnimation.Set_Line_Pos(hit.centroid,reflect_hit.centroid);
                        _Launch_Preview_Ball.position = reflect_hit.centroid;
                    }
                    
                   
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
                    _LineAnimation.Remove_Line();
                    Launch_Progetile();// GameManager의 Launch 함수 호출 
                }
            }
            
            /// <summary>
            /// 투사체를 발사시키고, State를 투사체 비행상채로 바꾸어주는 action
            /// </summary>
            private void Launch_Progetile()
            {
                launch_ienum = Launch();
                launch_possible = false; // 발사 가능 조건 판단 false
                _Launch_Preview_Ball.gameObject.SetActive(false); // 숨겨진 버튼 활성화 
                item_inactive_btn.gameObject.SetActive(true);
                _mediator.Event_Receive(Event_num.SET_LAUNCH_INFO);
                Charater.gameObject.GetComponent<Animator>().SetTrigger("Launch");
                // 발사 취소 활성화 
            }

            private IEnumerator Launch()
            {
                int ball_num = progetile_group.childCount;
                int sum = ball_num;
                Transform TR;
                is_launching = true;
                Vector3 pos = (Vector3) dir * POWER * (1+ ((float)launchCount/400f));
                tap_Btn.SetActive(true);
                if(launchCount < 150)
                    launchCount++;
                for (int i = 0; i < ball_num; i++)
                {
                    if (abort_launch)
                    {
                        launch_ienum = null;
                        is_launching = false;
                        damage_text.gameObject.SetActive(false);
                        Set_ALL_GROUND();
                        yield break;
                    }

                    TR = progetile_group.GetChild(0);
                    Rigidbody2D rigid = TR.GetComponent<Rigidbody2D>();
                    rigid.freezeRotation = false;
                    rigid.AddForce(pos);
                    rigid.AddTorque(3000f);
                    TR.GetComponent<CircleCollider2D>().enabled = true;
                    TR.SetParent(flight_group);
                    --sum;
                    damage_text.text = string.Format("{0:#,0}", sum);
                    yield return new WaitForSeconds(0.08f);
                }
                
                damage_text.gameObject.SetActive(false);
                is_launching = false;
                launch_ienum = null;
                if (abort_launch)
                {
                    Set_ALL_GROUND();
                    yield break;
                }
                yield return null;
            }
            

        #endregion

        #region Action

        public void Set_Item()
        {
            if (!_Item)
                _Item = true;
            
            else
                _Item = false;
        }
        private void Abort_Launch()
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
            tap_Btn.SetActive(false);
            StartCoroutine(Down_Ball());
        }

        public void Set_Pos(Vector2 Pos)
        {
            this.progetilePos = Pos;
            damage_text.transform.position = new Vector3((Mathf.Abs(progetilePos.x)>463f ?  (progetilePos.x<-463f ? -463f:463f):progetilePos.x),progetilePos.y-70f, 0f);
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
                    //_mediator.Event_Receive(Event_num.BALL_DOWN);
                    break;
                }

                yield return null;
            }
            
            for (int i = 0; i < progetile_group.childCount; i++)
            {
                tr = progetile_group.GetChild(i);
                tr.position = progetilePos;
            }
            if(!isPinata)
                _mediator.Event_Receive(Event_num.BOX_SPAWN);

            this.isPinata = false;
            abort_launch = false;
        }
        

        IEnumerator Down_Ball_Single(Transform TR)
        {
            int speed = 4000;
            Vector2 target_pos = new Vector2(TR.position.x, Ground_Y);
            TR.DOMove(progetilePos, 0.3f)
                .SetEase(Ease.InQuart)
                .OnComplete(()=>
                {
                    count++;
                    TR.DOKill(true);
                });

            yield return null;


        }
        #endregion
    
}
