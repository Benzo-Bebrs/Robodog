using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumeValue : MonoBehaviour
{
    public bool isOn;
    private AudioSource audioSrc;
    private float musicVolume = 1f;
    private void StartMusic()
    {
        isOn = true;
    }

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSrc.volume = musicVolume;   
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }

    public void OnOffSound()
    {
        if (!isOn)
        {
            AudioListener.volume = 1f;
            isOn = true;
        }
        else if (isOn)
        {
            AudioListener.volume = 0f;
            isOn = false;
        }
    }
}
