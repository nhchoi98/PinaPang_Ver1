using System.Collections;
using System.Collections.Generic;
using Badge;
using Ingame;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 발사 시작, 발사 중지, 발사 시 공의 속도를 조절해주는 스크립트. 
/// </summary>
public class LaunchManage : MonoBehaviour, IComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
{
        private IMediator _mediator;
        [Header("Launch_Info")]
        private bool launch_possible; // 발사를 할 수 있는 상태인지를 저장하는 booltype 변수 
        private Vector2 dir, drag_Start_Pos, dragging_Pos;
        private bool cancleDrag, startDrag;
        private Vector2 progetilePos;
        private IEnumerator launch_ienum;
        private int ball_count;
        private const float min_launchAngle = 0.2f;

        private bool abort_launch, is_launching;
        
        [Header("Launch_Power")] 
        public float POWER;
        private const float Ground_Y = -701.02f;

        [Header("Item")] 
        private bool _Item;
        public Button item_inactive_btn;
        private float speedconst = 1f;
    
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
            POWER =  (1f+((AbilityDAO.Get_Ball_Speed())/100f))*(3600f) ; // Ball speed 초기화 

            if (eventSystem == null)
            {
                eventSystem = GetComponent<EventSystem>();

            }

            SetDragThreshold();
        }
        
        /// <summary>
        /// 해상도에 맞게 민감도를 조정해주는 함수. 
        /// </summary>
        private void SetDragThreshold()
        {
            if (eventSystem != null)
            {
                eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
            }
        }

        /// <summary>
        /// 1.5배속 아이템이 실행되면, 볼 스피드와 관련된 상수를 조정해줌. 
        /// </summary>
        /// <param name="is_Activating"></param>
        public void Set_BallSpeed_Const(bool is_Activating)
        {
            if (is_Activating)
                speedconst = 1.5f;

            else
                speedconst = 1f;

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
                // 발사 가능상태로 만들어줌 
                case Event_num.Launch_Green:
                    launch_possible = true;
                    damage_text.text = string.Format("{0:#,0}", progetile_group.childCount);
                    damage_text.gameObject.SetActive(true);
                    drag_img.enabled = true;
                    break;
                
                // 발사 불능 상태로 만들어줌.
                case Event_num.Launch_Red:
                    launch_possible = false;
                    damage_text.gameObject.SetActive(false);
                    break;
                
                // 처음 게임에 들어오게 되면 (이어하기 아닌 경우), 발사 위치를 정중앙으로 초기화. 
                case Event_num.INIT_DATA:
                    progetilePos = new Vector3(0f, Ground_Y, 0f);
                    break;
                
                // 발사 중지 이벤트 발생시 호출됨 
                case Event_num.Abort_Launch:
                    Abort_Launch();
                    break;

                // 캐릭터가 발사 모션을 취하게 되면, 타이밍에 맞게 호출되어 실제로 공을 발사시켜주는 함수 
                case Event_num.Launch_MOTION:
                    drag_img.enabled = false;
                    StartCoroutine(launch_ienum);
                    break;
                
                // 피냐타가 파괴되었을 때, 공을 다 자동으로 내리기 위해서 호출되는 함수 
                case Event_num.Abort_Launch_PINATA:
                    isPinata = true;
                    Abort_Launch();
                    break;
                
                // 인게임 튜토리얼이 끝나는 이벤트가 발생하면 호출  
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
                if (!launch_possible) // 발사를 할 수 없는 조건이라면 드래깅을 시켜주지 않음 
                    return;

                drag_Start_Pos = progetilePos;
                _LineAnimation.Set_Start_Pos(progetilePos); // 라인을 그려줌 
            }

            // 드래그를 계속 하면 LineRenderer의 좌표를 바꾸어줌 
            public void OnDrag(PointerEventData eventData)
            {
                if (!launch_possible)
                    return;
                
                startDrag = true;
                
                dragging_Pos = eventData.pointerCurrentRaycast.worldPosition;
                Determine_vector = dragging_Pos - drag_Start_Pos; // 방향 벡터를 계산해줌. 방향벡터의 첫 시작점은 공에서 부터임 
                
                
                
                if ( Determine_vector.normalized.y < -min_launchAngle) // 만약에 사용자가 기준선 밑으로 드래깅 했다면.. 
                {
                    cancleDrag = true;
                    // 선이 사라지는 애니메이션 
                    _LineAnimation.Remove_Line();
                    return;
                }

                else // 아니라면 
                {
                    cancleDrag = false;
                    if (Determine_vector.normalized.y < min_launchAngle) // 기준선 밑에서 드래깅을 시작했다면..
                        return; // 선을 그려주지 않음 
                    
                    hit = Physics2D.CircleCast(progetilePos, 24.01f,Determine_vector, 10000,
                        1 << LayerMask.NameToLayer("Wall") | 1<< LayerMask.NameToLayer("Object")|1<< LayerMask.NameToLayer("Pinata")); //벽, 오브젝트, 피냐타 layer에만 충돌을 감지함 
                    
                    if (!_Item) // 꺾임 아이템을 쓰지 않았을 경우 
                    {
                        _Launch_Preview_Ball.position = hit.centroid;
                        _LineAnimation.Set_Line_Pos(hit.centroid,hit.centroid); // 선을 한 개만 만들어줌 
                    }

                    else // 아이템을 썼다면 
                    {
                        incomingVec = hit.centroid - (Vector2)progetilePos; 
                        reflect_Vec = Vector2.Reflect(incomingVec,hit.normal); // 반사되는 벡터를 계산함 
                        reflect_hit = Physics2D.CircleCast(hit.centroid, 23.99f,reflect_Vec, 10000,
                            1 << LayerMask.NameToLayer("Wall") | 1<< LayerMask.NameToLayer("Object") | 1<< LayerMask.NameToLayer("Pinata"));
                        _LineAnimation.Set_Line_Pos(hit.centroid,reflect_hit.centroid); // 선을 두 개 그려줌 
                        _Launch_Preview_Ball.position = reflect_hit.centroid; // 한 번 꺾여서 충돌이 예상되는 위치에 미리보기 공을 그려줌 
                    }
                    
                   
                    if(!_Launch_Preview_Ball.gameObject.activeSelf)
                        _Launch_Preview_Ball.gameObject.SetActive(true);
                }
                
            }
            
            public void OnEndDrag(PointerEventData eventData)
            {
                if (cancleDrag || !launch_possible)
                    return;

                else // 발사 불가능 조건이라면.. 
                {
                    dir = (dragging_Pos - drag_Start_Pos).normalized;
                    
                    if (dir.y < min_launchAngle) // 만약 사용자가 특정 각도 밑으로 드래깅 후 발사했다면 
                        dir = new Vector2(dir.x, min_launchAngle); // 특정 각도를 기준 으로 발사 
                    _LineAnimation.Remove_Line(); // 발사선을 지워줌 
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
                _Launch_Preview_Ball.gameObject.SetActive(false); // 레이저 끝에 보이는 공 
                item_inactive_btn.gameObject.SetActive(true); // 아이템 나중에 눌러주세요~ 하고 패널띄워주는 버튼이 켜기게함 
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
                Vector3 pos = (Vector3) dir * POWER * speedconst*(1+ ((float)launchCount/400f)); // 발사 속도를 조절해주는 변수 
                tap_Btn.SetActive(true);
                if(launchCount < 150)
                    launchCount++;
                
                for (int i = 0; i < ball_num; i++)
                {
                    if (abort_launch) // 발사 중지 명령이 들어오면 
                    {
                        launch_ienum = null;
                        is_launching = false;
                        damage_text.gameObject.SetActive(false); // 공 개수 텍스트를 꺼줌 
                        Set_ALL_GROUND(); // 공을 모두 내려줌 
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

        /// <summary>
        /// 발사 중지 버튼 눌렀을 때 공을 땅바닥으로 이동시켜주는 함수 
        /// </summary>
        private void Set_ALL_GROUND()
        {
            tap_Btn.SetActive(false);
            StartCoroutine(Down_Ball());
        }

        /// <summary>
        /// 공 발사 가능 개수 텍스트의 위치를 잡아주는 함수.
        /// </summary>
        /// <param name="Pos"></param>
        public void Set_Pos(Vector2 Pos)
        {
            this.progetilePos = Pos;
            damage_text.transform.position = new Vector3((Mathf.Abs(progetilePos.x)>463f ?  (progetilePos.x<-463f ? -463f:463f):progetilePos.x),progetilePos.y-70f, 0f);
        }

        /// <summary>
        /// 발사 중지 명령시, 공을 특정 위치로 이동시켜주는 반복자. 
        /// </summary>
        /// <returns></returns>
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
                if (count == target_num) // 공이 다 내려왔다면
                {
                    count = 0; // 갯수 세는 변수 초기화 
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
            
            if(!isPinata) // 피냐타를 소환하는 때가 아니라면
                _mediator.Event_Receive(Event_num.BOX_SPAWN); // 바로 박스를 소환하는 이벤트를 호출함 

            this.isPinata = false;
            abort_launch = false;
        }
        

        IEnumerator Down_Ball_Single(Transform TR)
        {
            int speed = 4000;
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
