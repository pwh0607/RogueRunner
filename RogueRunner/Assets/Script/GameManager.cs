using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState
{
    public int stage { get; set; }

    /*
        0 : ���� ����.
        1 : ���� ����.
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
    //�̱������� ����
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
        }else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� �ν��Ͻ��� �ı�
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

    //����ȯ��..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState.stage++; // ���� �ε�� ������ stage�� 1�� ����
        Debug.Log($"Stage: {gameState.stage}"); // ����� ������ ���� �������� ���
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
}
