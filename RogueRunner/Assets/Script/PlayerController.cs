using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Runtime.CompilerServices;

public class PlayerState {
    public int HP { get; set; }
    public float speed { get; set; }
    public Dictionary<string, int> skills { get; set; }

    public PlayerState(int hp, float speed)
    {
        this.HP = hp;
        this.speed = speed;
        skills = new Dictionary<string, int>();
    }
}

public class PlayerController : MonoBehaviour
{
    //player 스폰위치
    public GameObject SpawnPos;
    private PlayerState state;

    //UI 관련 오브젝트 참조
    public GameObject HPCnt;
    public GameObject CardPanel;
    public GameObject[] cards;
    public Button[] skillBtn;


    //코루틴용 함수
    IEnumerator SlowEffect()
    {
        float originalSpeed = state.speed;
        state.speed *= 0.2f;                // 속도를 0.6배로 줄임
        yield return new WaitForSeconds(3f);
        state.speed = originalSpeed;        // 원래 속도로 복구
    }

    IEnumerator MakeCard()
    {
        int i = -350;
        GameObject cardsParent = new GameObject("Cards");
        cardsParent.transform.SetParent(CardPanel.transform, false);
        //card는 프리팹이다.!!!
        foreach (GameObject card in cards)
        {
            GameObject newCard = Instantiate(card);
            newCard.GetComponent<RectTransform>().SetParent(cardsParent.transform, false);    // 부모 설정 및 로컬 위치 유지
            newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(i, 500);     // 로컬 위치 설정
            i += 350;

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = SpawnPos.transform.position;


        //Scene 시작시 GameManager에서 Card 프리팹 가져오기.
        cards = GameManager.Instance.getCardPrefab();
        showCard();

        //씬 시작시 플레이어 data를 GameManager에서 가져온다.
        initState();
    }

    // Update is called once per frame
    void Update()
    {
        //만약 진행 상태가 true인 경우만...
        if (GameManager.Instance.getStart())
        {
            Move();
            CheckHP();
            CheckSkill();
        }
    }
    
    //GameManager로부터 데이터 가져오기.
    void initState()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();
        state = new PlayerState(data.curHP, data.curSpeed);
        state.skills = data.skills;
    }

    void CheckHP()
    {
        HPCnt.GetComponent<TextMeshProUGUI>().text = state.HP.ToString();
        if (state.HP <= 0)
        {
            //게임 종료.
            Debug.Log("HP 모두 소진");
            GameManager.Instance.GameOver();
        }
    }
    void CheckSkill()
    {
        //skill 딕셔너리 중... cnt = 0 이면 비활성화
        if (state.skills["SHILED"] <= 0)
        {
            skillBtn[0].interactable = false;
        }
        else
        {
            skillBtn[0].interactable = true;
        }

        if (state.skills["BOMB"] <= 0)
        {
            skillBtn[1].interactable = false;
        }
        else
        {
            skillBtn[1].interactable = true;
        }

        if (state.skills["TIMER"] <= 0)
        {
            skillBtn[2].interactable = false;
        }
        else
        {
            skillBtn[2].interactable = true;
        }
    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        // 이동 방향 벡터 계산
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        // 이동
        transform.position += movement * state.speed * Time.deltaTime;
    }
    
    public void MinusHP()
    {
        state.HP--;
        Debug.Log("장애물에 맞닿았다. 남은 HP" + state.HP);

        /*
         2초 무적시간 추가하기
         */
    }

    public void showCard()
    {
        StartCoroutine(MakeCard());
    }

    public void showBuf()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌발생");
        //장애물에 부딪힌 경우.

    }   
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("트리거 발생");
        if (collision.gameObject.tag == "OBSTACLE")
        {
            MinusHP();
        }
        if (collision.gameObject.tag == "SLOW")
        {
            Debug.Log("SLOW");
            StartCoroutine(SlowEffect());
        }
    }
    public void ApplyCard(string cardName)
    {
        switch (cardName)
        {
            case "HPUP":
                Debug.Log("HP UP!");
                state.HP++;
                break;

            case "BOMB":
                Debug.Log("폭탄 스킬 추가");
                state.skills["BOMB"]++;
                break;

            case "SHILED":
                Debug.Log("실드 스킬 추가");
                state.skills["SHILED"]++;
                break;

            case "TIMER":
                Debug.Log("타이머 스킬 추가");
                state.skills["TIMER"]++;
                break;

            case "SPEEDUP":
                Debug.Log("스피드 UP!");
                state.speed *= 1.3f;
                break;
        }
    }


    //게임 시작 테스트용 코드(테스트 완료후 삭제 예정
    public void onClickStartBtn()
    {
        GameManager.Instance.setStart();
    }
}