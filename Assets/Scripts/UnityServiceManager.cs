using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public static class UnityServiceManager
{
    private static bool isInitialized = false;
    private static Task initTask;

    public static async Task InitializeAsync()
    {
        if (isInitialized) return;

        if (initTask == null)
        {
            initTask = InitializeInternal();
        }

        await initTask;
    }

    private static async Task InitializeInternal()
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("[UnityServicesManager] Signed in anonymously.");
            }
        }

        isInitialized = true;
        Debug.Log("[UnityServicesManager] Unity Services initialized.");
    }
}
