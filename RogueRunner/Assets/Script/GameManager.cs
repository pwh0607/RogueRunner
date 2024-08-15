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
        0 : ���� ����.
        1 : ���� ����.
     */
   // public bool isPaused { get; set; }
    /*      GameCode
     *      Before      ���� ������ ī�带 ���� ����
     *      Start       ���� ������  
     *      Pause       �Ͻ�����
     *      Death       �÷��̾ HP�� ��� �Ҹ�..
     *      StageClear  �������� Ŭ���� ����.
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
    
    //�������� Ŭ����� ���� ���¸� ����.
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
    //�̱������� ����
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
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
        //�� ���۽� ������ �Ͻ������� �Ǿ���Ѵ�.
        gameState = new GameState(0, "Before");
        playerData = new PlayerData(3, 20f, 0f);
    }

    //����ȯ��..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState.stage++;                          // ���� �ε�� ������ stage�� 1�� ����
        Debug.Log($"Stage: {gameState.stage}");     // ����� ������ ���� �������� ���
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

        // �����Ͽ� �����ϰ� 3���� �ε����� ����
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            prefab[i] = cardPrefab[indices[randomIndex]];
            indices.RemoveAt(randomIndex); // �ߺ� ������ ���� ������ �ε��� ����
        }

        return prefab;
    }

    public void StageClear()
    {
        //�������� Ŭ����� ���� ȭ�� ����.
        gameState.GameCode = "StageClear";
        Debug.Log("�������� Ŭ����.");
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void UpdatePlayerData(PlayerState p_state)
    {
        playerData.UpdatePlayerData(p_state.HP, p_state.speed, p_state.score, p_state.skills);
    }

    //card �̸��� ���� ȿ�� �и�.
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
