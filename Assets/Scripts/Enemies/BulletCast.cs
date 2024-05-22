using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCast : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Delete), 0.35f);
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}
