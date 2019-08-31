using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class skillDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject Temp;
    private Vector2 offset;
    private SkillMouseOver mouseOver;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Temp = Instantiate(gameObject);
        Temp.transform.SetParent(canvasSingleton.Instance.gameObject.transform);
        Temp.transform.SetAsLastSibling();
        Destroy(Temp.GetComponent<skillDragAndDrop>());
        setSize(Temp);
        offset = new Vector2(10, 10);
        mouseOver = Temp.GetComponent<SkillMouseOver>();
        if (mouseOver != null)
        {
            mouseOver.toggle(false);
        }
        PlayerMotivator.Instance.toggleDragAndDrop(true);
    }

    public void OnDrag(PointerEventData data)
    {
        if (Temp != null)
        {
            Temp.transform.position = data.position + offset;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (mouseOver != null)
        {
            mouseOver.toggle(true);
        }
        PlayerMotivator.Instance.toggleDragAndDrop(false);
        if (data.pointerDrag != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            ButtonSkillMouseover button;
            foreach (RaycastResult element in results)
            {
                button = element.gameObject.GetComponent<ButtonSkillMouseover>();
                if (button != null)
                {
                    button.setSkill(Temp);
                }
            }
        }
        if(Temp != null)
        {
            Destroy(Temp);
        }
    }

    public void setSize(GameObject o)
    {
        //o.transform.localScale = new Vector3(1.0f, 1.0f, 0);
        RectTransform rt = o.GetComponent<RectTransform>();
        if(rt != null)
        {
            rt.sizeDelta = new Vector2(40.0f, 40.0f);
        }
    }
}
