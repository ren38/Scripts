using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethods : Singleton<StaticMethods>
{
    public static void destroyGameObjectsInList(List<GameObject> instanceList)
    {
        foreach (GameObject obj in instanceList)
        {
            Destroy(obj);
        }
    }
}
