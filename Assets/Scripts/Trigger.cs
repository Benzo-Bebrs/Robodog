using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private string tagToCheckTrigger;

    public bool isTriggered { get; private set; }
    public bool yes;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tagToCheckTrigger == "" || collision.CompareTag(tagToCheckTrigger))
        {
            isTriggered = true;
            yes = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tagToCheckTrigger == "" || collision.CompareTag(tagToCheckTrigger))
        {
            isTriggered = false;
            yes = false;
        }
    }
}
