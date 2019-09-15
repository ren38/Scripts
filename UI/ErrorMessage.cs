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
    private bool setting;
    [SerializeField]
    private float fadeDuration = 1.0f;
    private float endTime;

    public void setActive(bool setting, string message)
    {
        this.setting = setting;
        TextBox.SetActive(setting);
        if (setting)
        {
            text.text = message;
        }
        text.color = Color.red;
        endTime = Time.time + fadeDuration;
    }

    private void Update()
    {
        if(setting)
        {
            if(endTime > Time.time)
            {
                text.color = new Color(1, 0, 0, (endTime - Time.time) / fadeDuration);
            }
            else
            {
                setActive(false);
            }
        }
    }

    public void setActive(bool setting)
    {
        text.text = "";
        this.setting = setting;
        TextBox.SetActive(setting);
    }
}
