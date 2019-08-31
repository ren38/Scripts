using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class SkillMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float nextPhase;
    private int phase = 0;
    private const float firstDelay = 1.0f;
    private const float secondDelay = 3.0f;
    private BaseSkill skill = null;
    private GameObject panelObject;
    private SkillInfoPanel panel;
    private Vector3 Offset;
    private bool MOEnabled = true;

    public void Start()
    {
        Offset = new Vector3(40, 40, 0);
        panel = SkillInfoPanel.Instance;
        panelObject = panel.gameObject;
        skill = gameObject.GetComponent<BaseSkill>();
        if(skill == null)
        {
            Debug.Log("Something went wrong. Skill mouse over on object " + gameObject.name + " didn't find the associated skill.");
        }
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        nextPhase = Time.time + firstDelay;
        phase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 1 && Time.time >= nextPhase && MOEnabled)
        {
            phase = 2;
            nextPhase = Time.time + secondDelay;
            panel.activate();
            panelObject.transform.position = this.gameObject.transform.position + Offset;
            panel.setSkillName(skill.getName());
            panel.setSkillDescription(skill.getShortDescription());
            panel.setEnergyCost(skill.getEnergyCost());
            panel.setHealthCost(skill.getHealthCost());
            panel.setCastTime(skill.getCastTime());
            panel.setCooldown(skill.getCooldown());
            panel.setAdrenalineCost(skill.getAdrenalCost());
        }
        else if (phase == 2 && Time.time >= nextPhase)
        {
            phase = 3;
            panel.setSkillDescription(skill.getLongDescription());
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        phase = 0;
        panel.deactivate();
    }

    public void toggle(bool setting)
    {
        MOEnabled = setting;
    }
}
