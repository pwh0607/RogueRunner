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
            //���� ���� ���۽� playerstate�� �ʱ�ȭ�Ѵ�.
            Debug.Log("������ ���� �����Ͽ� ���ο� �����Ͱ� �����˴ϴ�.");
            GameManager.Instance.initState();

            //�ʱ�ȭ ���� �ڽ��� ������ ĳ���ʹ� �ٽ� ���� �����Ѵ�.
            GameManager.Instance.setGameCharacter(character);
            //���� ���� ��ư
            GameManager.Instance.GoNextStage();
        }
        else
        {
            Debug.LogWarning("ĳ���͸� �������ּ���...");
        }
    }

    public void OnclickContinueBtn()
    {
        //ĳ���� ��� ������ �̾��� �� �̸� ��������
        string character = GetComponent<LobbyCharacterController>().getCharacterName();
        string sceneName = GameManager.Instance.gameState.nowScene;
        if (character != null)
        {
            GameManager.Instance.setGameCharacter(character);

            //���� �̾��ϱ� ��ư
            GameManager.Instance.GoContinueStage();
        }
        else
        {
            Debug.LogWarning("ĳ���͸� �������ּ���...");
        }
    }

    private void Start()
    {
        Debug.Log(UserSession.Instance.P_Id + "�� �������~ �κ��Դϴ�.");

        //nick ĭ�� ���.
        NickNameText.GetComponent<TextMeshProUGUI>().text = UserSession.Instance.Nickname;
    }
}