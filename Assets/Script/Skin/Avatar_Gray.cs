using UnityEngine.UI;
using UnityEngine;

namespace Avatar
{
    public static class Avatar_Gray 
    {
        public static void SetGrayScale(ref Image image, float amount = 0f)
        {
            image.material.SetFloat("_GrayscaleAmount", amount);
        }
        
    }
}
