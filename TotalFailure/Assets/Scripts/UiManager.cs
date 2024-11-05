using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button Button;
    [SerializeField] GameObject ButtonObject;

    void Start()
    {
        Button.onClick.AddListener(SwitchButtonState);

    }

    void SwitchButtonState()
    {
        ButtonObject.SetActive(!ButtonObject.activeSelf);
    }
}
