using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussedEffect : MonoBehaviour, IEffect
{
    private const float CONCUSSIONDURATION = 8.0f;
    protected ObjectActor subject;
    protected ObjectInteractable source;

    protected int conditionID;
    protected List<GameObject> instanceList;
    protected string effectName;
    protected string description;
    protected bool timed;
    protected float duration;
    protected float endTime;

    public void setup(ObjectActor subject, ObjectInteractable source)
    {
        instanceList = new List<GameObject>();
        timed = true;
        duration = CONCUSSIONDURATION;
        endTime = duration + Time.time;
        effectName = "Concussed";
        description = string.Format("Easily interrupted.");
        this.subject = subject;
        this.source = source;
        GameObjectObserver obs = subject.gameObject.AddComponent<GameObjectObserver>();
        obs.setupObserver(trigger);
        subject.rawHitSubscribe(obs);
    }

    private void trigger(GameObject i){
        subject.abruptSkillCancel();
    }

    public void apply(float deltaTime){ return; }

    public float getEnd(){ return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        clearIconInstances();
        Destroy(this);
    }
    public void clearIconInstances()
    {
        foreach (GameObject obj in instanceList)
        {
            Destroy(obj);
        }
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(4));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack(){ return; }

    public float getDuration(){ return duration; }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 17.0f)
        {
            endTime = Time.time + 17.0f;
        }
    }
}
