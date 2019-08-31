using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public void togglePanel(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
}
