
using System.Collections;
using Avatar;
using Ingame_Data;
using Progetile;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Ingame
{
    /// <summary>
    /// 볼의 생성, 플러스볼의 생성, 발사 위치를 관리하는 스크립트.
    /// </summary>
    public class BallManage : MonoBehaviour, IComponent
    {
        private IMediator _mediator;

        [Header("Ball_Sprite")]
        public Sprite ballImg; // 생성할 볼의 이미지를 저장. 
        public Image preview_Img; // 발사선을 생성할 때 생기는 미리보기 볼 이미지./ 
         
        private Vector2 progetile_Pos; // 발사 위치 
        [SerializeField] private GameObject progetile; // 발사할 볼의 prefab

        [Header("PlusBall")] 
        [SerializeField] private Transform plusPool;
        [SerializeField] private Transform plusActive;

        [Header("ProjetilePool")]
        [SerializeField] private Transform progetile_Pool;
        [SerializeField] private Transform progetileGroup;
        [SerializeField] private Transform flightGroup;

        [Header("Ballnum_info")] private int targetNum; // 볼이 몇 개 돌아와야하는지를 저장하는 var. 
        private int count_num = 0; // 현재 볼이 몇 개 돌아왔는지를 저장하는 var 

        public GameObject tap_btn;

        [SerializeField] private Transform charater;

        public Text ballNum;
        private const float Ground_Y = -701.02f; // 공의 절대적인 Y좌표. 
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
                Make_new_Ball(1); // 1 
        }
        
        #endregion

        #region Projetile_Pooling
        /// <summary>
        /// 발사할 공을 이미지를 맞추어 생성함. 
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// 플러스볼이 낙하하면 실행되는 Coroutine
        /// </summary>
        /// <param name="TR"></param>
        /// <returns></returns>
        private IEnumerator PlusMove(Transform TR)
        {
            TR.DOMove(progetile_Pos,0.4f).SetEase(Ease.InExpo);// 플러스볼을 땅바닥으로 내림. 
            TR.DOKill();
            TR.gameObject.SetActive(false);
            TR.gameObject.GetComponent<Animator>().enabled = true;
            TR.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            TR.GetChild(0).gameObject.SetActive(true);
            yield return null;
        }
        
        private IEnumerator plusball_move()
        {
            int target_num = plusActive.childCount;
            Transform tr;
            for (int i = 0; i < target_num; i++)
            {
                tr = plusActive.GetChild(0); // 먹은 플러스볼 만큼을 발사 위치로 옮겨줌. 이 친구들은 도착하면 plusball pool로 들어감,  
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
                
                // 발사 시 돌아와야하는 볼 개수를 초기화 해주고, 센 공의 개수를 0으로 초기화해줌. 
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

        /// <summary>
        /// data를 받아 이에 맞게 공도 만들어주고, 공 + 텍스트 + 캐릭터 위치도 잡아줌.
        /// </summary>
        /// <param name="data"></param>
        public void Load_Data(BallInfo_VO data)
        {
            progetile_Pos = data.ballPos; // 공의 소환 위치 지정 
            Make_new_Ball(data.ballNum);

            charater.transform.position = data.charPos;
            if (data.is_Fliped) // 캐릭터의 방향을 지정해주기 위함
            {
                charater.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                charater.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            // Step 3. 텍스트의 위치를 지정해줌
            
            ballNum.text = string.Format("{0:#,0}",data.ballNum);
            ballNum.gameObject.transform.position = new Vector3((Mathf.Abs(progetile_Pos .x)>463f ?  (progetile_Pos .x<-463f ? -463f:463f):progetile_Pos .x),progetile_Pos .y-70f, 0f);
        }

        /// <summary>
        /// Ground Component에서 호출되는 함수. 공이 땅바닥에 떨어지면, 돌아온 개수를 카운트 해줌. 
        /// </summary>
        public void Ball_Land()
        {
            ++count_num;
            if (count_num == targetNum) // 공이 모두 돌아왔다면 
            {
                Transform tr;
                int flight_num = flightGroup.childCount;
                for (int i = 0; i < flight_num; i++)
                {
                    tr =  flightGroup.GetChild(0);
                    tr.SetParent(progetileGroup); // 모두 progetileGroup으로 되돌려줌 
                }
                tap_btn.SetActive(false);
                _mediator.Event_Receive(Event_num.BOX_SPAWN); // 공이 다 돌아왔으므로 박스를 생성하는 이벤트를 시작함. 
            }
        }

        /// <summary>
        /// 볼의 개수, 발사 위치를 저장하게 해주는 함수. 이어하기용
        /// </summary>
        /// <returns></returns>
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
