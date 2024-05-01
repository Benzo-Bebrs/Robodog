using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Delete), 0.22f);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
