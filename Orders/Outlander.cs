using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlander : Order
{
    string NAME = "Outlander";
    string DESCRIPTION = "A delegate from the wilds, practiced in all aspects of the hunt.";
    int ID = 4;

    // Start is called before the first frame update
    void Start()
    {
        oName = NAME;
        description = DESCRIPTION;
        id = ID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
