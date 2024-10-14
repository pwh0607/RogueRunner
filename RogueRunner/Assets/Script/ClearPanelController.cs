using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPanelController : MonoBehaviour
{
    private float dropSpeed = 1000f;
    private float dropTime;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        dropTime = 0;

        rectTransform.localPosition = new Vector3(0, 520, 0);
    }

    void Update()
    {
        dropTime += Time.deltaTime;
        if (dropTime > 3.0f)
        {
            if (rectTransform.anchoredPosition.y > 0)
            {
                Vector2 newPosition = rectTransform.anchoredPosition;

                newPosition.y -= dropSpeed * Time.deltaTime;
                if (newPosition.y < 0)
                {
                    newPosition.y = 0;
                }
                rectTransform.anchoredPosition = newPosition;
            }
        }
    }
}
