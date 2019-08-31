﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActor : ObjectCombatable
{
    [SerializeField]
    private OrderEnum primaryOrder;
    private Order primaryOrderComponent;
    [SerializeField]
    private OrderEnum secondaryOrder;
    private Order secondaryOrderComponent;

    //navigation reference
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navAgent = null;

    static int objectType = 3;
    private List<IEffect> activeEffects;
    private List<(int, ObjectCombatable)> skillQueue;
    private const float ACTIONDELAY = 0.3f;
    private bool nextActionBool = true;
    private float nextActionTime;
    private bool skillActivating = false;
    private int skillActivatingIndex = -1;
    private float skillStartTime;
    private float skillFinishTime;
    private ObjectCombatable nextTarget;
    private GameObject skillReference;
    private (int, ObjectCombatable) activation;
    private float[] cooldown;

    private bool autoMovingIntoRange = false;
    const int SKILLBARSIZE = 8;
    [SerializeField]
    private GameObject[] skillBar = new GameObject[SKILLBARSIZE];

    private team myTeam;

    // Start is called before the first frame update
    void Awake()
    {
        maxHealth += getCon();
        currentHealth = maxHealth;
        activeEffects = new List<IEffect>();
        skillQueue = new List<(int, ObjectCombatable)>();
        dequeueObservers = new List<IntObserver>();
        enqueueObservers = new List<GameObjectObserver>();
        skillFinished = new List<IntObserver>();
        effectRemovalObservers = new List<IntObserver>();
        effectAdditionObservers = new List<GameObjectObserver>();
        effectListChangeObservers = new List<GameObjectObserver>();
        castingSpeedAdjusters = new List<FloatAdjuster>();
        cooldown = new float[8];
        base.setupCombatable();
        initializeOrders();
    }

    [SerializeField]
    protected float maxEnergy = 10;
    [SerializeField]
    protected float currentEnergy = 10;

    #region stats
    void changeCurrentEnergy(float delta)
    {
        if (!dead)
        {
            this.currentEnergy += delta;
            if (currentEnergy <= 0)
            {
                currentEnergy = 0;
            }
            else if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }
    }

    public float getCurrentEnergy(){return currentEnergy;}

    void changeCurrentEnergyPercent(float percentChange)
    {
        if (!dead)
        {
            this.currentEnergy += this.currentEnergy * percentChange;
        }
    } // percent change should be 0 < x >= 1

    public float getPercentEnergy()
    {
        return currentEnergy / maxEnergy;
    }

    void changeMaxEnergy(float delta)
    {
        if (!dead)
        {
            this.maxEnergy += delta;
        }
    }

    public float getMaxEnergy(){return maxEnergy;}

    void changeMaxEnergyPercent(float percentChange)
    {
        if (!dead)
        {
            this.maxEnergy += this.maxEnergy * percentChange;
        }
    }// percent change should be 0 < x >= 1
    #endregion

    #region General Functions
    public team getTeam(){ return myTeam; }
    public void setTeam(team newTeam) { myTeam = newTeam; }
    #endregion

    private void Update()
    {
        if (dead)
        {
            return;
        }
        changeCurrentEnergy(4.0f * Time.deltaTime);

        checkEffects();

        /*
         Skill activation handling
        */
        handleSkillActivation();

        /*
         Handle skill completion
        */
        if (Time.time >= skillFinishTime && skillActivating)
        {
            finishSkill();
        }

        if(skillActivating)
        {
            checkIfInRange();
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Order Handling
    public void setOrders(bool primaryChange, OrderEnum primary, bool secondaryChange, OrderEnum secondary)
    {
        if(primaryChange)
        {
            primaryOrder = primary;
        }
        if(secondaryChange)
        {
            secondaryOrder = secondary;
        }
        if (primaryOrder == secondaryOrder)
        {
            secondaryOrder = OrderEnum.none;
        }
        initializeOrders();
    }

    public void initializeOrders()
    {
        if(primaryOrderComponent != null)
        {
            Destroy(primaryOrderComponent);
        }
        primaryOrderComponent = addOrderComponent(primaryOrder);
        if (secondaryOrderComponent != null)
        {
            Destroy(secondaryOrderComponent);
        }
        secondaryOrderComponent = addOrderComponent(secondaryOrder);
    }

    public Order addOrderComponent(OrderEnum newOrder)
    {
        Order newOrderObject;
        if(newOrder == OrderEnum.devoted)
        {
            newOrderObject = gameObject.AddComponent<Devoted>();
        }
        else if (newOrder == OrderEnum.outlander)
        {
            newOrderObject = gameObject.AddComponent<Outlander>();
        }
        else if (newOrder == OrderEnum.preserver)
        {
            newOrderObject = gameObject.AddComponent<Preserver>();
        }
        else if (newOrder == OrderEnum.reclaimer)
        {
            newOrderObject = gameObject.AddComponent<Reclaimer>();
        }
        else if (newOrder == OrderEnum.serpent)
        {
            newOrderObject = gameObject.AddComponent<Serpent>();
        }
        else if (newOrder == OrderEnum.theurge)
        {
            newOrderObject = gameObject.AddComponent<Theurge>();
        }
        else if (newOrder == OrderEnum.warden)
        {
            newOrderObject = gameObject.AddComponent<Warden>();
        }
        else if (newOrder == OrderEnum.zealot)
        {
            newOrderObject = gameObject.AddComponent<Zealot>();
        }
        else
        {
            newOrderObject = null;
        }
        return newOrderObject;
    }

    public Order getPrimaryOrderComponent()
    { return primaryOrderComponent; }

    public Order getSecondaryOrderComponent()
    { return secondaryOrderComponent; }

    public OrderEnum getPrimaryOrderEnum()
    { return primaryOrder; }

    public OrderEnum getSecondaryOrderEnum()
    { return secondaryOrder; }
    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Effect Handling

    private List<IntObserver> effectRemovalObservers;
    private List<GameObjectObserver> effectAdditionObservers;
    private List<GameObjectObserver> effectListChangeObservers;

    public void checkEffects()
    {
        if (dead)
        {
            return;
        }

        Stack<int> endEffectsIndex = new Stack<int>();
        int count = 0;
        foreach (IEffect effect in activeEffects)
        {
            if (Time.time <= effect.getEnd())
            {
                effect.apply(Time.deltaTime);
            }
            else
            {
                endEffectsIndex.Push(count);
            }
            count++;
        }

        for (int i = endEffectsIndex.Count - 1; i >= 0; i--)
        {
            int ind = endEffectsIndex.Pop();
            foreach (var observer in effectRemovalObservers)
            {
                observer.trigger(ind);
            }
            foreach (var observer in effectListChangeObservers)
            {
                observer.trigger(gameObject);
            }
            IEffect temp = activeEffects[ind];
            activeEffects.RemoveAt(ind);
            temp.end(this);
        }
    }

    public void applyNewEffect(IEffect newEffect)
    {
        if (dead)
        {
            return;
        }

        activeEffects.Add(newEffect);
        foreach (var observer in effectAdditionObservers)
        {
            //translate newEffect to gameobject for the icon.
            GameObject addingEffect = newEffect.getIcon();
            observer.trigger(addingEffect);
        }
        foreach (var observer in effectListChangeObservers)
        {
            observer.trigger(gameObject);
        }
    }

    public List<GameObject> getEffectListObjects()
    {
        List<GameObject> effectList = new List<GameObject>();
        foreach (var e in activeEffects)
        {
            effectList.Add(e.getIcon());
        }
        return effectList;
    }

    public void effectRemoveSubscribe(IntObserver observer)
    {
        if (!effectRemovalObservers.Contains(observer))
            effectRemovalObservers.Add(observer);
        observer.connect(effectRemovalObservers);
    }

    public void effectAddSubscribe(GameObjectObserver observer)
    {
        if (!effectAdditionObservers.Contains(observer))
            effectAdditionObservers.Add(observer);
        observer.connect(effectAdditionObservers);
    }

    public void effectListChangeSubscribe(GameObjectObserver observer)
    {
        if (!effectListChangeObservers.Contains(observer))
            effectListChangeObservers.Add(observer);
        observer.connect(effectListChangeObservers);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Conditions
    public void beginBleed(ObjectInteractable source)
    {
        //check if there is already a bleed effect active.
        //if not, start one. else stack one
        BleedEffect preexisting = gameObject.GetComponent<BleedEffect>();
        if (preexisting == null)
        {
            BleedEffect bleeding = gameObject.AddComponent<BleedEffect>();
            bleeding.setup(this, source);
            applyNewEffect(bleeding);
        }
        else
        {
            preexisting.stack();
        }
    }
    
    public int endBleed()
    {
        BleedEffect preexisting = gameObject.GetComponent<BleedEffect>();
        if (preexisting == null)
        {
            int mult = preexisting.getMult();
            preexisting.end(this);
            return mult;
        }
        return 0;
    }

    public void beginBurning(ObjectInteractable source)
    {
        //check if there is already a bleed effect active.
        //if not, start one. else stack one
        BurningEffect preexisting = gameObject.GetComponent<BurningEffect>();
        if (preexisting == null)
        {
            BurningEffect burning = gameObject.AddComponent<BurningEffect>();
            burning.setup(this, source);
            applyNewEffect(burning);
        }
        else
        {
            preexisting.stack();
        }
    }

    public int endBurning()
    {
        BurningEffect preexisting = gameObject.GetComponent<BurningEffect>();
        if (preexisting == null)
        {
            int mult = preexisting.getMult();
            preexisting.end(this);
            return mult;
        }
        return 0;
    }

    public void beginChilled(ObjectInteractable source)
    {
        //check if there is already a bleed effect active.
        //if not, start one. else stack one
        ChilledEffect preexisting = gameObject.GetComponent<ChilledEffect>();
        if (preexisting == null)
        {
            ChilledEffect chilled = gameObject.AddComponent<ChilledEffect>();
            chilled.setup(this, source);
            applyNewEffect(chilled);
        }
        else
        {
            preexisting.stack();
        }
    }

    public float endChilled()
    {
        ChilledEffect preexisting = gameObject.GetComponent<ChilledEffect>();
        if (preexisting == null)
        {
            float mult = preexisting.getSpeedloss();
            preexisting.end(this);
            return mult;
        }
        return 0;
    }

    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Skillbooks
    [SerializeField]
        private List<int> skillbook;

        public void addSkillToSkillbook(int newSkill)
        {
            if(!skillbook.Contains(newSkill))
            {
                skillbook.Add(newSkill);
            }
        }

        public void addSkillListToSkillbook(List<int> skillList)
        {
            foreach(int i in skillList)
            {
                addSkillToSkillbook(i);
            }
        }

        public List<GameObject> getSkillBookGameObjects()
        {
            return SkillLibrary.Instance.getByListID(skillbook);
        }

        public List<int> getSkillBookInts()
        {
            return skillbook;
        }
        #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Skill Handling

    BaseSkill skillBeingActivated;
    private List<IntObserver> dequeueObservers;
    private List<GameObjectObserver> enqueueObservers;
    private List<IntObserver> skillFinished;

    private void handleSkillActivation()
    {
        if (skillQueue.Count != 0 && Time.time >= nextActionTime && nextActionBool)
        {
            activation = skillQueue[0];
            skillActivatingIndex = activation.Item1;
            if (skillBar[skillActivatingIndex] != null && !activation.Item2.getDeathState())
            {
                skillReference = skillBar[skillActivatingIndex];
                skillBeingActivated = skillReference.GetComponent<BaseSkill>();
                float distance = Vector3.Distance(this.gameObject.transform.position, activation.Item2.transform.position);
                if (skillBeingActivated != null)
                {
                    if (skillBeingActivated.getEnergyCost() <= getCurrentEnergy() && distance <= skillBeingActivated.getRange())
                    {
                        skillQueue.RemoveAt(0);
                        nextActionBool = false;
                        startSkill(activation.Item2);
                    }
                    else if (distance > skillBeingActivated.getRange())
                    {
                        float dist = skillBeingActivated.getRange();
                        navAgent.stoppingDistance = dist;
                        navAgent.SetDestination(activation.Item2.transform.position);
                    }
                }
                else
                {
                    Debug.Log("Skill got nulled.");
                }
            }
            else if (activation.Item2.getDeathState())
            {
                skillQueue.RemoveAt(0);
                foreach (var observer in dequeueObservers)
                {
                    observer.trigger(0);
                }
            }
        }
    }

    private void checkIfInRange()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, activation.Item2.transform.position);
        if (distance > skillBeingActivated.getRange())
        {
            abruptSkillCancel();
        }
    }

    public void skillDequeueSubscribe(IntObserver observer)
    {
        if (!dequeueObservers.Contains(observer))
            dequeueObservers.Add(observer);
        observer.connect(dequeueObservers);
    }

    public void skillEnqueueSubscribe(GameObjectObserver observer)
    {
        if (!enqueueObservers.Contains(observer))
            enqueueObservers.Add(observer);
        observer.connect(enqueueObservers);
    }

    public void skillFinishSubscribe(IntObserver observer)
    {
        if (!skillFinished.Contains(observer))
            skillFinished.Add(observer);
        observer.connect(skillFinished);
    }

    public void startSkill(ObjectCombatable target)
    {
        if (dead)
        {
            return;
        }
        if (skillBeingActivated != null)
        {
            nextTarget = target;
            skillActivating = true;
            float activateMultiple = 1;
            foreach(FloatAdjuster i in castingSpeedAdjusters)
            {
                activateMultiple = i.trigger(activateMultiple);
            }
            skillStartTime = Time.time;
            skillFinishTime = Time.time + (skillBeingActivated.getCastTime() * activateMultiple);
            changeCurrentEnergy(-skillBeingActivated.getEnergyCost());
            foreach (var observer in dequeueObservers)
            {
                observer.trigger(0);
            }
            target.gettingTargetted(this, skillBeingActivated);
            addTargets(target);
        }
    }

    private void addTargets(ObjectCombatable newTarget)
    {
        Debug.Log("adding target");
        if (myTeam != null)
        {
            Debug.Log(gameObject.name + " myTeam != null");
            ObjectActor actorTarget = (ObjectActor)newTarget;
            if (actorTarget != null)
            {
                Debug.Log("actor target found");
                team newEnemyTeam = actorTarget.getTeam();
                if(newEnemyTeam != null)
                {
                    Debug.Log("adding team");
                    myTeam.addEnemyTeam(newEnemyTeam);
                }
                else
                {
                    Debug.Log("adding individual");
                    myTeam.addEnemyIndividual(newTarget);
                }
            }
        }
    }
    /*
        * damage subscription. Returns the amount of damage taken.
        */
    private List<FloatAdjuster> castingSpeedAdjusters;

    public void skillStartSubscribe(FloatAdjuster observer)
    {
        if (!castingSpeedAdjusters.Contains(observer))
            castingSpeedAdjusters.Add(observer);
        observer.connect(castingSpeedAdjusters);
    }


    public void abruptSkillCancel()
    {
        nextActionTime = Time.time + ACTIONDELAY;
        nextActionBool = true;
        skillActivating = false;
    }

    public GameObject getReferenceToSkillInProgress()
    {
        return skillReference;
    }

    public string getNameOfSkillInProgress()
    {
        return skillBeingActivated.getName();
    }

    public bool getSkillActivating()
    {
        return skillActivating;
    }

    public float getSkillFinishTime()
    {
        return skillFinishTime;
    }

    public float getSkillProgress()
    {
        return ((Time.time - skillStartTime) / (skillFinishTime - skillStartTime));
    }

    private void finishSkill()
    {
        skillBeingActivated.activate(this, nextTarget);
        nextActionTime = Time.time + ACTIONDELAY;
        nextActionBool = true;
        skillActivating = false;
        cooldown[skillActivatingIndex] = Time.time + skillBeingActivated.getCooldown();
        foreach (var observer in skillFinished)
        {
            observer.trigger(skillActivatingIndex);
        }
        skillActivatingIndex = -1;
    }

    public GameObject[] getSkillBar()
    {
        return skillBar;
    }

    public void skillChange(int index, GameObject newSkill)
    {
        skillBar[index] = newSkill;
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Queue functions

    public void removeFromQueue(int index)
    {
        skillQueue.RemoveAt(index);
    }

    public void clearQueue()
    {
        for (int i = 0; i < skillQueue.Count; i++)
        {
            foreach (var observer in dequeueObservers)
            {
                observer.trigger(0);
            }
        }
        skillQueue.Clear();
    }

    public int queueCount()
    {
        return skillQueue.Count;
    }

    //Queue<(int, ObjectCombatable)> skillQueue;
    public void skillEnqueue(int skillIndex, ObjectCombatable target)
    {
        if (skillQueue.Count < 4 && !dead && cooldown[skillIndex] <= Time.time && !checkqueue(skillIndex) && skillActivatingIndex != skillIndex)
        {
            GameObject newSkill = skillBar[skillIndex];
            if (newSkill != null)
            {
                skillQueue.Add((skillIndex, target));
                foreach (var observer in enqueueObservers)
                {
                    observer.trigger(newSkill);
                }
            }
        }
        else
        {
            return;// cannot enqueue if the skillQueue is full or if you are dead.
        }
    }

    public bool checkqueue(int index)
    {
        foreach (var i in skillQueue)
        {
            if (i.Item1 == index)
            {
                return true;
            }
        }
        return false;
    }

    public List<GameObject> getDereferencedSkillQueue()
    {
        List<GameObject> queue = new List<GameObject>();
        foreach ((int, ObjectCombatable) element in skillQueue)
        {
            queue.Add(Instantiate(skillBar[element.Item1]));
        }
        return queue;
    }

    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region Cooldown Handling
    public float getCooldownTime(int index)
    {
        return cooldown[index];
    }

    private void zeroAllCooldowns()
    {
        for(int i = 0; i < 8; i++)
        {
            cooldown[i] = 0.0f;
        }
    }
    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    #endregion
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    /* ABILITY SCORES
     * 
     * These comments below are ideas and not at all set in stone.
     * 
     * Strength and constitution may be folded into brawn. charisma and spirit may be folded into spirit due
     * to the possible difficulty of creating meaningful social interactions using charisma, which is usually a
     * dump-stat in ttrpgs anyway.
    */
    #region Ability Score implementations
    [SerializeField]
    private int strength = 20; //Handles hit chance & damage for melee and thrown weapons.
    [SerializeField]
    private int dexterity = 20; //Part of hit chance for non-thrown ranged weapons, increases defense.
    [SerializeField]
    private int constitution = 20; //Determines max health, max energy, and increases resistances. 
    [SerializeField]
    private int intelligence = 20; //Determines most arcane spell effectiveness & enables certain skills and options.
    [SerializeField]
    private int wisdom = 20; //Determines most divine and primal spell effectiveness, increases defense slightly, increases critical strike chance. 
    [SerializeField]
    private int charisma = 20; //Determines most occult spell effectiveness

    void changeStr(int delta)
    {
        if (!dead)
        {
            this.strength += delta;
        }
    }

    public float getStr() { return strength; }


    void changeDex(int delta)
    {
        if (!dead)
        {
            this.dexterity += delta;
        }
    }

    public float getDex() { return dexterity; }


    void changeCon(int delta)
    {
        if (!dead)
        {
            this.constitution += delta;
        }
    }

    public float getCon() { return constitution; }


    void changeInt(int delta)
    {
        if (!dead)
        {
            this.intelligence += delta;
            if (wisdom >= 30 && intelligence >= 20)
            {
                TargetPanel.Instance.energyBarSet(true);
            }
            else
            {
                TargetPanel.Instance.energyBarSet(false);
            }
        }
    }

    public float getInt() { return intelligence; }


    void changeWis(int delta)
    {
        if (!dead)
        {
            this.wisdom += delta;
            if (wisdom >= 30 && intelligence >= 20)
            {
                TargetPanel.Instance.energyBarSet(true);
            }
            else if (wisdom >= 20)
            {
                TargetPanel.Instance.healthBarSet(true);
            }
            else
            {
                TargetPanel.Instance.healthBarSet(false);
            }
        }
    }

    public float getWis() { return wisdom; }

    void changeCha(int delta)
    {
        if (!dead)
        {
            this.charisma += delta;
        }
    }

    public float getCha() { return charisma; }
    #endregion
}