using UnityEngine;

namespace Skin
{
    public class Ball_Name 
    {
        public  string Set_Ball_Name(int index)
        {
            int ball_num = Calc_Index.Get_Ball_Num(index);
            switch (ball_num/1000)
            {
                default:
                    return default_ball(ball_num % 1000);
                
                case 1:// 그냥 구매해서 사는 공 
                    return Normal_Ball(ball_num % 1000);
                
                case 3:
                    return Level_Ball(ball_num % 1000);
                
                case 4:
                    return Package_Ball(ball_num % 1000);
                    
            }

        }
        
        private string default_ball(int ball_num)
        {
            string name;
            switch (ball_num)
            {
                default:
                    name = "Ball";
                    break;
                
            }
            return name;
        }

        private string Normal_Ball(int ball_num)
        {
            string name;
            switch (ball_num)
            {
                default:
                    name = "Tennis Ball";
                    break;
                
                case 1:
                    name = "BaseBall";
                    break;
                
                case 2:
                    name = "BasketBall";
                    break;
                
                case 3:
                    name = "Beach Ball";
                    break;
                
                case 4:
                    name = "Metal Bead";
                    break;
                
                case 5:
                    name = "Juggling Ball";
                    break;
                
                case 6:
                    name = "Earth";
                    break;
                
                case 7:
                    name = "Planet";
                    break;

                case 8:
                    name = "Ball 8";
                    break;
                
                case 9 :
                    name = "Hallabong";
                    break;
                
                case 10:
                    name = "Pizza";
                    break;
                
                case 11:
                    name = "Banana";
                    break;

                case 12:
                    name = "Golden Ball";
                    break;
                
                case 13:
                    name = "Jack O Lantern";
                    break;
                
                case 14:
                    name = "Ship's Wheel";
                    break;
                
                case 15:
                    name = "Shuriken";
                    break;
                
                case 16:
                    name = "Water Bubble";
                    break;
                
                case 17:
                    name = "Moon";
                    break;
                
                case 18 :
                    name = "Apple";
                    break;
                
                case 19 :
                    name = "Heart";
                    break;
                
                case 20 :
                    name = "Candy";
                    break;
                
                case 21 :
                    name = "Red Onion";
                    break;
                
                case 22 :
                    name = "French Fries";
                    break;
                
                case 23 :
                    name = "Skull";
                    break;
                
                case 24 :
                    name = "Cookie";
                    break;
                
                case 25 :
                    name = "Ladybug";
                    break;
            }

            return name;
        }

        private string Level_Ball(int ball_num)
        {
            switch (ball_num)
            {
                default:
                    return "Dalgona";
                
                case 1:
                    return "Toe Beans";
                
                case 2:
                    return "Smile";
                
                case 3:
                    return "imp";
                
                case 4:
                    return "Rose";
                
                case 5:
                    return "Hamburger";

                case 6:
                    return "Cherry Blossom";
                
                case 7:
                    return "Wine";
                
                case 8:
                    return "Steering Wheel";
            }
            
        }
        
        private string Package_Ball(int ball_num)
        {
            switch (ball_num)
            {
                default:
                    return "";
                case 0:
                    return "Mirror Ball";
                
                case 1:
                    return "Honey Jar";
                
                case 2:
                    return "Star";
                
                case 3:
                    return "flask";
            }
            
        }


        public Sprite Get_Ad_Img()
        {
            Sprite img = Resources.Load<Sprite>("Lobby/UI/Skin/Skin_Ad_Icon");
            return img;
        }

        public Sprite Get_Exp_img()
        {
            Sprite img = Resources.Load<Sprite>("Lobby/Challenge/Background/Challenge_Medal");
            return img;
        }
    }
}
