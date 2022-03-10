using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class Get_Price 
{
    private static IStoreController m_StoreController;

    public static void Set_Store_Controller(ref IStoreController controller)
    {
        m_StoreController = controller;
    }
    
    public static Product GetProduct(int index)
    {
        string _productId = Get_Product_Id(index);
        return m_StoreController.products.WithID(_productId);
    }
    
    

    private static string Get_Product_Id(int index)
    {
        switch (index)
        {
            default:
                throw new Exception();
            
            case 0:
                return "gem_30";
            
            case 1:
                return "gem_70";
            
            case 2:
                return "gem_300";
            
            case 3:
                return "gem_700";
            
            case 4:
                return "gem_1500";
            
            case 5:
                return "gem_3400";
            
            case 6:
                return "starter_pack";
            
            case 7:
                return "no_ads_new";
            
            case 8:
                return "astronaut";
            
            case 9:
                return "party";
            
            case 10:
                return "bear";
            
            case 11:
                return "science";
            
            case 12:
                return "astronaut_sale";
            
            case 13:
                return "party_sale";
            
            case 14:
                return "bear_sale";
            
            case 15:
                return "science_sale";
            
            case 16:
                return "item_package";
        }
        
        
        
    }
    
    
}
