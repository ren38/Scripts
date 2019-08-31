using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : Singleton<SkillInfoPanel>
{
    [SerializeField]
    private GameObject Panel = null;
    [SerializeField]
    private Text SkillName = null;
    [SerializeField]
    private Text SkillDescription = null;
    [SerializeField]
    private Text EnergyCost = null;
    [SerializeField]
    private GameObject EnergyBlock = null;
    [SerializeField]
    private Text HealthCost = null;
    [SerializeField]
    private GameObject HealthBlock = null;
    [SerializeField]
    private Text CastTime = null;
    [SerializeField]
    private GameObject TimeBlock = null;
    [SerializeField]
    private Text Cooldown = null;
    [SerializeField]
    private GameObject CooldownBlock = null;
    [SerializeField]
    private Text AdrenalineCost = null;
    [SerializeField]
    private GameObject AdrenalineBlock = null;

    public void setSkillName(string name){SkillName.text = name; }

    public void setSkillDescription(string description) { SkillDescription.text = description; }

    public void setEnergyCost(float cost)
    {
        if(cost >= 0)
        {
            EnergyBlock.SetActive(true);
            EnergyCost.text = cost.ToString();
        }
        else
        {
            EnergyBlock.SetActive(false);
        }
    }

    public void setHealthCost(float cost)
    {
        if (cost >= 0)
        {
            HealthBlock.SetActive(true);
            HealthCost.text = cost.ToString();
        }
        else
        {
            HealthBlock.SetActive(false);
        }
    }

    public void setCastTime(float cost)
    {
        if (cost >= 0)
        {
            TimeBlock.SetActive(true);
            CastTime.text = cost.ToString();
        }
        else
        {
            TimeBlock.SetActive(false);
        }
    }

    public void setCooldown(float cost)
    {
        if (cost >= 0)
        {
            CooldownBlock.SetActive(true);
            Cooldown.text = cost.ToString();
        }
        else
        {
            CooldownBlock.SetActive(false);
        }
    }

    public void setAdrenalineCost(float cost)
    {
        if (cost >= 0)
        {
            AdrenalineBlock.SetActive(true);
            AdrenalineCost.text = cost.ToString();
        }
        else
        {
            AdrenalineBlock.SetActive(false);
        }
    }

    public void deactivate()
    {
        Panel.SetActive(false);
    }

    public void activate()
    {
        Panel.SetActive(true);
    }
}
