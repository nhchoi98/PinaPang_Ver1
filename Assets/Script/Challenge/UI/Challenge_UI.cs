using System;
using System.Collections;
using Collection;
using Data;
using Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace Challenge
{
    public class Challenge_UI : MonoBehaviour
    {
        [Header("Commodity")]
        [SerializeField] private Text gem_text , main_gem;
        [SerializeField] private Text candy_text, main_candy;
        
        [SerializeField] private GameObject gem_obj;
        [SerializeField] private Animator gem_animator;
        private int gem;

        [SerializeField] private Button item_Btn;
        public Animator questAnimator;
        public bool rewardFlag;
        public AudioSource click;
        private void OnEnable()
        {
            gem = Playerdata_DAO.Player_Gem();
            gem_text.text = String.Format("{0:#,0}",gem);
            candy_text.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            if (rewardFlag)
            {
                questAnimator.SetTrigger("Alarm_on");
            }

            else
                questAnimator.SetTrigger("Alarm_off");
            
        }
        
        public void OnClick_Exit()
        {
            click.Play();
            main_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            main_candy.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            this.gameObject.SetActive(false);
        }
        

        public void Timer_Reset()
        {
            ChallengeDAO data = new ChallengeDAO();
            data.Init_data(true);
            item_Btn.interactable = true;
            item_Btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "(1/1)";
        }
        
        #region  Commodity_Flight
        public IEnumerator Get_Gem(int index, int gem)
        {
            for (int i = 0; i < gem; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Gem_Start_pos(index);
                GameObject obj = Instantiate(gem_obj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gem_animator;
                script.start_pos = start_pos;
                script.gem_text = this.gem_text;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }
            yield return null;
        }

        /// <summary>
        /// 버튼의 위치를 기반으로, 젬이 비행 시작할 위치를 지정해줌 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 Gem_Start_pos(int index)
        {
            Vector2 target_pos;
            switch (index)
            {
                default:
                    target_pos = new Vector2(318.8f, 40f);
                    break;
                
                case 1:
                    target_pos = new Vector2(318.8f, -131f);
                    break;
                
                case 2:
                    target_pos = new Vector2(318.8f, -302f);
                    break;
                
                case 3:
                    target_pos = new Vector2(318.8f, -473f);
                    break;
                
                case 4:
                    target_pos = new Vector2(318.8f, -644f);
                    break;
            }
            return target_pos;
        }
        
        
        private void set_text(object sender, EventArgs e)
        {
            gem += 1;
            gem_text.text = string.Format("{0:#,0}", gem);
        }
        #endregion
    }
}
