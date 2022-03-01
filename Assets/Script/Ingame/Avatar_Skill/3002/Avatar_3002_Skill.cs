using System;
using System.Collections;
using System.Collections.Generic;
using Box;
using camera;
using Ingame;
using Manager;
using Setting;
using UnityEngine;
using UnityEngine.UI;

public class Avatar_3002_Skill : MonoBehaviour, IAvatarSkill
{
    [Header("Data")]
    [SerializeField] private Transform boxGroup;
    private Camera_Shake _cameraShake;
    private Transform[,] target_Left;
    private Transform[,] target_Right;
    private Transform[,] halfBox;
    private bool is_target_calced;
    private bool list_update = false;
    int row = 0;
    int col = 0;
    private bool dir_bool;
    
    [Header("UI")]
    public GameObject selectPanel;
    public GameObject startAni;
    public GameObject panel_Control;
    public Animator charater_ani;

    public GameObject particle;
        
    [Header("mediator")] private IMediator _mediator;
    
    [Header("Skill_Btn")] public Transform skillBtn;
    private const int target_num = 30; 
    private int count = 11; 
    private bool pos_activate = true;
    private bool firstCount = true;


    [Header("Skill_Data")] 
    private int row_count = 0;
    public bool check_land = false;
    public AudioSource growl_sound;
    public AudioSource landSound;
    public AudioSource distroy_Sound;
    public Animator ground_effect;
    private void Start()
    {
        Init();
    }

    #region Interface_Action
    private  void Init()
    {
        boxGroup = GameObject.FindWithTag("BoxGroup").transform;
        _cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Shake>();
        is_target_calced = false;
        charater_ani = GameObject.FindWithTag("Player").GetComponent<Animator>();
        _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
        charater_ani.gameObject.GetComponent<Charater_Contorller>().groundEffect = this.ground_effect;
        // 프리펩 로드해야함 
    }

    public void Set_Box_Layer(bool is_on)
    {
        if (is_on)
        {
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                if (boxGroup.GetChild(i).gameObject.CompareTag("Box") ||
                    boxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                {
                    boxGroup.GetChild(i).gameObject.GetComponent<IBoxselect>().Activate_Grid();
                }
            }
        }

        else
        {
            for (int i = 0; i < boxGroup.childCount; i++)
            {
                if (boxGroup.GetChild(i).gameObject.CompareTag("Box") ||
                    boxGroup.GetChild(i).gameObject.CompareTag("Obstacle"))
                {
                    boxGroup.GetChild(i).gameObject.GetComponent<IBoxselect>().Deactivate_Grid();
                }
            }
        }
    }
    
    public void OnClick_Skill_Activate()
    {
        is_target_calced = false;
        if(!list_update)
            Set_Box_Target(); // 박스 타깃 계산 

        Set_Box_Layer(true);
        selectPanel.SetActive(true);
        _mediator.Event_Receive(Event_num.Launch_Red);
        panel_Control.SetActive(true);
        Init_SkillBtn();
        // Select panel 활성화 
    }

    // 그리드 선택을 놓으면 start ani 활성화 시킴  
    public void Set_Animation()
    {
        selectPanel.SetActive(false);
        panel_Control.SetActive(false);
        Set_Box_Layer(false);
        growl_sound.Play();
        startAni.gameObject.SetActive(true);  // 캐릭터 화나게 하기 
    }

    /// <summary>
    /// 스킬을 실제로 작동시킴  
    /// </summary>
    public void Activate()
    {
        StartCoroutine(Check_Land());
    }

    IEnumerator Check_Land()
    {
        check_land = false;
        while (true)
        {
            if (check_land)
            {
                landSound.Play(); // 착지 사운드 플레이 
                _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(30f, 0.23f, false, false));
                break;
            }

            yield return null;
        }
        
        if (dir_bool) // 왼쪽을 선택했을때
            StartCoroutine(Remove_LeftBox());

        else
            StartCoroutine(Remove_RightBox());

