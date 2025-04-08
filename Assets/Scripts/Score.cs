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
}

