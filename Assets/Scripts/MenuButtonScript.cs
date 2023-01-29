using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// while the button is clicked, its text label gets moved down by a distance value
// once the button is released, the text label moves back up

public class MenuButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform textTransform;
    [SerializeField] float distance = 12;

    public void OnPointerDown(PointerEventData eventData)
    {
        textTransform.anchoredPosition -= new Vector2(0, distance);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        textTransform.anchoredPosition += new Vector2(0, distance);
    }
}
