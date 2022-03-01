using System;
using System.Collections;
using System.Collections.Generic;
using Collection;
using Data;
using Ingame;
using log;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tutorial
{
    public class Tutorial_Lobby : MonoBehaviour
    {
        [Header("MAIN_BUTTON")] 
        public GameObject skinBtn, questBtn, shopBtn, collectionBtn, attendanceBtn;
        public GameObject bestScore_Obj;
        public GameObject exchangeBtn;
        
        [Header("Candy_UI")] 
        [SerializeField] private Text candyText;
        public GameObject candyObj;
        public Animator candy_animator;
        public GameObject candyBar;
        private int candy;
        
        [SerializeField] private GameObject rewardPanel;

        [Header("Exchange_Panel")] public GameObject exchangeTutorial;
        public GameObject exchangeTutorial_Panel;
        public AudioSource candy_Play;
        private void Awake()
        {
            // 나머지 버튼 띄워주기 
            candy = Playerdata_DAO.Player_Candy();
            candyText.text = String.Format("{0:#,0}",Playerdata_DAO.Player_Candy());
            candyBar.SetActive(true);
            rewardPanel.SetActive(true);
            skinBtn.SetActive(false);
            questBtn.SetActive(false);
            shopBtn.SetActive(false);
            collectionBtn.SetActive(false);
            bestScore_Obj.SetActive(false);
            exchangeBtn.SetActive(false);
            attendanceBtn.SetActive(false);
            //exchangeTutorial.SetActive(true);
        }

        /// <summary>
        /// 튜토리얼 보상 버튼을 누르면 실행되는 함수 
        /// </summary>
        public void OnClick_rewardPanel_Ok()
        {
            candy_Play.Play();
            StartCoroutine(Get_Gem(15));
            rewardPanel.SetActive(false);
            Show_Exchange_Tutorial();
        }

        /// <summary>
        /// 교환소 튜토리얼 진입을 위한 레이어 조정 및 요소 띄워주기 
        /// </summary>
        private void Show_Exchange_Tutorial()
        {
            exchangeTutorial.SetActive(true);
            exchangeBtn.SetActive(true);
            Tutorial_Log.Exchange_Tutorial_Start();
        }

        #region Exchange_Panel

        public void Click_Tutorial_Exchange()
        {
            candyBar.SetActive(false);
            exchangeTutorial.SetActive(false);
            exchangeTutorial_Panel.SetActive(true);
        }
        

        #endregion
        
        #region Candy_Flying
        public IEnumerator Get_Gem(int num)
        {
            Vector2 start_pos = Vector2.zero; 
            for (int i = 0; i < num; i++)
            {
                Gem_Flying script;
                GameObject obj = Instantiate(candyObj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                obj.transform.localScale = new Vector2(2f, 2f);
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.candy_animator;
                script.start_pos = start_pos;
                script.gem_text = this.candyText;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-142f, 861f);
            }
            yield return null;
        }
        
        
        private void set_text(object sender, EventArgs e)
        {
            candy += 2;
            candyText.text = string.Format("{0:#,0}", candy);
        }
        

        #endregion
        
    }
}
