using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource soundButton;

    public void PlayThisSoundEffect()
    {
        soundButton.Play();
    }
}
