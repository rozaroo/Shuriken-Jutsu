using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    private int score;
    void Start() 
    {
        scoreText.text = score.ToString();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore",0).ToString();
    }
    public void UpdateBestScore() 
    {
        if (score > PlayerPrefs.GetInt("BestScore",0)) 
        {
            PlayerPrefs.SetInt("BestScore",score);
            bestScoreText.text = score.ToString();
        }
    }
    public void UpdateScore() 
    {
        score++;
        scoreText.text = score.ToString();
        UpdateBestScore();
    }
    //Metodo para terminar la partida y guardar score 
    public void EndGame() 
    {
        Debug.Log("Juego terminado. Puntaje: "+ score);
        CloudSaveSystem cloudSave = FindObjectOfType<CloudSaveSystem>();
        if (cloudSave != null) 
        {
            if (string.IsNullOrEmpty(cloudSave.playerNameInput.text)) 
            {
                Debug.Log("Por favor ingresar tu nombre antes de guardar el score");
                return;
            }
            cloudSave.SaveNewScore(score);
        }
        else 
        {
            Debug.LogWarning("No se encontro el CloudSaveSystem en la escena.");
        }
    }
    public void SaveScoreWithName(string playerName) 
    {
        UpdateBestScore();
        await CloudSaveSystem.Instance.AddNewScore("PlayerName", playerScore);
    }
    public int GetCurrentScore() 
    {
        reutn score;
    }
}

