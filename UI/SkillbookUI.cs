using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillbookUI : Singleton<SkillbookUI>
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject Content;
    [SerializeField]
    private bool active = false;
    private List<GameObject> skillList;
    private List<GameObject> instanceLibrary;

    public void toggleSkillBook()
    {
        //if not active, make it active
        if(!active)
        {
            createDefaultList();
            activate();
        }
        //if active, deactivate it
        else
        {
            deactivate();
        }
    }
    
    private void buildLibraryFromList()
    {
        instanceLibrary = new List<GameObject>();
        foreach (GameObject skill in skillList)
        {
            GameObject instance = Instantiate(skill, Content.transform);
            instance.AddComponent<skillDragAndDrop>();
            instanceLibrary.Add(instance);
        }
    }

    private void createDefaultList()
    {
        //generate objects to be layed out in the panel
        List<GameObject> knownSkills = SkillLibrary.Instance.getByListID(PlayerMotivator.Instance.getSkillBook());
        skillList = new List<GameObject>();
        OrderEnum pri = PlayerMotivator.Instance.GetPrimaryOrderEnum();
        OrderEnum sec = PlayerMotivator.Instance.GetSecondaryOrderEnum();
        foreach (GameObject skill in knownSkills)
        {
            BaseSkill component = skill.GetComponent<BaseSkill>();
            // make sure that it has a baseSkill component, just in case
            if (component != null)
            {
                OrderEnum skillOrder = component.getOrder();
                // and check if the object is of the right order, then add it to the list
                if (skillOrder == pri || skillOrder == sec || skillOrder == OrderEnum.none)
                {
                    skillList.Add(skill);
                }
            }
        }
    }

    public void filterList(string search)
    {
        destroyList();
        createFilteredList(search);
        buildLibraryFromList();
    }

    private void createFilteredList(string search)
    {
        //generate objects to be layed out in the panel
        List<GameObject> knownSkills = SkillLibrary.Instance.getByListID(PlayerMotivator.Instance.getSkillBook());
        skillList = new List<GameObject>();
        OrderEnum pri = PlayerMotivator.Instance.GetPrimaryOrderEnum();
        OrderEnum sec = PlayerMotivator.Instance.GetSecondaryOrderEnum();
        foreach (GameObject skill in knownSkills)
        {
            BaseSkill component = skill.GetComponent<BaseSkill>();
            // make sure that it has a baseSkill component, just in case
            if (component != null)
            {
                OrderEnum skillOrder = component.getOrder();
                // and check if the object is of the right order, then add it to the list
                if (skillOrder == pri || skillOrder == sec || skillOrder == OrderEnum.none)
                {
                    // Gets the name and searches it for any instances of the search string
                    if (component.getName().ToLower().Contains(search.ToLower())) 
                    {
                        skillList.Add(skill);
                    }
                }
            }
        }
    }


    private void destroyList()
    {
        skillList.Clear();
        foreach (GameObject instance in instanceLibrary)
        {
            Destroy(instance);
        }
    }

    public void updateList()
    {
        if(active)
        {
            destroyList();
            createDefaultList();
        }
    }

    private void activate()
    {
        buildLibraryFromList();
        active = true;
        Panel.SetActive(active);
    }

    private void deactivate()
    {
        destroyList();
        active = false;
        Panel.SetActive(active);
    }
}
