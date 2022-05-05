
using System.Collections.Generic;
using Ingame_Data;
using UnityEngine;
using Item;
using System.IO;
using Manager;
using Newtonsoft.Json;

namespace Ingame
{
    public class Observer_Item : MonoBehaviour, IObserver_Ingame
    {
        private List<ItemInfoVO> itemData;
        [SerializeField] private Transform itemGroup;

        [Header("Load_Data")] private bool isInit = false;
        [SerializeField] private LocateBox _locateBox;

        [Header("Item_Prefab")] public GameObject colItem, rowItem, crossItem, randomDir;

        [Header("Setting")]
        JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings(); // 세팅 정보에 관한 정보 저장. 

        public void Update_Status()
        {
            if (!isInit)
            {
                itemData = new List<ItemInfoVO>();
                isInit = true;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }

            if (itemData.Count != 0)
                itemData.Clear();

            for (int i = 0; i < itemGroup.childCount; i++)
            {
                IItem_Data info = itemGroup.GetChild(i).gameObject.GetComponent<IItem_Data>();
                ItemInfoVO data = new ItemInfoVO();
                data.pos_X = itemGroup.GetChild(i).position.x;
                data.pos_Y = info.Get_Row();
                data.type = info.Get_Type();
                itemData.Add(data);
            }
            // 추가추가 

            Write_Data();
        }

        public void LoadData_ToIngame()
        {
            Read_Data();
            const float Y_START_POS = 571f;
            const float Y_offset = -155f;
            // 게임의 정보를 불러오는 액션
            for (int i = 0; i < itemData.Count; i++)
            {
                Transform TR = null;
                switch (itemData[i].type)
                {
                    case ItemType.colItem:
                        TR = Instantiate(colItem).transform;
                        TR.gameObject.GetComponent<Col_Item>().locateBox = this._locateBox;
                        

                        break;

                    case ItemType.rowItem:
                        TR = Instantiate(rowItem).transform;
                        TR.gameObject.GetComponent<Raw_Item>().locateBox = this._locateBox;
                        break;

                    case ItemType.randomDir:
                        TR = Instantiate(randomDir).transform;
                        break;

                    case ItemType.crossItem:
                        TR = Instantiate(crossItem).transform;
                        TR.gameObject.GetComponent<Raw_Item>().locateBox = this._locateBox;
                        TR.gameObject.GetComponent<Col_Item>().locateBox = this._locateBox;
                        break;

                }

                TR.SetParent(itemGroup);
                TR.position = new Vector3(itemData[i].pos_X, (Y_START_POS+((itemData[i].pos_Y)*Y_offset)),0); 
                TR.gameObject.GetComponent<IItem_Data>().Set_Row(itemData[i].pos_Y+1);
                TR.gameObject.GetComponent<IItem_Data>().Set_Load(); // 다시 불러와서 필드 내의 어떤 아이템 공격할지 지정해줌 

            }

            isInit = true;
            itemData = new List<ItemInfoVO>();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        private void Read_Data()
        {
            // Step 1. 파일이 없으면, 만들고 시작 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Itemdata.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                ItemInfoVO data = new ItemInfoVO();
                data.pos_X = 1f;
                data.pos_Y = 1;
                data.type = ItemType.colItem;
                // 더미 데이터를 하나 넣어놓음.
                var DATA_STR = JsonUtility.ToJson(data);
                File.WriteAllText(DATA_PATH, DATA_STR);
            }

            else
            {
                var json_string = File.ReadAllText(DATA_PATH);
                itemData = JsonConvert.DeserializeObject<List<ItemInfoVO>>(json_string);
            }
        }

        private void Write_Data()
        {
            // Step 1. 파일이 없으면, 만들고 시작 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Itemdata.json";
            if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                ItemInfoVO data = new ItemInfoVO();
                data.pos_X = 1f;
                data.pos_Y = 1;
                data.type = ItemType.colItem;
                // 더미 데이터를 하나 넣어놓음.
                var DATA_STR = JsonUtility.ToJson(data);
                File.WriteAllText(DATA_PATH, DATA_STR);
                // 더미 데이터를 하나 넣어놓음.
            }

            else
            {
                // 호출됨 
                var str = JsonConvert.SerializeObject(itemData, settings);
                File.WriteAllText(DATA_PATH, str);
            }
            // 저장할 애들을 List에서 빼와서 Arr의 형태로 저장함.
        }

        internal class Wrapper
        {
            [JsonProperty("JsonValues")] public ItemInfoVO data { get; set; }

        }
    }
}

