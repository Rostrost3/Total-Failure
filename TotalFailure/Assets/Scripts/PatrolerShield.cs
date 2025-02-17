using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolerShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���� ����� �������� ����
        {
            var player = collision.GetComponent<IHealth>();
            if (player != null)
            {
                player.TakeHealth(3); // ����� �������� ����
                Destroy(gameObject); // ������� ���
            }
        }
    }
}
