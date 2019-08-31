using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    private string effectName;
    private string description;
    private bool timed;
    private float duration;

    public void setup(string name, string description, bool timed, float duration)
    {
        effectName = name;
        this.description = description;
        this.timed = timed;
        this.duration = duration;
    }

    public void getData(out string name, out string description, out bool timed, out float duration)
    {
        name = effectName;
        description = this.description;
        timed = this.timed;
        duration = this.duration;
    }
}
