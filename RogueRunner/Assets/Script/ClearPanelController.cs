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
        //���� ��ġ
        rectTransform.localPosition = new Vector3(0, 520, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //�ִϸ��̼� �Ϸ� ��(3�� ��) �г� ������.
        dropTime += Time.deltaTime;
        if (dropTime > 3.0f)
        {
            if (rectTransform.anchoredPosition.y > 0)
            {
                // ���� ��ġ�� �����ͼ�
                Vector2 newPosition = rectTransform.anchoredPosition;
                // y ���� ���ҽ�Ŵ
                newPosition.y -= dropSpeed * Time.deltaTime;
                if (newPosition.y < 0)
                {
                    newPosition.y = 0;
                }
                // �ٽ� �Ҵ�
                rectTransform.anchoredPosition = newPosition;
            }
        }
    }
}
