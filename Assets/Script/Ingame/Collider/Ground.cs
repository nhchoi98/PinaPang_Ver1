using System.Collections;
using Ingame;
using UnityEngine;
using DG.Tweening;

/// <summary>
///  바닥에 공이 다시 착지하면, 카운트를 올려주는 함수. 시작 안했다면 코루틴을 시작시켜줘 
/// 바닥에 공이 몇 개 돌아왔는지 판단해줌. 공의 콜라이더 또한 꺼준다.
/// </summary>
namespace Collider
{
    public class Ground : MonoBehaviour, IComponent
    {
        [SerializeField] private LaunchManage lm;
        [SerializeField] private BallManage bm;
        
        [SerializeField] private Transform Charater;
        [SerializeField] private IMediator _mediator;
        private const float CHAR_Ground_Y = -655f;
        private const float Ground_Y = -701.02f;
        
        [SerializeField]
        private bool first_land;
        private Vector2 first_Pos;
        public bool tutorial = false;
        public Vector3 target_rotation = new Vector3(0f, 0f, 0f);

        private int charater_index;
        
        private void Awake()
        {
            if (PlayerPrefs.GetInt("Tutorial_Basic_Ingame", 0) == 0)
            { 
                tutorial = true;
            }
        }

        public void Set_Charater_Index(int index)
        {
            if (tutorial)
                return;
            else
                Charater.position = new Vector2(Get_X_Offset(), CHAR_Ground_Y);

        }

        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8) 
                yield break;

            else
            {
                Rigidbody2D rigid = collision.gameObject.GetComponent<Rigidbody2D>();
                rigid.freezeRotation = true;
                rigid.velocity = Vector2.zero;
                collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                collision.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x, Ground_Y);
                collision.transform.rotation =  Quaternion.Euler(target_rotation);
                if (!first_land)
                {
                    first_Pos = collision.gameObject.transform.position;
                    first_land = true;
                    lm.Set_Pos(first_Pos);
                    bm.Set_Pos(first_Pos);
                    StartCoroutine(_Charater_Move());
                }

                StartCoroutine(BallMove(collision.gameObject.transform));
                bm.Ball_Land();
            }

            yield return null;
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.Launch_Green:
                    first_land = false;
                    break;
            }
           
        }
        
        private Vector2 Determine_Charater_Pos(Vector2 Pos)
        {
            float X_offset = 130f;
            Vector2 Target_Pos;

            if (Pos.x < 0f)
            {
                Target_Pos = new Vector2(Pos.x + X_offset, Charater.position.y);
                Charater.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                Charater.GetChild(1).GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                Target_Pos = new Vector2(Pos.x - X_offset, Charater.position.y);
                Charater.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                Charater.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
            }
            return Target_Pos;
        }

        public void Tutorial_Down()
        {
            lm.Set_Pos(first_Pos);
            bm.Set_Pos(first_Pos);
            StartCoroutine(_Charater_Move());
            bm.Ball_Land();
        }
        private IEnumerator _Charater_Move()
        {
            Vector2 Target_Pos;
            Transform TR = Charater;
            float speed = 500f;
            _mediator.Event_Receive(Event_num.CHARATER_MOVE);
            if (tutorial)
            {
                _mediator.Event_Receive(Event_num.CHARATER_ARRIVE);
                yield break;

            }

            Target_Pos = Determine_Charater_Pos(first_Pos);
            Charater.gameObject.GetComponent<Animator>().SetTrigger("Move");

            while (true)
            {
                TR.position = Vector3.MoveTowards(TR.position, Target_Pos, speed * Time.deltaTime);
                if ((Vector2)TR.position == Target_Pos)
                    break;
                
                yield return null;
            }
            _mediator.Event_Receive(Event_num.CHARATER_ARRIVE);
            Charater.gameObject.GetComponent<Animator>().SetTrigger("Arrive");
            yield return null;

            
        }

        private IEnumerator BallMove(Transform TR)
        {
            int speed = 3000;
            TR.DOMove(first_Pos, 0.3f)
                .SetEase(Ease.InQuart)
                .OnComplete(() => { TR.DOKill(true); });

            yield return null;

        }




        private float Get_X_Offset()
        {
            var char_num = Calc_Index.Get_Avatar_Num(charater_index);
            switch (char_num)
            {
                default:
                    return 135f;
                
                case 9:
                    return 145f;
                
                case 1000:
                    return 175f;
                
            }
        }
    }
}
