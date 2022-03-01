using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public static class Determine_Sound
    {
       
        public static string Sound_name(int theme_name)
        {
            string path;
            path = "Sound/BGM/" + theme_name + "/";
            
            
            return path;
        }
    }
}
