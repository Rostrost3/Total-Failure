using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaShooting : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPos;

    public float startTimer = 2f;
    private float timer;
    private GameObject player;

    public MedusaMovement medusa;

    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 10)
        {
            if(timer <= 0 && !isShooting)
            {
                isShooting = true;
                medusa.IsShooting = true;
                Shoot();
                Invoke("Shoot", 0.5f);
                StartCoroutine(ResetShooting());
                timer = startTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    void Shoot()
    {   
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    }

    IEnumerator ResetShooting()
    {
        yield return new WaitForSeconds(1f); // Через секунду после атаки перестаёт целиться в игрока
        isShooting = false;
        medusa.IsShooting = false;
    }
}
