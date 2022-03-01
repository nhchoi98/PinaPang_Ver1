using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;

namespace Skin
{
    public class Avatar_Price_Update : MonoBehaviour
    {
        public Transform char_tr;
        private void OnEnable()
        {
            for (int i = 0; i < char_tr.childCount; i++)
            {
                var price = Avatar_Price(i);
                if (price == 0)
                    char_tr.GetChild(i).GetChild(7).GetChild(3).gameObject.SetActive(true);
                
                else
                    char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.SetActive(true);
                
                if(Playerdata_DAO.Player_Gem()<price)
                    char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.GetComponent<Text>().text
                        = "<color=red>" + String.Format("{0:#,0}", price) +"</color>";
                
                else
                    char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.GetComponent<Text>().text
                        = String.Format("{0:#,0}", price);
            }
        }

        private int Avatar_Price(int index)
        {

            switch (Calc_Index.Get_Avatar_Num(index ))
            {
                default:
                    return 0;
                
                case 0:
                case 1:
                    return 30;
                case 2:
                    return 60;
                
                case 3:
                case 4:
                case 5:
                case 6:
                    return 80;
                
                case 7:
                case 8:
                case 9:
                    return 95;
                
                case 10:
                case 11:
                case 12:
                    return 100;
                
                case 14:
                case 15:
                    return 110;
                
                case 16:
                case 17:
                case 18:
                    return 150;
                
                case 1000:
                    return 0;
                case 1001:
                    return 500;
                
                case 1003:
                    return 550;
                
                case 1004:
                    return 580;
                
                case 2001:
                    return 1000;
                
            }   
        }
    }
}

