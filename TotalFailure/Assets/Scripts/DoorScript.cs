using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorScript : MonoBehaviour
{

    public int requiredKeys = 3;
    private bool isPlayerNear = false;
    public TextMeshProUGUI messageText;


    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false); 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            HideMessage();
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            var player = FindObjectOfType<PlayerAttackAndHealth>(); 
            if (player != null && player.countOfKeys >= requiredKeys)
            {
                LoadNextLevel();
            }
            else
            {
                Debug.Log("Собраны не все ключи!");
                ShowMessage("You didn't collect all the keys!");
            }
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Это был последний уровень!");
        }
    }

    private void ShowMessage(string text)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideMessage)); 
            Invoke(nameof(HideMessage), 2f); 
        }
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}
