using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCombatable : ObjectInteractable
{
    /*
     * Combatable objects cannot suffer from normal effects,
     * only burning and certain circumstantial effects.
     */
    static int objectType = 2;
    [SerializeField]
    protected bool dead = false;
    [SerializeField]
    private Color deadColor;

    void Awake()
    {
        setupCombatable();
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
    }
    */

    protected void setupCombatable()
    {
        //HitObservers = new List<GameObjectObserver>();
        //DamageObservers = new List<FloatAdjuster>();

        targettedObservers = new List<comboObserver>();
        deathObservers = new List<GameObjectObserver>();

        rawHitObservers = new List<GameObjectObserver>();
        rawDamageObservers = new List<FloatAdjuster>();
        piercingHitObservers = new List<GameObjectObserver>();
        piercingDamageObservers = new List<FloatAdjuster>();
        bludgeoningHitObservers = new List<GameObjectObserver>();
        bludgeoningDamageObservers = new List<FloatAdjuster>();
        slashingHitObservers = new List<GameObjectObserver>();
        slashingDamageObservers = new List<FloatAdjuster>();
        fireHitObservers = new List<GameObjectObserver>();
        fireDamageObservers = new List<FloatAdjuster>();
        coldHitObservers = new List<GameObjectObserver>();
        coldDamageObservers = new List<FloatAdjuster>();
        electricHitObservers = new List<GameObjectObserver>();
        electricDamageObservers = new List<FloatAdjuster>();
        occultHitObservers = new List<GameObjectObserver>();
        occultDamageObservers = new List<FloatAdjuster>();
        acidHitObservers = new List<GameObjectObserver>();
        acidDamageObservers = new List<FloatAdjuster>();
        poisonHitObservers = new List<GameObjectObserver>();
        poisonDamageObservers = new List<FloatAdjuster>();
        physicalHitObservers = new List<GameObjectObserver>();
        physicalDamageObservers = new List<FloatAdjuster>();
        magicHitObservers = new List<GameObjectObserver>();
        magicDamageObservers = new List<FloatAdjuster>();
        arcaneHitObservers = new List<GameObjectObserver>();
        arcaneDamageObservers = new List<FloatAdjuster>();
        radiantHitObservers = new List<GameObjectObserver>();
        radiantDamageObservers = new List<FloatAdjuster>();
        drainingHitObservers = new List<GameObjectObserver>();
        drainingDamageObservers = new List<FloatAdjuster>();
        divineHitObservers = new List<GameObjectObserver>();
        divineDamageObservers = new List<FloatAdjuster>();

        rawHealingObservers = new List<GameObjectObserver>();
        rawHealingDamageObservers = new List<FloatAdjuster>();
        radiantHealingHitObservers = new List<GameObjectObserver>();
        radiantHealingDamageObservers = new List<FloatAdjuster>();
        bioHealingHitObservers = new List<GameObjectObserver>();
        bioHealingDamageObservers = new List<FloatAdjuster>();
        mechHealingHitObservers = new List<GameObjectObserver>();
        mechHealingDamageObservers = new List<FloatAdjuster>();

        deadColor = Color.Lerp(baseColor, Color.green, 0.4f);
        base.setupInteractable();
    }

    [SerializeField]
    protected float maxHealth = 50;
    [SerializeField]
    protected float currentHealth = 50;

    #region No Obs health changes
    public void takeDamageNoObs(float delta, ObjectInteractable source)//This should be for typeless damages; very rare. deltas should be positive.
    {
        if (!dead)
        {
            this.currentHealth -= delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void takeHealingNoObs(float delta, ObjectInteractable source)//healing over time adds. deltas should be positive.
    {
        if (!dead)
        {
            this.currentHealth += delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
    #endregion

    public bool getDeathState()
    {
        return dead;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getPercentHealth()
    {
        return currentHealth / maxHealth;
    }

    public void changeCurrentHealthPercent(float percentChange)
    {
        if (!dead)
        {
            this.currentHealth += this.currentHealth * percentChange;
        }
    }

    public void changeMaxHealth(float delta)
    {
        if (!dead)
        {
            this.maxHealth += delta;
        }
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void changeMaxHealthPercent(float percentChange)
    {
        if (!dead)
        {
            this.maxHealth += this.maxHealth * percentChange;
        }
    }

    public override void beingHighlighted(bool value)
    {
        if (!dead)
        {
            if (value)
            {
                rend.material.color = highlighted;
            }
            else
            {
                rend.material.color = baseColor;
            }
        }
    }

    public override void beingSelected(bool value)
    {
        if (!dead)
        {
            if (value)
            {
                rend.material.color = selected;
            }
            else
            {
                rend.material.color = baseColor;
            }
        }
    }




    #region Armor Functions
    public float trasnslateArmor(int armor)
    {
        float fArmor = (float)armor;
        double suppression = 1.083775 * (Mathf.Atan(Mathf.Pow(fArmor, 1.8f) / 1800) * 2 / Mathf.PI) - .0838;
        return (float)suppression;
    }

    public float applyArmor(int armor, float damage)
    {
        return damage * (1 - trasnslateArmor(armor));
    }
    #endregion

    #region Death Handling
    /*
     * death subscription. Returns game object that caused death.
     */
    private List<GameObjectObserver> deathObservers;

    public void deathSubscribe(GameObjectObserver observer)
    {
        if(deathObservers == null)
        { Debug.Log("WTF"); }


        if (!deathObservers.Contains(observer))
            deathObservers.Add(observer);
        observer.connect(deathObservers);
    }

    private void notifyDeathSubscribers(GameObject source, bool nowDead)
    {
        foreach (var observer in deathObservers)
        {
            observer.trigger(source);
        }
    }
    #endregion

    #region Targetted Handling
    /*
     * targetted subscription. notifies things when this object is the
     * target of an attack. sends a reference to the skill and attacker.
     */
    private List<comboObserver> targettedObservers;

    public void targettedSubscribe(comboObserver observer)
    {
        if (targettedObservers == null)
        { Debug.Log("targettedObservers is null"); }


        if (!targettedObservers.Contains(observer))
            targettedObservers.Add(observer);
        observer.connect(targettedObservers);
    }

    protected void notifyTargettedSubscribers(ObjectInteractable source, BaseSkill skill)
    {
        foreach (var observer in targettedObservers)
        {
            observer.trigger((source, skill));
        }
    }

    public void gettingTargetted(ObjectInteractable source, BaseSkill skill)
    {
        notifyTargettedSubscribers(source, skill);
    }
    #endregion



    #region Raw Damage Handling
    public float takeRawDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in rawHitObservers)
                {                    observer.trigger(source.gameObject);
                }
                foreach (var observer in rawDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            this.currentHealth -= delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            return delta;
        }
        return 0.0f;
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> rawHitObservers;

    public void rawHitSubscribe(GameObjectObserver observer)
    {
        if (!rawHitObservers.Contains(observer))
            rawHitObservers.Add(observer);
        observer.connect(rawHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> rawDamageObservers;

    public void rawDamageSubscribe(FloatAdjuster observer)
    {
        if (!rawDamageObservers.Contains(observer))
            rawDamageObservers.Add(observer);
        observer.connect(rawDamageObservers);
    }
    #endregion

    //Physical
    #region Physical Damage Handling

    [SerializeField]
    private bool takesPhysicalDamage = true;
    [SerializeField]
    private int physicalArmor = 0;
    
    public float takePhysicalDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesPhysicalDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in physicalHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in physicalDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(applyArmor(physicalArmor, delta), source);
        }
        else { return 0; }
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> physicalHitObservers;

    public void physicalHitSubscribe(GameObjectObserver observer)
    {
        if (!physicalHitObservers.Contains(observer))
            physicalHitObservers.Add(observer);
        observer.connect(physicalHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> physicalDamageObservers;

    public void physicalDamageSubscribe(FloatAdjuster observer)
    {
        if (!physicalDamageObservers.Contains(observer))
            physicalDamageObservers.Add(observer);
        observer.connect(physicalDamageObservers);
    }
    #endregion

    #region Piercing Damage Handling

    [SerializeField]
    private bool takesPiercingDamage = true;
    [SerializeField]
    private int piercingArmor = 0;

    public float takePiercingDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesPiercingDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in piercingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in piercingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takePhysicalDamage(delta, source);
        }
        else{ return 0; }
        
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> piercingHitObservers;

    public void piercingHitSubscribe(GameObjectObserver observer)
    {
        if (!piercingHitObservers.Contains(observer))
            piercingHitObservers.Add(observer);
        observer.connect(piercingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> piercingDamageObservers;

    public void piercingDamageSubscribe(FloatAdjuster observer)
    {
        if (!piercingDamageObservers.Contains(observer))
            piercingDamageObservers.Add(observer);
        observer.connect(piercingDamageObservers);
    }
    #endregion

    #region Bludgeoning Damage Handling

    [SerializeField]
    private bool takesBludgeoningDamage = true;
    [SerializeField]
    private int bludgeoningArmor = 0;
    public float takeBludgeoningDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesBludgeoningDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in bludgeoningHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in bludgeoningDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takePhysicalDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> bludgeoningHitObservers;

    public void bludgeoningHitSubscribe(GameObjectObserver observer)
    {
        if (!bludgeoningHitObservers.Contains(observer))
            bludgeoningHitObservers.Add(observer);
        observer.connect(bludgeoningHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> bludgeoningDamageObservers;

    public void bludgeoningDamageSubscribe(FloatAdjuster observer)
    {
        if (!bludgeoningDamageObservers.Contains(observer))
            bludgeoningDamageObservers.Add(observer);
        observer.connect(bludgeoningDamageObservers);
    }
    #endregion

    #region Slashing Damage Handling

    [SerializeField]
    private bool takesSlashingDamage = true;
    [SerializeField]
    private int slashingArmor = 0;
    public float takeSlashingDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesSlashingDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in slashingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in slashingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takePhysicalDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> slashingHitObservers;

    public void slashingHitSubscribe(GameObjectObserver observer)
    {
        if (!slashingHitObservers.Contains(observer))
            slashingHitObservers.Add(observer);
        observer.connect(slashingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> slashingDamageObservers;

    public void slashingDamageSubscribe(FloatAdjuster observer)
    {
        if (!slashingDamageObservers.Contains(observer))
            slashingDamageObservers.Add(observer);
        observer.connect(slashingDamageObservers);
    }
    #endregion

    //Elemental
    #region Fire Damage Handling

    [SerializeField]
    private bool takesFireDamage = true;
    [SerializeField]
    private int fireArmor = 0;
    public float takeFireDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesFireDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in fireHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in fireDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            BurningEffect burning = GetComponent<BurningEffect>();
            if(burning != null)
            {
                burning.addDuration(delta / 100);
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> fireHitObservers;

    public void fireHitSubscribe(GameObjectObserver observer)
    {
        if (!fireHitObservers.Contains(observer))
            fireHitObservers.Add(observer);
        observer.connect(fireHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> fireDamageObservers;

    public void fireDamageSubscribe(FloatAdjuster observer)
    {
        if (!fireDamageObservers.Contains(observer))
            fireDamageObservers.Add(observer);
        observer.connect(fireDamageObservers);
    }
    #endregion

    #region Cold Damage Handling

    [SerializeField]
    private bool takesColdDamage = true;
    [SerializeField]
    private int coldArmor = 0;
    public float takeColdDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesColdDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in coldHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in coldDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            ChilledEffect chilled = GetComponent<ChilledEffect>();
            if (chilled != null)
            {
                chilled.addLoss(delta / 100);
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> coldHitObservers;

    public void coldHitSubscribe(GameObjectObserver observer)
    {
        if (!coldHitObservers.Contains(observer))
            coldHitObservers.Add(observer);
        observer.connect(coldHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> coldDamageObservers;

    public void coldDamageSubscribe(FloatAdjuster observer)
    {
        if (!coldDamageObservers.Contains(observer))
            coldDamageObservers.Add(observer);
        observer.connect(coldDamageObservers);
    }
    #endregion

    #region Electric Damage Handling

    [SerializeField]
    private bool takesElectricDamage = true;
    [SerializeField]
    private int electricArmor = 0;
    public float takeElectricDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesElectricDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in electricHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in electricDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> electricHitObservers;

    public void electricHitSubscribe(GameObjectObserver observer)
    {
        if (!electricHitObservers.Contains(observer))
            electricHitObservers.Add(observer);
        observer.connect(electricHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> electricDamageObservers;

    public void electricDamageSubscribe(FloatAdjuster observer)
    {
        if (!electricDamageObservers.Contains(observer))
            electricDamageObservers.Add(observer);
        observer.connect(electricDamageObservers);
    }
    #endregion

    #region Acid Damage Handling

    [SerializeField]
    private bool takesAcidDamage = true;
    [SerializeField]
    private int acidArmor = 0;
    public float takeAcidDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesAcidDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in acidHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in acidDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> acidHitObservers;

    public void acidHitSubscribe(GameObjectObserver observer)
    {
        if (!acidHitObservers.Contains(observer))
            acidHitObservers.Add(observer);
        observer.connect(acidHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> acidDamageObservers;

    public void acidDamageSubscribe(FloatAdjuster observer)
    {
        if (!acidDamageObservers.Contains(observer))
            acidDamageObservers.Add(observer);
        observer.connect(acidDamageObservers);
    }
    #endregion

    //Bio
    #region Poison Damage Handling

    [SerializeField]
    private bool takesPoisonDamage = true;
    [SerializeField]
    private int poisonArmor = 0;
    public float takePoisonDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesPoisonDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in poisonHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in poisonDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> poisonHitObservers;

    public void poisonHitSubscribe(GameObjectObserver observer)
    {
        if (!poisonHitObservers.Contains(observer))
            poisonHitObservers.Add(observer);
        observer.connect(poisonHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> poisonDamageObservers;

    public void poisonDamageSubscribe(FloatAdjuster observer)
    {
        if (!poisonDamageObservers.Contains(observer))
            poisonDamageObservers.Add(observer);
        observer.connect(poisonDamageObservers);
    }
    #endregion

    //Magical
    #region Magic Damage Handling

    [SerializeField]
    private bool takesMagicDamage = true;
    [SerializeField]
    private int magicArmor = 0;
    public float takeMagicDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesMagicDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in magicHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in magicDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> magicHitObservers;

    public void magicHitSubscribe(GameObjectObserver observer)
    {
        if (!magicHitObservers.Contains(observer))
            magicHitObservers.Add(observer);
        observer.connect(magicHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> magicDamageObservers;

    public void magicDamageSubscribe(FloatAdjuster observer)
    {
        if (!magicDamageObservers.Contains(observer))
            magicDamageObservers.Add(observer);
        observer.connect(magicDamageObservers);
    }
    #endregion

    #region Occult Damage Handling

    [SerializeField]
    private bool takesOccultDamage = true;
    [SerializeField]
    private int occultArmor = 0;
    public float takeOccultDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesOccultDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in occultHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in occultDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeMagicDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> occultHitObservers;

    public void occultHitSubscribe(GameObjectObserver observer)
    {
        if (!occultHitObservers.Contains(observer))
            occultHitObservers.Add(observer);
        observer.connect(occultHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> occultDamageObservers;

    public void occultDamageSubscribe(FloatAdjuster observer)
    {
        if (!occultDamageObservers.Contains(observer))
            occultDamageObservers.Add(observer);
        observer.connect(occultDamageObservers);
    }
    #endregion

    #region Arcane Damage Handling

    [SerializeField]
    private bool takesArcaneDamage = true;
    [SerializeField]
    private int arcaneArmor = 0;
    public float takeArcaneDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesArcaneDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in arcaneHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in arcaneDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeMagicDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> arcaneHitObservers;

    public void arcaneHitSubscribe(GameObjectObserver observer)
    {
        if (!arcaneHitObservers.Contains(observer))
            arcaneHitObservers.Add(observer);
        observer.connect(arcaneHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> arcaneDamageObservers;

    public void arcaneDamageSubscribe(FloatAdjuster observer)
    {
        if (!arcaneDamageObservers.Contains(observer))
            arcaneDamageObservers.Add(observer);
        observer.connect(arcaneDamageObservers);
    }
    #endregion

    #region Divine Damage Handling

    [SerializeField]
    private bool takesDivineDamage = true;
    [SerializeField]
    private int divineArmor = 0;
    public float takeDivineDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesDivineDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in divineHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in divineDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeMagicDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> divineHitObservers;

    public void divineHitSubscribe(GameObjectObserver observer)
    {
        if (!divineHitObservers.Contains(observer))
            divineHitObservers.Add(observer);
        observer.connect(divineHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> divineDamageObservers;

    public void divineDamageSubscribe(FloatAdjuster observer)
    {
        if (!divineDamageObservers.Contains(observer))
            divineDamageObservers.Add(observer);
        observer.connect(divineDamageObservers);
    }
    #endregion

    //Polarity
    #region Radiant Damage Handling

    [SerializeField]
    private bool takesRadiantDamage = true;
    [SerializeField]
    private int RadiantArmor = 0;
    public float takeRadiantDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesRadiantDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in radiantHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in radiantDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }

    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> radiantHitObservers;

    public void radiantHitSubscribe(GameObjectObserver observer)
    {
        if (!radiantHitObservers.Contains(observer))
            radiantHitObservers.Add(observer);
        observer.connect(radiantHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> radiantDamageObservers;

    public void radiantDamageSubscribe(FloatAdjuster observer)
    {
        if (!radiantDamageObservers.Contains(observer))
            radiantDamageObservers.Add(observer);
        observer.connect(radiantDamageObservers);
    }
    #endregion

    #region Draining Damage Handling

    [SerializeField]
    private bool takesDrainingDamage = true;
    [SerializeField]
    private int drainingArmor = 0;
    public float takeDrainingDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead && takesDrainingDamage)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in drainingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in drainingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else { return 0; }
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> drainingHitObservers;

    public void drainingHitSubscribe(GameObjectObserver observer)
    {
        if (!drainingHitObservers.Contains(observer))
            drainingHitObservers.Add(observer);
        observer.connect(drainingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> drainingDamageObservers;

    public void drainingDamageSubscribe(FloatAdjuster observer)
    {
        if (!drainingDamageObservers.Contains(observer))
            drainingDamageObservers.Add(observer);
        observer.connect(drainingDamageObservers);
    }
    #endregion



    #region Raw Healing Handling
    /*
     * healing subscription
     */

    public float takeRawHealing(float delta, ObjectInteractable source)//healing adds. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in rawHealingObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in rawHealingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            this.currentHealth += delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            return delta;
        }
        else
            return 0;
    }

    private List<GameObjectObserver> rawHealingObservers;

    public void rawHealingHitSubscribe(GameObjectObserver observer)
    {
        if (!rawHealingObservers.Contains(observer))
            rawHealingObservers.Add(observer);
        observer.connect(rawHealingObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> rawHealingDamageObservers;

    public void rawHealingDamageSubscribe(FloatAdjuster observer)
    {
        if (!rawHealingDamageObservers.Contains(observer))
            rawHealingDamageObservers.Add(observer);
        observer.connect(rawHealingDamageObservers);
    }
    #endregion

    #region Radiant Healing Handling
    /*
     * healing subscription
     */

    public float takeRadiantHealing(float delta, ObjectInteractable source)//healing adds. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in radiantHealingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in radiantHealingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawHealing(delta, source);
        }
        else { return 0; }
    }

    private List<GameObjectObserver> radiantHealingHitObservers;

    public void radiantHealingHitSubscribe(GameObjectObserver observer)
    {
        if (!radiantHealingHitObservers.Contains(observer))
            radiantHealingHitObservers.Add(observer);
        observer.connect(radiantHealingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> radiantHealingDamageObservers;

    public void radiantHealingDamageSubscribe(FloatAdjuster observer)
    {
        if (!radiantHealingDamageObservers.Contains(observer))
            radiantHealingDamageObservers.Add(observer);
        observer.connect(radiantHealingDamageObservers);
    }
    #endregion

    #region Bio Healing Handling
    /*
     * healing subscription
     */

    public float takeBioHealing(float delta, ObjectInteractable source)//healing adds. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in bioHealingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in bioHealingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawHealing(delta, source);
        }
        else { return 0; }
    }

    private List<GameObjectObserver> bioHealingHitObservers;

    public void bioHealingHitSubscribe(GameObjectObserver observer)
    {
        if (!bioHealingHitObservers.Contains(observer))
            bioHealingHitObservers.Add(observer);
        observer.connect(bioHealingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> bioHealingDamageObservers;

    public void bioHealingDamageSubscribe(FloatAdjuster observer)
    {
        if (!bioHealingDamageObservers.Contains(observer))
            bioHealingDamageObservers.Add(observer);
        observer.connect(bioHealingDamageObservers);
    }
    #endregion

    #region Mech Healing Handling
    /*
     * healing subscription
     */

    public float takeMechHealing(float delta, ObjectInteractable source)//healing adds. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in mechHealingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in mechHealingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawHealing(delta, source);
        }
        else { return 0; }
    }

    private List<GameObjectObserver> mechHealingHitObservers;

    public void mechHealingHitSubscribe(GameObjectObserver observer)
    {
        if (!mechHealingHitObservers.Contains(observer))
            mechHealingHitObservers.Add(observer);
        observer.connect(mechHealingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> mechHealingDamageObservers;

    public void mechHealingDamageSubscribe(FloatAdjuster observer)
    {
        if (!mechHealingDamageObservers.Contains(observer))
            mechHealingDamageObservers.Add(observer);
        observer.connect(mechHealingDamageObservers);
    }
    #endregion

    /* Damage Types:
     * Piercing
     * Bludgeoning
     * Slashing
     * Fire
     * Cold
     * Electric
     * Occult
     * Acid
     * Poison
     * Radiant
     * Draining
     */

    /* Healing types:
     * radiant: creatures can only recieve this type of healing once every 30 seconds, else the healing = 0, etherial type creatures can only recieve this and raw healing
     * biological: only works on biological creatures
     * mechanical: only works on mechanical creatures
     * raw: all creatures can recieve this type, and there is no restriction to it.
     */
}
