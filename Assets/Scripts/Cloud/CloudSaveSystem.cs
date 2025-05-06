using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class CloudSaveSystem : MonoBehaviour
{
    [Header("UI References")]
    public InputField playerNameInput;
    public Text scoreText;
    public Text levelText;
    public Text syncStatusText;
    public Button saveButton;
    public Button loadButton;

    public Text rankingText;
    
    // Datos del jugador
    private string playerName = "Player";
    private int score = 0;
    private int level = 1;
    
    // Estado de la nube
    private bool isInitialized = false;
    private bool isSaving = false;
    private bool isLoading = false;

    private List<PlayerScore> leaderboard = new List<PlayerScore>();
    private const string leaderboardKey = "leaderboard";
    
    async void Start()
    {
        // Deshabilitar botones hasta que se inicialice
        saveButton.interactable = false;
        loadButton.interactable = false;
        
        // Inicializar Unity Gaming Services
        await InitializeUnityServices();
    }
    
    private async Task InitializeUnityServices()
    {
        try
        {
            syncStatusText.text = "Inicializando...";
            
            // Inicializar Unity Services
            await UnityServices.InitializeAsync();
            
            // Iniciar sesión con autenticación anónima
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            
            syncStatusText.text = "Conectado como: " + AuthenticationService.Instance.PlayerId;
            Debug.Log("Iniciado sesión con ID: " + AuthenticationService.Instance.PlayerId);
            
            // Habilitar botones después de inicializar
            saveButton.interactable = true;
            loadButton.interactable = true;
            isInitialized = true;
            
            // Cargar datos automáticamente al iniciar
            await LoadGameData();
        }
        catch (System.Exception ex)
        {
            syncStatusText.text = "Error al inicializar: " + ex.Message;
            Debug.LogError("Error al inicializar Unity Services: " + ex);
        }
    }
    //Guardar nuevo puntaje
    public async void SaveNewScore(int newScore) 
    {
        if (string.IsNullOrEmpty(playerNameInput.text)) 
        {
            syncStatusText.text = "¡Nombre de jugador requerido!";
            return;
        }
        string playerName = playerNameInput.text;
        //Agregar nuevo puntaje
        leaderboard.Add(new PlayerScore(playerName, newScore));
        //Ordenar y mantener solo top 5
        leaderboard = leaderboard.OrderByDescending(p => p.score).Take(5).ToList();
        await SaveLeaderboard();
        DisplayLeaderboard();
    }
    //Guardar la lista en la nube
    private async Task SaveLeaderboard() 
    {
        if (!isInitialized || isSaving) return;
        isSaving = true;
        syncStatusText.text = "Guardando ranking...";
        try 
        {
            //Serializar lista a JSON
            string jsonData = JsonUtility.ToJson(new PlayerScoreListWrapper { scores = leaderboard });
            var data = new Dictionary<string, object> { { leaderboardKey, jsonData } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            syncStatusText.text = "¡Ranking guardado!";
            Debug.Log("Leaderboard guardado.");
        }
        catch (Exception ex) 
        {
            syncStatusText.text = "Error al guardar: " + ex.Message;
            Debug.LogError("Error al guardar leaderboard: " + ex);
        }
        finally 
        {
            isSaving = false;
        }
    }

    //Cargar la lista de la nube
    public async void LoadLeaderboardButton() 
    {
        await LoadLeaderboard();
        DisplayLeaderboard();
    }

    private async Task LoadLeaderboard() 
    {
        if (!isInitialized || isLoading) return;
        isLoading = true;
        syncStatusText.text = "Cargando ranking...";
        try 
        {
            var keys = new HashSet<string> { leaderboardKey };
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
            if (data.TryGetValue(leaderboardKey, out var leaderboardValue)) 
            {
                string jsonData = leaderboardValue.Value.GetAs<string>();
                //Deserializar JSON
                PlayerScoreListWrapper wrapper = JsonUtility.FromJson<PlayerScoreListWrapper>(jsonData);
                leaderboard = wrapper?.scores ?? new List<PlayerScore>();
                syncStatusText.text = "Ranking cargado.";
                Debug.Log("Leaderboard cargado.");
            }
        }
        catch (Exception ex) 
        {
            syncStatusText.text = "Error al cargar: " + ex.Message;
            Debug.LogError("Error al cargar leaderboard: "+ ex);
        }
        finally 
        {
            isLoading = false;
        }
    }
    //Mostrar ranking en pantalla
    private void DisplayLeaderboard() 
    {
        rankingText.text = "Top 5:\n";
        int rank = 1;
        foreach (var entry in leaderboard)
        {
            rankingText.text += $"{rank}. {entry.playerName} - {entry.score}\n";
            rank++;
        }
    }
    //Wrapper para serializar listas con JsonUtility 
    [Serializable]
    private class PlayerScoreListWrapper 
    {
        public List<PlayerScore> scores;
    }
    public void TryAddNewScore(int newScore, string newPlayerName) 
    {
        playerScores.Add(new PlayerScore { playerName = newPlayerName, score = newScore} );
        playerScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (playerScores.Count > 5) playerScores.RemoveAt(playerScores.Count - 1);
        SaveLeaderboard();
    }



    // Método para guardar datos
    public async void SaveGameDataButton()
    {
        // Verificar si hay un jugador con nombre vacío
        if (string.IsNullOrEmpty(playerNameInput.text))
        {
            syncStatusText.text = "¡Nombre de jugador requerido!";
            return;
        }
        
        playerName = playerNameInput.text;
        await SaveGameData();
    }
    
    private async Task SaveGameData()
    {
        if (!isInitialized || isSaving)
            return;
        
        isSaving = true;
        syncStatusText.text = "Guardando...";
        
        try
        {
            // Crear un diccionario con los datos a guardar
            var playerData = new Dictionary<string, object>
            {
                { "playerName", playerName },
                { "score", score },
                { "level", level },
                { "lastSaved", System.DateTime.UtcNow.ToString() }
            };
            
            // Guardar datos en Unity Cloud Save
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
            
            syncStatusText.text = "¡Datos guardados correctamente!";
            Debug.Log("Datos guardados: " + string.Join(", ", playerData));
        }
        catch (System.Exception ex)
        {
            syncStatusText.text = "Error al guardar: " + ex.Message;
            Debug.LogError("Error al guardar datos: " + ex);
        }
        finally
        {
            isSaving = false;
        }
    }
    
    // Método para cargar datos
    public async void LoadGameDataButton()
    {
        await LoadGameData();
    }
    
    private async Task LoadGameData()
    {
        if (!isInitialized || isLoading)
            return;
        
        isLoading = true;
        syncStatusText.text = "Cargando...";
        
        try
        {
            // Definir las claves que queremos cargar
            var keys = new HashSet<string> { "playerName", "score", "level", "lastSaved" };
            
            // Cargar datos de Unity Cloud Save
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
            
            // Procesar los datos cargados
            if (data.TryGetValue("playerName", out var nameValue))
            {
                playerName = nameValue.Value.GetAs<string>();
                playerNameInput.text = playerName;
            }
            
            if (data.TryGetValue("score", out var scoreValue))
            {
                score = scoreValue.Value.GetAs<int>();
                scoreText.text = "Puntuación: " + score;
            }
            
            if (data.TryGetValue("level", out var levelValue))
            {
                level = levelValue.Value.GetAs<int>();
                levelText.text = "Nivel: " + level;
            }
            
            string lastSaved = "Nunca";
            if (data.TryGetValue("lastSaved", out var lastSavedValue))
            {
                lastSaved = lastSavedValue.Value.GetAs<string>();
            }
            
            syncStatusText.text = "Datos cargados (último guardado: " + lastSaved + ")";
            Debug.Log("Datos cargados correctamente");
        }
        catch (System.Exception ex)
        {
            syncStatusText.text = "Error al cargar: " + ex.Message;
            Debug.LogError("Error al cargar datos: " + ex);
        }
        finally
        {
            isLoading = false;
        }
    }
    
    // Métodos para la demostración
    public void IncrementScore()
    {
        score += 10;
        scoreText.text = "Puntuación: " + score;
    }
    
    public void IncrementLevel()
    {
        level++;
        levelText.text = "Nivel: " + level;
    }
    
    // Método para eliminar datos (para pruebas)
    public async void DeleteCloudData()
    {
        if (!isInitialized)
            return;
        
        try
        {
            syncStatusText.text = "Eliminando datos...";
            
            // Definir las claves que queremos eliminar
            var keys = new List<string> { "playerName", "score", "level", "lastSaved" };
            
            // Eliminar datos de Unity Cloud Save
            await CloudSaveService.Instance.Data.Player.DeleteAsync(keys);
            
            syncStatusText.text = "Datos eliminados de la nube";
            Debug.Log("Datos eliminados correctamente");
        }
        catch (System.Exception ex)
        {
            syncStatusText.text = "Error al eliminar: " + ex.Message;
            Debug.LogError("Error al eliminar datos: " + ex);
        }
    }
}