using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public enum ITEM_LIST
    {
        CANDY,
        GEM_1,
        GEM_3,
        GEM_5,
    }
    
    
    public class DailyShop_Item
    {
        private ITEM_LIST _itemList;
        
        public ITEM_LIST Get_Result()
        {
            _itemList =  Get_WhichItem();
            return _itemList;
        }
        
        /// <summary>
        /// 뽑기 시행 결과를 리턴해주는 함수 
        /// </summary>
        public ITEM_LIST Get_WhichItem()
        {
            int rand = UnityEngine.Random.Range(0, 100);
            // 재화에 해당하는 경우 
            if (rand < 70)
            {
                // 캔디 30개에 해당 
                if (rand < 35)
                    return ITEM_LIST.CANDY;

                else
                {
                    if (rand < 53)
                        return ITEM_LIST.GEM_1;
                    
                    else if (rand >= 53 && rand < 65)
                        return ITEM_LIST.GEM_3;

                    else
                        return ITEM_LIST.GEM_5;
                }
            }

            else
                return ITEM_LIST.GEM_5;
        }
    }
}
