using System.Collections;
using System.Collections.Generic;
using Data;
using Timer;
using UnityEngine;

namespace Shop
{
    public class Gem_Controller 
    {
        private Gem_Goods gemGoods;
        private Transform package_group;

        public Gem_Controller()
        {
            
            gemGoods = GameObject.FindWithTag("gem_shop").GetComponent<Gem_Goods>();
        }
        
        public void Get_Gem(int index)
        {
            if (index == 101 )
            {
                StarterTimer starterTimer = GameObject.FindWithTag("Starter_Timer").GetComponent<StarterTimer>();
                starterTimer.Purchase(); // 타이머를 꺼줌 
                Package_DataDAO starterData = new Package_DataDAO(0);
                starterData.Set_Data();
            }
            gemGoods.Get_Merchandise_Gem(index);
            
        }
    }
    
}
