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
            // ����� ���� � ��������� ������
            textComponent.text = string.Empty;
            StartDialogue();
        }
        else
        {
            // ����������� ���� � ��������� ��������� ���������� �������
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // ���� ���� �� ���������� (������, �� � �������)
        if (PlayerPrefs.GetInt("ContinueGame", 0) == 0)
        {
            // ���� ������ ������ Enter ��� ����� ������ ����
            if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
            {
                if (textComponent.text == lines[index]) { NextLine(); }
                else
                {
                    // ������������� ��� �������� � ����� ���������� ������� ������
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    void StartDialogue()
    {
        // ��������� ����������� ��������� �� ����� �������
        pM.isDialogueActive = true;

        // �������� ������ � ������ ������
        index = 0;

        // �������� �������� ��� ������ ������
        StartCoroutine(TypeLine());
    }

    // �������� ����� �� ������
    IEnumerator TypeLine()
    {
        // ��� ������ ����� � ������
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); // �������� ����� �������
        }
    }

    void NextLine()
    {
        // ���� ���� ��� ������
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty; // ������� �����
            StartCoroutine(TypeLine()); // �������� ��������� ������
        }
        else
        {
            // ��������� ���������� ����, ����� ��� ������ ��������
            gameObject.SetActive(false);
            pM.isDialogueActive = false; // ��������� ���������
        }
    }
}
