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
    1. ���������� �Ѿ������ �ش� PlayerState�� ������ GameManager�� Update�� �صд�.
    2. ���������� �Ѿ���ʰ�, Option���� ������ ��ư�� Ŭ���� GameManager�� Update�� ���� �ʴ´�.
    ���������� �Ѿ�� ���� Game Manager�� ����Ǿ��ִ� ������ �����صд�.
    ������ ������ �κ�� �̵�������, GameManager�� �ִ� ������ DB�� Update�� �صд�.
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

    //��ź ������
    public GameObject BombPrefab;
    //�ǵ� ��ų ������Ʈ
    public GameObject ShiledEffect;

    public GameObject[] BufList;

    //UI ���� ������Ʈ ����
    public GameObject HPCnt;
    public GameObject CardPanel;
    private GameObject[] cards;             //card ����Ʈ�� GameManager���� �޾ƿ���.
    public GameObject ScoreBoard;
    public GameObject OptionPanel;
    public GameObject StageClearPanel;
    public GameObject StageClear_Score;
    public GameObject GameOverPanel;
    public GameObject GameOver_Score;

    public GameObject player;

    public Button[] skillBtn;

    //�ִϸ��̼� ����
    private Animator animator;

    //�ڷ�ƾ�� �Լ�
    IEnumerator SlowEffect()
    {
        float originalSpeed = state.speed;
        BufList[0].SetActive(true);
        state.speed *= 0.2f;                    // �ӵ��� 0.6��� ����
        yield return new WaitForSeconds(3f);
        state.speed = originalSpeed;            // ���� �ӵ��� ����
        BufList[0].SetActive(false);
    }

    IEnumerator MakeCard()
    {
        int i = -350;
        GameObject cardsParent = new GameObject("Cards");
        cardsParent.transform.SetParent(CardPanel.transform, false);
        //card�� �������̴�.!!!
        foreach (GameObject card in cards)
        {
            GameObject newCard = Instantiate(card);
            newCard.GetComponent<RectTransform>().SetParent(cardsParent.transform, false);    // �θ� ���� �� ���� ��ġ ����
            newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(i, 500);     // ���� ��ġ ����
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
        //collider ���ø� ���� ������� ����.
        gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("Collider ���� " + gameObject.GetComponent<Collider>().enabled);

        // 1�ʰ� ����.
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Collider>().enabled = true;
        Debug.Log("1�� ��, Collider ���� " + gameObject.GetComponent<Collider>().enabled);
    }

    void Start()
    {
        //3���� ĳ����
        animator = gameObject.transform.GetChild(3).GetComponent<Animator>();

        //Scene ���۽� GameManager���� Card ������ ��������.
        cards = GameManager.Instance.getCardPrefab();
        OptionPanel.SetActive(false);
        ShowCard();

        //�� ���۽� �÷��̾� data�� GameManager���� �����´�.
        initState();
    }

    void Update()
    {
        bool isClear = false;
        //���� �Ͻ����� ���°� true�� ��츸...
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
        //�������� Ŭ���� �����̰�, �������� Ŭ���� �г��� Ȱ��ȭ �� ���¶��...?
        if(GameManager.Instance.getGameCode() == "StageClear" && StageClearPanel.active)
        {
            //���콺 ��Ŭ���̳�, �����̽� �ٸ� ���� ���¶��... ���� ������ �̵�.
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
                GameManager.Instance.GoNextStage();
                GameManager.Instance.UpdatePlayerData(state);
            }
        }
    }

    void ShowClearPan()
    {
        StageClearPanel.SetActive(true);

        //�Ҽ��� �ø��Ͽ� state.score�ø���.
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

    //GameManager�κ��� ������ ��������.
    void initState()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();
        Debug.Log($"GameManager�� ���� ������ ������ : HP : {data.curHP}, speed : {data.curSpeed}, Score : {data.curScore}, SHILED : {data.skills["SHILED"]}, TIMER : {data.skills["TIMER"]}");

        state = new PlayerState(data.curHP, data.curSpeed, data.curScore);
        state.skills = data.skills;
    }

    //score�� 1�ʸ��� 1�����ϵ��� ����.
    public float n = 0; // ������Ű���� �ϴ� ����
    private float elapsedTime = 0f; // ��� �ð�
    public float incrementInterval = 1f; // ���� ���� (��)

    void ScoreUp()
    {
        //�ʴ����� ���� ����
        //���Ŀ� ���������� ���� ������ ������ ���.
        elapsedTime += Time.deltaTime;

        // ��� �ð��� ������ ������ ������
        if (elapsedTime >= incrementInterval)
        {
            // n ���� ������ŵ�ϴ�.
            state.score += 1;

            // ��� �ð��� �����մϴ�.
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
            //���� ����.
            Debug.Log("HP ��� ����");
            animator.SetBool("isDeath", true);
            GameManager.Instance.GameOver();
        }
    }

    void CheckSkill()
    {
        //skill ��ųʸ� ��... cnt = 0 �̸� ��Ȱ��ȭ
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
            //�Ŵ������� ���� ����
            if (OptionPanel.active)
            {
                GameManager.Instance.setGameCode("Paused");
            }else
            {
                GameManager.Instance.setGameCode("Start");
            }
            Debug.Log("�Ͻ����� ���� : " + GameManager.Instance.getGameCode());
            Debug.Log("Ÿ�̸� ��� ���� : " + StageManager.Instance.isStopSkilled);
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        // �̵� ���� ���� ���
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        // �̵�
        transform.position += movement * state.speed * Time.deltaTime;
    }
    
    public void MinusHP()
    {
        state.HP--;
        Debug.Log("��ֹ��� �´�Ҵ�. ���� HP" + state.HP);
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
                //�ǵ�
                ShiledCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (state.skills["BOMB"] > 0)
            {
                //��ź
                BombCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //timer
            if (state.skills["TIMER"] > 0)
            {
                StartCoroutine(Skill_Timer());
                Debug.Log("Ÿ�̸� ON");
            }
        }
    }

    //skills
    void BombCasting()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        //��ź
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

        //�ǵ� ����Ʈ ����
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
                Debug.Log("��ź ��ų �߰�");
                state.skills["BOMB"]++;
                break;

            case "SHILED":
                Debug.Log("�ǵ� ��ų �߰�");
                state.skills["SHILED"]++;
                break;

            case "TIMER":
                Debug.Log("Ÿ�̸� ��ų �߰�");
                state.skills["TIMER"]++;
                break;

            case "SPEEDUP":
                Debug.Log("���ǵ� UP!");
                state.speed *= 1.3f;
                break;
        }
    }

    //�ִϸ��̼� ���� �޼���
    public void PlayRun() {
        //���� ���۽� �޸��� ������� ����.
        animator.Play("m_run");
    }

    //���� ���� �׽�Ʈ�� �ڵ�(�׽�Ʈ �Ϸ��� ���� ����
    public void onClickStartBtn()
    {
        GameManager.Instance.setGameCode("Start");
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Ʈ���� �߻�");
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

    //�÷��̾ �������� Ŭ����, ���� ���� ��, Player �����͸� GameManager���� ������.
    public void UpdateState()
    {
        GameManager.Instance.UpdatePlayerData(state);
    }

    public PlayerState getCurPlayerState()
    {
        return state;
    }
}