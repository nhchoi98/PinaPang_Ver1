
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Theme
{
    public class ColorManager
    {
        private int index;

        public ColorManager(int index) => this.index = index;
    
        public Color Set_Box_Color()
        {
            Color color;
            switch (index)
            {
                default:
                    color = new Color(255f / 255f, 174f / 255f, 0f);
                    break;

                case 1:
                    color = new Color(255f / 255f, 173f / 255f, 215f / 255f);
                    break;
            }

            return color;
        }


        public  Color[] Set_Particle()
        {
            Color[] colorset = new Color[3];
            switch (index)
            {
                default:
                    colorset[0] = (new Color(211f/255f, 182f/255f, 80f/255f));
                    colorset[1] = (new Color(249f/255f, 182f/255f, 75f/255f));
                    colorset[2] = (new Color(247f/255f, 145f/255f, 49f/255f));
                    break;
                
                case 1:
                    colorset[0] = (new Color(254f/255f, 190f/255f, 183f/255f));
                    colorset[1] = (new Color(250f/255f, 156f/255f, 187f/255f));
                    colorset[2] = (new Color(211f/255f, 182f/255f, 209f/255f));
                    break;
            }

            return colorset;
        }
    }
}
