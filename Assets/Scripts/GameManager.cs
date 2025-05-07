using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverCanvas;
    public Score score;
    public TMP_InputField playerNameInput;
    void Start() 
    {
        Time.timeScale = 1;
    }
    public async Task GameOver() 
    {
        gameoverCanvas.SetActive(true);
        Time.timeScale = 0;
        string playerName = playerNameInput.text;
        int currentScore = score.GetCurrentScore();
        await CloudSaveSystem.Instance.AddNewScore(playerName, currentScore);
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

