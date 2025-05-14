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
    public GameObject ShurikenSelector;
    public GameObject BackButton;
    public GameObject ShowButton;
    public GameObject PlayButton;
    public GameObject VolumeButton;
    public GameObject ExitButton;
    private bool showSlider;
    private bool showShuriken;
    private async void Start()
    {
        if (scoreManager != null) await scoreManager.DownloadAndShowBestScore();
        else return;
        showSlider = false;
        showShuriken = false;
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
    public void ShowShurikens()
    {
        ShurikenSelector.SetActive(true);
        PlayButton.SetActive(false);
        VolumeButton.SetActive(false);
        ExitButton.SetActive(false);
        BackButton.SetActive(true);
        ShowButton.SetActive(false);
    }
    public void Back()
    {
        ShurikenSelector.SetActive(false);
        PlayButton.SetActive(true);
        VolumeButton.SetActive(true);
        ExitButton.SetActive(true);
        BackButton.SetActive(false);
        ShowButton.SetActive(true);
    }
}
