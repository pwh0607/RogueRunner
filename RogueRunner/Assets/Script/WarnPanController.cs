using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarnPanController : MonoBehaviour, IPointerClickHandler
{

    public void setWarnText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "WarnPan Å¬¸¯!");
        gameObject.SetActive(false);
    }
}
