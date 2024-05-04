using UnityEngine;
using System.Collections;

public class DestroyAfter2Seconds : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}