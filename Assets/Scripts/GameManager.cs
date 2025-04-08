using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject gameoverCanvas;
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
        SceneManager.LoadScene(1);
    }
    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
    }
}

