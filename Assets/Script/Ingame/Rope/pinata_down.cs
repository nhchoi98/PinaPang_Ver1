using Challenge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using camera;
using Setting;
using Collection;
using Ingame;
using Score;

namespace Pinata
{
    /// <summary>
    /// 피냐타가 박살날 때 관여하는 함수 
    /// </summary>
    public class pinata_down : MonoBehaviour
    {
        [SerializeField]
        private Camera_Shake CS; // 카메라를 흔드는 스크립트 참조 

        private CollectionManager _collectionManager;
        private Transform Die_Pool;
        Transform BoxGroup;
        List<Transform> Box_Remove = new List<Transform>();
        public LineRenderer lr; // 라인렌더러 연결 변수 
        public PolygonCollider2D Collider2D; // 콜라이더 연결 번수 
        private List<IDestroy_Action> action = new List<IDestroy_Action>();
        private AudioSource Killed_Sound,Destroy_Sound;
        private IEnumerator whatrunning;
        [SerializeField]
        private GameObject parent, particle, pinata_img;
        public RopePhysics RopePhysics;
        private IMediator _mediator;
        
        
        
        private void Awake()
        {
            SoundManager sm = GameObject.FindWithTag("SM").GetComponent<SoundManager>();
            CS = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Shake>();
            Die_Pool = GameObject.FindWithTag("Die_Pool").transform;
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            _collectionManager = GameObject.FindWithTag("collection_manager").GetComponent<CollectionManager>();
            BoxGroup = GameObject.FindWithTag("BoxGroup").transform;
            Killed_Sound = sm.Pinata_expansion;
            Destroy_Sound = sm.Pinata_exploce;
        }

        public void active_Destroy()
        {
            destroy_start();
            Collider2D.enabled = false;
            StartCoroutine(Scale_Down());
            for (int i = 0; i < BoxGroup.childCount; i++) // 필드상 박스만을 파괴함 
            {
                if (BoxGroup.GetChild(i).gameObject.CompareTag("Box") || BoxGroup.GetChild(i).gameObject.CompareTag("Half") || BoxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                {
                    if (BoxGroup.GetChild(i).gameObject.CompareTag("Box") || BoxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                    {
                        action.Add(BoxGroup.GetChild(i).gameObject.GetComponent<IDestroy_Action>());
                        Box_Remove.Add(BoxGroup.GetChild(i));
                    }

                    else if (BoxGroup.GetChild(i).gameObject.CompareTag("Half"))
                    {
                        switch (BoxGroup.GetChild(i).transform.childCount)
                        {
                            default:
                                break;
                            
                            case 1:
                                action.Add(BoxGroup.GetChild(i).GetChild(0).gameObject.GetComponent<IDestroy_Action>());
                                Box_Remove.Add(BoxGroup.GetChild(i).GetChild(0));
                                break;
                        
                            case 2:
                                Box_Remove.Add(BoxGroup.GetChild(i).GetChild(0));
                                Box_Remove.Add(BoxGroup.GetChild(i).GetChild(1));
                                action.Add(BoxGroup.GetChild(i).GetChild(0).gameObject.GetComponent<IDestroy_Action>());
                                action.Add(BoxGroup.GetChild(i).GetChild(1).gameObject.GetComponent<IDestroy_Action>());
                                break;
                        }
                    }

                    else
                    {
                        Destroy(BoxGroup.GetChild(i).gameObject);
                    }
                }
            }
            
            _mediator.Event_Receive(Event_num.PINATA_DIE);
        }

        public void destroy_start()
        {
            Init_info();
            Killed_Sound.Play();
            whatrunning = CS.Shake_Cam(15f, 1.8f, true, true);
            CS.StartCoroutine(whatrunning);
        }
        
        /// <summary>
        /// 애니메이션과 연결되는 함수 
        /// </summary>
        IEnumerator  Scale_Down()
        {
            while (true)
            {
                if (!particle.activeSelf)
                    break;

                yield return null;
            }
            RopePhysics.enabled = false;
            Destroy(lr);
            Killed_Sound.Stop();
            StopCoroutine(whatrunning);
            StartCoroutine(Pinata_Scale_Down());
            CS.StartCoroutine(CS.Shake_Cam(60f, 1.0f, false, true));
            yield return new WaitForSeconds(0.05f);
            Vibration.Vibrate(200);
            Destroy_Sound.Play();
            _collectionManager.StartCoroutine(_collectionManager.Get_Collection(transform.position));
            box_remove_all();
        }


        IEnumerator Pinata_Scale_Down()
        {
            Destroy(lr);
            while (true)
            {
                if (pinata_img.transform.localScale.x < 0.05)
                    break;

                pinata_img.transform.localScale = new Vector2(pinata_img.transform.localScale.x - 3f * Time.deltaTime,
                    pinata_img.transform.localScale.y - 3f * Time.deltaTime);
                yield return null;
            }

            pinata_img.transform.localScale = Vector2.zero;
            yield return new WaitForSeconds(1.8f);
            Set_To_Pool();
        }

        private void box_remove_all()
        {
            for (int i = 0; i < action.Count ; i++)
                    StartCoroutine(box_remove(i));
                
        }
        
        
        private IEnumerator box_remove(int index)
        {
            action[index].Destroy_Action();
            yield return null;
        }

        private void Init_info()
        {
            _mediator.Event_Receive(Event_num.Abort_Launch_PINATA);
        }
        
        
        private void Set_To_Pool()
        {
            parent.gameObject.transform.SetParent(Die_Pool); // 죽어버렷 
            parent.gameObject.SetActive(false);
        }
        
    }
}
