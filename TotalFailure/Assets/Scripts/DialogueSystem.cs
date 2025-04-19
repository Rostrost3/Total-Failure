using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    PlayerMovement pM;

    private bool isDialogueActive = false;

    private int index;

    void Start()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string dialogueShownKey = currentScene + "_DialogueShown";

        pM = FindAnyObjectByType<PlayerMovement>();

        if (PlayerPrefs.GetInt(dialogueShownKey, 0) == 0)
        {
            // Диалог ещё не был показан на этой сцене
            textComponent.text = string.Empty;
            StartDialogue();
            PlayerPrefs.SetInt(dialogueShownKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Диалог уже был показан на этой сцене
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }


    void StartDialogue()
    {
        isDialogueActive = true;
        pM.isDialogueActive = true;

        index = 0;
        StartCoroutine(TypeLine());
    }


    // Печатает текст по буквам
    IEnumerator TypeLine()
    {
        // Для каждой буквы в строке
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); // Задержка между буквами
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            pM.isDialogueActive = false;
            isDialogueActive = false;
        }
    }

}
