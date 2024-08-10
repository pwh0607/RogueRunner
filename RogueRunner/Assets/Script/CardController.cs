using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private float dropSpeed = 1000f;
    private RectTransform rectTransform;
    public GameObject player;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        player = GameObject.FindWithTag("PLAYER");
    }

    // Update is called once per frame
    void Update()
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

    public void onClickCard()
    {
        Debug.Log(gameObject.name + "ī�� Ŭ��");

        //ī�� Ŭ���� ���� ����
        GameManager.Instance.setStart();

        //player�� ī�� ȿ�� �����ϱ�
        player.GetComponent<PlayerController>().ApplyCard(gameObject.tag);

        Destroy(gameObject.transform.parent.gameObject);
    }
}