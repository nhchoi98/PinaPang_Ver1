
using System.Collections.Generic;
using Block;
using Item;
using Manager;
using Pinata;
using Tutorial;
using UnityEngine;

namespace Ingame
{
    /// <summary>
    /// 박스들의 위치만 잡아주는 함수 
    /// </summary>
    public class LocateBox : MonoBehaviour
    {
        [SerializeField] private SoundManager SM;
        
        [Header("Blocknum")] private int targetNum;
        private bool candle;
        private bool is_tutorial = false;

        [Header("Transform")] 
        public Transform boxGroup;
        public Transform respawnGroup;
        public Transform plusballGroup;
        public Transform pinataGroup;
        
        [Header("Candle")] 
        [SerializeField] private BonusCharater bonusCharater;
        private List<Vector2> Spawn_Pos_List = new List<Vector2>();
        private Determine_CandleObj _candleObj;
        private bool candle_spawned;

        [Header("Item")]
        [SerializeField] private GameObject raw_Item;
        [SerializeField] private GameObject col_Item;
        [SerializeField] private GameObject both_Item;
        [SerializeField] private GameObject randomDir;
        [SerializeField] private Transform ItemGroup;

        [Header("Pinata")] 
        [SerializeField] private GameObject pinata;

        private bool pinata_ison;
        private bool both_item = false;
        public int pinata_num { get; private set; }

        [SerializeField] private Determine_BoxType _boxType;
        

        // 로드 데이터 필요 
        private void Awake()
        {
            _candleObj = new Determine_CandleObj();
        }

        #region Action
        public void Locate_Box()
        {
            Init_SpawnList();
            if (Tutorial_Candle_Data.Get_respawn_Data()&& _boxType.Get_Stage() == 5)
            {
                _candleObj.Set_Charater_Target(_boxType.Get_Stage());
            }
            
            candle = _candleObj.Determine_Candle(pinata_ison,_boxType.Get_Stage());
            
            targetNum = respawnGroup.childCount;
            Make_PlusBall();
            Make_Item();
            for (int i = 0; i < targetNum; i++)
                Which_Pos();

            candle_spawned = false;
            Spawn_Pos_List.Clear();
            _candleObj.Save_Data(_boxType.Get_Stage(), pinata_ison);
        }

        public void Set_Both_Item()
        {
            if (!both_item)
                both_item = true;

            else
                both_item = false;
        }

        private void Which_Pos()
        {
            int rand = UnityEngine.Random.Range(0, Spawn_Pos_List.Count);
            bool Is_move = false;
            Transform target_obj = respawnGroup.GetChild(0);
            target_obj.SetParent(boxGroup);
            target_obj.SetAsFirstSibling();

            if (candle && Tutorial_Candle_Data.Get_respawn_Data())
            {
                rand = 4; // 위치 고정시키기 
                Tutorial_Candle_Data.Set_respawn_Data(); // 다시 원복 시키기 
            }
            
            target_obj.position = Spawn_Pos_List[rand]; // # 1. 포지션 결정 
            if (!target_obj.CompareTag("Obstacle") && candle)
            {
                if (target_obj.CompareTag("Box"))
                    target_obj.GetComponent<IBox>().Set_Candle(_candleObj.which_charater(_boxType.Get_Stage()));


                else
                {
                    // 반반 박스에 해당되는 경우 
                    int half_rand = UnityEngine.Random.Range(0, 2);
                    target_obj.transform.GetChild(half_rand).GetComponent<IBox>().Set_Candle(_candleObj.which_charater(_boxType.Get_Stage()));

                }

                candle = false;
            }

            if (target_obj.CompareTag("Half"))
            {
                target_obj.transform.GetChild(0).SetParent(boxGroup);
                target_obj.transform.GetChild(0).SetParent(boxGroup);
            }
            
            Spawn_Pos_List.RemoveAt(rand);
        }

        private void Make_PlusBall()
        {
            int rand = UnityEngine.Random.Range(0, Spawn_Pos_List.Count);
            Transform PlusBall =  plusballGroup.GetChild(0); // 플러스볼 먼저 생성 
            PlusBall.GetChild(0).gameObject.SetActive(true);
            PlusBall.gameObject.GetComponent<Animator>().enabled = true;
            PlusBall.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            PlusBall.transform.SetParent(boxGroup);
            PlusBall.gameObject.SetActive(true);
            PlusBall.transform.position = Spawn_Pos_List[rand];
            Spawn_Pos_List.RemoveAt(rand);
            
        }

