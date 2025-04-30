using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharonFireWave : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifeTime = 4f;
    private Vector2 direction = Vector2.left;
    public LayerMask whatIsSolid;

    private void Start()
    {
        GameObject charon = GameObject.FindGameObjectWithTag("Charon");
        if (charon != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), charon.GetComponent<Collider2D>());
        }

        Invoke("DestroyFireWave", lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 2f, direction, 2f, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy"))
            {
                IDamageable enemy = hitInfo.collider.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            DestroyFireWave();
        }
    }
   
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (direction.x < 0)
            transform.localScale = new Vector2(-1f, 1f);
        else
            transform.localScale = new Vector2(1f, 1f);
    }

    void DestroyFireWave()
    {
        Destroy(gameObject);
    }
}
