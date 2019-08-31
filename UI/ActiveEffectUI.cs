using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEffectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] rows = null;
    [SerializeField]
    private GameObject panel = null;
    private List<GameObject> effects;
    private int displaying = 0;
    [SerializeField]
    private ObjectActor target = null;
    protected IntObserver removalSub;
    protected GameObjectObserver additionSub;
    protected GameObjectObserver death;

    void Awake()
    {
        effects = new List<GameObject>();
    }

    private void targetDied(GameObject source)
    {
        clearList();
        displayList();
    }

    void Start()
    {
        if(target != null)
        {
            effects = target.getEffectListObjects();
            displaying = effects.Count;
            displayList();
            setup();
        }
    }

    public void unsetList()
    {
        if (target != null)
        {
            clearList();
        }

        if (removalSub != null) { removalSub.complete(); }
        if (additionSub != null) { additionSub.complete(); }
        if (death != null) { death.complete(); }
    }

    public void setList(ObjectActor target)
    {
        unsetList();
        this.target = target;
        effects = target.getEffectListObjects();
        foreach (GameObject o in effects)
        {
            setIconSize(o);
        }
        displaying = effects.Count;
        displayList();
        setup();
    }

    public void setup()
    {
        if (this.target != null)
        {
            removalSub = gameObject.AddComponent<IntObserver>();// dequeue index. usually 0.
            removalSub.setupObserver(shiftByIndex);
            target.effectRemoveSubscribe(removalSub);

            additionSub = gameObject.AddComponent<GameObjectObserver>();
            additionSub.setupObserver(effectAddToList);
            target.effectAddSubscribe(additionSub);

            death = gameObject.AddComponent<GameObjectObserver>();
            death.setupObserver(targetDied);
            target.deathSubscribe(death);
        }
    }
    
    public void effectAddToList(GameObject newEffect)
    {
        SkillMouseOver mouseOver = newEffect.GetComponent<SkillMouseOver>();
        if(mouseOver != null)
        {
            Destroy(mouseOver);
        }
        setIconSize(newEffect);
        effects.Add(newEffect);
        displaying++;
        displayList();
    }

    public void resetList(GameObject m)
    {
        clearList();
        foreach (GameObject effect in target.getEffectListObjects())
        {
            SkillMouseOver mouseOver = effect.GetComponent<SkillMouseOver>();
            if (mouseOver != null)
            {
                Destroy(mouseOver);
            }
            effects.Add(effect);
        }
        displayList();
    }
    
    public void shiftByIndex(int index)
    {
        GameObject temp = effects[index];
        EffectMouseOver e = temp.GetComponent<EffectMouseOver>();
        e.stop();
        effects.RemoveAt(index);
        Destroy(temp);
        displaying--;
        displayList();
    }
    
    public void displayList()
    {
        int count = 0;
        int activeRow = 0;
        if(effects.Count > 0)
        {
            panel.SetActive(true);
            for (int i = 0; i < effects.Count; i++)
            {
                rows[activeRow].SetActive(true);
                if (activeRow < rows.Length)
                {
                    effects[i].transform.SetParent(rows[activeRow].transform);
                }
                count++;
                if (count == 16 && (i + 1) < effects.Count)
                {
                    activeRow++;
                    count = 0;
                }
            }
            for (int i = activeRow + 1; i < rows.Length; i++)
            {
                rows[i].SetActive(false);
            }
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void clearList()
    {
        displaying = 0;
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            Destroy(effects[i].gameObject);
        }
        effects.Clear();
    }

    public void setIconSize(GameObject o)
    {
        o.transform.localScale = new Vector3(0.5f, 0.5f, 0);
    }
}
