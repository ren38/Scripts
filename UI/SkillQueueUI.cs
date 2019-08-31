using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillQueueUI : MonoBehaviour
{
    private const int QUEUELIMIT = 4;
    [SerializeField]
    private GameObject[] buttons = null;
    private List<GameObject> skills;
    private int displaying = 0;
    [SerializeField]
    private ObjectActor target = null;
    protected IntObserver dequeue;
    protected GameObjectObserver enqueue;
    protected GameObjectObserver death;


    void Awake()
    {
        skills = new List<GameObject>();
    }

    private void targetDied(GameObject source)
    {
        clearQueue();
    }

    public void unsetQueue()
    {
        if (this.target != null)
        {
            clearQueue();
        }

        if (dequeue != null) { dequeue.complete(); }
        if (enqueue != null) { enqueue.complete(); }
        if (death != null) { death.complete(); }
    }

    public void setQueue(ObjectActor target)
    {
        unsetQueue();
        this.target = target;
        skills = target.getDereferencedSkillQueue();
        displaying = skills.Count;
        for (int i = 0; i < skills.Count; i++)
        {
            buttons[i].SetActive(true);
        }
        displayQueue();
        setup();
    }

    public void setup()
    {
        if (this.target != null)
        {
            dequeue = gameObject.AddComponent<IntObserver>();// dequeue index. usually 0.
            dequeue.setupObserver(skillRemovedByIndex);
            target.skillDequeueSubscribe(dequeue);

            enqueue = gameObject.AddComponent<GameObjectObserver>();
            enqueue.setupObserver(skillEnqueued);
            target.skillEnqueueSubscribe(enqueue);

            death = gameObject.AddComponent<GameObjectObserver>();
            death.setupObserver(targetDied);
            target.deathSubscribe(death);
        }
    }

    /*
    public void skillUIDequeue()
    {
        shiftByIndex(0);
    }
    */

    public void skillUIEnqueue(GameObject newSkill)
    {
        buttons[displaying].SetActive(true);
        skills.Add(Instantiate(newSkill, buttons[displaying].transform));
        skills[displaying].transform.SetSiblingIndex(0);
        if (displaying == 0)
        {
            skills[displaying].transform.localScale = new Vector2(0.75f, 0.75f);
            skills[displaying].transform.localPosition = new Vector2(30, 30);
        }
        else
        {
            skills[displaying].transform.localScale = new Vector2(0.5f, 0.5f);
            skills[displaying].transform.localPosition = new Vector2(20, 20);
        }
        displaying++;
    }

    public void skillEnqueued(GameObject newSkill)
    {
        skillUIEnqueue(newSkill);
    }

    
    public void skillRemovedByIndex(int index)
    {
        shiftByIndex(index);
    }
    

    public void OnCompleted()
    {
        dequeue.complete();
        enqueue.complete();
        death.complete();
    }

    public void removeFromQueue(int index)
    {
        target.removeFromQueue(index);
        shiftByIndex(index);
    }

    public void shiftByIndex(int index)
    {
        GameObject temp = skills[index];
        skills.RemoveAt(index);
        Destroy(temp);
        displaying--;
        displayQueue();
    }

    public void displayQueue()
    {
        int count = 0;
        foreach (GameObject skill in skills)
        {
            if (count == 0)
            {
                skill.transform.SetParent(buttons[0].transform);
                skill.transform.SetSiblingIndex(0);
                skill.transform.localScale = new Vector2(0.75f, 0.75f);
                skill.transform.localPosition = new Vector2(30, 30);
            }

            else
            {
                skill.transform.SetParent(buttons[count].transform);
                skill.transform.SetSiblingIndex(0);
                skill.transform.localPosition = new Vector2(20, 20);
            }
            count++;
        }

        while (count < 4)
        {
            buttons[count].SetActive(false);// This works because the displaying should be 1 greater
                                            // than the index of the button that needs to be hidden.
            count++;
        }
    }

    public void clearQueue()
    {
        displaying = 0;
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            Destroy(skills[i].gameObject);
        }
        skills.Clear();
        displayQueue();
    }
}