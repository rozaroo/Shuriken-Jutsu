using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Score scoreManager;
    public GameObject slider;
    private bool showSlider;
    private async void Start()
    {
        if (scoreManager != null) await scoreManager.DownloadAndShowBestScore();
        else return;
        showSlider = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowVolumeSlider()
    {
        showSlider = !showSlider;
        if (showSlider) slider.SetActive(false);
        else slider.SetActive(true);
    }
}
