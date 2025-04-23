using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Transform playerTransform;
    public float activationDistance = 1f;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Получаем ссылку на игрока
    }


    private void Update()
    {
        var player = playerTransform.GetComponent<ITakeKeys>();

        if (Vector3.Distance(playerTransform.position, transform.position) <= activationDistance)
        {
            UIMessageManager.Instance.ShowMessage("Press E to collect the key.");
        }

        if (Vector3.Distance(playerTransform.position, transform.position) <= activationDistance &&
            Input.GetKeyDown(KeyCode.E))
        {
            UIMessageManager.Instance.ShowMessage("You collected the key!", 2f);

            player.TakeKey();

            Destroy(gameObject);
        }
        
    }

    
}
