using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState
{
    public int stage { get; set; }

    /*
        0 : 게임 중지.
        1 : 게임 진행.
     */
    public bool isStart { get; set; }

    public GameState(int stage, bool isStart)
    {
        this.stage = stage;
        this.isStart = isStart;
    }
}

public class PlayerData
{
    public int curHP { get; set; }
    public float curSpeed { get; set; }
    public Dictionary<string, int> skills { get; set; }

    public PlayerData(int HP, float speed)
    {
        curHP = HP;
        curSpeed = speed;
        skills = new Dictionary<string, int>();
        
        skills.Add("BOMB", 0);
        skills.Add("SHILED", 0);
        skills.Add("TIMER", 0);
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
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시 파괴되지 않도록 설정
        }else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
        }
    }

    GameState gameState;
    PlayerData playerData;

    public GameObject[] obstaclePrefab;
    public GameObject[] slowPrefab;
    public GameObject[] cardPrefab;

    void Start()
    {
        gameState = new GameState(0, false);
        playerData = new PlayerData(3, 20f);
    }

    //씬전환시..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState.stage++; // 씬이 로드될 때마다 stage를 1씩 증가
        Debug.Log($"Stage: {gameState.stage}"); // 디버그 용으로 현재 스테이지 출력
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

    public bool getStart()
    {
        return gameState.isStart;
    }

    public void setStart()
    {
        gameState.isStart = !gameState.isStart;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
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
}
