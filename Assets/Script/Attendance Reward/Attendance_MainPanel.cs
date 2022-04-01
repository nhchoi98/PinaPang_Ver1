using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Attendance
{
    public class Attendance_MainPanel : MonoBehaviour
    {
        [SerializeField] private Attendance_UI ui;
        public GameObject leftBtn;
        public Scrollbar scrollbar;
        void Start()
        {
            if (ui.data.Get_is_get(3) || ui.data.Get_is_get(4) )
            {
                leftBtn.SetActive(true);
                scrollbar.value = 1f; 
            }
            
        }


    }
}
