using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestView : Singleton<QuestView>
{
    private QuestLog log;
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Text description;
    [SerializeField]
    private GameObject QuestPanel;
    [SerializeField]
    private bool active;


    public void Setup()
    {
        log = QuestLog.Instance;
        // grabbing the initial list
        updateList(log.getActiveQuestNames());
        QuestPanel.SetActive(true);
        dropdown.onValueChanged.AddListener(delegate {
            grabQuest();
        });
        QuestPanel.SetActive(active);

        grabQuest();
    }

    public void updateList(List<string> quests)
    {
        QuestPanel.SetActive(true);
        //something has changed about the list in the dropdown
        dropdown.ClearOptions();
        dropdown.AddOptions(quests);
        QuestPanel.SetActive(active);
    }


    public void updateView(Quest newQuest)
    {
        // changing active quest details

    }

    public void setDropdown(List<string> newList)
    {
        // set list in the dropdown

    }

    public void grabQuest()
    {
        Quest a = log.GetQuestByName(dropdown.options[dropdown.value].text);
        if(a != null)
        {
            QuestPanel.SetActive(true);
            description.text = a.getDescription();
            QuestPanel.SetActive(active);
        }
    }

    public void toggleWindow()
    {
        active = !active;
        QuestPanel.SetActive(active);
    }
}
