using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    // Roberto
    public static AnalyticsManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServiceManager.InitializeAsync();
            GiveConsent();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log($"Consent given! We can get the data!!!");
    }

  
    public void ShurikenSelected(int index, string user_id, int count) 
    {
        ShurikenSelectedEvent evt = new ShurikenSelectedEvent
        {
            Index = index,
            usuario_identified = user_id,
            Count = count
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
    
    public void gameFinished(float playTime, string user_id) 
    {
        gameFinishedEvent evt = new gameFinishedEvent
        {
            PlayTime = playTime,
            usuario_identified = user_id
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
}
