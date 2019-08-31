using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkillMouseover : MonoBehaviour
{
    [SerializeField]
    private int skillbarIndex;

    public void triggerSkill()
    {
        PlayerMotivator.Instance.playerTriggerSkill(skillbarIndex);
    }

    public int getButtonIndex(){return skillbarIndex;}

    public void setSkill(GameObject newSkill)
    {
        PlayerMotivator.Instance.replaceSkill(skillbarIndex, newSkill);
    }
}
