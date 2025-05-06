using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject gameoverCanvas;
    public Score score;
    void Start() 
    {
        Time.timeScale = 1;
    }
    public void GameOver() 
    {
        gameoverCanvas.SetActive(true);
        Time.timeScale = 0;
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

