using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    private Boy boy;
    void Start()
    {
        Invoke(nameof(Delete), 3f);
        boy = Boy.Instance;
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = boy.transform.position + new Vector3(0, 3f, 0);
    }
}
