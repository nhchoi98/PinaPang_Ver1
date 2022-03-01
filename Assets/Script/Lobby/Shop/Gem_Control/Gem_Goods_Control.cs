using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class Gem_Goods_Control : MonoBehaviour
    {
        public Transform discount_TR;

        private void OnEnable()
        {
            for (int i = 0; i < discount_TR.childCount; i++)
            {
                if(PlayerPrefs.GetInt("Gem_"+i.ToString(),0) == 0)
                    discount_TR.GetChild(i).GetChild(3).gameObject.SetActive(true);
                
                else
                    discount_TR.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
        }
    }
}
