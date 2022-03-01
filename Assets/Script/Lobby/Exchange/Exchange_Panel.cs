using System;
using Toy;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Attendance;
using Challenge;
using Collection;
using Data;
using log;

namespace Exchange
{
    public class Exchange_Panel : MonoBehaviour
    {
        [Header("Alarm")] public GameObject alarmObj;
        
        public Text main_gem, main_candy, panel_gem;
        [SerializeField] private Button candyExchange;

        [Header("Candy_Info")] public Text candy_Text;
        public Text candy_result_gem;
        
        [Header("Gem_Animation")] public GameObject gem_flying;
        public Animator gem_animator;
        [SerializeField] private QuestManager _questManager;
        private int gem;
        public AudioSource gemSound;
        public GameObject panel;

        [Header("Gem_Info")] private Text candyGem;
        public GameObject exchange_Tutorial;
        public GameObject outline_Btn;

        [SerializeField] private Attendance_UI _attendanceUI;
        private int gemconst = 0;
        private int arriveCount = 0;
        private int targetCount = 0;
        private int targetGem = 0;

        public void Onclick_PanelOn()
        {
            if (PlayerPrefs.GetInt("Tutorial_Exchange", 0) == 1)
            {
                gem = Playerdata_DAO.Player_Gem();
                if (Playerdata_DAO.Player_Candy() < 5)
                {
                    candy_Text.text
                        = "<color=red>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()) + "</color>/5";
                    candyExchange.interactable = false;
                }

                else
                {
                    candy_Text.text
                        = "<color=#00FF00>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()) + "</color>/5";
                    candyExchange.interactable = true;
                }

                panel_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
                candy_result_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy() / 5);
            }

            else
            {
                gem = Playerdata_DAO.Player_Gem();
                if (Playerdata_DAO.Player_Candy()+30 < 5)
                {
                    candy_Text.text
                        = "<color=red>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()) + "</color>/5";
                    candyExchange.interactable = false;
                }

                else
                {
                    candy_Text.text
                        = "<color=#00FF00>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()+30) + "</color>/5";
                    candyExchange.interactable = true;
                }

                panel_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
                candy_result_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()+30 / 5);
            }

            panel.SetActive(true);
        }

        public GameObject quantity;
        
        private void Awake()
        {
            Determine_AlarmOn();
        }

        #region Alarm
        private void Determine_AlarmOn()
        {
            bool flag = false;
            if (Playerdata_DAO.Player_Candy() > 4)
                flag = true;

            if(flag == true)
                alarmObj.SetActive(true);

            else
                alarmObj.SetActive(false);
        }

        #endregion

        private void Set_Button_UI(int toyCount = -1)
        {
            main_candy.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            candy_result_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy() / 5);
            if (Playerdata_DAO.Player_Candy() < 5)
            {
                candy_Text.text
                    = "<color=red>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()) + "</color>/5";
                candyExchange.interactable = false;
            }

            else
            {
                candy_Text.text
                    = "<color=#00FF00>" + String.Format("{0:#,0}", Playerdata_DAO.Player_Candy()) + "</color>/5";
                candyExchange.interactable = true;
            }
        }
        
        public void OnClick_CandyToGem()
        {
            if (PlayerPrefs.GetInt("Tutorial_Exchange", 0) == 0)
            {
                exchange_Tutorial.SetActive(false);
                outline_Btn.SetActive(false);
                PlayerPrefs.SetInt("Tutorial_Exchange", 1);
                PlayerPrefs.SetInt("First_Attenance",1);
                Tutorial_Log.Exchange_Tutorial_End();
                Playerdata_DAO.Set_Player_Candy(30);
            }

            var target_gem = Playerdata_DAO.Player_Candy() / 5;
            Playerdata_DAO.Set_Player_Gem(target_gem);
            Playerdata_DAO.Set_Player_Candy((-target_gem*5));
            StartCoroutine(Get_Gem(target_gem,true));// 젬 연출 
            _questManager.Set_Trade_Candy();
            gemSound.Play();
            Set_Button_UI();
            
        }
        

        public void OnClick_close()
        {
            Determine_AlarmOn();
            main_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            main_candy.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            if (PlayerPrefs.GetInt("First_Attenance", 0) == 1)
            {
                PlayerPrefs.SetInt("First_Attenance",0);
                _attendanceUI.OnClick_Open_Panel();
                PlayerPrefs.SetInt("Show_Up", 0);

            }

            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        #region Gem_Animation

        public IEnumerator Get_Gem(int gem, bool is_candy)
        {
            Vector2 start_pos;
            float y_start;
            GameObject quantity_obj;
            if (is_candy)
            {
                start_pos = new Vector2(21f, -384f);
                y_start = -384f;
            }

            else
            {
                start_pos = new Vector2(21f, -577f);
                y_start = -577f;
            }

            quantity_obj = Instantiate(quantity, new Vector3(0f, y_start+100f, -1f), Quaternion.identity);
            quantity_obj.GetComponent<Text>().text = "+" + gem.ToString();
            quantity_obj.transform.SetParent(panel.transform);
            StartCoroutine(Quantity_Flying(quantity_obj.transform));
            // Pre. 글자 띄우는 Prefab 로드하기 
            // Step 1. 글자가 뜰 위치 지정 
            // Step 2. 애니메이션 활성화 
            if (gem > 50)
            {
                gemconst = (gem / 10);
            }

            else
                gemconst = 1;

            targetCount = (gem / gemconst);
            targetGem = this.gem + gem;
            for (int i = 0; i < targetCount; i++)
            {
                Gem_Flying script;
                GameObject obj = Instantiate(gem_flying, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                obj.transform.localScale = new Vector2(80f, 80f);
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gem_animator;
                script.start_pos = start_pos;
                script.gem_text = this.panel_gem;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }
            
            yield return null;
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
        
        private void set_text(object sender, EventArgs e)
        {
            gem += gemconst;
            panel_gem.text = string.Format("{0:#,0}", gem);
            ++arriveCount;
            if (arriveCount == targetCount)
            {
                arriveCount = 0;
                if (gem != targetGem)
                {
                    panel_gem.text = string.Format("{0:#,0}", targetGem);
                    targetGem = gem;
                }
            }
        }
        // 젬 날아가는 애니메이션 
    }
}
