using System.Collections;
using System.Collections.Generic;
using log;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Badge_Last_Tutorial : MonoBehaviour, IPointerDownHandler
{
    public AudioSource type_sound;

    private int stage = 0;
        
    [Header("Text_Control")]
    private bool type_touch_pos;
    private bool is_typing;
    private bool jump_text = false;
    [SerializeField] private Text bubbleText;
    public GameObject triangle;
    
    [SerializeField] private Transform charater;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject snackBtn;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (is_typing)
            jump_text = true;
            
        if (type_touch_pos)
        {
            bubbleText.text = null; // text 초기화 
            triangle.gameObject.SetActive(false);
            type_touch_pos = false;
            StartCoroutine(OnType(0.025f, Get_Type_Data(stage++)));
                
        }
            
    }
    
    private void OnEnable()
    {
        type_touch_pos = false;
        is_typing = false;
        StartCoroutine(OnType(0.025f, Get_Type_Data(stage++)));
        charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
        charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
        charater.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel_2";
        charater.gameObject.GetComponent<Animator>().SetTrigger("Happy");
        
    }
    
    IEnumerator OnType(float interval, string Say)
    {
        int typecount = 0;
        bool skip = false;
        bubbleText.text = null; // text 초기화 
        Event_Occur(ref skip);
        is_typing = true;
        if (skip)
            yield break;
            
        for (int i = 0; i < Say.Length; i++) 
        { 
            bubbleText.text = Say.Substring(0, i + 1); 
            yield return new WaitForSecondsRealtime(interval); 
            if (jump_text)
            {
                bubbleText.text = Say.ToString();
                break;
            }

            if (typecount == 1)
            {
                typecount = 0;
                type_sound.Play();
            }

            else
                typecount++;
        }
        jump_text = false;
        is_typing = false;
        yield return new WaitForSeconds(0.2f);
        if(stage !=2)
            triangle.gameObject.SetActive(true);
    }
    
    private void Event_Occur(ref bool return_value)
    {
        switch (stage)
        {
            case 1:
                type_touch_pos = true;
                break;
                
            case 2:
                type_touch_pos = true;
                break;
            
            case 3:
                charater.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
                charater.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
                charater.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object_2";
                PlayerPrefs.SetInt("Tutorial_Snack", 1);
                return_value = true;
                snackBtn.SetActive(true);
                Tutorial_Log.Skin_Tutorial_End();
                parent.SetActive(false);
                break;
                
                
        }
    }
    
    #region Text_Data

    private string Get_Type_Data(int stage)
    {
        switch (stage)
        {
            default:
                break;
                
            case 0:
                return "THANK YOU!";
                
            case 1:
                return "LET'S COLLECT A LOT OF SNACKS !";
        }

        return null;
    }

    #endregion

}
