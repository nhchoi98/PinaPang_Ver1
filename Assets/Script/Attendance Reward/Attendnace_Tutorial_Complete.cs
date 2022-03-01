using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Attendnace_Tutorial_Complete : MonoBehaviour
{
    [SerializeField] private GameObject collectionBtn;
    [SerializeField] private GameObject attendanceBtn;
    [SerializeField] private GameObject skinBtn;
    [SerializeField] private GameObject questBtn;
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private GameObject bestScore;
    private Attendnace_Tutorial_Complete _tutorialComplete;
    private int which_stage = 0;
    private const int targetStage = 5;
    void Start()
    {
        _tutorialComplete = this;
        StartCoroutine(Button_Control(10f));
    }

    IEnumerator Button_Control( float speed)
    {
        GameObject whichBtn;
        Transform tr;
        Animator animator = null;
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        whichBtn = Set_Button(); // 버튼을 할당받는다. 
        tr = whichBtn.transform;
        if (which_stage != 5)
        {
            animator = whichBtn.GetComponent<Animator>();
            animator.enabled = false;
        }

        tr.localScale = Vector3.zero; // 먼저 크기를 0으로 줄여줌 
        tr.gameObject.SetActive(true);
        
        while (true) // 크기를 목표 크기 까지 키워줌 
        {
            tr.localScale = Vector3.Lerp(tr.localScale, targetScale, Time.deltaTime * speed);
            if ((tr.localScale.x > 0.99))
            {
                tr.localScale = targetScale;
                if (which_stage != 5)
                    animator.enabled = true;
                
                break;
            }
            yield return null;
        }

        if (which_stage == 0)
        {
            if (PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 1)
                ++which_stage;

            else
                which_stage += 2;
        }

        else
            ++which_stage;

        if (which_stage > targetStage)
        {
            _tutorialComplete.enabled = false;
            yield break;
        }

        else
            yield return StartCoroutine(Button_Control(10f));
    }

    private GameObject Set_Button()
    {
        switch (which_stage)
        {
            case 0:
                return attendanceBtn;
            
            case 1:
                return collectionBtn;
            
            case 2:
                return skinBtn;
            
            case 3:
                return questBtn;
            
            case 4:
                return shopBtn;
            
            case 5:
                return bestScore;
            
            default:
                throw new Exception();
        }
        
        
        
    }
}
