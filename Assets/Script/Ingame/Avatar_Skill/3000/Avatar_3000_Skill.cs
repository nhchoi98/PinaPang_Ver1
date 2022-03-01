using System;
using System.Collections;
using System.Collections.Generic;
using Block;
using Box;
using camera;
using Manager;
using Setting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ingame
{
    /// <summary>
    /// 우주비행사의 스킬을 담당함 
    /// </summary>
    public class Avatar_3000_Skill : MonoBehaviour, IAvatarSkill
    {
        [Header("Data")]
        [SerializeField] private Transform boxGroup;
        private Camera_Shake _cameraShake;
        private Transform[,] targetList; // 부실 박스가 있는지 없는지를 저장하는 MATRIX
        private Transform[,] targetList_second;
        private List<Transform> target;
        private List<Transform> target_second ;
        private bool is_target_calced;
        private bool firstCount = true;
        int row = 0;
        int col = 0;
        
        [Header("UI")]
        public GameObject selectPanel;
        public GameObject startAni;
        public GameObject panel_Control;
        
        [Header("mediator")] private IMediator _mediator;

        [Header("Rockect_Pool")] 
        public GameObject misslie;
        public Transform rocketPool;
        private int rocket_target_num = 0;
        private int rocket_count = 0;

        [Header("Skill_Btn")] public Transform skillBtn;
        private const int target_num = 30; 
        private int count = 11;  //11
        private bool pos_activate = true;
        private event EventHandler rocket_event;

        private void Start()
        {
            Init();
        }
        
        public void Set_SkillBtn(Transform tr)
        {
            skillBtn = tr;
            skillBtn.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Astronaut");
            skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = 10.ToString();
            skillBtn.GetChild(0).gameObject.SetActive(true);
            skillBtn.GetChild(1).gameObject.SetActive(true);
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            tr.gameObject.SetActive(true);
        }

        public void OnClick_Skill_Activate()
        {
            is_target_calced = false;
            Set_Box_Target(); // 박스 타깃 계산 
            selectPanel.SetActive(true);
            _mediator.Event_Receive(Event_num.Launch_Red);
            panel_Control.SetActive(true);
            // Select panel 활성화 
        }

        private void Init_SkillBtn()
        {
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            skillBtn.GetChild(0).gameObject.SetActive(true);
            skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = 30.ToString();
            skillBtn.GetChild(1).gameObject.SetActive(true);
            count = 30; 
            pos_activate = false;
            
        }

        #region Animation
        IEnumerator ShowUp_Message()
        {
            // 로켓풀 할당하는 코루틴 실행 
            rocket_target_num = target.Count+target_second.Count;
            Set_Rocket();
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(5f, 2f, true, true));
            StartCoroutine(Launch_End_Standby());
            startAni.SetActive(true);// 이미지 띄우기
            yield return null;
        }
        
        public IEnumerator Launch_Rocket()
        {
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(15f, 0.2f, false, true));
            for (int i = 0; i < rocketPool.childCount; i++)
            {
                rocketPool.GetChild(i).gameObject.SetActive(true);
            }
            
            yield return null;
        }

        IEnumerator Launch_End_Standby()
        {
            while (true)
            {
                if (rocket_count == rocket_target_num)
                    break;
                
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            rocket_target_num = 0;
            rocket_count = 0;
            _mediator.Event_Receive(Event_num.Launch_Green);
            panel_Control.SetActive(false);
            // 지금 턴 부터 제한 카운트 시작! 
        }
        
        public void Set_Animation()
        {
            StartCoroutine(ShowUp_Message());
        }
        #endregion

        #region Rocket_Event

        private void Target_Destroy(object obj, EventArgs arge)
        {
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(60f, 0.1f, false, true));
            Vibration.Vibrate(200);
            rocket_count++;
        }
        private void Set_Rocket()
        {
            Vector2 pos;
            Rocket rocket;
            int arc_rand;
            for (int i = 0; i < target.Count; i++)
            {
                Debug.Log("진입");
                pos  = misslePos();
                GameObject obj = Instantiate(misslie, pos, Quaternion.identity);
                rocket = obj.GetComponent<Rocket>();
                rocket.targetPos = target[i];
                arc_rand = UnityEngine.Random.Range(0, 2);
                if (arc_rand == 0)
                {
                    arc_rand = UnityEngine.Random.Range(1, 3);
                    rocket.arc_const = (-0.25f) * arc_rand;
                }

                else
                {
                    arc_rand = UnityEngine.Random.Range(1, 3);
                    rocket.arc_const = (0.25f) * arc_rand;
                }

                rocket.m_Speed = UnityEngine.Random.Range(700, 1100);
                rocket.m_HeightArc = UnityEngine.Random.Range(200, 350);
                rocket.rocket_event += rocket_event;
                obj.transform.SetParent(rocketPool);
            }
            
            for (int i = 0; i < target_second.Count; i++)
            {
                pos  = misslePos();
                GameObject obj = Instantiate(misslie, pos, Quaternion.identity);
                rocket = obj.GetComponent<Rocket>();
                rocket.targetPos = target_second[i];
                arc_rand = UnityEngine.Random.Range(0, 2);
                if (arc_rand == 0)
                {
                    arc_rand = UnityEngine.Random.Range(1, 3);
                    rocket.arc_const = (-0.25f) * arc_rand;
                }

                else
                {
                    arc_rand = UnityEngine.Random.Range(1, 3);
                    rocket.arc_const = (0.25f) * arc_rand;
                }

                rocket.m_Speed = UnityEngine.Random.Range(700, 1100);
                rocket.m_HeightArc = UnityEngine.Random.Range(200, 350);
                rocket.rocket_event += rocket_event;
                obj.transform.SetParent(rocketPool);
            }
            
        }

        private Vector2 misslePos()
        {
            int X_OFFSET = 30;
            int X_START =  -30;
            int Y_VALUE = -1025;
            int rand = UnityEngine.Random.Range(0, 3);
            Vector2 pos = new Vector2((X_START + (X_OFFSET * rand)), Y_VALUE);
            return pos;
        }

        #endregion

        #region Action
        private void Set_Box_Target()
        {
            if (!is_target_calced)
            {
                int row = 0;
                int col = 0;
                targetList = new Transform[7, 8];
                targetList_second = new Transform[7, 8];
                IBox ibox;
                // Step 1. 타깃팅 가능한 박스가 있는지 확인 
                for (int i = 0; i <boxGroup.childCount; i++)
                {
                    if (boxGroup.GetChild(i).gameObject.CompareTag("Box") || boxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                    {
                        ibox = boxGroup.GetChild(i).gameObject.GetComponent<IBox>();
                        boxGroup.GetChild(i).gameObject.GetComponent<IBoxselect>().Activate_Grid();
                        _Determine_Pos.Calc_Which_Grid(ref row, ref col, boxGroup.GetChild(i).position);
                        if (ibox.whichRow() == 0)
                            continue;

                        if (targetList[ibox.whichRow() - 1, col] != null)
                            targetList_second[ibox.whichRow() - 1, col] = boxGroup.GetChild(i);
                        
                        else
                            targetList[ibox.whichRow()-1, col] = boxGroup.GetChild(i);
                    }
                }

                is_target_calced = true;
            }
        }

        // 총 10개의 타깃을 선정함
        private void Determine_Target()
        {
            int col_lowerbound= col-2;
            int col_upperbound = col+2;
            int row_lowerbound = row -2;
            int row_upperbound = row +2;

            for (int i = 0; i < boxGroup.childCount; i++)
            {
                if(boxGroup.GetChild(i).gameObject.CompareTag("Box") ||boxGroup.GetChild(i).gameObject.CompareTag("Obstacle") )
                    boxGroup.GetChild(i).gameObject.GetComponent<IBoxselect>().Deactivate_Grid();

            }

            selectPanel.SetActive(false);
            
            target= new List<Transform>();
            target_second = new List<Transform>();
            if (row_lowerbound < 1)
                row_lowerbound = 1;

            if (row_upperbound > 7)
                row_upperbound = 7;

            if (col_lowerbound < 0)
                col_lowerbound = 0;

            if (col_upperbound > 7)
                col_upperbound = 7;

            for (int i = row_lowerbound; i <= row_upperbound; i++)
            {
                for (int k = col_lowerbound; k <= col_upperbound; k++)
                {
                    if(targetList_second[i-1,k]!=null)
                        target_second.Add(targetList[i-1, k]);
                    
                    if(targetList[i-1,k] !=null)
                        target_second.Add(targetList[i-1,k]);

                }
            }

            Set_Animation();
            // 애니메이션 시작 

        }

        private  void Init()
        {
            boxGroup = GameObject.FindWithTag("BoxGroup").transform;
            _cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Shake>();
            is_target_calced = false;
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            rocket_event += Target_Destroy;
            // 프리펩 로드해야함 
        }

        public void Cancle()
        {
            Time.timeScale = 1f;
            _mediator.Event_Receive(Event_num.Launch_Green);
            selectPanel.SetActive(false);
            panel_Control.SetActive(false);
            for (int i = 0; i <boxGroup.childCount; i++)
            {
                if (boxGroup.GetChild(i).gameObject.CompareTag("Box") || boxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                    boxGroup.GetChild(i).gameObject.GetComponent<IBoxselect>().Deactivate_Grid();
                
            }
        }

        public void Set_count()
        {
            if (!pos_activate)
            {
                --count;
                if (count != 0)
                {
                    skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = count / (float) target_num;
                    skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = count.ToString();
                }

                else
                {
                    pos_activate = true;
                    skillBtn.GetChild(0).gameObject.SetActive(false);
                    skillBtn.GetChild(1).gameObject.SetActive(false);
                    skillBtn.gameObject.GetComponent<Button>().interactable = true;
                    skillBtn.gameObject.GetComponent<Animator>().SetTrigger("interactable");
                }
            }
            
            else if (firstCount)
            {
                --count;
                if (count != 0)
                {
                    skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = count / (float) 10;
                    skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = count.ToString();
                }

                else
                {
                    firstCount = false;
                    skillBtn.GetChild(0).gameObject.SetActive(false);
                    skillBtn.GetChild(1).gameObject.SetActive(false);
                    skillBtn.gameObject.GetComponent<Button>().interactable = true;
                    skillBtn.gameObject.GetComponent<Animator>().SetTrigger("interactable");
                }
            }
        }
        
        public void Activate()
        {
            Time.timeScale = 1f;
            Determine_Target();
            Init_SkillBtn();
        }

        public bool Get_Target_List(int row, int col)
        {
            return targetList[row-1, col];

        }
        
        public void Set_rowcol_data(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        #endregion
       

    }
}
