using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    /*
     * This is something like a switch or a lever that will not move or act alone.
     * items, creatures that cannot be harmed or killed but should be named, and other interactables.
     */ 



    static int objectType = 1;

    // Start is called before the first frame update
    void Start()
    {
        setupInteractable();
    }

    public void setupInteractable()
    {
        return;
    }

    [SerializeField]
    private bool defaultColors = true;
    [SerializeField]
    protected Renderer rend;
    [SerializeField]
    protected Color baseColor;
    [SerializeField]
    protected Color selected;
    [SerializeField]
    protected Color highlighted;

    private void Awake()
    {
        rend = gameObject.GetComponent<Renderer>();
        if (defaultColors)
        {
            baseColor = rend.material.color;
            highlighted = Color.Lerp(baseColor, Color.white, 0.2f);
            selected = Color.Lerp(baseColor, Color.green, 0.4f);
        }
    }

    public virtual void beingHighlighted(bool value)
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

    public virtual void beingSelected(bool value)
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
    public int getObjectType()
    {
        return objectType;
    }

}
