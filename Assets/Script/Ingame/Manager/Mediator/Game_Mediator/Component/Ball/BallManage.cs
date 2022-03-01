
using System.Collections;
using Avatar;
using Ingame_Data;
using Progetile;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Ingame
{
    public class BallManage : MonoBehaviour, IComponent
    {
        private IMediator _mediator;

        [Header("Ball_Sprite")]
        public Sprite ballImg;
        public Image preview_Img;
        
        private Vector2 progetile_Pos;
        [SerializeField] private GameObject progetile;

        [Header("PlusBall")] 
        [SerializeField] private Transform plusPool;
        [SerializeField] private Transform plusActive;

        [Header("ProjetilePool")]
        [SerializeField] private Transform progetile_Pool;
        [SerializeField] private Transform progetileGroup;
        [SerializeField] private Transform flightGroup;

        [Header("Ballnum_info")] private int targetNum;
        private int count_num = 0;

        public GameObject tap_btn;

        [SerializeField] private Transform charater;
        
        private const float Ground_Y = -701.02f;
        private void Awake()
        {
            BallDAO data = new BallDAO();
            int ball_num  = data.Get_BallEquipped_Data(); // 장착한 공의 index를 불러옴 
            ballImg = Set_Avatar_UI.Set_Ball_Img(ball_num); // #1. ball 이미지 불러옴 
            preview_Img.sprite = ballImg;
            // #2. plusball 풀 이미지 바꿔서 채우기 
            for (int i = 0; i < plusPool.childCount; i++)
                plusPool.GetChild(i).gameObject.GetComponent<Image>().sprite = ballImg;
            
            // #3. 파티클 활성 유무 체크하기 
        }

        #region Init_Data

        /// <summary>
        /// 처음 게임 시작시 호출되는 함수 
        /// </summary>
        private void Init_Data()
        {
            progetile_Pos = new Vector2(0, Ground_Y );
            // 새로운공을 생성해냄 
            if(progetileGroup.childCount== 0)
                Make_new_Ball(1); 
        }
        
        #endregion

        #region Projetile_Pooling
        private GameObject pooling_progetile()
        {
            GameObject Obj;
            if (progetile_Pool.childCount > 1)
            {
                Obj = progetile_Pool.GetChild(0).gameObject;
                Obj.gameObject.SetActive(true);
                Obj.transform.position = progetile_Pos;
            }

            else
            {
                Obj = Instantiate(progetile, progetile_Pos, Quaternion.identity);
                Obj.GetComponent<SpriteRenderer>().sprite = ballImg;
            }

            return Obj;
        }
        

        #endregion

        #region PlusBall_MOve
        
        private IEnumerator PlusMove(Transform TR)
        {
            while (true)
            {
                TR.position = Vector2.MoveTowards(TR.position, progetile_Pos, 3000f * Time.deltaTime);
                if (progetile_Pos == (Vector2)TR.position)
                    break;
                yield return null;
            }

            TR.DOMove(progetile_Pos,0.4f).SetEase(Ease.InExpo);
            TR.DOKill();
            TR.gameObject.SetActive(false);
            TR.gameObject.GetComponent<Animator>().enabled = true;
            TR.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            TR.GetChild(0).gameObject.SetActive(true);
        }
        
        private IEnumerator plusball_move()
        {
            int target_num = plusActive.childCount;
            Transform tr;
            for (int i = 0; i < target_num; i++)
            {
                tr = plusActive.GetChild(0);
                tr.SetParent(plusPool);
                tr.SetAsLastSibling();
                StartCoroutine(PlusMove(tr));
            }
            yield return null;
        }
        #endregion
        
        // 플러스볼 이동하는 액션이 나와야함
        public void Set_Pos(Vector2 Pos)
        {
            this.progetile_Pos = Pos;
        }
        /// <summary>
        /// 박스가 내려올 때 플러스볼 먹는 액션 취함. 이 때, 내 공으로 만들어줌 
        /// </summary>
        private void Make_new_Ball(int num)
        {
            GameObject Obj;
            for (int i = 0; i <num ; i++)
            {
                Obj = pooling_progetile();
                Obj.GetComponent<SpriteRenderer>().sprite = ballImg; // 원래 컴포넌트 = ball
                Obj.transform.SetParent(progetileGroup);
                Obj.gameObject.layer = 3;
            }
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                // 스폰시 플러스볼을 만들어줌 
                case Event_num.BOX_SPAWN:
                    Make_new_Ball(plusActive.childCount);
                    StartCoroutine(plusball_move());
                    break;
                
                case Event_num.INIT_DATA:
                    Init_Data();
                    break;
                
                case Event_num.SET_LAUNCH_INFO:
                    targetNum = progetileGroup.childCount;
                    count_num = 0;
                    break;
                
                case Event_num.Tutorial_First:
                    if(flightGroup.childCount!=0)
                        Destroy(flightGroup.GetChild(0).gameObject);
                    Init_Data();
                    break;
            }
            
        }

        public void Ball_Land()
        {
            ++count_num;
            if (count_num == targetNum)
            {
                Transform tr;
                int flight_num = flightGroup.childCount;
                for (int i = 0; i < flight_num; i++)
                {
                    tr =  flightGroup.GetChild(0);
                    tr.SetParent(progetileGroup);
                }
                tap_btn.SetActive(false);
                _mediator.Event_Receive(Event_num.BOX_SPAWN);
            }
        }

        public BallInfo_VO Set_BallInfo()
        {
            BallInfo_VO data = new BallInfo_VO();
            data.ballNum = progetileGroup.childCount;
            data.ballPos = (Vector2)progetile_Pos;
            data.charPos = charater.position;
            if (charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX)
                data.is_Fliped = true;

            else
                data.is_Fliped = false;
            
            return data;
        }
    }
}