        check_land = false;
    }

    #region Get_Rid_Of_Box

    IEnumerator Remove_LeftBox()
    {
        row_count = 0;
        Vector2 pos;
        Transform tr;
        for (int i = 7; i >= 0; i--)
        {
            for (int k = 0; k < 4; k++)
            {
                if (halfBox[k, i] != null)
                {
                    tr = halfBox[k, i];
                    halfBox[k, i] = null;
                    k--;
                }

                else
                    tr = target_Left[k, i];
                
                Instantiate(particle,_Determine_Pos.Which_Pos(i, k),Quaternion.identity);
                if (tr && (tr.CompareTag("Box") || tr.CompareTag("Obstacle")))
                {
                    tr.gameObject.GetComponent<IDestroy_Action>().Destroy_Action();

                }
            }

            
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(20f, 0.3f, false, false));
            distroy_Sound.Play();
            row_count++;
            yield return new WaitForSeconds(0.3f);
        }
        _mediator.Event_Receive(Event_num.Launch_Green);
        yield return null;
    }


    IEnumerator Remove_RightBox()
    {
        row_count = 0;
        Vector2 pos;
        Transform tr;
        for (int i = 7; i >= 0; i--)
        {
            for (int k = 4; k < 8; k++)
            {
                if (halfBox[k, i] != null)
                {
                    tr = halfBox[k, i];
                    halfBox[k, i] = null;
                    k--;
                }
                
                else
                    tr = target_Right[k, i];
                
                Instantiate(particle,_Determine_Pos.Which_Pos(i, k),Quaternion.identity);
                if(tr && (tr.CompareTag("Box") || tr.CompareTag("Obstacle")))
                    tr .gameObject.GetComponent<IDestroy_Action>().Destroy_Action();
            }
            distroy_Sound.Play();
            _cameraShake.StartCoroutine(_cameraShake.Shake_Cam(20f, 0.3f, false, false));
            row_count++;
            yield return new WaitForSeconds(0.3f);
        }
        _mediator.Event_Receive(Event_num.Launch_Green);
        yield return null;
    }
    
    #endregion
    

    public void Set_Dir(bool is_left)
    {
        if (is_left)
            dir_bool = true;

        else
            dir_bool = false;
        
    }

    public void Cancle()
    {
        Time.timeScale = 1f;
        Set_Box_Layer(false);
        _mediator.Event_Receive(Event_num.Launch_Green);
        selectPanel.SetActive(false);
        panel_Control.SetActive(false);
    }

    public void Set_count()
    {
        list_update = false;
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

    public void Set_SkillBtn(Transform tr)
    {
        skillBtn = tr;
        skillBtn.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/Skill_icon/Skill_TeddyBear");
        skillBtn.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
        skillBtn.GetChild(1).gameObject.GetComponent<Text>().text = 10.ToString();
        skillBtn.GetChild(0).gameObject.SetActive(true);
        skillBtn.GetChild(1).gameObject.SetActive(true);
        skillBtn.gameObject.GetComponent<Button>().interactable = false;
        tr.gameObject.SetActive(true);
    }
    #endregion

    #region Skill_Btn
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
    #endregion

    #region Skill_Action

    private void Set_Box_Target()
    {
        target_Left = new Transform[8, 8];
        target_Right = new Transform[8, 8];
        halfBox = new Transform[8, 8];
        for (int i = 0; i < boxGroup.childCount; i++)
        {
            int row = 0;
            int col = 0;
            _Determine_Pos.Calc_Which_Grid(ref row, ref col, boxGroup.GetChild(i).position);
            Debug.Log(boxGroup.GetChild(i).position + " row:" + row + "col:" + col);
            if (col < 4)
            {
                if (target_Left[col,row] != null)
                    halfBox[col, row] = boxGroup.GetChild(i);

                else
                    target_Left[col, row] = boxGroup.GetChild(i);

            }

            else
            {
                if(target_Right[col,row]!=null) 
                    halfBox[col, row] = boxGroup.GetChild(i);
                
                else
                    target_Right[col, row] = boxGroup.GetChild(i);

            }

        }
        list_update = true;
    }
    #endregion
}
