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
            GameManager.Instance.initState();
            GameManager.Instance.setGameCharacter(character);
            GameManager.Instance.GoNextStage();
        }
        else
        {
            //캐릭터 선택 경고창 띄우기.
        }
    }

    public void OnclickContinueBtn()
    {
        string character = GetComponent<LobbyCharacterController>().getCharacterName();
        string sceneName = GameManager.Instance.gameState.nowScene;
        if (character != null)
        {
            GameManager.Instance.setGameCharacter(character);
            GameManager.Instance.GoContinueStage();
        }
        else
        {
            //캐릭터 선택 경고창 띄우기.
        }
    }

    private void Start()
    {
        NickNameText.GetComponent<TextMeshProUGUI>().text = UserSession.Instance.Nickname;
    }
}