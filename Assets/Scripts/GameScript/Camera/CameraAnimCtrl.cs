using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimCtrl : MonoBehaviour
{
    private CamController cam;
    private Transform player;

    void Start()
    {   
        cam = FindObjectOfType<CamController>();
        player = GameObject.Find("Player").transform;
    }


    public void changeCameraSize(float size)
    {
        CamController.changeCameraSizeEvent?.Invoke(size);
    }

    public void focusOn0bject(int _focus)
    { 
        bool focus = _focus > 0;
        Transform follow0bject = focus ? transform : player;

        CamController.changeFollowTargetEvent?.Invoke(follow0bject);
        cam.transposer.m_ScreenX = 0.5f;
        cam.transposer.m_ScreenY = 0.5f;
    }

    public void changeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void ShakeCamera (float strenght) 
    {
        CamController.cameraShake?.Invoke(strenght, 1f, 1f);
    }   
}