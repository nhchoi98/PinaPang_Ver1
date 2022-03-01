
using UnityEngine;

namespace Badge
{
    public class Calc_Badge_Index 
    {
        [Header("Const")]
        private const int NORMAL_MAX = 6; // 노말 배지 개수 
        private const int RARE_MAX = 3;  // 레어 배지 개수 
        private const int UNI_MAX = 3; // 유니크 배지 개수 
        
        public int Get_Badge_Num(int index)
        {
            int return_value = 0;
            if (index < NORMAL_MAX)
                return_value += index;
        
            else if (index >= NORMAL_MAX && index < RARE_MAX + NORMAL_MAX)
                return_value += (1000 + index-NORMAL_MAX);

            else
                return_value += (2000 + index-(NORMAL_MAX+RARE_MAX));
            
            return return_value;
            
        }

        public int Get_Badge_index(int value)
        {
            if (value / 1000 == 0)
                return value;
            
            else if (value / 1000 == 1)
                return (NORMAL_MAX + (value % 1000));

            else
                return (NORMAL_MAX + RARE_MAX+ (value % 1000));
        }
        
        public int Get_Badge_MaxIndex()
        {
            return (NORMAL_MAX + RARE_MAX + UNI_MAX);
        }
        
        public  Sprite Set_Badge_Img(int index)
        {
            int num = Get_Badge_Num(index);
            string path = "Lobby/Badge/Badge_Img/" + num.ToString();
            Sprite Img;
            Img = Resources.Load<Sprite>(path);
            return Img;
        }

        #region name

        
        public string name(int index)
        {
            
            switch (index)
            {
                default:
                    return "Bear Jelly";
                
                case 1:
                    return "Chocolate";
                
                case 2:
                    return "Strawberry Cake";
                
                case 3:
                    return "Colorful Candy";
                
                case 4:
                    return "Chocolate Muffin";
                
                case 1000:
                    return "Sweet Candy";
                
                case 1001:
                    return "Mango Jelly";
                
                case 2000:
                    return "Lollipop";
                
                case 2001:
                    return "Chocochip Cookie";
            }
        }

        #endregion
       
    }
}
