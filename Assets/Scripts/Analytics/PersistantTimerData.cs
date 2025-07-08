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
    // Función para detener timer y subir datos
    public void UploadData()
    {
        isRunning = false;
        MainMenuManager menuManager = FindObjectOfType<MainMenuManager>();
        string userId = menuManager.userId;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("[PersistantTimerData] No se encontró userId para subir datos.");
            return;
        }
        AnalyticsManager.Instance?.gameFinished(timer, userId);
        Debug.Log($"[PersistantTimerData] Tiempo total jugado: {timer} segundos, usuario: {userId}");
    }
}
