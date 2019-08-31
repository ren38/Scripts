using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionLibrary : Singleton<ConditionLibrary>
{
    [SerializeField]
    private List<GameObject> library;

    public GameObject getInstanceByID(int id)
    {
        GameObject condition = Instantiate(library[id]);
        return condition;
    }
}
