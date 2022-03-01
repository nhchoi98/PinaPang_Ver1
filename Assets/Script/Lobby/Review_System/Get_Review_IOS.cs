using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Review
{
    public class Get_Review_IOS 
    {
        public Get_Review_IOS()
        {
            Call();
        }
        
        public void Call()
        {
            iOSReviewRequest.Request();

        }
    }
}
