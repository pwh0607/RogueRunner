using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.Experimental;

public class CharacterInform
{
    public GameObject CharacterObj;
    public Sprite HeadIcon;

    public CharacterInform(GameObject CharacterObj, Sprite HeadIcon)
    {
        this.CharacterObj = CharacterObj;
        this.HeadIcon = HeadIcon;
    }
}

public class GameState
{
    public int stage { get; set; }
    public string GameCode { get; set; }
    public string nowScene { get; set; }

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

    public void MapPlayerData(int HP, float Score, float Speed, Dictionary<string, int> skills)
    {
        this.curHP = HP;
        this.curScore = Score;
        this.curSpeed = Speed;
        this.skills = skills;
    }
    
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
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        apiManager = GetComponent<APIManager>();

        setCharacterDic();

        SceneList = new string[3];
        SceneList[0] = "Forest";
        SceneList[1] = "Grave";
        SceneList[2] = "Desert";          

        apiManager.GetTemp();
    }

    public GameState gameState;
    public PlayerData playerData;

    public GameObject[] obstaclePrefab;
    public GameObject[] slowPrefab;
    public GameObject[] cardPrefab;

    //캐릭터 종류 리스트
    public GameObject[] CharacterList;
    public Sprite[] CharIconList;
    public GameObject PlayerPrefab;
    public Dictionary<string, CharacterInform> CharacterDic;

    private string[] SceneList;

    private string selectedCharacter;
   
    APIManager apiManager;

    void Start()
    {
        Debug.Log("GameManager Start 메서드 실행됨.");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    //씬전환시..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Lobby")
        {
            gameState.stage++;                          
            SpawnPlayer();
        }
    }

    public void setCharacterDic()
    {
        CharacterDic = new Dictionary<string, CharacterInform>();

        for (int i = 0; i < CharacterList.Length && i < CharIconList.Length; i++)
        {
            CharacterDic[CharacterList[i].name] = new CharacterInform(CharacterList[i], CharIconList[i]);
        }
    }
    
    public GameObject[] getCardPrefab()
    {
        GameObject[] prefab = new GameObject[3];
        List<int> indices = new List<int>();
        for (int i = 0; i < cardPrefab.Length; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            prefab[i] = cardPrefab[indices[randomIndex]];
            indices.RemoveAt(randomIndex); 
        }

        return prefab;
    }

    public void StageClear()
    {
        gameState.GameCode = "StageClear";
    }

    public void GameOver()
    {
        gameState.GameCode = "GameOver";
        initState();
    }

    public void initState()
    {
        playerData = new PlayerData();
        gameState = new GameState();
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void UpdatePlayerData(PlayerState p_state)
    {
        playerData.UpdatePlayerData(p_state);
    }

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

    public void GoNextStage()
    {
        int ran;
        if (gameState.stage == 0)
        {
            ran = Random.Range(0, SceneList.Length);
        }
        else
        { 
            do
            {
                ran = Random.Range(0, SceneList.Length);
            } while (SceneList[ran] == gameState.nowScene);
        }

        string newScene = SceneList[ran];

        gameState.GameCode = "Before";
        GameManager.Instance.gameState.nowScene = SceneList[ran];
        SceneManager.LoadScene(newScene);
    }

    public void GotoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void GoContinueStage()
    {
        string sceneName = gameState.nowScene;
        SceneManager.LoadScene(sceneName);
    }

    public GameObject[] getCharacterList()
    {
        return CharacterList;
    }

    public void setGameCharacter(string character)
    {
        playerData.curCharacter = character;
    }

    void SpawnPlayer()
    {
        GameObject SpawnPos = StageManager.Instance.getSpawnPos();
        GameObject CharacterPrefab = CharacterDic[playerData.curCharacter].CharacterObj;

        if (CharacterPrefab != null)
        {
            GameObject player = Instantiate(PlayerPrefab, SpawnPos.transform.position, SpawnPos.transform.rotation);
            GameObject characterInstance = Instantiate(CharacterPrefab, player.transform);
            characterInstance.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            characterInstance.transform.SetParent(player.transform);
        }
    }

    public void SavePlayerData()
    {
        apiManager.SendPlayerData(playerData, gameState);
    }

    private void OnApplicationQuit()
    {

    }

    public void SendPlayerScore()
    {
        apiManager.SendFinalScoreData(playerData, gameState);
    }
}