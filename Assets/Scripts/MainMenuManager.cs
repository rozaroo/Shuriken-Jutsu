using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public CloudSaveSystem cloud;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI bestScoreText;
    private async void Start()
    {
        showSlider = false;
        showShuriken = false;
        await Task.Delay(10);
        if (cloud != null) 
        {
            string playerName = cloud.GetPlayerName();
            int score = cloud.GetScore();
            if (playerNameText != null) playerNameText.text = "PlayerName: "+ playerName;
            if (bestScoreText != null) bestScoreText.text = "Score: " + score;
        }
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
        slider.SetActive(showSlider);
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
