using System;
using Ball;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Skin
{
    public class Ball_Price_Update : MonoBehaviour
    {
        public Transform char_tr;
        private void OnEnable()
        {
            for (int i = 0; i < char_tr.childCount; i++)
            {
                var price = Ball_Price.Price(i);
                if (Calc_Index.Get_Ball_Num(i) /1000 == 1 ||Calc_Index.Get_Ball_Num(i) /1000 == 0 || Calc_Index.Get_Ball_Num(i)/1000  == 3) 
                {
                    if (Playerdata_DAO.Player_Gem() < price)
                        char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.GetComponent<Text>().color = Color.red;

                    else
                        char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.GetComponent<Text>().color = Color.white;

                    char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.SetActive(true);
                    char_tr.GetChild(i).GetChild(7).GetChild(1).gameObject.GetComponent<Text>().text =
                        String.Format("{0:#,0}", Ball_Price.Price(i));
                }

            }
        }
    }
}
