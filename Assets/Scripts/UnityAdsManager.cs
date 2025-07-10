using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId = "5897123";
    [SerializeField] bool testMode = true;
    void Start() 
    {
        Advertisement.Initialize(androidGameId, testMode, this);
    }
    public void OnInitializationComplete() 
    {
        Debug.Log("Unity Ads initialized");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}

