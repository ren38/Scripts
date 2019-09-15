using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Note that this does not inherit from BasicMotivator, as there is only one of these
// and that one does not use any of the normal ways of interacting with the world.

public class PlayerMotivator : Singleton<PlayerMotivator>
{

    //selection references
    private ObjectInteractable selectedObject;
    private ObjectInteractable highlightedObject;

    //navigation reference
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navAgent = null;

    //camera reference and variables
    [SerializeField]
    private GameObject characterCamera = null;
    [SerializeField]
    private float cameraXSensitivity = 60;
    [SerializeField]
    private float cameraYSensitivity = 60;
    [SerializeField]
    private float yMaxAngle = 60;
    [SerializeField]
    private float yMinAngle = -30;
    [SerializeField]
    private float maxDistance = 20;
    [SerializeField]
    private float minDistance = 5;
    private float cameraX;
    private float cameraY;
    private RaycastHit obstructionDetector;
    private RaycastHit hit;
    private float playerScrollDistance;
    private float distance;


    private bool cameraControl = false;
    [SerializeField]
    private ObjectActor actorSelf = null;
    private GameObject[] skillBar;
    [SerializeField]
    private GameObject[] skillSlots = null;


    [SerializeField]
    private SkillQueueUI QueueUI = null;
    [SerializeField]
    private Slider PlayerHealthBar = null;
    [SerializeField]
    private Text PlayerHealthNum = null;
    [SerializeField]
    private Slider PlayerEnergyBar = null;
    [SerializeField]
    private Text PlayerEnergyNum = null;
    [SerializeField]
    private SkillProgressBar PlayerProgressBar = null;
    private bool lastUpdateActivationState;

    [SerializeField]
    private float defaultStoppingDist = 0;

    private bool dragAndDrop = false;

