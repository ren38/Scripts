using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute : BaseSkill
{
    public override void activate(ObjectActor self, ObjectCombatable target)
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
    }
}

