using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute : BaseSkill
{
    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        float num = target.getPercentHealth();
        if (num < 0.25f)
        {
            float damage = self.getStr() * 0.5f + 50.0f;
            target.takeSlashingDamage(damage, (ObjectInteractable)self);
        }
        else if (num < 0.5f)
        {
            float damage = self.getStr() * 0.5f + 30.0f;
            target.takeSlashingDamage(damage, (ObjectInteractable)self);
        }
        else
        {
            message = "Target's health was not low enough.";
            return true;
        }
        message = "";
        return false;
    }
}

