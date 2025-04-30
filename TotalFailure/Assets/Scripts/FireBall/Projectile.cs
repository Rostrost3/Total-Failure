using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1f;
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask whatIsSolid;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid); //Невидимая линия, начальная точка, в какую сторону, длина и с какой маской должна столкнуться
        if(hitInfo.collider != null)
        {
            if(hitInfo.collider.CompareTag("Enemy") || hitInfo.collider.CompareTag("Charon"))
            {
                IDamageable enemy = hitInfo.collider.GetComponent<IDamageable>();
                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
