using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden : Order
{
    string NAME = "Warden";
    string DESCRIPTION = "A master of weapons and armor, and a stalwart sentinel.";
    int ID = 3;

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
