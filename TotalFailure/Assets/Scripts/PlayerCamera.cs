using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        Vector3 xyz = transform.position;
        xyz.x = player.position.x;
        xyz.y = player.position.y;
        xyz.z = -10;

        transform.position = xyz;
    }
}
