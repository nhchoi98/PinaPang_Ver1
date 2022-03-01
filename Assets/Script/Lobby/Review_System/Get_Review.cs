using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Google.Play.Review;
#endif

using UnityEngine;

namespace Review
{
    public class Get_Review
    {
        #if UNITY_ANDROID
        private Google.Play.Review.ReviewManager _reviewManager;
        #endif
        public Get_Review()
        {
#if UNITY_ANDROID
            _reviewManager = new ReviewManager();
            Android_Review();
#endif        
        }

        void Android_Review()
        {
#if UNITY_ANDROID      
            var reviewManager = new ReviewManager();

            // start preloading the review prompt in the background
            var playReviewInfoAsyncOperation = reviewManager.RequestReviewFlow();
            // define a callback after the preloading is done
            playReviewInfoAsyncOperation.Completed += playReviewInfoAsync =>
            {
                if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
                {
                    // display the review prompt
                    var playReviewInfo = playReviewInfoAsync.GetResult();
                    reviewManager.LaunchReviewFlow(playReviewInfo);
                    PlayerPrefs.SetInt("Is_Reviewed", 1);
                }

                else
                {
                    Debug.Log("호출 오류");
                }
            };
#endif
        }
    }
}
