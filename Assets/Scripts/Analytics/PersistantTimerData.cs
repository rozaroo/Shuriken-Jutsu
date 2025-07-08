using System;
using UnityEngine;

public class PersistantTimerData : MonoBehaviour
{
    private float timer = 0.0f;
    public static PersistantTimerData Instance { get; private set; }
    private bool isRunning = true;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (isRunning) timer += Time.deltaTime;
    }
    public void UploadData()
    {
        // Obtener el user_id desde el GameManager
        //string userId = GameManager.Instance.userId;
        //isRunning = false;
        //AnalyticsManager.instance.gameFinished(timer, userId);
    }
}
