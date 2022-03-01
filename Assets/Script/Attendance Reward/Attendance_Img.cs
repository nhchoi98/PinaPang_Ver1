using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Attendance_Img : MonoBehaviour
{
    public SpriteAtlas attendance_atlas;
    [SerializeField]
    public Transform imgTR;

    private const int childCount = 20;
    void Awake()
    {
        Transform tr;
        for (int i = 0; i < 20; i++)
        {
            tr = imgTR.GetChild(i);
            tr.GetChild(0).gameObject.GetComponent<Image>().sprite = 
                attendance_atlas.GetSprite("Attendance_List_Collectable_Bg");
            tr.gameObject.GetComponent<Image>().sprite = attendance_atlas.GetSprite("Attendance_List_Bg");
            tr.GetChild(2).gameObject.GetComponent<Image>().sprite =
                attendance_atlas.GetSprite("Attendance_List_Collected_Bg");
            tr.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite=
                attendance_atlas.GetSprite("Attendance_Reward_Check");
            
        }
    }


}
