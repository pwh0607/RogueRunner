using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using System;

/*
    1. 스테이지가 넘어갈때마다 해당 PlayerState의 값들을 GameManager에 Update를 해둔다.
    2. 스테이지를 넘어가지않고, Option에서 나가기 버튼을 클릭시 GameManager에 Update를 하지 않는다.
    스테이지가 넘어갈때 마다 Game Manager에 저장되어있는 값들을 저장해둔다.
    게임을 나가고 로비로 이동했을때, GameManager에 있는 값들을 DB에 Update를 해둔다.
 */

public class PlayerState {
    public int HP { get; set; }
    public float speed { get; set; }
    public float score { get; set; }

    public Dictionary<string, int> skills { get; set; }

    public PlayerState(int hp, float speed, float score)
    {
        this.HP = hp;
        this.speed = speed;
        this.score = score;
        skills = new Dictionary<string, int>();
    }
}

public class PlayerController : MonoBehaviour
{
    private PlayerState state;

    //폭탄 프리팹
    public GameObject BombPrefab;
    //실드 스킬 오브젝트
    public GameObject ShiledEffect;

    public GameObject[] BufList;

    //UI 관련 오브젝트 참조
    public GameObject HPCnt;
    public GameObject CardPanel;
    private GameObject[] cards;             //card 리스트는 GameManager에서 받아오기.
    public GameObject ScoreBoard;
    public GameObject OptionPanel;
    public GameObject StageClearPanel;
    public GameObject StageClear_Score;
    public GameObject GameOverPanel;
    public GameObject GameOver_Score;

    public GameObject player;

    public Button[] skillBtn;

    //애니메이션 제어
    private Animator animator;

    //코루틴용 함수
    IEnumerator SlowEffect()
    {
        float originalSpeed = state.speed;
        BufList[0].SetActive(true);
        state.speed *= 0.2f;                    // 속도를 0.6배로 줄임
        yield return new WaitForSeconds(3f);
        state.speed = originalSpeed;            // 원래 속도로 복구
        BufList[0].SetActive(false);
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

    IEnumerator Skill_Timer()
    {
        StageManager.Instance.isStopSkilled = true;
        yield return new WaitForSeconds(2f);
        StageManager.Instance.isStopSkilled = false;
    }

    IEnumerator Skill_Shiled()
    {
        //collider 무시를 통해 무적기능 구현.
        gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("Collider 상태 " + gameObject.GetComponent<Collider>().enabled);

        // 1초간 무적.
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Collider>().enabled = true;
        Debug.Log("1초 후, Collider 상태 " + gameObject.GetComponent<Collider>().enabled);
    }

    void Start()
    {
        //3번이 캐릭터
        animator = gameObject.transform.GetChild(3).GetComponent<Animator>();

        //Scene 시작시 GameManager에서 Card 프리팹 가져오기.
        cards = GameManager.Instance.getCardPrefab();
        OptionPanel.SetActive(false);
        ShowCard();

        //씬 시작시 플레이어 data를 GameManager에서 가져온다.
        initState();
    }

    void Update()
    {
        bool isClear = false;
        //만약 일시정지 상태가 true인 경우만...
        if (GameManager.Instance.getGameCode() == "Start")
        {
            Move();
            CheckHP();
            CheckSkill();
            ScoreUp();
            SkillCasting();
            UpdateSkillCnt();
        }
        PauseGame();

        if (!isClear)
        {
            if (GameManager.Instance.getGameCode() == "StageClear")
            {
                ShowClearPan();
                animator.SetBool("isClear", true);
                isClear = true;
            }
        }
        //스테이지 클리어 상태이고, 스테이지 클리어 패널이 활성화 된 상태라면...?
        if(GameManager.Instance.getGameCode() == "StageClear" && StageClearPanel.active)
        {
            //마우스 우클릭이나, 스페이스 바를 누른 상태라면... 다음 씬으로 이동.
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
                GameManager.Instance.GoNextStage();
                GameManager.Instance.UpdatePlayerData(state);
            }
        }
    }

    void ShowClearPan()
    {
        StageClearPanel.SetActive(true);

        //소수점 올림하여 state.score올리기.
        int ceilScore = (int)Math.Ceiling(state.score);
        StageClear_Score.GetComponent<TextMeshProUGUI>().text = ceilScore.ToString();
        GameManager.Instance.UpdatePlayerData(state);
    }

    void ShowOverPan()
    {
        GameOverPanel.SetActive(true);

        int ceilScore = (int)Math.Ceiling(state.score);
        GameOver_Score.GetComponent<TextMeshProUGUI>().text = ceilScore.ToString();
        GameManager.Instance.UpdatePlayerData(state);
    }

