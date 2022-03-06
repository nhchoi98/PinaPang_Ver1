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

            for (int i = 0; i < boxGroup.childCount; i++)
            {
                Move_List.Add(boxGroup.GetChild(i));
            }
            
            for(int i =0; i< itemGroup.childCount; i++)
                item_List.Add(itemGroup.GetChild(i));
            
            StartCoroutine(Pinata_MoveDown());
            for (int i = 0; i < item_List.Count; i++)
            {
                bool value = item_List[i].gameObject.GetComponent<Determine_Destroy>().Set_Destroy();
                if (!value)
                {
                    if (item_List[i].gameObject.layer == 11)
                        item_List[i].gameObject.GetComponent<Col_Item>().Update_List();
                }
                
                StartCoroutine(Move_Down_Item(item_List[i],value));
            }
            
            for (int i = 0; i < Move_List.Count-1; i++)
            {
                StartCoroutine(Move_Down(Move_List[i]));
            }

            if(boxGroup.childCount!=0) yield return StartCoroutine(Move_Down(Move_List[Move_List.Count-1]));
            
            mediator.Event_Receive(Event_num.MOVE_DONE);
        }
        
        #region IEnumerator
        private IEnumerator Move_Down(Transform TR)
        {
            float speed = 500f;
            int row;
            Vector3 Target_Pos;

            if (TR == null)
                yield break;
            
            if (TR.CompareTag("Box") || TR.CompareTag("Obstacle"))
            {
                row = TR.gameObject.GetComponent<IBox>().whichRow();
                TR.gameObject.GetComponent<IBox>().Set_Row(1);
                Target_Pos = new Vector3(TR.position.x, _Determine_Pos.Which_Pos(row+1,0).y, 0f);
            }
            
            else
                Target_Pos = new Vector3(TR.position.x, TR.position.y-155f, 0f);

            if (TR == null)
                yield break;
            
            if (TR.CompareTag("Half"))
            {
                Destroy(TR.gameObject);
                yield break;
            }

            TR.DOMove(Target_Pos, 0.8f)
                .SetEase(Ease.OutQuint)
                .OnComplete(()=>
                {
                    TR.position = Target_Pos;
                    TR.DOKill(true);
                });

            if (Target_Pos.y < -500f && Target_Pos.y > -520f)
            {
                if (!TR.gameObject.CompareTag("PlusBall") && !TR.gameObject.CompareTag("ingame_gem") &&
                    !TR.gameObject.CompareTag("Obstacle"))
                    warnControl.Set_Flag(true);
            }
            
            
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

                    if (TR.gameObject.CompareTag("PlusBall"))
                    {
                        TR.gameObject.SetActive(false);
                        TR.SetParent(plusball_Pool);
                    }

                    else if (TR.gameObject.CompareTag("ingame_gem"))
                        Destroy(TR.gameObject);

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

            if(Is_destroy)
                TR.GetComponent<Determine_Destroy>().Set_Animation();
            
            yield return null;
            
        }

        private IEnumerator Pinata_MoveDown()
        {
            if (pinataGroup.childCount!=0)
            {
                pinataGroup.GetChild(0).gameObject.GetComponent<Pinata.RopePhysics>().Add_Component();
                pinataGroup.GetChild(0).gameObject.transform.GetChild(0).GetChild(1).GetComponent<Momentum_calc>().Set_SpingJoint_length();
                yield return new WaitForSeconds(0.1f);
                if (PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 0)
                {
                    GameManage gameManage = GameObject.FindWithTag("GameController").GetComponent<GameManage>();
                    gameManage.Event_Receive(Event_num.TUTORIAL_PINATA);
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
