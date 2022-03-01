using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class Tutorial_Basic_Type 
    {
        public string Get_Target_Text(int stage)
        {
            switch (stage)
            {
                default:
                    return null;

                case 1:
                    return "Thank you for coming to my birthday party :)";
                
                case 2:
                    return "Do you Want to open the presents with me?";
                
                case 3:
                    return "Okay Good! I'll tell you how to do";
                
                case 4:
                    return " ";
                
                case 5:
                    return "You Made IT!";
                
                case 6:
                    return "Let's get started!";
            }
            
            
        }

    }
}
