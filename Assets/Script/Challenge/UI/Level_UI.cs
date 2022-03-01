
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Review;
using Unity.Mathematics;

namespace Challenge
{

    public class Trop_Args
    {
        public int _const;

        public Trop_Args(int index)
        {
            _const = index;
        }
    }
    
    public class Level_UI : MonoBehaviour
    {
        public Text level_text;
        public Text exp_info;
        public Slider exp_slider;
        private EXP_DAO leveldata;

        [Header("UI_Ani")] 
        private int exp;
        private int level;
        [SerializeField] private GameObject trop_obj;
        [SerializeField] private GameObject levelup_particle;
        [SerializeField] private QuestManager _questManager;

        public Image medal_img;

        [Header("Tier_Table")] 
        [SerializeField] private Quest_Table_UI QuestTableUI;

        private EXP_List TARGET;

        /// <summary>
        /// 레벨 정보를 UI에 반영해줌 
        /// </summary>
        private void Awake()
        {
            Medal_Img img_data = new Medal_Img();
            leveldata = new EXP_DAO();
            TARGET = new EXP_List();
            level = leveldata.Get_User_Level();
            exp = leveldata.Get_User_EXP();
            var target_value = new EXP_Target();
            if (level < 45)
            {
                level_text.text = "LV." + level.ToString();
                exp_slider.value = (float) (leveldata.Get_User_EXP() / (float) target_value.Get_User_Target(level));
                exp_info.text = leveldata.Get_User_EXP().ToString() + "/" +
                                target_value.Get_User_Target(level).ToString();
            }

            // 만렙을 달성했을 때 
            else
            {
                level_text.text = "LV.MAX";
                exp_slider.value = 1f;
                exp_info.text = "MAX";

            }

            medal_img.sprite = img_data.Img((level-1) / 5);

            if (level == 2 && PlayerPrefs.GetInt("Is_Reviewed", 0) == 0)
            {
                #if UNITY_ANDROID
                    Get_Review review = new Get_Review();
        
                #elif  UNITY_IOS
                    Get_Review_IOS review_ios = new Get_Review_IOS();
                
                #endif

            }
        }
        /// <summary>
        /// 어떤 보상 버튼을 클릭했느냐에 따라서 레벨보상을 UI에 반영해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_User_Exp(int index, int tr_index)
        {
            int target_exp = 0;
            StartCoroutine(Set_User_Ani(index, tr_index)); // 레벨업 할 경우 연출이 들어가야함
        }

        private IEnumerator Set_User_Ani(int index, int tr_index)
        {
            int target_exp = TARGET.Target_return(index); // 몇 개 생성?
            int const_index = 1;
            Vector2 start_pos = Trop_Start_pos(tr_index);
            var ex_level = leveldata.Get_User_Level();
            if (leveldata.Set_User_Exp(index))
            {
                //int type = Get_Reward_type(); 
                //_questManager.Set_Quest(type);
                var level = leveldata.Get_User_Level(); // 계산된 레벨을 다시 불러옴
                
                if (level == 3 && PlayerPrefs.GetInt("Is_Reviewed", 0) == 0)
                {
                    #if UNITY_ANDROID
                        Get_Review review = new Get_Review();
                            
                    #elif  UNITY_IOS
                        Get_Review_IOS review_ios = new Get_Review_IOS();
                                    
                    #endif

                }
                
                for(int i =0; i<level-ex_level; i++)
                    QuestTableUI.LevelUp();
                // 레벨 데이터 다 반영되었으므로 퀘스트 패널 데이터 바꾸어줌 
            }

            switch (target_exp)
            {
                default:
                    const_index = 1;
                    break;
                
                case 30:
                case 40:
                    const_index = 2;
                    break;
                
                case 60:
                    const_index = 3;
                    break;

            }

            for (int i = 0; i < (target_exp/const_index); i++)
            {
                Trop_Flying script;
                GameObject obj = Instantiate(trop_obj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                script = obj.GetComponent<Trop_Flying>();
                script._const = const_index;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-411.2f, 174f);
                // Step 1. 오브젝트를 날려줌 
                yield return null;
            }
        }

        private Vector2 Trop_Start_pos(int index)
        {
            Vector2 target_pos;
            switch (index)
            {
                default:
                    target_pos = new Vector2(318.4f, 91.8f);
                    break;
                
                case 1:
                    target_pos = new Vector2(318.4f, -79.2f);
                    break;
                
                case 2:
                    target_pos = new Vector2(318.4f, -250.2f);
                    break;
                
                case 3:
                    target_pos = new Vector2(318.4f, -421.2f);
                    break;
                
                case 4:
                    target_pos = new Vector2(318.4f, -592.2f);
                    break;
            }
            return target_pos;
        }
        
        
        /// <summary>
        /// ui상에 데이터를 반영해줌 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_text(object sender, Trop_Args e)
        {
            exp+=(e._const);
            var target_value = new EXP_Target();
            GameObject obj;
            if (exp >= target_value.Get_User_Target(level))
            {
                ++level;
                obj = Instantiate(levelup_particle, new Vector2(0f, -59f), quaternion.identity); // 레벨업 연출 
                obj.transform.SetParent(level_text.gameObject.transform);
                Medal_Img img_data = new Medal_Img();
                level_text.text = "LV." + level.ToString();
                if (level == 45)
                {
                    exp_slider.value = 1f;
                    exp_info.text = "MAX".ToString();
                }
                
                else
                {
                    exp_slider.value = (float) (1 / (float) target_value.Get_User_Target(level));
                    exp_info.text = "0/" + target_value.Get_User_Target(level).ToString();
                }
                exp = 0;
                medal_img.sprite = img_data.Img((level-1) / 5);
            }
            
            else
            {
                if (level == 45)
                {
                    level_text.text = "LV." + level.ToString();
                    exp_slider.value = 1f;
                    exp_info.text = "MAX".ToString();
                }
                
                else
                {
                    exp_slider.value = (float) (exp / (float) target_value.Get_User_Target(level));
                    exp_info.text = exp.ToString() +"/"+ target_value.Get_User_Target(level).ToString();
                }
            }
        }
        
        /// <summary>
        /// 아이템 잠금해제시, 어떤 아이템이 잠금 해제되었는지를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        private int Get_Reward_type()
        {
            int return_value;
            switch (leveldata.Get_User_Level())
            {
                default:
                    return_value = -1;
                    break;
                
                case 11:
                    return_value = 2000;
                    break;
                
                case 21:
                    return_value = 2001;
                    break;
                
                case 31:
                    return_value = 2002;
                    break;
            }

            return return_value;
        }

        /// <summary>
        /// 사용자의 점수를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public int Get_Level()
        {
            return level;
        }
    }
}
