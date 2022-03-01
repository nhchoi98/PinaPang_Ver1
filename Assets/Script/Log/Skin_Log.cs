
using UnityEngine;
using Firebase.Analytics;

namespace Skin
{
    public class Skin_Log : MonoBehaviour
    {
        #region Skin
        public static void Buy_Avatar(int index)
        {
            int num = Calc_Index.Get_Avatar_Num(index);

            switch (num)
            {
                default:
                    return;


                case 1:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Minx"));
                    }

                    catch
                    {
                    }
                    break;

                case 2:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Tennis Boy"));
                    }

                    catch
                    {
                    }

                    break;

                case 3:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","BaseBall Boy"));
                    }

                    catch
                    {
                    }

                    break;

                case 4:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","BaseBall Boy"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 5:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Basketball Boy"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 6:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Marine Boy"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 7:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Scamp"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 8:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Firefighter"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 9:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Magician"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 10:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Dracula"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 11:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Ninja"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 12:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Nerd"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 13:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Scientist"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 14:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Secret Agent"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 15:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Disco Singer"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 16:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Noblesse"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 17:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Silent Comedian"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 18:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Ballerina"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 19:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Emperor"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 1000:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Baby Driver"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 1001:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Clawn"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 1003:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Pizza Boy"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 1004:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Stone"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 2001:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Avatar_buy",new Parameter("AVATAR_name","Pajama"));
                    }

                    catch
                    {
                    }
                    break;
            }


        }
        #endregion

        #region Ball
        public static void Buy_Ball_Log(int index)
        {
            int num = Calc_Index.Get_Ball_Num(index);
            switch (num)
            {
                case 1000:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Tennis"));
                    }

                    catch
                    {
                    }
                    break;
                
                case 1001:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","BaseBall"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                
                case 1002:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Basketball"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1003:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Beach Ball"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1004:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Metal Bead"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1005:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Juggling ball"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1006:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Earth"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1007:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Planet"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1008:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Steering Wheel"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1009:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Ball 8"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1010:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Hallabong"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1011:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Pizza"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1012:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Banana"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1013:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Golden Ball"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1014:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Jack O Lantern"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1015:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Ship's Wheel"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1016:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Shuriken"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1017:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Water Bubble"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1018:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Moon"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1019:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Apple"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1020:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Heart"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1021:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Candy"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 1022:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Red Onion"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3000:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Dalgona"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3001:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Toe Beans"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3002:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Smile"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3003:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Cute Devil"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3004:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Rose"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3005:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Hamburger"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3006:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","French Fries"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3007:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Cookie"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3008:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Ladybug"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3009:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Skull"));
                    }

                    catch
                    {
                        
                    }
                    break;
                
                case 3010:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Ball_buy",new Parameter("Ball_Name","Cherry Blossom"));
                    }

                    catch
                    {
                        
                    }
                    break;
            }

        }
        #endregion

        #region Line

        public static void Buy_Line(int index)
        {
            switch (index)
            {
                default:
                    return;


                case 1:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Dot"));
                    }

                    catch
                    {
                    }

                    break;


                case 2:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Red"));
                    }

                    catch
                    {
                    }

                    break;

                case 3:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Pink"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 4:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Black"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 5:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "BaseBall"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 6:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Sand"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 7:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Road"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 8:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Pizza Topping"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 9:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Formula"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 10:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Rabbit Footprint"));
                    }

                    catch
                    {
                    }

                    break;
                
                case 11:
                    try
                    {
                        FirebaseAnalytics.LogEvent("Line_buy", new Parameter("Line_Name", "Red Carpet"));
                    }

                    catch
                    {
                    }

                    break;
            }
        }


        #endregion
    }
}
