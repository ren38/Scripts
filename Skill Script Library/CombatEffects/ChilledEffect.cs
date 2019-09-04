using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChilledEffect : BaseCondition
{
    private const float CHILLEDDURATION = 20.0f;
    private float SPEEDLOSS = 0.1f;
    protected FloatAdjuster obs;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        instanceList = new List<GameObject>();
        timed = true;
        duration = CHILLEDDURATION;
        endTime = duration + Time.time;
        effectName = "Burning";
        description = string.Format("Skill activation takes {0}x longer.", 1.0f + SPEEDLOSS);
        this.subject = subject;
        this.source = source;
        obs = subject.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(change);
        subject.skillStartSubscribe(obs);
    }

    public float change(float num)
    {
        return num + SPEEDLOSS;
    }

    public float getSpeedloss()
    {
        return SPEEDLOSS;
    }

    public override void stack()
    {
        addDuration(8.0f);
        addLoss(0.1f);
        effectName = ("Chilled x" + (1.0f + SPEEDLOSS));
        description = string.Format("Skill activation takes {0}x longer.", 1.0f + SPEEDLOSS);
        base.stack();
    }

    public void addLoss(float delta)
    {
        SPEEDLOSS += delta;
        if (SPEEDLOSS >= 2.0f)
        {
            SPEEDLOSS = 2.0f;
        }
    }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 60.0f)
        {
            endTime = Time.time + 60.0f;
        }
    }
}
