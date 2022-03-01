

namespace Theme
{
    public class Theme_Purcondition 
    {
        public static int Coin_Condition(int index)
        {
            int condition;
            switch (index)
            {
                default:
                    condition = 50;
                    break;
                case 0 :
                    condition = 0;
                    break;
            }

            return condition;
        }
    }
}
