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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "카드 클릭");
        //카드 클릭시 게임 시작
        GameManager.Instance.setGameCode("Start");
       // Debug.Log("일시정지 유무 : " + GameManager.Instance.getGameCode());
        //Debug.Log("타이머 사용 유무 : " + StageManager.Instance.isStopSkilled);

        //player에 카드 효과 적용하기
        player.GetComponent<PlayerController>().ApplyCard(gameObject.tag);
        player.GetComponent<PlayerController>().PlayRun();
        Destroy(gameObject.transform.parent.gameObject);
    }
}