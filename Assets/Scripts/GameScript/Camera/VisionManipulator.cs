using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManipulator : MonoBehaviour
{
    [Header("Размер камеры (слево")]
    public float leftCameraSize;
    [Header("Размер камеры (справа")]
    public float rightCameraSize;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x) //Если пошёл направо
            {
                CamController.changeCameraSizeEvent?.Invoke(rightCameraSize);
            }
            else //Если пошёл налево
            {
                CamController.changeCameraSizeEvent?.Invoke(leftCameraSize);
            }
        }
    }
}
