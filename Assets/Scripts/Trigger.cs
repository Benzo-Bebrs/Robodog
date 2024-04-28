using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private string tagToCheckTrigger;

    public bool isTriggered { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tagToCheckTrigger == "" || collision.CompareTag(tagToCheckTrigger))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tagToCheckTrigger == "" || collision.CompareTag(tagToCheckTrigger))
        {
            isTriggered = false;
        }
    }
}
