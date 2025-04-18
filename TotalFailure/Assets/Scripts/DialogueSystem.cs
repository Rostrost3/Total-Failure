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

    private int index;

    void Start()
    {
        pM = FindAnyObjectByType<PlayerMovement>();

        if (PlayerPrefs.GetInt("ContinueGame", 0) == 0)
        {
            // Новая игра — запускаем диалог
            textComponent.text = string.Empty;
            StartDialogue();
        }
        else
        {
            // Продолжение игры — полностью отключаем диалоговую систему
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Если игра не продолжена (значит, мы в диалоге)
        if (PlayerPrefs.GetInt("ContinueGame", 0) == 0)
        {
            // Если нажата кнопка Enter или левая кнопка мыши
            if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
            {
                if (textComponent.text == lines[index]) { NextLine(); }
                else
                {
                    // Останавливаем все корутины и сразу показываем текущую строку
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    void StartDialogue()
    {
        // Отключаем возможность двигаться во время диалога
        pM.isDialogueActive = true;

        // Начинаем диалог с первой строки
        index = 0;

        // Начинаем корутину для печати текста
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
        // Если есть еще строки
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty; // Очищаем текст
            StartCoroutine(TypeLine()); // Печатаем следующую строку
        }
        else
        {
            // Закрываем диалоговое окно, когда все строки пройдены
            gameObject.SetActive(false);
            pM.isDialogueActive = false; // Разрешаем двигаться
        }
    }
}