    //GameManager로부터 데이터 가져오기.
    void initState()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();
        Debug.Log($"GameManager로 부터 가져온 데이터 : HP : {data.curHP}, speed : {data.curSpeed}, Score : {data.curScore}, SHILED : {data.skills["SHILED"]}, TIMER : {data.skills["TIMER"]}");

        state = new PlayerState(data.curHP, data.curSpeed, data.curScore);
        state.skills = data.skills;
    }

    //score가 1초마다 1증가하도록 설정.
    public float n = 0; // 증가시키고자 하는 변수
    private float elapsedTime = 0f; // 경과 시간
    public float incrementInterval = 1f; // 증가 간격 (초)

    void ScoreUp()
    {
        //초단위로 점수 증가
        //이후에 스테이지에 따라 점수가 증가량 향상.
        elapsedTime += Time.deltaTime;

        // 경과 시간이 설정된 간격을 넘으면
        if (elapsedTime >= incrementInterval)
        {
            // n 값을 증가시킵니다.
            state.score += 1;

            // 경과 시간을 리셋합니다.
            elapsedTime = 0f;
        }
        Debug.Log("score : " + state.score);
        int showScore = (int)Math.Ceiling(state.score);
        ScoreBoard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = showScore.ToString();
    }

    void CheckHP()
    {
        HPCnt.GetComponent<TextMeshProUGUI>().text = state.HP.ToString();
        if (state.HP <= 0)
        {
            //게임 종료.
            Debug.Log("HP 모두 소진");
            animator.SetBool("isDeath", true);
            GameManager.Instance.GameOver();
        }
    }

    void CheckSkill()
    {
        //skill 딕셔너리 중... cnt = 0 이면 비활성화
        if (state.skills["SHILED"] <= 0)
        {
            skillBtn[0].interactable = false;
        }else
        {
            skillBtn[0].interactable = true;
        }

        if (state.skills["BOMB"] <= 0)
        {
            skillBtn[1].interactable = false;
        }
        else{
            skillBtn[1].interactable = true;
        }

        if (state.skills["TIMER"] <= 0)
        {
            skillBtn[2].interactable = false;
        }else
        {
            skillBtn[2].interactable = true;
        }
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionPanel.SetActive(!OptionPanel.active);
            //매니저에게 중지 설정
            if (OptionPanel.active)
            {
                GameManager.Instance.setGameCode("Paused");
            }else
            {
                GameManager.Instance.setGameCode("Start");
            }
            Debug.Log("일시정지 유무 : " + GameManager.Instance.getGameCode());
            Debug.Log("타이머 사용 유무 : " + StageManager.Instance.isStopSkilled);
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
    }

    public void ShowCard()
    {
        StartCoroutine(MakeCard());
    }

    public void SkillCasting()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(state.skills["SHILED"] > 0)
            {
                //실드
                ShiledCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (state.skills["BOMB"] > 0)
            {
                //폭탄
                BombCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //timer
            if (state.skills["TIMER"] > 0)
            {
                StartCoroutine(Skill_Timer());
                Debug.Log("타이머 ON");
            }
        }
    }

    //skills
    void BombCasting()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        //폭탄
        GameObject bombInstance = Instantiate(BombPrefab);

        if (bombInstance != null)
        {
            bombInstance.SetActive(true);
            bombInstance.transform.position = spawnPos;
        }
        state.skills["BOMB"]--;
    }

    void ShiledCasting()
    {
        StartCoroutine(Skill_Shiled());

        //실드 이펙트 연출
        GameObject ShiledInstance = Instantiate(ShiledEffect, gameObject.transform.position, gameObject.transform.rotation);
        ParticleSystem particleSystem = ShiledInstance.GetComponent<ParticleSystem>();
        ShiledInstance.SetActive(true);
        ShiledInstance.transform.SetParent(transform);
        particleSystem.Play();
        Destroy(ShiledInstance, 1.5f);
    }

    void UpdateSkillCnt()
    {
        skillBtn[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = state.skills["SHILED"].ToString();
        skillBtn[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = state.skills["BOMB"].ToString();
        skillBtn[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = state.skills["TIMER"].ToString();
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

    //애니메이션 제어 메서드
    public void PlayRun() {
        //게임 시작시 달리기 모션으로 변경.
        animator.Play("m_run");
    }

    //게임 시작 테스트용 코드(테스트 완료후 삭제 예정
    public void onClickStartBtn()
    {
        GameManager.Instance.setGameCode("Start");
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

    //플레이어가 스테이지 클리어, 게임 종료 후, Player 데이터를 GameManager에게 보내기.
    public void UpdateState()
    {
        GameManager.Instance.UpdatePlayerData(state);
    }

    public PlayerState getCurPlayerState()
    {
        return state;
    }
}