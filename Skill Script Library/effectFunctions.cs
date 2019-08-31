using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class effectFunctions
{
    // GameObject instance = ConditionLibrary.Instance.getInstanceByID(conditionID);
    // instanceList.Add(instance);

    public static GameObject setupIcon(GameObject instance, string name, string description, bool timed, float endTime)
    {
        UIEffect e = instance.AddComponent<UIEffect>();
        e.setup(name, description, timed, endTime);
        EffectMouseOver m = instance.AddComponent<EffectMouseOver>();
        m.setup(e);
        return instance;
    }

    public static void iconUpdate(List<GameObject> instanceList, string newName, string newDescription, bool newTimed, float newEndTime)
    {
        foreach (var instance in instanceList)
        {
            if (instance != null)
            {
                UIEffect e = instance.GetComponent<UIEffect>();
                if (e != null)
                {
                    e.setup(newName, newDescription, newTimed, newEndTime);
                }
                EffectMouseOver m = instance.GetComponent<EffectMouseOver>();
                if (m != null)
                {
                    m.pull();
                }
            }
        }
    }
}
