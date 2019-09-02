using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMethods : Singleton<EffectMethods>
{
    public static void destroyGameObjectsInList(List<GameObject> instanceList)
    {
        foreach (GameObject obj in instanceList)
        {
            Destroy(obj);
        }
    }
}
