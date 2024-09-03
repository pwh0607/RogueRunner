using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPanelController : MonoBehaviour
{
    private float dropSpeed = 1000f;
    private float dropTime;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        dropTime = 0;
        //최초 위치
        rectTransform.localPosition = new Vector3(0, 520, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //애니메이션 완료 후(3초 뒤) 패널 내리기.
        dropTime += Time.deltaTime;
        if (dropTime > 3.0f)
        {
            if (rectTransform.anchoredPosition.y > 0)
            {
                // 현재 위치를 가져와서
                Vector2 newPosition = rectTransform.anchoredPosition;
                // y 값을 감소시킴
                newPosition.y -= dropSpeed * Time.deltaTime;
                if (newPosition.y < 0)
                {
                    newPosition.y = 0;
                }
                // 다시 할당
                rectTransform.anchoredPosition = newPosition;
            }
        }
    }
}
