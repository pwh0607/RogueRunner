using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using System;

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

    public GameObject BombPrefab;
    public GameObject ShiledEffect;
    public GameObject[] BufList;

    public GameObject HPCnt;
    public GameObject CardPanel;
    private GameObject[] cards;             
    public GameObject ScoreBoard;
    public GameObject OptionPanel;
    
    public GameObject StageClearPanel;
    public GameObject StageClear_Score;

    public GameObject GameOverPanel;
    public GameObject GameOver_Score;

    public GameObject player;

    public Button[] skillBtn;

    private Animator animator;
    private bool isClear;

    IEnumerator SlowEffect()
    {
        float originalSpeed = state.speed;
        BufList[0].SetActive(true);
        state.speed *= 0.2f;                    
        yield return new WaitForSeconds(3f);
        state.speed = originalSpeed;            
        BufList[0].SetActive(false);
    }

    IEnumerator MakeCard()
    {
        int i = -350;
        GameObject cardsParent = new GameObject("Cards");
        cardsParent.transform.SetParent(CardPanel.transform, false);
 
        foreach (GameObject card in cards)
        {
            GameObject newCard = Instantiate(card);
            newCard.GetComponent<RectTransform>().SetParent(cardsParent.transform, false);    
            newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(i, 500);     
            i += 400;

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Skill_Timer()
    {
        StageManager.Instance.isStopSkilled = true;
        BufList[1].SetActive(true);
        yield return new WaitForSeconds(2f);
        StageManager.Instance.isStopSkilled = false;
        BufList[1].SetActive(false);
    }

    IEnumerator Skill_Shiled()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Collider>().enabled = true;
    }

    void Start()
    {
        animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        cards = GameManager.Instance.getCardPrefab();
        OptionPanel.SetActive(false);
        isClear = false;
        ShowCard();

        initState();
    }

    void Update()
    {
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

        if (!isClear && GameManager.Instance.getGameCode() == "StageClear")
        {
            ShowClearPan();
            animator.SetBool("isClear", true);
            isClear=true;         
        }
        
        if(StageClearPanel.activeSelf)
        {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
                GameManager.Instance.GoNextStage();
                GameManager.Instance.UpdatePlayerData(state);
            }
        }

        if (GameOverPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.SendPlayerScore();
                GameManager.Instance.GotoLobby();
            }
        }
    }

    void ShowClearPan()
    {
        StageClearPanel.SetActive(true);

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

    void initState()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();
        state = new PlayerState(data.curHP, data.curSpeed, data.curScore);
        state.skills = data.skills;
    }

    private float sceneTime = 0f;
    private float elapsedTime = 0f;
    public float incrementInterval = 1f;

    void ScoreUp()
    {
        if (GameManager.Instance.getGameCode() == "Start")
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= incrementInterval)
            {
                state.score += 1 * GameManager.Instance.gameState.stage;
                sceneTime += 1;
                elapsedTime = 0f;
            }
        }

        int showScore = (int)Math.Ceiling(state.score);
        ScoreBoard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = showScore.ToString();

        if (sceneTime >= 100f)
        {
            GameManager.Instance.StageClear();
            sceneTime = 0;
        }
    }

    void CheckHP()
    {
        HPCnt.GetComponent<TextMeshProUGUI>().text = state.HP.ToString();
        if (state.HP <= 0)
        {
            Debug.Log("HP 모두 소진 GameOver");
            animator.SetBool("isDeath", true);
            GameManager.Instance.GameOver();
            ShowOverPan();
        }
    }

    void CheckSkill()
    {
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
            OptionPanel.SetActive(!OptionPanel.activeSelf);
            if (OptionPanel.activeSelf)
            {
                GameManager.Instance.setGameCode("Paused");
            }else
            {
                GameManager.Instance.setGameCode("Start");
            }
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        transform.position += movement * state.speed * Time.deltaTime;
    }
    
    public void MinusHP()
    {
        state.HP--;
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
                state.skills["SHILED"]--;
                ShiledCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (state.skills["BOMB"] > 0)
            {
                BombCasting();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (state.skills["TIMER"] > 0)
            {
                state.skills["TIMER"]--;
                StartCoroutine(Skill_Timer());
            }
        }
    }

    void BombCasting()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
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
                state.HP++;
                break;

            case "BOMB":
                state.skills["BOMB"]++;
                break;

            case "SHILED":
                state.skills["SHILED"]++;
                break;

            case "TIMER":
                state.skills["TIMER"]++;
                break;

            case "SPEEDUP":
                state.speed *= 1.3f;
                break;
        }
    }

    public void PlayRun() {
        animator.Play("m_run");
    }

    public void onClickStartBtn()
    {
        GameManager.Instance.setGameCode("Start");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "OBSTACLE")
        {
            MinusHP();
        }

        if (collision.gameObject.tag == "SLOW")
        {
            StartCoroutine(SlowEffect());
        }
    }

    public void UpdateState()
    {
        GameManager.Instance.UpdatePlayerData(state);
    }
    
    public PlayerState getCurPlayerState()
    {
        return state;
    }
}