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

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 6)
        {
            if(timer <= 0)
            {
                Shoot();
                Invoke("Shoot", 0.5f);
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
}
