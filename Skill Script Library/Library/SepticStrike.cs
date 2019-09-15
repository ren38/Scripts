using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SepticStrike: BaseSkill
{
    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        target.takePiercingDamage(10.0f, (ObjectInteractable)self);
        float damage;
        if(target.gameObject.GetComponent<BleedEffect>() != null)
        {
            damage = self.getStr() * 0.5f + 40.0f;
        }
        else
        {
            damage = self.getStr() * 0.5f;

        }
        target.takePoisonDamage(damage, (ObjectInteractable)self);
        message = "";
        return false;
    }
}
