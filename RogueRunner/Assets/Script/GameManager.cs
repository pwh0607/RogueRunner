using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.Experimental;

/*
    GameState는 게임 진행중 계속 갱신된다.
    PlayerState는 게임 진행중인 상태에서 갱신됩니다. 
    => GameManager에서는 사용자가 게임 중단이나 종료 후, 로비로 이동할때 state들이 갱신된다.
 */

/*      GameCode
 *      Before      게임 시작전 카드를 고르는 상태
 *      Start       게임 진행중  
 *      Pause       일시정지
 *      Death       플레이어가 HP를 모두 소모..
 *      StageClear  스테이지 클리어 상태.
 */

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
    //처음 시작시 게임 스탯.
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

    //DB로 부터 데이터를 받아와서 매핑
    public void MapPlayerData(int HP, float Score, float Speed, Dictionary<string, int> skills)
    {
        this.curHP = HP;
        this.curScore = Score;
        this.curSpeed = Speed;
        this.skills = skills;
    }
    
    //스테이지 클리어시 현재 상태를 저장
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

        apiManager = GetComponent<APIManager>();

        setCharacterDic();

        //씬 이름 추가.
        SceneList = new string[3];
        SceneList[0] = "Forest";
        SceneList[1] = "Grave";
        SceneList[2] = "Desert";          

        //DB에서 이어하기 데이터 가져오기.
        apiManager.GetPlayerData();
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

    //서버와의 연결형 참조
    APIManager apiManager;

    void Start()
    {
        Debug.Log("GameManager Start 메서드 실행됨.");
        
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
    
    //씬전환시..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Lobby")
        {
            Debug.Log("SpawnPlayer 호출");
            Debug.Log($"Stage: {gameState.stage}");     // 디버그 용으로 현재 스테이지 출력
            gameState.stage++;                          // 씬이 로드될 때마다 stage를 1씩 증가
            SpawnPlayer();
        }
        else
        {
            Debug.Log("현재 씬은 Lobby입니다.");
        }
    }

    //데이터 출력.
    void printTotalState()
    {
        Debug.Log($"PlayerState : {playerData.p_id}, {playerData.curHP},{playerData.curSpeed},{playerData.curScore}");
        Debug.Log($"GameState : {gameState.nowScene}, {gameState.stage}");
    }

    public void setCharacterDic()
    {
        //캐릭터 Dictionary 생성. <캐릭터 이름, 캐릭터 오브젝트>
        CharacterDic = new Dictionary<string, CharacterInform>();

        for (int i = 0; i < CharacterList.Length && i < CharIconList.Length; i++)
        {
            CharacterDic[CharacterList[i].name] = new CharacterInform(CharacterList[i], CharIconList[i]);
        }
    }

    public void GameOver()
    {
        gameState.GameCode = "Death";
        Debug.Log("Game Over...");
        apiManager.
        //게임 오버시 점수창 띄우기.
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
    }

    //게임 새로 시작시 state를 새로 설정한다.
    public void initState()
    {
        playerData = new PlayerData();
        gameState = new GameState();
    }

    public PlayerData GetPlayerData()
    {
        Debug.Log("데이터 가져오기...");
        printTotalState();
        return playerData;
    }

    public void UpdatePlayerData(PlayerState p_state)
    {
        playerData.UpdatePlayerData(p_state);
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

    //다음 씬으로 넘어가기
    public void GoNextStage()
    {
        int ran;
        if (gameState.stage == 0)
        {
            //첫시작인 경우...
            ran = Random.Range(0, SceneList.Length);
        }
        else
        {   //이어하기 데이터가 있는 경우...
            do
            {
                //서로 다른 값인 경우...
                ran = Random.Range(0, SceneList.Length);
            } while (SceneList[ran] == gameState.nowScene);
        }

        string newScene = SceneList[ran];

        Debug.Log(SceneList[ran] + "스테이지로 넘어갑니다...");
        gameState.GameCode = "Before";
        GameManager.Instance.gameState.nowScene = SceneList[ran];
        SceneManager.LoadScene(newScene);

        //stage 넘어가면 state update
    }

    public void GotoLobby()
    {
        //로비로 이동하기전 데이터를 랭크 리스트에 넣기.
        /*추가 사항*/
        Debug.Log("로비로 이동합니다...");
        SceneManager.LoadScene("Lobby");
    }

    public void GoContinueStage()
    {
        //이어하기 버튼 클릭시 씬 넘버대로 넘어각.
        string sceneName = gameState.nowScene;

        Debug.Log("스탯 초기화현상 확인 GoContinueStage()");
        printTotalState();
        SceneManager.LoadScene(sceneName);
    }

    public GameObject[] getCharacterList()
    {
        return CharacterList;
    }

    //로비 매니저로 부터 게임 캐릭터를 받아 게임 씬에서 해당 캐릭터를 출력.
    public void setGameCharacter(string character)
    {
        Debug.Log("선택된 캐릭터 : " + character);
        playerData.curCharacter = character;
    }

    void SpawnPlayer()
    {
        //스폰 위치와 선택된 캐릭터의 인스턴스 가져오기
        GameObject SpawnPos = StageManager.Instance.getSpawnPos();
        //(clone) 제거하기.
        GameObject CharacterPrefab = CharacterDic[playerData.curCharacter].CharacterObj;
        if (CharacterPrefab != null)
        {
            GameObject player = Instantiate(PlayerPrefab, SpawnPos.transform.position, SpawnPos.transform.rotation);
            GameObject characterInstance = Instantiate(CharacterPrefab, player.transform);
            characterInstance.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            characterInstance.transform.SetParent(player.transform);

            Debug.Log("캐릭터 생성...!");
        }
        else
        {
            Debug.Log("캐릭터 프리팹 가져오기 실패...");
        }
    }

    public void SavePlayerData()
    {
        //로비로 이동시 데이터들을 서버로 보내 DB 값들을 갱신한다.
        apiManager.SendPlayerData(playerData, gameState);
    }

    //게임 종료시 캐릭터 스탯을 DB에 갱신하는 정보를 서버에 보낸다.
    private void OnApplicationQuit()
    {
        Debug.Log("애플리케이션이 종료됩니다.");

        // 종료 전에 필요한 작업 수행
        // 데이터 저장함수.
    }
}