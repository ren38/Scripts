using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussedEffect : BaseCondition
{
    private const float CONCUSSIONDURATION = 8.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = CONCUSSIONDURATION;
        effectName = "Concussed";
        description = string.Format("Easily interrupted.");
        GameObjectObserver obs = subject.gameObject.AddComponent<GameObjectObserver>();
        obs.setupObserver(trigger);
        subject.rawHitSubscribe(obs);
        base.setup(subject, source);
    }

    private void trigger(GameObject i){
        subject.abruptSkillCancel();
    }

    public override void stack()
    {
        addDuration(2.0f);
    }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 12.0f)
        {
            endTime = Time.time + 12.0f;
        }
        effectFunctions.iconUpdate(instanceList, effectName, description, timed, endTime);
    }
}
