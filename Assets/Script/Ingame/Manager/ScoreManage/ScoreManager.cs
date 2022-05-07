using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Challenge;
using Data;
using Manager;
using Block;
using Ingame;
using Warn;

namespace Score
{
    /// <summary>
    /// 인게임 내에서 스코어와 관련된 내용을 처리하는 함수. 
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [Header("Data")] [SerializeField] private DataManager DM;

        [Header("Combo")] public GameObject combo3, combo5, combo8;
        private Queue<IEnumerator> comboQueue = new Queue<IEnumerator>(); // 콤보 띄울 순서를 저장하고 있는 함수 
        private IEnumerator nowRun;

        //Destroy_info
        public IBox.event_delegate args;
        
        [Header("QuestManager")] [SerializeField]
        private QuestManager questManager;
        
        [Header("Combo_Info")]
        public int  comboCount;
        private int pinata_count, combo_z_count;

        [Header("Prefab_Pool")] 
        [SerializeField]
        private Transform Combo_Field;
        [SerializeField]
        private Transform Combo_Score;
        [SerializeField] private Transform charater;

        [Header("Warn")] [SerializeField] private Ingame_Warn warn;
        private const float lowerBound = -410f;
        private const float upperBound = -540f;
        
        // Score_Info
        public int score { get; private set; }
        public int basic_score { get; private set; }
        public int good_socre { get; private set; }
        public int great_score { get; private set; }
        public int excellent_score { get; private set; }
        public int box_remove { get; private set; }
        public float score_const { get; private set; } // 점수 배율을 조정해주는 상수 
        public int Best_Score { get; private set; }
        private bool bestScore_level;
        
        [Header("Score_UI")]
        public Text _score, _score_outline1,_score_outline2,_score_outline3, _score_outline4, _score_outline5;
        public GameObject bestscore;
        public Transform stageUI;
        
        private float item_const = 0;

        private void Set_BestScore_Color()
        {
            _score.color = new Color(249f/255f, 78f/255f, 135f/255f);
            _score_outline1.color = new Color(147f/255f, 12f/255f, 129f/255f);
            _score_outline2.color = new Color(147f/255f, 12f/255f, 129f/255f);
            _score_outline3.color = new Color(147f/255f, 12f/255f, 129f/255f);
            _score_outline4.color = new Color(147f/255f, 12f/255f, 129f/255f);
            _score_outline5.color = new Color(147f/255f, 12f/255f, 129f/255f);
        }

        private void Set_Score_Text(int score)
        {
            _score.text = string.Format("{0:#,0}", score);
            _score_outline1.text = string.Format("{0:#,0}", score);
            _score_outline2.text = string.Format("{0:#,0}", score);
            _score_outline3.text = string.Format("{0:#,0}", score);
            _score_outline4.text = string.Format("{0:#,0}", score);
            _score_outline5.text = string.Format("{0:#,0}", score);
        }
        
        public void Load_Score_Data(int score, int stage, bool isBest)
        {
            this.score = score;
            _score.text = string.Format("{0:#,0}", score);
            _score_outline1.text = string.Format("{0:#,0}", score);
            _score_outline2.text = string.Format("{0:#,0}", score);
            _score_outline3.text = string.Format("{0:#,0}", score);
            _score_outline4.text = string.Format("{0:#,0}", score);
            _score_outline5.text = string.Format("{0:#,0}", score);
            if (isBest)
            {
                Best_Score = score;
                Set_BestScore_Color();
                bestScore_level = true;
            }

            Set_scoreConst_LoadData(stage);
        }

        public bool Is_Best()
        {
            return bestScore_level;
        }
        
        private void Awake()
        {
            args = new IBox.event_delegate(Combo);
            score_const = 1; 
            Best_Score = Playerdata_DAO.Player_BestScore();
        }

        #region Combo
        private void Combo(object obj, DestroyArgs args)
        {
            int combo;
            combo = ++comboCount;
            Set_Boxremove(args);
            comboQueue.Enqueue(Set_Combo_Text(combo)); // 콤보 글씨 삽입 
            if (comboQueue.Count != 0 && nowRun == null) 
            {
                nowRun = comboQueue.Dequeue();
                StartCoroutine(nowRun);
            }
            
        }

        public IEnumerator Get_Pinata_Remove()
        {
            ++pinata_count;
            int num = (int) ((1000 *(pinata_count) * score_const*item_const));
            score += num;
            basic_score += num;
            questManager.Set_Score(num);
            Set_Bestscore();
            Set_Score_Text(score);
            yield return null;
        }

        /// <summary>
        /// 콤보 글씨의 색상을 지정해주는 함수 
        /// </summary>
        /// <returns></returns>
        private Color Set_Color(int count)
        {
            Color color;
            switch (count)
            {
                default:
                    color = new Color(255f/255f, 124f/255f, 193f/255f);
                    break;

                case 1:
                case 2:
                    color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                
                case 3:
                case 4:
                    color = new Color(100f/255f, 214f/255f, 255f/255f);
                    break;
                
                case 5:
                case 6:
                case 7:
                    color = new Color(253f / 255f, 194f / 255f, 25f / 255f);
                    break;
            }

            return color;
        }
        private IEnumerator Set_Combo_Text(int combo)
        {
            // 캐릭터 위치 위에 표기 
            combo_z_count -= 1;
            Combo_Field.GetChild(0).position =  new Vector3(charater.position.x,  charater.position.y+120,combo_z_count);
            Combo_Field.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = combo.ToString(); 
            Combo_Field.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().color =  Set_Color(combo);
            Combo_Field.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().color =  Set_Color(combo);
            Combo_Field.GetChild(0).gameObject.GetComponent<ICombo_Pool>().CanvasOn();
            yield return new WaitForSeconds(0.2f);
            nowRun = null;

            if (comboQueue.Count != 0)
            {
                nowRun = comboQueue.Dequeue();
                StartCoroutine(nowRun);
            }
            
            yield return null;
            
        }

