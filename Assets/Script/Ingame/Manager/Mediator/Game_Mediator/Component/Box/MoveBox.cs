using System;
using System.Collections;
using System.Collections.Generic;
using Block;
using DG.Tweening;
using Item;
using Manager;
using UnityEngine;
using Warn;

namespace Ingame
{
    /// <summary>
    /// 박스를 생성하면, 필드에 있는 아이템 및 플러스볼, 박스들을 모두 한 줄 내려주는 역할을 담당하는 스크립트. 
    /// </summary>
    public class MoveBox : MonoBehaviour, IComponent
    {
        [SerializeField] private Transform boxGroup;
        [SerializeField] private Transform pinataGroup;
        [SerializeField] private Transform itemGroup;
        [SerializeField] private Transform plusball_Pool;
        [SerializeField] private Ingame_Warn warnControl;
        private IMediator mediator;
        
        private IEnumerator Move_Down()
        {
            List<Transform> item_List = new List<Transform>();
            List<Transform> Move_List = new List<Transform>();

            // 내릴 박스들을 먼저 List에 옮겨셔 저장해놓음 
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                Move_List.Add(boxGroup.GetChild(i));
            }
            
            // 내릴 아이템들을 List에 옮겨서 저장해놓음 
            for(int i =0; i< itemGroup.childCount; i++)
                item_List.Add(itemGroup.GetChild(i));
            
            // 피냐타를 내림 
            StartCoroutine(Pinata_MoveDown());
            for (int i = 0; i < item_List.Count; i++)
            {
                bool value = item_List[i].gameObject.GetComponent<Determine_Destroy>().Set_Destroy(); // 아이템이 1회 이상 피격 당했다면.. 아이템을 필드상에서 제거함. 
                if (!value) // 피격 당하지 않았다면 
                {
                    if (item_List[i].gameObject.layer == 11) // 열 아이템 이라면 
                        item_List[i].gameObject.GetComponent<Col_Item>().Update_List(); // 어떤걸 파괴할 지에 대한 리스트를 업데이트 해줌 
                }
                
                StartCoroutine(Move_Down_Item(item_List[i],value)); 
            }
            
            // 박스 오브젝트를 코루틴을 이용해 내림 
            for (int i = 0; i < Move_List.Count-1; i++)
            {
                StartCoroutine(Move_Down(Move_List[i]));
            }

            if(boxGroup.childCount!=0) yield return StartCoroutine(Move_Down(Move_List[Move_List.Count-1])); // 이 박스가 내려올 때 까지 다음 액션 실행을 대기함. 

            mediator.Event_Receive(Event_num.MOVE_DONE); // 중재자에게 필드상 모든 오브젝트를 다 내렸음을 알려줌. 
        }
        
