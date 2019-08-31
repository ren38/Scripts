using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    protected QuestLog log;
    protected int prioraty;

    protected string qName;
    protected string description;
    protected List<int> skillRewards;

    public string getName()
    {
        return qName;
    }
    public string getDescription() { return description; }

    protected void addToLog(QuestLog log)
    {
        log.addQuest(this);
        this.log = log;
    }

    protected void complete()
    {
        QuestLog.Instance.finishQuest(this);
    }

    public List<int> getRewardSkillList()
    {
        return skillRewards;
    }
}
