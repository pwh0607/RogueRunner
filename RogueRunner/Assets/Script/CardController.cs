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

    public void onClickCard()
    {
        Debug.Log(gameObject.name + "카드 클릭");

        //카드 클릭시 게임 시작
        GameManager.Instance.setStart();

        //player에 카드 효과 적용하기
        player.GetComponent<PlayerController>().ApplyCard(gameObject.tag);

        Destroy(gameObject.transform.parent.gameObject);
    }
}