        private void Set_Boxremove(DestroyArgs args)
        {
            int num = 0;
            bool is_combo = false;
            int item_num = 0;
            int combo_num = 0;
            int basic_num = (int) ((40) * score_const);
            basic_score += (int)((40) * score_const);
            switch (comboCount)
            {
                default:
                    combo_num = (int)(120f * score_const);
                    excellent_score += (int)(120 * score_const);
                    break;

                case 0:
                case 1:
                case 2:
                    break;
                
                case 3:
                    good_socre += (int)(40f*score_const);
                    combo_num = (int)(40*score_const);
                    Instantiate(combo3);
                    break;
                
                case 4:
                    good_socre += (int)(40f*score_const);
                    combo_num = (int)(40f* score_const);
                    break;

                case 5:
                    great_score += (int)(80f*score_const);
                    combo_num =  (int)(80*score_const);
                    questManager.Set_Combo();
                    Instantiate(combo5);
                    is_combo = true;
                    break;

                case 6:
                case 7:
                    great_score += (int)(80f*score_const);
                    combo_num =  (int)(80f*score_const);
                    break;
                
                case 8:
                    combo_num = (int)(120f * score_const);
                    excellent_score += ((int)(120f * score_const));
                    Instantiate(combo8);
                    break;
            }
            
            item_num = (int)((combo_num+basic_num)*item_const);
            // Step 1. 박스 위치에 점수 획득 prefab 들어가야함 
            Combo_Score.GetChild(0).position = args.pos;
            if (item_const == 0.5f)
            {
                Combo_Score.GetChild(0).GetChild(1).gameObject.SetActive(true);
                Combo_Score.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "+" + item_num.ToString();
            }
            
            else
                Combo_Score.GetChild(0).GetChild(1).gameObject.SetActive(false);
            
            Combo_Score.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "+" + (combo_num+basic_num).ToString(); 
            Combo_Score.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().color =  Set_Color(comboCount);
            Combo_Score.GetChild(0).gameObject.GetComponent<ICombo_Pool>().CanvasOn();
            
            // Step 2. 위에 스코어 텍스트에 점수 반영 
            box_remove++;
            score += (combo_num+item_num+basic_num);
            Set_Bestscore();
            Set_Score_Text(score);
            questManager.Set_Box(args.type);
            questManager.Set_Score(combo_num+item_num+basic_num);

            if (args.pos.y > upperBound && args.pos.y < lowerBound)
            {
                if (warn.Get_Flag())
                    warn.Determine_On();
            }
        }

        #endregion
        
        #region Set_Var
        public void Set_Item_Const(bool is_open)
        {
            if (is_open)
                item_const = 0.5f;

            else
                item_const = 0f;

        }

        public void Set_Count_Zero()
        {
            comboCount = 0;
            combo_z_count = 0;
        }
        
        public void Set_Bestscore()
        {
            if (score >= Best_Score)
            {
                if (!bestScore_level)
                {
                    bestscore.SetActive(true); // 베스트 표시 활성화 
                    Set_BestScore_Color();
                    Best_Score = score;
                    bestScore_level = true;
                }
                // 연출 표시 
            }
        }

        /// <summary>
        /// 이어하기 시 배율을 결정해주는 함수 
        /// </summary>
        /// <param name="stage"></param>
        public void Set_scoreConst_LoadData(int stage)
        {
            if (stage < 21)
                score_const = 1f;
            
            else if (stage >= 21 && stage < 51)
                score_const = 1.5f;
            
            else if (stage >= 51 && stage < 101)
                score_const = 2.0f;
            
            else if(stage >= 101 && stage <201)
                score_const = 2.5f;
            
            else if(stage>=201 && stage<301)
                score_const = 3f;

            else
                score_const = 4f;
        }
        
        
        public void Set_score_const(int stage)
        {
            switch (stage)
            {
                case 21:
                    stageUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        "Stage 21";
                    stageUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text =
                        "Score x1.5";
                    stageUI.transform.gameObject.GetComponent<Animator>().SetTrigger("show");
                    score_const = 1.5f;
                    break;
                
                case 51:
                    stageUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        "Stage 51";
                    stageUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text =
                        "Score x 2";
                    stageUI.transform.gameObject.GetComponent<Animator>().SetTrigger("show");
                    score_const = 2f;
                    break;
                
                case 101:
                    stageUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        "Stage 101";
                    stageUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text =
                        "Score x 2.5";
                    stageUI.transform.gameObject.GetComponent<Animator>().SetTrigger("show");
                    score_const = 2.5f;
                    break;
                
                case 201:
                    stageUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        "Stage 201";
                    stageUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text =
                        "Score x 3";
                    stageUI.transform.gameObject.GetComponent<Animator>().SetTrigger("show");
                    score_const = 3f;
                    break;
                
                case 301:
                    stageUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        "Stage 301";
                    stageUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text =
                        "Score x 4";
                    stageUI.transform.gameObject.GetComponent<Animator>().SetTrigger("show");
                    score_const = 4f;
                    break;
            }
        }
        #endregion
    }
    
    #region Args
    /// <summary>
    /// 박스 파괴시 이벤트를 보내 어떻게 Combo를 Set하는지를 정해주는 class 
    /// </summary>
    public class DestroyArgs: EventArgs
    {
        public Vector2 pos;
        public blocktype type;

        public DestroyArgs(Vector2 pos, blocktype type)
        {
            this.pos = pos;
            this.type = type;
        }
    }
    
    #endregion
}
