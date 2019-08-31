using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theurge : Order
{
    string NAME = "Theurge";
    string DESCRIPTION = "A scholar of the occult, studying through their travels.";
    int ID = 2;

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