        #region IEnumerator
        private IEnumerator Move_Down(Transform TR)
        {
            float speed = 500f;
            int row;
            Vector3 Target_Pos;

            if (TR == null)
                yield break;
            
            if (TR.CompareTag("Box") || TR.CompareTag("Obstacle") || TR.CompareTag("PlusBall"))
            {
                row = TR.gameObject.GetComponent<IBox>().whichRow();
                TR.gameObject.GetComponent<IBox>().Set_Row(-1);
                Target_Pos = new Vector3(TR.position.x, _Determine_Pos.Which_Pos(row+1,0).y, 0f); // 박스가 움직일 위치를 계산함. 
            }
            
            else
                Target_Pos = new Vector3(TR.position.x, TR.position.y-155f, 0f);

            if (TR == null)
                yield break;
            
            if (TR.CompareTag("Half")) // half박스의 partent는 빈 오브젝트이므로, 이 오브젝트는 파괴해줌. halfbox는 parent 밑에 두 개의 삼각형이 있는 구조라, 생성될 때 이미 parent에서 벗어남. 
            {
                Destroy(TR.gameObject);
                yield break;
            }

            // 실제로 박스를 내리는 애니메이션을 실행시켜주는 함수. 
            TR.DOMove(Target_Pos, 0.8f)
                .SetEase(Ease.OutQuint)
                .OnComplete(()=>
                {
                    TR.position = Target_Pos;
                    TR.DOKill(true);
                });

            // 경고 기준선 보다 밑으로 내려가는 박스가 있다면, 위험 알람을 켜줌  
            if (Target_Pos.y < -500f && Target_Pos.y > -520f)
            {
                if (!TR.gameObject.CompareTag("PlusBall") && !TR.gameObject.CompareTag("ingame_gem") &&
                    !TR.gameObject.CompareTag("Obstacle"))
                    warnControl.Set_Flag(true);
            }
            
            
            // 죽음 기준선보다 밑으로 내려가는 박스가 있다면, 플레이어 죽음 선언을 해줌. 
            else if (Target_Pos.y < -520f)
            {
                // 특정 줄 이하로 내려가면
                if (!TR.gameObject.CompareTag("PlusBall") && !TR.gameObject.CompareTag("ingame_gem") &&
                    !TR.gameObject.CompareTag("Obstacle"))
                {
                    mediator.Event_Receive(Event_num.USER_DIE);
                    TR.GetComponent<IDestroy_Action>().Destroy_Action();
                }

                else
                {

                    // 플러스볼이 기준선 보다 밑으로 내려가면, 플러스볼을 파괴함.
                    if (TR.gameObject.CompareTag("PlusBall"))
                    {
                        TR.gameObject.SetActive(false);
                        TR.SetParent(plusball_Pool);
                    }

                    // 젬이 기준선보다 밑으로 내려가면, 젬을 파괴함. 
                    else if (TR.gameObject.CompareTag("ingame_gem"))
                        Destroy(TR.gameObject);

                    // 장애물 박스가 기준선보다 밑으로 내려가면, 장애물 박스를 파괴함. 
                    else if (TR.gameObject.CompareTag("Obstacle"))
                        TR.gameObject.GetComponent<IDestroy_Action>().Destroy_Action();

                    
                }
            }
            yield return null;
        }
        

        private IEnumerator Move_Down_Item(Transform TR, bool Is_destroy)
        {
            float speed = 500f;
            Vector3 Target_Pos = new Vector3(TR.position.x, TR.position.y-155f, 0f);
            TR.DOMove(Target_Pos, 0.8f).SetEase(Ease.OutQuint)
                .OnComplete(()=>
                {
                    TR.DOKill(true);
                });
            if (TR.position.y < -510f)
            { // 특정 줄 이하로 내려가면
                TR.GetComponent<Determine_Destroy>().Set_Animation();
                yield return null;
            }

            if(Is_destroy) // 필드 아이템이 1회 피격되어서 필드에서 사라져야 한다면..
                TR.GetComponent<Determine_Destroy>().Set_Animation(); // 사라지게 만듬 

            else
            {
                TR.gameObject.GetComponent<IItem_Data>().Set_Row(-1);
            }
            
            yield return null;
            
        }

        private IEnumerator Pinata_MoveDown()
        {
            if (pinataGroup.childCount!=0)
            {
                pinataGroup.GetChild(0).gameObject.GetComponent<Pinata.RopePhysics>().Add_Component(); // 피냐타가 메달려있는 줄의 그래픽적 길이를 조정해줌. 
                pinataGroup.GetChild(0).gameObject.transform.GetChild(0).GetChild(1).GetComponent<Momentum_calc>().Set_SpingJoint_length(); // 파냐타의 줄 길이를 길게 해줌 
                yield return new WaitForSeconds(0.1f);
                // 만약 튜토리얼 피냐타가 나타났다면 
                if (PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 0)
                {
                    GameManage gameManage = GameObject.FindWithTag("GameController").GetComponent<GameManage>();
                    gameManage.Event_Receive(Event_num.TUTORIAL_PINATA); // 튜토리얼 피냐타와 관련된 메시지를 띄워줌. 
                }
            }

            yield return null;
        }

        #endregion

        public void Set_Mediator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            StartCoroutine(Move_Down());

        }
    }
}
