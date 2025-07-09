using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId = "1234567";
    [SerializeField] bool testMode = true;
    void Start() 
    {
        Advertisement.Initialize(androidGameId, testMode, this);
    }
    public void OnInitializationComplete() 
    {
        Debug.Log("Unity Ads initialized");
    }
}

