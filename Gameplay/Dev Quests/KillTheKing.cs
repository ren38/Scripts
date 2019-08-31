using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTheKing : Quest
{
    [SerializeField]
    private ObjectActor king;
    private GameObjectObserver obs;

    void Awake()
    {
        name = "Kill The King";
        description = "Go kill King Arnold. He's not a nice guy, and I want my burger back.";
        skillRewards = new List<int>();
        skillRewards.Add(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        //addToLog(QuestLog.Instance);

        //conditions for success
        
        obs = gameObject.AddComponent<GameObjectObserver>();
        obs.setupObserver(targetEliminated);
        king.deathSubscribe(obs);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void targetEliminated(GameObject killer)
    {
        complete();
    }

}
