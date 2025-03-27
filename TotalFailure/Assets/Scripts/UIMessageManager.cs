using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMessageManager : MonoBehaviour
{
   
    public static UIMessageManager Instance; // Глобальная ссылка

    public TextMeshProUGUI messageText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ShowMessage(string text, float duration = 2f)
    {
        
        if (messageText != null)
        {
            messageText.text = text;
            messageText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideMessage));
            Invoke(nameof(HideMessage), duration);
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
