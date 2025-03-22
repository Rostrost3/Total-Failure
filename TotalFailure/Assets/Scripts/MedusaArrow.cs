using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaArrow : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    public float damage = 1f;
    public float lifeTime;
    public float distance;
    public LayerMask whatIsSolid;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 140);

        Invoke("DestroyArrow", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid); //Невидимая линия, начальная точка, в какую сторону, длина и с какой маской должна столкнуться
        Debug.Log(hitInfo.collider);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                IDamageable playerHealth = hitInfo.collider.GetComponent<IDamageable>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
            DestroyArrow();
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
