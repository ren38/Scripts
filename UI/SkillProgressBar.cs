using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillProgressBar : MonoBehaviour
{
    [SerializeField]
    private GameObject barObject = null;
    [SerializeField]
    private Slider ProgressBar = null;
    [SerializeField]
    private Text PlayerProgressNum = null;
    private bool fading = false;
    [SerializeField]
    private GameObject iconLocation = null;
    private GameObject skill;

    public void setName(string name)
    {
        PlayerProgressNum.text = name;
    }

    public void setPercent(float value)
    {
        ProgressBar.value = value;
    }

    public void placeIcon(GameObject newSkill)
    {
        skill = Instantiate(newSkill, iconLocation.transform);
        skill.transform.localScale = new Vector2(0.5f, 0.5f);
        skill.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void removeIcon()
    {
        Destroy(skill);
    }

    public void startFade()
    {
        //This is where a fade effect will go. Unsure how to do this right, for now.
    }

    public void setActive(bool setting)
    {
        barObject.SetActive(setting);
    }

    private void Update()
    {
        //this needs to fade, not just disappear
    }
}
