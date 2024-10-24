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
        // �̱��� �ν��Ͻ� ����
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
            //ĳ���� ���� ���â ����.
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
            //ĳ���� ���� ���â ����.
        }
    }

    private void Start()
    {
        NickNameText.GetComponent<TextMeshProUGUI>().text = UserSession.Instance.Nickname;
    }
}