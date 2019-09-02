using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPanel : Singleton<TargetPanel>
{
    public static TargetPanel targetPanel;
    [SerializeField]
    private GameObject panel = null;
    [SerializeField]
    private Text TargetName = null;
    [SerializeField]
    private Slider TargetHealthBar = null;
    [SerializeField]
    private GameObject TargetHealthBarObject = null;
    [SerializeField]
    private Text TargetHealthNum = null;
    [SerializeField]
    private Slider TargetEnergyBar = null;
    [SerializeField]
    private GameObject TargetEnergyBarObject = null;
    [SerializeField]
    private Text TargetEnergyNum = null;
    [SerializeField]
    private ObjectActor Player = null;
    [SerializeField]
    private ActiveEffectUI effects = null;
    [SerializeField]
    private GameObject effectsObject = null;
    private ObjectInteractable TargetInteractable;
    private ObjectCombatable TargetCombatable;
    private ObjectActor TargetActor;
    private int targetType = 0; //0 means unset
    private bool HealthBarActive = false;
    private bool EnergyBarActive = false;

    [SerializeField]
    private SkillQueueUI targetQueue = null;
    [SerializeField]
    private GameObject queueObject = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (HealthBarActive && targetType >= 2) // Actor or combatable
        {
            TargetHealthBar.value = TargetCombatable.getPercentHealth();
            TargetHealthNum.text = Mathf.Round(TargetCombatable.getCurrentHealth()).ToString();
            if(EnergyBarActive)
            {
                TargetEnergyBar.value = TargetActor.getPercentEnergy();
                TargetEnergyNum.text = Mathf.Round(TargetActor.getCurrentEnergy()).ToString();
            }
        }
    }

    public void Deactivate()
    {
        TargetActor = null;
        TargetCombatable = null;
        TargetInteractable = null;
        targetType = 0;
        TargetName.text = "";
        panel.SetActive(false);
        TargetHealthBarObject.SetActive(false);
        if(queueObject != null)
        {
            queueObject.SetActive(false);
        }
    }

    private void resetTargetPanel()
    {
        //healthBarSet(false);
        /*
        if (TargetHealthBarObject == null)
        {
            Debug.Log("HealthbarObject is null");
        }
        */
        if (TargetHealthBarObject.activeSelf)
        {
            TargetHealthBarObject.SetActive(false);
        }
        //energyBarSet(false);
        if (TargetEnergyBarObject.activeSelf)
        {
            TargetEnergyBarObject.SetActive(false);
        }
        if(targetType == 3)
        {
            targetQueue.unsetQueue();
        }
    }

    public void newActorTarget(ObjectActor Target, string name)
    {
        Debug.Log("New Actor found");
        effectsObject.SetActive(true);
        panel.SetActive(true);
        resetTargetPanel();
        this.TargetActor = Target;
        this.TargetCombatable = Target as ObjectCombatable;
        targetType = 3;
        TargetName.text = name;
        queueObject.SetActive(true);
        targetQueue.setQueue(TargetActor);
        effects.setList(TargetActor);
        //if (Player.getWis() >= 20)
        //{
        TargetHealthBarObject.SetActive(true);
        healthBarSet(true);
        //if (Player.getWis() >= 30 && Player.getInt() >= 20)
        //{
        TargetEnergyBarObject.SetActive(true);
        energyBarSet(true);
        //}
        //}
        
    }
    public void newCombatableTarget(ObjectCombatable Target, string name)
    {
        effectsObject.SetActive(false);
        panel.SetActive(true);
        resetTargetPanel();
        this.TargetCombatable = Target;
        targetType = 2;
        TargetName.text = name;
        TargetHealthBarObject.SetActive(true);
        //if (Player.getWis() >= 20)
        //{
        TargetHealthBarObject.SetActive(true);
        healthBarSet(true);
        //}
        TargetEnergyBarObject.SetActive(false);
    }
    public void newInteractableTarget(ObjectInteractable Target, string name)
    {
        effectsObject.SetActive(false);
        panel.SetActive(true);
        resetTargetPanel();
        this.TargetInteractable = Target;
        targetType = 1;
        TargetName.text = name;
        TargetHealthBarObject.SetActive(false);
        TargetEnergyBarObject.SetActive(false);
    }

    public void healthBarSet(bool setting) { HealthBarActive = setting; }
    public void energyBarSet(bool setting) { EnergyBarActive = setting; }
}
