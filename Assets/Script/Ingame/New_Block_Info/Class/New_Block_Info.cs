using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace NewBlock
{
    public class New_Block_Info
    {
        public Sprite previewImg;
        public String description;
        public String name;
        private Determine_Block_Condition data;
        private bool value = false;
        public New_Block_Info(int type, int blockNum) => Set_InfoActive_Condition(type, blockNum);
        

        private void Set_InfoActive_Condition(int type, int block_num)
        {
            data = new Determine_Block_Condition(block_num,type);
            value = data.Get_Value();

        }

        public bool Get_value()
        {
            return value;
        }
        
        
        /// <summary>
        /// 인포메이션 패널의 UI 요소를 변경해줌 
        /// </summary>
        /// <param name="Obj"></param>

        public void Set_InfoPanel_UI(ref GameObject Obj)
        {
            Obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = data.Get_Name();
            Obj.transform.GetChild(1).gameObject.GetComponent<Text>().text = data.Get_Description();
            Obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = data.Get_Img();
            return;
        }

    }
}
