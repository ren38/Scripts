using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float startTime;
    private bool active;
    private bool setupNeeded;
    private const float delay = 1.0f;
    private GameObject panelObject;
    private EffectInfoPanel panel;
    private UIEffect EffectData;
    private Vector3 Offset;

    public void setup(UIEffect e)
    {
        panel = EffectInfoPanel.Instance;
        panelObject = panel.gameObject;
        panel.deactivate();
        active = false;
        setupNeeded = false;
        Offset = new Vector3(20, 20, 0);
        EffectData = e;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        startTime = Time.time + delay;
        active = true;
        setupNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Time.time >= startTime && setupNeeded)
        {
            panel.activate();
            panelObject.transform.position = this.gameObject.transform.position + Offset;
            string name;
            string description;
            bool timed;
            float endTime;
            EffectData.getData(out name, out description, out timed, out endTime);
            panel.setup(name, description, timed, endTime);
            setupNeeded = false;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        stop();
    }

    public void stop()
    {
        if(active)
        {
            active = false;
            panel.deactivate();
        }
    }

    public void pull()
    {
        setupNeeded = true;
    }
}
