using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using TMPro;


public class CloudSaveSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI syncStatusText;

    public TextMeshProUGUI rankingText;
    
    // Datos del jugador
    private string playerName;
    private int score;
    private int level = 1;
    public TextMeshProUGUI leaderboardtext;
    
    // Estado de la nube
    private bool isInitialized = false;
    private bool isSaving = false;
    private bool isLoading = false;

    private List<PlayerScore> leaderboard = new List<PlayerScore>();
    private const string leaderboardKey = "leaderboard";
    public static CloudSaveSystem Instance;

    // Configuración de encriptación
    private string secretKey = "yjd7HnM90!xpQw54";

    private async void Awake() 
    {
        InitializeUnityServices();
        //if (Instance == null) 
        //{
         //   Instance = this;
            //DontDestroyOnLoad(gameObject);
            //InitializeUnityServices();
        //}
        //else Destroy(gameObject);
        if (playerNameInput == null) Debug.Log("Null");
        if (scoreText == null) Debug.Log("Null");
        if (levelText == null) Debug.Log("Null");
        if (syncStatusText == null) Debug.Log("Null");
        if (rankingText == null) Debug.Log("Null");
    }

    private async void InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn) 
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in anonymously");
        }
        isInitialized = true;
        await LoadGameData();
        await LoadLeaderboard();
        DisplayLeaderboard();
        Debug.Log("Unity Services Initialized");
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
        // Encriptar nombre y score
        string encryptedName = Convert.ToBase64String(EncryptData(playerName, secretKey));
        string encryptedScore = Convert.ToBase64String(EncryptData(newScore.ToString(), secretKey));
        //Agregar nuevo puntaje
        leaderboard.Add(new PlayerScore(encryptedName, encryptedScore));
        //Ordenar y mantener solo top 5
        leaderboard = leaderboard.OrderByDescending(p => int.Parse(DecryptData(Convert.FromBase64String(p.score), secretKey))).Take(5).ToList();
        Debug.Log($"Player: {playerName} Score: {newScore}");
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
        //syncStatusText.text = "Cargando ranking...";
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
                //syncStatusText.text = "Ranking cargado.";
                Debug.Log("Leaderboard cargado.");
            }
        }
        catch (Exception ex) 
        {
            //syncStatusText.text = "Error al cargar: " + ex.Message;
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
        leaderboardtext.text = ""; // Limpia el texto anterior
        foreach (var entry in leaderboard) 
        {
            string decryptedName = DecryptData(Convert.FromBase64String(entry.playerName), secretKey);
            string decryptedScore = DecryptData(Convert.FromBase64String(entry.score), secretKey);
            if (!string.IsNullOrEmpty(decryptedName) && !string.IsNullOrEmpty(decryptedScore) && leaderboardtext != null) leaderboardtext.text += $"{decryptedName} - {decryptedScore}\n";
            else leaderboardtext.text += $"[Datos corruptos o mal encriptados]\n";
        }
    }
    //Wrapper para serializar listas con JsonUtility 
    [Serializable]
    private class PlayerScoreListWrapper 
    {
        public List<PlayerScore> scores;
    }
    public async Task AddNewScore(string newPlayerName, int newScore) 
    {
        string encryptedName = Convert.ToBase64String(EncryptData(newPlayerName, secretKey));
        string encryptedScore = Convert.ToBase64String(EncryptData(newScore.ToString(), secretKey));
        leaderboard.Add(new PlayerScore (encryptedName, encryptedScore));
        leaderboard.Sort((a, b) => int.Parse(DecryptData(Convert.FromBase64String(b.score), secretKey)).CompareTo(int.Parse(DecryptData(Convert.FromBase64String(a.score), secretKey))));
        if (leaderboard.Count > 5) leaderboard.RemoveAt(leaderboard.Count - 1);
        await SaveLeaderboard();
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
            string encryptedName = Convert.ToBase64String(EncryptData(playerName, secretKey));
            string encryptedScore = Convert.ToBase64String(EncryptData(score.ToString(), secretKey));

            // Crear un diccionario con los datos encryptados a guardar
            var playerData = new Dictionary<string, object>
            {
                { "playerName", encryptedName },
                { "score", encryptedScore },
                { "level", level },
                { "lastSaved", DateTime.UtcNow.ToString() }
            };
            
            // Guardar datos en Unity Cloud Save
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
            
            
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
    
    private async Task LoadGameData() //Desde aca debe cargar los datos
    {
        if (!isInitialized || isLoading) return;
        isLoading = true;
        try
        {
            // Definir las claves que queremos cargar
            var keys = new HashSet<string> { "playerName", "score" };
            
            // Cargar datos de Unity Cloud Save
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
            
            // Procesar los datos cargados
            if (data.TryGetValue("playerName", out var nameValue))
            {
                byte[] encryptedBytes = Convert.FromBase64String(nameValue.Value.GetAs<string>());
                playerName = DecryptData(encryptedBytes, secretKey);
                playerNameInput.text = playerName;
            }
            
            if (data.TryGetValue("score", out var scoreValue))
            {
                byte[] encryptedBytes = Convert.FromBase64String(scoreValue.Value.GetAs<string>());
                string decryptedScoreStr = DecryptData(encryptedBytes, secretKey);
                if (int.TryParse(decryptedScoreStr, out int decryptedScore)) 
                {
                    score = decryptedScore;
                    scoreText.text = "Puntaje: " + score; 
                }
            }
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
    
 
    [Serializable]
    public class PlayerScoreList 
    {
        public List<PlayerScore> scores = new List<PlayerScore>();
    }
    public async Task SaveScores(List<PlayerScore> scores) 
    {
        PlayerScoreList scoreList = new PlayerScoreList { scores = scores };
        string jsonString = JsonUtility.ToJson(scoreList);
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> 
        {
            { leaderboardKey, jsonString }
        });
        Debug.Log("Scores saved to Cloud Save");
    }
    //Cargar lista de puntajes desde la nube
    public async Task<List<PlayerScore>> LoadScores() 
    {
        var saveData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{ leaderboardKey });
        if (saveData.TryGetValue(leaderboardKey, out var cloudData)) 
        {
            string jsonString = cloudData.ToString();
            PlayerScoreList scoreList = JsonUtility.FromJson<PlayerScoreList>(jsonString);
            Debug.Log("Scores loaded from Cloud Save");
            return scoreList.scores;
        }
        else 
        {
            Debug.Log("No scores found in Cloud Save");
            return new List<PlayerScore>();
        }
    }/*
    public async Task AddNewScore(string playerName, int score) 
    {
        List<PlayerScore> currentScores = await LoadScores();
        PlayerScore newScore = new PlayerScore(playerName = playerName, score = score);
        currentScores.Add(newScore);
        currentScores = currentScores.OrderByDescending(ps => ps.score).ToList();
        await SaveScores(currentScores);
    }*/
    public async Task<int> GetBestScores()
    {
        var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "BestScore" });
        if (savedData.TryGetValue("BestScore", out var bestScoreStr))
        {
            int bestScore = int.Parse(bestScoreStr);
            return bestScore;
        }
        else return 0;
    }
    //Encryptar
    private byte[] EncryptData(string data, string key)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (SHA256 sha256 = SHA256.Create()) keyBytes = sha256.ComputeHash(keyBytes);
            aes.Key = keyBytes;
            aes.GenerateIV();
            using (MemoryStream ms = new MemoryStream()) 
            { 
                ms.Write(aes.IV,0,aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs)) sw.Write(data);
                return ms.ToArray();
            }
        }
    }
    private string DecryptData(byte[] data, string key) 
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (SHA256 sha256 = SHA256.Create()) keyBytes= sha256.ComputeHash(keyBytes);
            aes.Key = keyBytes;
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;
            try 
            {
                using (MemoryStream ms = new MemoryStream(data, iv.Length, data.Length - iv.Length))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs)) return sr.ReadToEnd();
            }
            catch { return null; }
        }
    }
    public string GetPlayerName() 
    { 
        return playerName;
    }
    public int GetScore() 
    {
        return score;
    }
} //{}
