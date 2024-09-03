using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "ī�� Ŭ��");
        //ī�� Ŭ���� ���� ����
        GameManager.Instance.setGameCode("Start");
       // Debug.Log("�Ͻ����� ���� : " + GameManager.Instance.getGameCode());
        //Debug.Log("Ÿ�̸� ��� ���� : " + StageManager.Instance.isStopSkilled);

        //player�� ī�� ȿ�� �����ϱ�
        player.GetComponent<PlayerController>().ApplyCard(gameObject.tag);
        player.GetComponent<PlayerController>().PlayRun();
        Destroy(gameObject.transform.parent.gameObject);
    }
}