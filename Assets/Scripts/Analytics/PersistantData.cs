using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantData : MonoBehaviour
{
    public static PersistantData Instance { get; private set; }
    private bool dontDestroyOnLoad = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            else Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    public void UploadData()
    {
        //if (GameManager.Instance == null || GameManager.Instance.userId == null)
        //{
            //Debug.LogError("GameManager o userId es null al intentar subir datos.");
            //return;
        //}
        // Obtener el user_id desde el GameManager
        //string userId = GameManager.Instance.userId;

        //AnalyticsManager.instance.playerDeathsPerLevel(deathsinlevelone, CommonEnemy, level1ID, userId);
        //Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {level1ID} fue {deathsinlevelone} por un {CommonEnemy}");
        //AnalyticsManager.instance.playerDeathsPerLevel(deathsinleveltwo, CommonEnemy, levelID, userId);
        //Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinleveltwo} por un {CommonEnemy}");
    }
}
