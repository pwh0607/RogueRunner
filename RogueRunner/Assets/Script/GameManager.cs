using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.Experimental;

/*
    GameState�� ���� ������ ��� ���ŵȴ�.
    PlayerState�� ���� �������� ���¿��� ���ŵ˴ϴ�. 
    => GameManager������ ����ڰ� ���� �ߴ��̳� ���� ��, �κ�� �̵��Ҷ� state���� ���ŵȴ�.
 */

    /*      GameCode
     *      Before      ���� ������ ī�带 ���� ����
     *      Start       ���� ������  
     *      Pause       �Ͻ�����
     *      Death       �÷��̾ HP�� ��� �Ҹ�..
     *      StageClear  �������� Ŭ���� ����.
     */

public class GameState
{
    public int stage { get; set; }
    public string GameCode { get; set; }
    public string nowScene { get; set; }
    //ó�� ���۽� ���� ����.
    public GameState()
    {
        this.stage = 0;
        this.GameCode = "Before";
        this.nowScene = "Forest";
    }
    public void MapGameState(int Stage, string SceneName)
    {
        this.stage= Stage;
        this.nowScene= SceneName;
    }
}

public class PlayerData
{
    public string p_id { get; set; }
    public int curHP { get; set; }
    public float curSpeed { get; set; }
    public float curScore { get; set; }
    public string curCharacter { get; set; }
    public Dictionary<string, int> skills { get; set; }

    public PlayerData()
    {
        p_id = UserSession.Instance.P_Id;
        curHP = 3;
        curSpeed = 20f;
        curScore = 0;
        curCharacter = "";
        skills = new Dictionary<string, int>();
        skills.Add("BOMB", 3); skills.Add("SHILED", 3); skills.Add("TIMER", 3);
    }

    //DB�� ���� �����͸� �޾ƿͼ� ����
    public void MapPlayerData(int HP, float Score, float Speed, string CharName, Dictionary<string, int> skills)
    {
        this.curHP = HP;
        this.curScore = Score;
        this.curSpeed = Speed;
        this.curCharacter = CharName;
        this.skills = skills;
    }
    
    //�������� Ŭ����� ���� ���¸� ����
    public void UpdatePlayerData(PlayerState p_state)
    {
        this.curHP = p_state.HP;
        this.curSpeed = p_state.speed;
        this.curScore = p_state.score;
        this.skills = p_state.skills;
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

        apiManager = GetComponent<APIManager>();

        //ĳ���� Dictionary ����.
        CharacterDic = new Dictionary<string, GameObject>();

        for (int i = 0; i < CharacterList.Length; i++)
        {
            if (!CharacterDic.ContainsKey(CharacterList[i].name))
            {
                CharacterDic.Add(CharacterList[i].name, CharacterList[i]);
            }
        }

        //�� �̸� �߰�.
        SceneList = new string[3];
        SceneList[0] = "Forest";
        SceneList[1] = "Grave";
        SceneList[2] = "Desert";          //���� �߰��� ���ο� ��.

        //DB���� �̾��ϱ� ������ ��������.
        //������ initState() ����.
        apiManager.GetPlayerData();
    }

    public GameState gameState;
    public PlayerData playerData;

    public GameObject[] obstaclePrefab;
    public GameObject[] slowPrefab;
    public GameObject[] cardPrefab;

    //ĳ���� ���� ����Ʈ
    public GameObject[] CharacterList;
    public GameObject PlayerPrefab;
    public Dictionary<string, GameObject> CharacterDic;

    private string[] SceneList;

    private string selectedCharacter;

    //�������� ������ ����
    APIManager apiManager;

    void Start()
    {
        Debug.Log("GameManager Start �޼��� �����.");
        
        //printTotalState();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void printTotalState()
    {
        Debug.Log($"PlayerState : {playerData.p_id}, {playerData.curHP},{playerData.curSpeed},{playerData.curScore}");
        Debug.Log($"GameState : {gameState.nowScene}, {gameState.stage}");
    }

    //����ȯ��..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Lobby")
        {
            Debug.Log("SpawnPlayer ȣ��");
            Debug.Log($"Stage: {gameState.stage}");     // ����� ������ ���� �������� ���
            gameState.stage++;                          // ���� �ε�� ������ stage�� 1�� ����
            SpawnPlayer();
        }
        else
        {
            Debug.Log("���� ���� Lobby�Դϴ�.");
        }
    }

    public void GameOver()
    {
        gameState.GameCode = "Death";
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
    }

    //���� ���� ���۽� state�� ���� �����Ѵ�.
    public void initState()
    {
        playerData = new PlayerData();
        gameState = new GameState();
    }

    public PlayerData GetPlayerData()
    {
        Debug.Log("������ ��������...");
        printTotalState();
        return playerData;
    }

    public void UpdatePlayerData(PlayerState p_state)
    {
        playerData.UpdatePlayerData(p_state);
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

    //���� ������ �Ѿ��
    public void GoNextStage()
    {
        int ran;
        if (gameState.stage == 0)
        {
            //ù������ ���...
            ran = Random.Range(0, SceneList.Length);
        }
        else
        {
            //���� �ٸ� ���� ���...
            do
            {
                ran = Random.Range(0, SceneList.Length);
            } while (SceneList[ran] == gameState.nowScene);
        }

        Debug.Log(SceneList[ran] + "���������� �Ѿ�ϴ�...");
        gameState.GameCode = "Before";
        GameManager.Instance.gameState.nowScene = SceneList[ran];
        SceneManager.LoadScene(SceneList[ran]);

        //stage �Ѿ�� state update
    }

    public void GoContinueStage()
    {
        //�̾��ϱ� ��ư Ŭ���� �� �ѹ���� �Ѿ.
        string sceneName = gameState.nowScene;

        Debug.Log("���� �ʱ�ȭ���� Ȯ�� GoContinueStage()");
        printTotalState();
        SceneManager.LoadScene(sceneName);
    }

    public GameObject[] getCharacterList()
    {
        return CharacterList;
    }

    //�κ� �Ŵ����� ���� ���� ĳ���͸� �޾� ���� ������ �ش� ĳ���͸� ���.
    public void setGameCharacter(string character)
    {
        Debug.Log("���õ� ĳ���� : " + character);
        playerData.curCharacter = character;
    }

    void SpawnPlayer()
    {
        //���� ��ġ�� ���õ� ĳ������ �ν��Ͻ� ��������
        GameObject SpawnPos = StageManager.Instance.getSpawnPos();
        //(clone) �����ϱ�.
        GameObject CharacterPrefab = CharacterDic[playerData.curCharacter];
        if (CharacterPrefab != null)
        {
            GameObject player = Instantiate(PlayerPrefab, SpawnPos.transform.position, SpawnPos.transform.rotation);
            GameObject characterInstance = Instantiate(CharacterPrefab, player.transform);
            characterInstance.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            characterInstance.transform.SetParent(player.transform);

            Debug.Log("ĳ���� ����...!");
        }
        else
        {
            Debug.Log("ĳ���� ������ �������� ����...");
        }
    }

    public void SavePlayerData()
    {
        //�κ�� �̵��� �����͵��� ������ ���� DB ������ �����Ѵ�.
        apiManager.SendPlayerData(playerData, gameState);
    }

    //���� ����� ĳ���� ������ DB�� �����ϴ� ������ ������ ������.
    private void OnApplicationQuit()
    {
        Debug.Log("���ø����̼��� ����˴ϴ�.");

        // ���� ���� �ʿ��� �۾� ����
        // ������ �����Լ�.
    }
}