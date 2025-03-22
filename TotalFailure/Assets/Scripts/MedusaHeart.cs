using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaHeart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<IHealth>();
            if (player != null)
            {
                player.TakeHealth(15);
                gameObject.SetActive(false);
            }
        }
    }

}
