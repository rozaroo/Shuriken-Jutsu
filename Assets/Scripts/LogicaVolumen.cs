using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicaVolumen : MonoBehaviour
{
    public Slider slider;
    public AudioSource audioSource;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        slider.value = savedVolume;
        slider.onValueChanged.AddListener(ChangeSlider);
        if (audioSource != null) audioSource.volume = slider.value;
    }
    public void ChangeSlider(float valor) 
    {
        PlayerPrefs.SetFloat("volumenAudio", valor);
        if (MusicPlayer.Instance != null && MusicPlayer.Instance.audioSource != null) MusicPlayer.Instance.audioSource.volume = valor;
    }
}

