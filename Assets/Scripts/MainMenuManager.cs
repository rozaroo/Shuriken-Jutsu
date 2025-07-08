using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public static MainMenuManager Instance { get; private set; }
    public string userId;

    private void Awake()
    {
        //if (Instance == null)
        //{
            //Instance = this;
            //DontDestroyOnLoad(gameObject); // Solo si quieres que persista
        //}
        //else
        //{
            //Destroy(gameObject);
            //return;
        //}
        if (PlayerPrefs.HasKey("UserId")) userId = PlayerPrefs.GetString("UserId");
        else
        {
            userId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("UserId", userId);
        }
    }

    private async void Start()
    {
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
