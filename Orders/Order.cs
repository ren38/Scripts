using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Order : MonoBehaviour
{
    protected string oName;
    protected string description;
    protected int id;

    public string getName()
    {
        return oName;
    }
    public string getDescription()
    {
        return description;
    }
    public int getID()
    {
        return id;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
