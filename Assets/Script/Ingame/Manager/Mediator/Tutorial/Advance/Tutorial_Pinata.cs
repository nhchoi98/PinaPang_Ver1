using System;
using System.Collections;
using System.Collections.Generic;
using log;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_Pinata : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button pauseBtn, itemBtn;
    [SerializeField] private Image pinata_img;
    private bool canGoNext = false;
    private bool jump_text = false;

    public Text descText;
    
    [Header("Text")]
    [SerializeField] private GameObject triangle;
    
    public GameObject parentObj;

    private void Awake()
    {
        Tutorial_Log.Pinata_Tutorial_Start();
        StartCoroutine(Type_Desc());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canGoNext)
        {
            triangle.gameObject.SetActive(false);
            pinata_img.sprite = Resources.Load<Sprite>("Ingame/Tutorial/Ingame_Tutorial_Box");
            StartCoroutine(Type_Desc2());
        }

        if (jump_text)
        {
            pauseBtn.interactable = true;
            itemBtn.interactable = true;
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("Tutorial_Pinata", 1); // 피냐타 튜토리얼 종료 
            Tutorial_Log.Pinata_Tutorial_End();
            parentObj.gameObject.SetActive(false);
        }
    }
    
    IEnumerator Type_Desc()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        triangle.gameObject.SetActive(true);
        canGoNext = true;
    }

    IEnumerator Type_Desc2()
    {
        descText.text = "LET'S BREAK  <color=#DCA90A>PINATA </color>\nAND GET different kind of TOYS :)";
        yield return new WaitForSecondsRealtime(0.5f);
        triangle.gameObject.SetActive(true);
        jump_text  = true;
    }
}
