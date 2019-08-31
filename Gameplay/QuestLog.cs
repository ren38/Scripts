using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : Singleton<QuestLog>
{
    [SerializeField]
    private List<Quest> activeQuests;
    [SerializeField]
    private List<Quest> completedQuests;
    private QuestView Viewer;

    void Start()
    {
        Viewer = QuestView.Instance;
        Viewer.Setup();
        /*        
            Viewer.AddListener(delegate {
            GetQuestByName(Viewer.options[Viewer.value].text);
            });
        */
    }

    public Quest GetQuestByName(string name)
    {
        foreach (Quest q in activeQuests)
        {
            if(q.getName() == name)
            {
                return q;
            }
        }
        return null;
    }

    public void addQuest(Quest newQuest)
    {
        activeQuests.Add(newQuest);
        pushQuestListUpdate();
    }

    public void finishQuest(Quest finishedQuest)
    {
        PlayerMotivator.Instance.resolveQuestRewards(finishedQuest);
        activeQuests.Remove(finishedQuest);
        completedQuests.Add(finishedQuest);
    }

    public List<Quest> getActiveQuests()
    {
        return activeQuests;
    }

    private void pushQuestListUpdate()
    {
        Viewer.updateList(getActiveQuestNames());
    }

    public List<string> getActiveQuestNames()
    {
        List<string> names = new List<string>();
        foreach(Quest q in activeQuests)
        {
            names.Add(q.getName());
        }
        return names;
    }
}
