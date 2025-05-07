using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public CloudSaveSystem cloudSave;
    public int score;
    void Start() 
    {
        scoreText.text = score.ToString();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore",0).ToString();
    }
    public void UpdateBestScore() 
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore) 
        {
            PlayerPrefs.SetInt("BestScore",score);
            bestScoreText.text = score.ToString();
            cloudSave.SaveNewScore(score);
        }
    }
    public void UpdateScore() 
    {
        score += 20;
        //score++;
        scoreText.text = score.ToString();
        UpdateBestScore();
    }
    //Metodo para terminar la partida y guardar score 
    public void EndGame() 
    {
        Debug.Log("Juego terminado. Puntaje: "+ score);
        if (cloudSave != null) 
        {
            if (string.IsNullOrEmpty(cloudSave.playerNameInput.text)) 
            {
                Debug.Log("Por favor ingresar tu nombre antes de guardar el score");
                return;
            }
            UpdateBestScore();
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (score >= bestScore) cloudSave.SaveNewScore(score);
        }
        else 
        {
            Debug.LogWarning("No se encontro el CloudSaveSystem en la escena.");
        }
    }
    public async Task SaveScoreWithName(string playerName) 
    {
        UpdateBestScore();
        await CloudSaveSystem.Instance.AddNewScore(playerName, score);
    }
    public int GetCurrentScore() 
    {
        return score;
    }
}

