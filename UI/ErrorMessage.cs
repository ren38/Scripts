using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : Singleton<ErrorMessage>
{
    [SerializeField]
    private GameObject TextBox;
    [SerializeField]
    private Text text;

    public void setActive(bool setting, string message)
    {
        TextBox.SetActive(setting);
        if (setting)
        {
            text.text = message;
        }
    }

    public void setActive(bool setting)
    {
        TextBox.SetActive(setting);
    }
}
