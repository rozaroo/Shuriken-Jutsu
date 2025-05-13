using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMute : MonoBehaviour
{
    public GameObject Audio;
    public GameObject musicOn;
    public GameObject musicOff;
    private bool muted;

    void Start()
    {
        muted = false;
    }
    public void Mute() 
    {
        muted = !muted;
        if (muted) 
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

