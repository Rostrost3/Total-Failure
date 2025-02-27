using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<ITakeKeys>();
            if ( player != null)
            {
                player.TakeKey();
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
            
            
        }
    }
}
