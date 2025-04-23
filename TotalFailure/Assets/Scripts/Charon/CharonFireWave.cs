using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonFireWave : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 5;
    public float lifeTime = 3f;
    private Vector2 direction = Vector2.left;

    private void Start()
    {
        Invoke("DestroyFireWave", lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
            DestroyFireWave();
        }
    }

    void DestroyFireWave()
    {
        Destroy(gameObject);
    }
}
