using System.Collections;
using System;
using Challenge;
using Data;
using Toy;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class Collection_MainUI : MonoBehaviour
    {
        [Header("Preset_UI")] 
        private Color disable_Color = new Color(0f, 0f, 0f, 188f / 255f);
        [SerializeField] private Sprite disable_back;
        [SerializeField] private Transform content;
        
        [Header("Panel")]
        [SerializeField] private GameObject toyExchange_Panel;
        [SerializeField] private Button exchageBtn;
        [SerializeField] private Text gemText;
        [SerializeField] private Text toyText;
        [SerializeField] private Image itemImg;
        [SerializeField] private Image item_smallImage;
        [SerializeField] private Text toy_description;
        [SerializeField] private Text toy_name;
        private const int TOYNUM = 50;
        
        [Header("Toy_Data")]
        private int index;
        SingleToyDAO singletoyData;
        private int toyCount;
        private Toy_Desc _desc = new Toy_Desc();

        [Header("Gem_Data")] private int gem;
        [SerializeField] private Text collection_GemText;
        [SerializeField] private Text mainGem;
        [SerializeField] private GameObject quantity;
        [SerializeField] private GameObject gem_flying;
        [SerializeField] private Animator gemAnimator;

        [Header("User_Level_Data")] 
        private ToyDAO userLevel;
        [SerializeField] private Text level;
        [SerializeField] private Text levelProgress_Text;
        [SerializeField] private Slider levelSlider;
        [SerializeField] private Text candleInfo;

        public AudioSource gemSound;
        private void Awake()
        {
            userLevel = new ToyDAO();
            Set_Whole_Count();
            Set_Level_Text();
            Set_Level_Slider();
            Set_Candle_Text();
        }

        private void OnEnable()
        {
            gem = Playerdata_DAO.Player_Gem();
            collection_GemText.text = String.Format("{0:#,0}", gem);
        }

        public void OnClick_Main_Close()
        {
            mainGem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            this.gameObject.SetActive(false);
        }
        
        public void OnClick_Item(int index)
        {
            this.index = index;
            Set_Toy_Img();
            Set_Toy_Desc();
            Set_Toy_Num();
            Determine_Exchange_Possible();
            // 패널의 UI 요소들을 지정해줌 
            // 패널을 켜줌 
            toyExchange_Panel.SetActive(true);
        }

        #region Set_Panel

        private void Set_Toy_Img()
        {
            itemImg.sprite = Resources.Load<Sprite>("Lobby/Collection/Collection_" + index.ToString());
            item_smallImage.sprite = itemImg.sprite;
        }

        private void Set_Toy_Desc()
        {
            toy_name.text = _desc.Get_Toy_Name(index);
            toy_description.text = _desc.Get_Toy_Description(index);
        }

        /// <summary>
        /// 현재 소유하고 있는 toy num과, 이를 토대로 교환 가능한 젬 개수를 표시해줌. 
        /// </summary>
        private void Set_Toy_Num()
        {
            singletoyData = new SingleToyDAO(index);
            toyCount = singletoyData.Get_Toy_Count();
            if (toyCount / 3 < 1)
                toyText.text = "<color=#FF0000>" + String.Format("{0:#,0}", toyCount) + "</color>/3"; // 패널에 개수 표시 

            else 
                toyText.text =  String.Format("{0:#,0}", toyCount) + "/3"; // 패널에 개수 표시 
            
            gemText.text = String.Format("{0:#,0}", toyCount/3); 

        }

        private void Determine_Exchange_Possible()
        {
            singletoyData = new SingleToyDAO(index);
            if (singletoyData.Get_Toy_Count() < 3)
                exchageBtn.interactable = false;
            
            else
                exchageBtn.interactable = true;
        }
        #endregion

        #region Panel_Action

        public void OnClick_Panel_Close()
        {
            toyExchange_Panel.SetActive(false);
        }

        public void OnClick_Exchange()
        {
            // # 1. 젬이 날라가는 애니메이션. 
            // # 2. 창이 닫혀야함. 
            int gem_count = toyCount / 3;
            gemSound.Play();
            singletoyData.Set_Exchange();
            StartCoroutine(Get_Gem(gem_count));
            Set_Toy_Num();
            Set_Single_Count(index);
            exchageBtn.interactable = false;
        }
        

        #endregion
        /// <summary>
        /// 교환 이벤트가 발생 할 경우, 한 개의 텍스트만 숫자를 바꾸어줌
        /// </summary>
        /// <param name="index"></param>
        private void Set_Single_Count(int index)
        {
            SingleToyDAO singletoyData = new SingleToyDAO(index);
            int count = singletoyData.Get_Toy_Count();
            Transform item = content.GetChild(index);
            item.GetChild(2).gameObject.GetComponent<Text>().text = String.Format("{0:#,0}", count); // 텍스트에 숫자 지정 
            // 텍스트로 몇 개 습득했는지 지정하기. 
        }

        /// <summary>
        /// 전체 컬렉션의 Count를 설정해주는 함수 
        /// </summary>
        private void Set_Whole_Count()
        {
            SingleToyDAO singletoyData;
            int count;
            for (int i = 0; i < TOYNUM; i++)
            {
                singletoyData = new SingleToyDAO(i);
                count = singletoyData.Get_Toy_Count();
                Transform item = content.GetChild(i);
                if (singletoyData.Get_IsLocked()) // 얻은 적이 없을 경우 
                {
                    item.GetChild(1).gameObject.GetComponent<Image>().color = disable_Color;
                    item.gameObject.GetComponent<Image>().sprite = disable_back; // 매터리얼 지정 
                }

                item.GetChild(2).gameObject.GetComponent<Text>().text = String.Format("{0:#,0}", count); // 텍스트에 숫자 지정 
            }
        }

        #region Gem_Animation
        public IEnumerator Get_Gem(int gem)
        {
            Vector2 start_pos;
            float y_start;
            GameObject quantity_obj;
            start_pos = new Vector2(21f, -577f);
            y_start = -577f;
            
            quantity_obj = Instantiate(quantity, new Vector3(0f, y_start+100f, -1f), Quaternion.identity);
            quantity_obj.GetComponent<Text>().text = "+" + gem.ToString();
            StartCoroutine(Quantity_Flying(quantity_obj.transform));
            // Pre. 글자 띄우는 Prefab 로드하기 
            // Step 1. 글자가 뜰 위치 지정 
            // Step 2. 애니메이션 활성화 
            for (int i = 0; i < gem; i++)
            {
                Gem_Flying script;
                GameObject obj = Instantiate(gem_flying, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                obj.transform.localScale = new Vector2(80f, 80f);
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gemAnimator;
                script.start_pos = start_pos;
                script.gem_text = this.collection_GemText;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }
            yield return null;
        }
        
        private void set_text(object sender, EventArgs e)
        {
            gem += 1;
            collection_GemText.text = string.Format("{0:#,0}", gem);
        }
        
        IEnumerator Quantity_Flying(Transform TR)
        {
            float T_time = 1f;
            float time = 0f;
            float speed = 100f;
            Vector2 targetpos = new Vector2(TR.position.x, TR.position.y + 120f);
            while (true)
            {
                if (time >= T_time)
                    break;
                TR.position = Vector2.Lerp(TR.position, targetpos, Time.deltaTime);//new Vector2(TR.position.x, TR.position.y + (speed * Time.deltaTime));
                time += Time.deltaTime;
                yield return null;
            }
            TR.gameObject.GetComponent<Animator>().SetTrigger("end");
            yield return new WaitForSeconds(0.5f);
            Destroy(TR.gameObject);
        }
        #endregion

        #region  Set_Level_Data

        private void Set_Level_Text()
        {
            level.text = "LV. " + userLevel.Get_Level().ToString();
        }

        private void Set_Level_Slider()
        {
            
            if (userLevel.Get_Level() < 11)
            {
                levelProgress_Text.text = String.Format("{0:#,0}", userLevel.Get_count()) + "/" +
                                          String.Format("{0:#,0}", userLevel.Get_Target());
        
                levelSlider.value = (userLevel.Get_count()-userLevel.Get_Previous_Target()) / (float)userLevel.Get_Interval();
            }
            
            else
            {
                levelProgress_Text.text = "MAX";
                levelSlider.value = 1f;
            }
        }

        private void Set_Candle_Text()
        {
            if(userLevel.Get_Level()>1)
                candleInfo.text = "CANDLE BOX + " + ((userLevel.Get_Level() - 1) * 2).ToString() +"%";
            else
                candleInfo.text = "CANDLE BOX + " + 0.ToString() +"%";
        }
        #endregion
    }
}
