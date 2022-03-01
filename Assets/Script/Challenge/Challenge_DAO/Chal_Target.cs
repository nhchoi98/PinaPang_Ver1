

namespace Challenge
{
    public class Chal_Target 
    {
        public int Get_Chal_Target(int index)
        {
            var value = 0;
            switch (index)
            {
                default:
                    value = 2;
                    break;
                
                case 1:
                case 16:
                    value = 4;
                    break;
                
                case 2:
                case 17:
                    value = 6;
                    break;
                
                case 3:
                    value = 200;
                    break;
                
                case 4:
                    value = 400;
                    break;
                
                case 5:
                    value = 600;
                    break;
                
                case 6:
                    value = 50;
                    break;
                
                case 7 :
                    value = 100;
                    break;
                
                case 8:
                    value = 150;
                    break;
                
                
                case 9:
                    value = 20;
                    break;
                
                case 10:
                    value = 40;
                    break;
                
                case 11:
                    value = 60;
                    break;
                
                case 12:
                    value = 5000;
                    break;
                
                case 13:
                    value = 10000;
                    break;
                
                case 14:
                    value = 15000;
                    break;
                
                case 18:
                case 24:
                    value = 1;
                    break;
                
                case 19:
                case 25:
                    value = 2;
                    break;
                
                case 20:
                case 26:
                    value = 3;
                    break;
                
                case 21:
                    value = 5;
                    break;
                
                case 22:
                    value = 7;
                    break;
                
                case 23:
                    value = 10;
                    break;
                
                case 27:
                    value = 4;
                    break;
            }

            return value;
        }
        
    }
}
