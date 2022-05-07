using System;
using System.Collections;
using System.Collections.Generic;
using Challenge;
using DG.Tweening;
using Ingame;
using Toy;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class CollectionManager : MonoBehaviour, IComponent
    {
        private IMediator _mediator;
        
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private ToyDAO dao;
        public List<Tuple<int, int>> collectionList;  // Item 1: 어떤거? Item 2: 몇 개? 
        public GameObject flight_obj;
        public Animator text_animator;
        private int collection_num = 0;
        public Text col_Text;
        public GameObject col_Text_obj, obj;
        private string path ;
        private int now_collection, target_collection;
        private int collection_count;

        [SerializeField] private Observer_CollectionInfo _collectionManager;

        public int total_ingame_count { get; private set; }

        /// <summary>   
        /// 어떤 테마를 착용하고 들어왔는지 초기화해줌 
        /// </summary>
        private void Awake()
        {
            dao = new ToyDAO();
            collection_count = 0;
            collection_num = dao.Get_count();
            col_Text.text = "X" + string.Format("{0:#,0}", collection_num);
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            total_ingame_count = 0;
            collectionList = new List<Tuple<int, int>>();
            // 정보를 추가적으로 불러와야 할 경우, 여기서 불러옴 

        }

        public void Load_Data(ref List<Tuple<int, int>> collectionList)
        {
            this.collectionList = new List<Tuple<int, int>>();
            for (int i = 0; i < collectionList.Count; i++)
            {
                this.collectionList.Add(collectionList[i]);
            }

            total_ingame_count = collectionList.Count; 
            collection_num = dao.Get_count();
            col_Text.text = "X" + string.Format("{0:#,0}", collection_num+total_ingame_count);
        }
        
        private void Set_Collection()
        {
            int rand = UnityEngine.Random.Range(0, 50);
            path = "Lobby/Collection/Collection_" + rand.ToString();
            SingleToyDAO singleToyDao = new SingleToyDAO(rand);
            singleToyDao.Set_Toy_Count();
            for (int i = 0; i < collectionList.Count; i++)
            {
                if (collectionList[i].Item1 == rand)
                {
                    collectionList[i] = new Tuple<int, int>(collectionList[i].Item1, collectionList[i].Item2 + 1);
                    return;
                }
            }

            collectionList.Add(new Tuple<int, int>(rand, 1));
        
        }

        /// <summary>
        /// 어떤 컬렉션을 얻었는지 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> Get_Collection_List(int index)
        {
            return collectionList[index];
        }

        public int Get_Collection_Num()
        {
            return collectionList.Count;
        }
        
        /// <summary>
        /// 피냐타가 죽으면 호출되는 함수. 랜덤으로 어떤 컬렉션이 떴는지를 결정해줌.
        /// </summary>
        public IEnumerator Get_Collection(Vector2 pos)
        {
            //Step 1. 어떤 컬렉션 띄울지 결정함.
            var rand = UnityEngine.Random.Range(1, 4);
            target_collection = rand;
            total_ingame_count += rand;
            _questManager.Set_Collection(rand);
            
            // Step 3. 컬렉션을 비행하게 만듬
            col_Text_obj.GetComponent<Animator>().SetTrigger("Start");
            dao.Set_Count(rand);
            if (PlayerPrefs.GetInt("Tutorial_Pinata_Done", 0) == 0)
            {
                PlayerPrefs.SetInt("First_Collection", 1);
                PlayerPrefs.SetInt("Tutorial_Pinata_Done",1);
            }

            for (int i = 0; i < rand; i++)
            {
                //Step 2. 리스트와 데이터에 반영해줌.
                Set_Collection();
                obj = Instantiate(flight_obj, pos, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
                StartCoroutine(Flight_Object(obj.transform, i));
            }

            _collectionManager.Get_CollectionData(ref collectionList);
            yield return null;
        }

        IEnumerator Flight_Object(Transform TR, int index)
        {
            float speed = 350f;
            Vector2 Target = new Vector2((index - 1) * 100f, (index - 1) * 100f);
            
            // 일단 방사 시킴
            TR.DOMove(Target, 1.0f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(1.5f);
            Target = new Vector2(361f, 555f);
            TR.gameObject.GetComponent<Animator>().SetTrigger("down");
            TR.DOMove(Target, 1.0f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    text_animator.SetTrigger("Scale");
                    ++collection_num;
                    col_Text.text = "X" + string.Format("{0:#,0}", collection_num);
                    ++collection_count;
                    if (collection_count == target_collection)
                    {
                        text_animator.SetBool("Arrive", true);
                        collection_count = 0;
                        _mediator.Event_Receive(Event_num.COLLECTION_GET);
                    }

                    col_Text.text = "X" + string.Format("{0:#,0}", collection_num);
                    TR.DOKill();
                    Destroy(TR.gameObject);
                });



        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
        }
    }
    
}