    // team
    private team playersTeam;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        Vector3 angles = transform.eulerAngles;
        cameraX = angles.y;
        cameraY = angles.x;
        QueueUI.setup();
        actorSelf.isPlayer = true;
    }

    void Awake()
    {
        if (actorSelf == null)
        {
            Debug.Log("Warning! actorSelf not found.");
        }
        if (characterCamera == null)
        {
            Debug.Log("Warning! Main Camera not found.");
        }
        if (navAgent == null)
        {
            Debug.Log("Warning! NavMeshAgent not found.");
        }

        skillBar = actorSelf.getSkillBar();
        if (skillBar == null)
        {
            Debug.Log("Warning! skillBar not here after pulling from actor.");
            skillBar = new GameObject[8];
        }
        int index = 0;
        foreach (GameObject target in skillBar)
        {
            if (target != null)
            {
                Instantiate(target, skillSlots[index].transform).transform.SetSiblingIndex(0);
                index++;
            }
        }
    }


    private void Update()
    {
        //selection handler
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Object ObjectHit = hit.collider.GetComponent<Object>();
            ObjectInteractable interactableObjectHit = hit.collider.GetComponent<ObjectInteractable>();
            ObjectCombatable CombatableObjectHit = hit.collider.GetComponent<ObjectCombatable>();
            ObjectActor ActorObjectHit = hit.collider.GetComponent<ObjectActor>();
            if ((interactableObjectHit != null) && !EventSystem.current.IsPointerOverGameObject())
            {
                /*
                 * Really need to start documenting this earlier in the process.
                 * 
                 * 
                 * This bit is handling highlighting and dehighlighting as the mouse passes over something.
                 */
                if (interactableObjectHit != selectedObject)
                {
                    if (highlightedObject == null)
                    {
                        interactableObjectHit.beingHighlighted(true);
                        highlightedObject = interactableObjectHit;
                    }
                    else
                    {
                        highlightedObject.beingHighlighted(false);
                        highlightedObject = interactableObjectHit;
                        highlightedObject.beingHighlighted(true);
                    }
                }
                /*
                 * This handles the player's selection and reselection system.
                 */
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedObject != interactableObjectHit)
                    {
                        if (selectedObject != null)
                        {
                            selectedObject.beingSelected(false);
                        }
                        interactableObjectHit.beingSelected(true);
                        selectedObject = interactableObjectHit;
                        highlightedObject = null;
                        string name = ObjectHit.name;
                        if(ActorObjectHit != null)
                        {
                            TargetPanel.Instance.newActorTarget(ActorObjectHit, name);
                        }
                        else if (CombatableObjectHit != null)
                        {
                            TargetPanel.Instance.newCombatableTarget(CombatableObjectHit, name);
                        }
                        else
                        {
                            TargetPanel.Instance.newInteractableTarget(interactableObjectHit, name);
                        }
                    }
                }
            }
            else if (highlightedObject != null && (EventSystem.current.IsPointerOverGameObject() || hit.collider.tag == "Terrain"))
            {
                highlightedObject.beingHighlighted(false);
                highlightedObject = null;
            }

            //movement
            if (Input.GetMouseButton(0) && hit.collider.gameObject.tag == "Terrain" 
                && !EventSystem.current.IsPointerOverGameObject() 
                && !actorSelf.getDeathState() && !dragAndDrop)
            {
                navAgent.stoppingDistance = defaultStoppingDist;
                navAgent.SetDestination(hit.point);
                if (Input.GetButton("Shift"))
                {
                    //MoveAction newAction = new MoveAction(hit.point);
                    //ActionQueue.Instance.Enqueue(newAction);
                }
            }


        }

        else if (highlightedObject != null)
        {
            highlightedObject.beingHighlighted(false);
            highlightedObject = null;
        }


        PlayerHealthBar.value = actorSelf.getPercentHealth();
        PlayerHealthNum.text = Mathf.Round(actorSelf.getCurrentHealth()).ToString();
        PlayerEnergyBar.value = actorSelf.getPercentEnergy();
        PlayerEnergyNum.text = Mathf.Round(actorSelf.getCurrentEnergy()).ToString();

        if(actorSelf.getSkillActivating() && !lastUpdateActivationState)
        {
            PlayerProgressBar.setActive(true);
            PlayerProgressBar.setName(actorSelf.getNameOfSkillInProgress());
            PlayerProgressBar.placeIcon(actorSelf.getReferenceToSkillInProgress());
        }
        else if (actorSelf.getSkillActivating() && lastUpdateActivationState)
        {
            PlayerProgressBar.setPercent(actorSelf.getSkillProgress());
        }
        else
        {
            PlayerProgressBar.removeIcon();
            PlayerProgressBar.setActive(false);
        }
        lastUpdateActivationState = actorSelf.getSkillActivating();



        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerTriggerSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerTriggerSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerTriggerSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerTriggerSkill(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerTriggerSkill(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            playerTriggerSkill(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerTriggerSkill(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerTriggerSkill(7);
        }
    }

    private void LateUpdate()
    {
        //camera
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            cameraControl = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            cameraControl = false;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            playerScrollDistance = Mathf.Clamp(playerScrollDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
            distance = playerScrollDistance;
        }

        if (cameraControl)
        {

            cameraX += Input.GetAxis("Mouse X") * cameraXSensitivity * 0.02f;
            cameraY -= Input.GetAxis("Mouse Y") * cameraYSensitivity * 0.02f;

            cameraY = Mathf.Clamp(cameraY, yMinAngle, yMaxAngle);
        }
        Quaternion rotation = Quaternion.Euler(cameraY, cameraX, 0);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + transform.position;

        int ignoredLayers = 9; // 9 is the actors layer, since actors should never obstruct the view, since they will be moving too much, and one should probably be paying attention to them anyway.
        if (Physics.Linecast(transform.position, position, out obstructionDetector, ignoredLayers)) 
        {
            distance = Vector3.Distance(obstructionDetector.point, transform.position) - 1;
            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + transform.position;
        }
        

        characterCamera.transform.rotation = rotation;
        characterCamera.transform.position = position;
    }

    public void playerTriggerSkill(int index)
    {
        ObjectCombatable combatableTarget = (ObjectCombatable)selectedObject;
        if (combatableTarget != null)
        {
            actorSelf.skillEnqueue(index, combatableTarget);
        }
    }

    public List<int> getSkillBook()
    {
        return actorSelf.getSkillBookInts();
    }

    public void replaceSkill(int index, GameObject newSkill)
    {
        BaseSkill skill = newSkill.GetComponent<BaseSkill>();
        GameObject ToBeDestroyed = skillBar[index];

        if (skill != null)
        {
            foreach (GameObject s in skillBar)
            {
                if(s != null)
                {
                    BaseSkill t = s.GetComponent<BaseSkill>();
                    if (t != null)
                    {
                        if (t.getID() == skill.getID())
                        {
                            // replaceSkill(System.Array.IndexOf(skillBar, s), ToBeDestroyed);
                            int switchIndex = System.Array.IndexOf(skillBar, s);
                            Destroy(skillBar[switchIndex]);
                            placeSkill(switchIndex, ToBeDestroyed);
                            // instead, call the place function twice
                        }
                    }
                }
            }
        }
        placeSkill(index, newSkill);
        // seperate this into a function
        if (ToBeDestroyed != null)
        {
            Debug.Log("Destroying: " + ToBeDestroyed.name);
            Destroy(ToBeDestroyed);
        }
    }

    private void placeSkill(int index, GameObject newSkill)
    {
        if (newSkill != null)
        {
            skillBar[index] = Instantiate(newSkill, skillSlots[index].transform);
            RectTransform rt = skillBar[index].GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(80.0f, 80.0f);
            }
            skillBar[index].transform.localPosition = new Vector3(0, 0, 0);
            skillBar[index].transform.SetSiblingIndex(0);
            actorSelf.skillChange(index, skillBar[index]);
        }
    }
    public void toggleDragAndDrop(bool setting)
    {
        dragAndDrop = setting;
    }

    public void resolveQuestRewards(Quest finishedQuest)
    {
        learnAllSkillsInList(finishedQuest.getRewardSkillList());
    }

    public void learnSkill(int newSkill)
    {
        actorSelf.addSkillToSkillbook(newSkill);
        SkillbookUI.Instance.updateList();
    }

    public void learnAllSkillsInList(List<int> list)
    {
        actorSelf.addSkillListToSkillbook(list);
        SkillbookUI.Instance.updateList();
    }

    public OrderEnum GetPrimaryOrderEnum()
    {
        return actorSelf.getPrimaryOrderEnum();
    }

    public OrderEnum GetSecondaryOrderEnum()
    {
        return actorSelf.getSecondaryOrderEnum();
    }

    public void setTeam(team team)
    {
        playersTeam = team;
        actorSelf.setTeam(team);
    }
}
