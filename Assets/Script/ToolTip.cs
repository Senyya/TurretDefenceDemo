using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("ObjectText Information")]
    public string objectname;

    [TextArea]
    public string objectInfo;

    public string objectCost;

    [Header("Display Information")]
    public GameObject toolTipWindow;
    public Text displayName;
    public Text displayInfo;
    public Text displayCost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipWindow.SetActive(true);

        if (toolTipWindow != null)
        {
            displayName.text = objectname;
            displayInfo.text = objectInfo;
            displayCost.text = objectCost;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipWindow.SetActive(false);
    }

}
