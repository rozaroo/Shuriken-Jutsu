using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Score scoreManager;
    private async void Start()
    {
        await scoreManager.DownloadAndShowBestScore();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
