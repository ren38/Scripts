using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectInfoPanel : Singleton<EffectInfoPanel>
{
    [SerializeField]
    private GameObject Panel = null;
    [SerializeField]
    private Text EffectName = null;
    [SerializeField]
    private Text EffectDescription = null;
    [SerializeField]
    private Text TimeText = null;
    [SerializeField]
    private GameObject Timer = null;

    private bool timerActive;
    private float endtime;


    public void setup(string name, string description, bool setting, float endtime)
    {
        EffectName.text = name;
        EffectDescription.text = description;
        timerActive = setting;
        Timer.SetActive(setting);
        if(setting)
        {
            this.endtime = endtime;
            TimeText.text = parseDuration(endtime - Time.time);
        }
    }
    
    public void deactivate()
    {
        timerActive = false;
        Panel.SetActive(false);
    }

    public void activate()
    {
        Panel.SetActive(true);
    }

    private string parseDuration(float remainingDuration)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(remainingDuration);
        //int minutes = (int) (remainingDuration / 60);
        //int seconds = (int) (remainingDuration % 60);
        string line = t.ToString(@"dd\.hh\:mm\:ss");
        return line;
    }

    public void updateTimer(string line)
    {
        TimeText.text = line;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Starting effect info panel " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            float remainingDuration = (float) System.Math.Floor(endtime - Time.time);
            updateTimer(parseDuration(remainingDuration));
        }
    }
}
