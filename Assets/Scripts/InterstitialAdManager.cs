using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class InterstitialAdManager : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnitId = "Interstitial_Android"; //El ID debe coincidir con el del dashboard
    private string adUnitId;
    private int nextSceneIndex = -1;
    void Start() 
    {
        adUnitId = androidAdUnitId;
    }
    public void ShowAdAndLoadScene(int sceneIndex) 
    {
        if (Advertisement.IsReady(adUnitId)) 
        {
            nextSceneIndex = sceneIndex;
            Advertisement.Show(adUnitId, this);
            
        }
        else SceneManager.LoadScene(sceneIndex);
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED) SceneManager.LoadScene(nextSceneIndex);
    }
    public void OnUnityAdsShowStart(string placementId) {}
    public void OnUnityAdsShowClick(string placementId) {}
    public void OnUnityAdsShowFailed(string placementId, UnityAdsShowError error, string message) 
    {
        Debug.LogError($"Ad failed {message}, loading scene anyway");
        SceneManager.LoadScene(nextSceneIndex);
    }
}

