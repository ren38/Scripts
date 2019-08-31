using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devoted : Order
{
    string NAME = "Devoted";
    string DESCRIPTION = "A faithful knight errant consecrated in word to a god.";
    int ID = 0;

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
