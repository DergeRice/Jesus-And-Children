using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
public class ShopPanel : MonoBehaviour
{
    #if UNITY_ANDROID
    private ReviewManager _reviewManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _reviewManager = new ReviewManager();
    }
#endif
    // Update is called once per frame
    void Update()
    {
        
    }

    public void testAndroid()
    {
        // StoreReview.Open();
        #if UNITY_IOS
            UnityEngine.iOS.Device.RequestStoreReview();
            AdsInitializer.instance.RemoveAd(true);
        #elif UNITY_ANDROID
            // StoreReview.Open();
            Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}"); 
            AdsInitializer.instance.RemoveAd(true);
            // StartCoroutine(StartGooglePlayReview());
            // Debug.Log("cll");
        #endif

    }
#if UNITY_ANDROID
    IEnumerator StartGooglePlayReview()
    {
        var reviewManager = new ReviewManager();
 
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
 
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError("requestFlowOperation Error ::" + requestFlowOperation.Error.ToString());
            yield break;
        }
 
        var playReviewInfo = requestFlowOperation.GetResult();
 
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
 
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError("launchFlowOperation Error ::" + launchFlowOperation.Error.ToString());
            yield break;
        }
 
    }
    #endif
}
