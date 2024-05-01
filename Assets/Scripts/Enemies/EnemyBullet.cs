using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] GameObject explodePrefab;
    private float lifeTime = 0;
    private float maxLifeTime = 3;

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.HealthPoint -= 1;
            Explode();
        }
        else if (!collision.isTrigger && !collision.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
