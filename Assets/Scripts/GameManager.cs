using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverCanvas;
    public Score score;
    public TMP_InputField playerNameInput;
    void Start() 
    {
        Time.timeScale = 1;
    }
    public void GameOver() 
    {
        gameoverCanvas.SetActive(true);
        Time.timeScale = 0;
        score.SaveScoreWithName(playerNameInput.text);
    }
    public void Restart() 
    {
        score.EndGame();
        SceneManager.LoadScene(1);
    }
    public void MainMenu() 
    {
        score.EndGame();
        SceneManager.LoadScene(0);
    }
}

