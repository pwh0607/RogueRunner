using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject NickNameText;
    string p_idText;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnclickStartBtn()
    {
        string character = GetComponent<LobbyCharacterController>().getCharacterName();
        if(character != null)
        {
            //게임 새로 시작시 playerstate를 초기화한다.
            Debug.Log("게임을 새로 시작하여 새로운 데이터가 생성됩니다.");
            GameManager.Instance.initState();

            //초기화 이후 자신이 선택한 캐릭터는 다시 새로 세팅한다.
            GameManager.Instance.setGameCharacter(character);
            //게임 시작 버튼
            GameManager.Instance.GoNextStage();
        }
        else
        {
            Debug.LogWarning("캐릭터를 선택해주세요...");
        }
    }

    public void OnclickContinueBtn()
    {
        //캐릭터 명과 이전에 이어한 씬 이름 가져오기
        string character = GetComponent<LobbyCharacterController>().getCharacterName();
        string sceneName = GameManager.Instance.gameState.nowScene;
        if (character != null)
        {
            GameManager.Instance.setGameCharacter(character);

            //게임 이어하기 버튼
            GameManager.Instance.GoContinueStage();
        }
        else
        {
            Debug.LogWarning("캐릭터를 선택해주세요...");
        }
    }

    private void Start()
    {
        Debug.Log(UserSession.Instance.P_Id + "님 어서오세요~ 로비입니다.");

        //nick 칸에 출력.
        NickNameText.GetComponent<TextMeshProUGUI>().text = UserSession.Instance.Nickname;
    }
}