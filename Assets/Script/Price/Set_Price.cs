using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상품들의 가격을 지정해주는 class 
/// </summary>
public class Set_Price : MonoBehaviour
{
    public int index;
    public Text priceText;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            priceText.text = String.Format("{0}", Get_Price.GetProduct(index).metadata.localizedPriceString);
        }

        catch
        {
            
        }
    }


}