        private void Make_Item()
        {
            int rawProb;
            int colProb;
            int dirProb;
            
            if (targetNum > 6) // 필드 아이템 소환 여부를 결정짓는 조건문 
                return;

            if (boxGroup.childCount > 19 && boxGroup.childCount < 30)
            {
                rawProb = 20000;
                colProb = rawProb + 20000;
                dirProb = colProb + 7500;
            }
            
            else if (boxGroup.childCount > 29)
            {
                rawProb = 25000;
                colProb = rawProb + 25000;
                dirProb = colProb + 10000;
            }

            else
            {
                rawProb = 13000;
                colProb = rawProb + 13000;
                dirProb = colProb + 7500;
            }
            

            Transform TR;
            int rand = UnityEngine.Random.Range(0, 100000);
            int pos_rand = UnityEngine.Random.Range(0, Spawn_Pos_List.Count);

            if (!both_item)
            {
                if (rand >= 0 && rand < rawProb)
                {
                    TR = Instantiate(raw_Item).transform;
                    TR.SetParent(ItemGroup);
                    TR.position = Spawn_Pos_List[pos_rand];
                    TR.gameObject.GetComponent<IItem_Data>().Set_Type(ItemType.rowItem);
                    TR.gameObject.GetComponent<Raw_Item>().locateBox = this;
                }

                else if (rand >= rawProb && rand < colProb)
                {
                    TR = Instantiate(col_Item).transform;
                    TR.SetParent(ItemGroup);
                    TR.position = Spawn_Pos_List[pos_rand ];
                    TR.gameObject.GetComponent<IItem_Data>().Set_Type(ItemType.colItem);
                    TR.gameObject.GetComponent<Col_Item>().locateBox = this;
                }


                else if (rand >= colProb && rand < dirProb)
                {
                    TR = Instantiate(randomDir).transform;
                    TR.SetParent(ItemGroup);
                    TR.gameObject.GetComponent<IItem_Data>().Set_Type(ItemType.randomDir);
                    TR.position = Spawn_Pos_List[pos_rand];
                }
            }

            else // 양쪽 아이템 사용한 경우 
            {
                if (rand >= 0 && rand < colProb)
                {
                    TR = Instantiate(both_Item).transform;
                    TR.SetParent(ItemGroup);
                    TR.gameObject.GetComponent<Raw_Item>().locateBox = this;
                    TR.gameObject.GetComponent<Col_Item>().locateBox = this;
                    TR.gameObject.GetComponent<IItem_Data>().Set_Type(ItemType.crossItem);
                    TR.position = Spawn_Pos_List[pos_rand];
                }

                else if (rand >= colProb && rand < dirProb)
                {
                    TR = Instantiate(randomDir).transform;
                    TR.SetParent(ItemGroup);
                    TR.gameObject.GetComponent<IItem_Data>().Set_Type(ItemType.randomDir);
                    TR.position = Spawn_Pos_List[pos_rand];
                }
            }
            
            Spawn_Pos_List.RemoveAt(pos_rand );
            
        }

        private void Init_SpawnList()
        {
            for (int i = 0; i < 8; i++) Spawn_Pos_List.Add(_Determine_Pos.Which_Pos(0, i));
        }
        
        #endregion
        
        #region Pinata_Spawn
        /// <summary>
        /// 어떤 종류의 피냐타를 리스폰 할 지 결정해주는 함수 
        /// </summary>
        /// <returns></returns>
        private GameObject Determine_Pinata()
        {
            int rand = UnityEngine.Random.Range(0,10); // 나중에 끝에 부분 변경해랑
            pinata_num = rand;

            if (PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 0)
            {
                rand = 0;
                pinata_num = 0; 
            }

            GameObject Obj;
            switch(rand)
            {
                default:
                    Obj = Resources.Load("Ingame/Pinyata/Prefab/Donkey_Pinata") as GameObject;
                    break;

                case 1:
                    Obj = Resources.Load("Ingame/Pinyata/Snake/Snake_Pinata") as GameObject;
                    break;
                
                case 2:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Owl/OWL_Pinata");
                    break;
                
                case 3:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Cat/CAT_Pinata");
                    break;
                
                case 4:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Rabbit/Rabbit_Pinata");
                    break;
                
                case 5:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Bear/Bear_Pinata");
                    break;
                
                case 6:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Elephant/Elephant_Pinata");
                    break;
                
                case 7:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Parrot/Parrot_Pinata");
                    break;

                case 8:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Frog/Frog_Pinata");
                    break;
                
                case 9:
                    Obj = Resources.Load<GameObject>("Ingame/Pinyata/Fish/Fish_Pinata");
                    break;
            }

            return Obj;
        }
        
        public void Pinata_Spawn(int stage)
        {
            int rand = UnityEngine.Random.Range(1, 7);
            Vector3 Spawn_Pos = _Determine_Pos.Which_Pos(0, rand); // 스폰할 위치 랜덤으로 저장 
            GameObject Obj;
            pinata = Determine_Pinata();
            Obj = Instantiate(pinata, new Vector3(Spawn_Pos.x,0f,0f),Quaternion.identity);
            Obj.transform.GetChild(0).GetChild(1).GetComponent<Pinata_Down>().Original_Hp = stage * 3; // 수정해야함 
            Obj.transform.SetParent(pinataGroup);
            SM.Pinata_showup.Play();
            pinata_ison = true;
        }
        
        public bool is_pinata()
        {
            return pinata_ison;
        }

        public void pinata_die()
        {
            _candleObj.Set_Charater_Target(_boxType.Get_Stage()+1);
            pinata_ison = false;
        }

        #endregion

        public void Set_Is_tutorial()
        {
            if (is_tutorial)
                is_tutorial = false;

            else
                is_tutorial = true;
        }
    }
}
