using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements.Experimental;

public class GameState
{
    public int stage { get; set; }

    /*
        0 : 게임 중지.
        1 : 게임 진행.
     */
   // public bool isPaused { get; set; }
    /*      GameCode
     *      Before      게임 시작전 카드를 고르는 상태
     *      Start       게임 진행중  
     *      Pause       일시정지
     *      Death       플레이어가 HP를 모두 소모..
     *      StageClear  스테이지 클리어 상태.
     */
    public string GameCode { get; set; }

    public GameState(int stage, string GameCode)
    {
        this.stage = stage;
        this.GameCode = GameCode;
    }
}

public class PlayerData
{
    public int curHP { get; set; }
    public float curSpeed { get; set; }
    public float curScore { get; set; }
    public Dictionary<string, int> skills { get; set; }

    public PlayerData(int HP, float speed, float score)
    {
        curHP = HP;
        curSpeed = speed;
        curScore = score;
        skills = new Dictionary<string, int>();
        
        skills.Add("BOMB", 10);
        skills.Add("SHILED", 10);
        skills.Add("TIMER", 10);
    }
    
    //스테이지 클리어시 현재 상태를 저장.
    public void UpdatePlayerData(int HP, float speed, float score, Dictionary<string, int> skills)
    {
        curHP = HP;
        curSpeed = speed;
        curScore = score;
        this.skills = skills;
    }
}

public class GameManager : MonoBehaviour
{
    //싱글톤으로 세팅
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }else
        {
            Destroy(gameObject);    
        }
    }

    GameState gameState;
    PlayerData playerData;

    public GameObject[] obstaclePrefab;
    public GameObject[] slowPrefab;
    public GameObject[] cardPrefab;

    void Start()
    {
        //씬 시작시 게임이 일시정지가 되어야한다.
        gameState = new GameState(0, "Before");
        playerData = new PlayerData(3, 20f, 0f);
    }

    //씬전환시..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState.stage++;                          // 씬이 로드될 때마다 stage를 1씩 증가
        Debug.Log($"Stage: {gameState.stage}");     // 디버그 용으로 현재 스테이지 출력
    }

    public void GameOver()
    {
        Debug.Log("Game Over...");
    }
    
    public GameObject[] getCardPrefab()
    {
        GameObject[] prefab = new GameObject[3];
        List<int> indices = new List<int>();
        for (int i = 0; i < cardPrefab.Length; i++)
        {
            indices.Add(i);
        }

        // 셔플하여 랜덤하게 3개의 인덱스를 선택
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            prefab[i] = cardPrefab[indices[randomIndex]];
            indices.RemoveAt(randomIndex); // 중복 방지를 위해 선택한 인덱스 제거
        }

        return prefab;
    }

    public void StageClear()
    {
        //스테이지 클리어시 게임 화면 멈춤.
        gameState.GameCode = "StageClear";
        Debug.Log("스테이지 클리어.");
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void UpdatePlayerData(PlayerState p_state)
    {
        playerData.UpdatePlayerData(p_state.HP, p_state.speed, p_state.score, p_state.skills);
    }

    //card 이름에 따라 효과 분리.
    public void setSkill(string card)
    {
        switch (card)
        {
            case "HP":
                break;

            case "BOMB":
                break;

            case "SHILED":
                break;

            case "TIMER":
                break;

            case "SPEEDUP":
                playerData.curSpeed *= 1.2f;
                break;
        }
    }

    public string getGameCode()
    {
        return gameState.GameCode;
    }
    public void setGameCode(string code)
    {
        gameState.GameCode = code;
    }
}
