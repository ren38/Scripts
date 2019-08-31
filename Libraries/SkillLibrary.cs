using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : Singleton<SkillLibrary>
{
    [SerializeField]
    private List<GameObject> library;

    void Start()
    {
        if(library != null)
        {
            int i = 0;
            foreach(GameObject o in library)
            {
                BaseSkill skill = o.GetComponent<BaseSkill>();
                if(skill != null)
                {
                    skill.setID(i);
                    i++;
                }
            }
        }
    }

    public GameObject getByID(int id)
    {
        return library[id];
    }

    public List<GameObject> getByListID(List<int> list)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (int id in list)
        {
            returnList.Add(getByID(id));
        }
        return returnList;
    }
}
