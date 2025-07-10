using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class InterstitialAdManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
{
    [SerializeField] string androidAdUnitId = "Interstitial_Android"; //El ID debe coincidir con el del dashboard
    private string adUnitId;
    private int nextSceneIndex = -1;
    private bool adLoaded = false;
    void Start() 
    {
        adUnitId = androidAdUnitId;
        LoadAd();
    }
    public void ShowAdAndLoadScene(int sceneIndex) 
    {
        nextSceneIndex = sceneIndex;
        if (adLoaded) Advertisement.Show(adUnitId, this);
        else SceneManager.LoadScene(sceneIndex);
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED) 
        {
            SceneManager.LoadScene(nextSceneIndex);
            LoadAd(); //Prepara el siguiente anuncio
        }
        else if (placementId == adUnitId) SceneManager.LoadScene(nextSceneIndex); //Tambien carga si se cerro antes
        
    }
    public void OnUnityAdsShowStart(string placementId) {}
    public void OnUnityAdsShowClick(string placementId) {}
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) 
    {
        Debug.LogError($"Ad failed {message}, loading scene anyway");
        SceneManager.LoadScene(nextSceneIndex);
        LoadAd(); //Intenta cargar uno nuevo
    }
    public void LoadAd()
    {
        adLoaded = false;
        Advertisement.Load(adUnitId, this);
    }
    public void OnUnityAdsAdLoaded(string placementId) 
    {
        if (placementId == adUnitId) adLoaded = true;
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad: {error.ToString()} - {message}");
        adLoaded = false;
    }
}

