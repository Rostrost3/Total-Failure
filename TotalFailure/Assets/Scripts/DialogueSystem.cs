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
        textComponent.text = string.Empty;
        pM = FindAnyObjectByType<PlayerMovement>();
        StartDialogue();
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index]) { NextLine(); }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        pM.isDialogueActive = true;
        index = 0;
        StartCoroutine(TypeLine());
        
        
    }

    //Prints text
    IEnumerator TypeLine()
    {
        //Types each char separately
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
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
        else { gameObject.SetActive(false); pM.isDialogueActive = false; }
    }
}
