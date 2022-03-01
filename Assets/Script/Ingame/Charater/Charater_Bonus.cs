
namespace Avatar
{
    public class Charater_Bonus
    {
        public int bonus_score(int index)
        {
            if (index < Calc_Index.Get_Avatar_Noraml_Target())
            {
                switch (index)
                {
                    default:
                        return 0;
                    
                    case 1:
                        return 10;
                    
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        return 20;
                    
                    case 7:
                    case 8:
                    case 9:
                        return 30;    
                    
                    case 10:
                    case 11:
                    case 12:
                        return 40;   
                    
                    case 13:
                    case 14:
                    case 15:
                        return 50;                  
                    
                    case 16:
                    case 17:
                    case 18:
                        return 70; 
                    
                    case 19:
                        return 100;
                }
                
                
            }
        

            else if (index >= Calc_Index.Get_Avatar_Noraml_Target() && index < Calc_Index.Get_Avatar_Rare_Target())
            {
                switch (Calc_Index.Get_Avatar_Num(index))
                {
                    default:
                        return 150;
                    
                    case 1002:
                    case 1004:
                    case 1005:
                        return 170;
                    
                    case 1003:
                        return 160;
                    
                    case 1006:
                        return 180;
                }
            }

            else
            {
                switch (Calc_Index.Get_Avatar_Num(index))
                {
                    default:
                        return 300;

                }
            }
                

        }
    }
}
