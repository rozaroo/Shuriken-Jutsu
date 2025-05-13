using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class AudioMute : MonoBehaviour
{
    public GameObject Audio;
    public GameObject musicOn;
    public GameObject musicOff;
    private bool muted;

    void Start()
    {
        muted = false;
        musicOff.SetActive(true);
    }
    public void Mute() 
    {
        if (muted == true) 
        {
            Audio.SetActive(false);
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }
        else
        {
            Audio.SetActive(true);
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }
    }
    
    
}

