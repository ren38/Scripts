using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindComponent
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }// credit to fafase
    // https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
}
