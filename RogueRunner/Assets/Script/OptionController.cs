using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    Button RestartBtn;              //�ش� �������� ����� ��ư
    Button SettingBtn;              //ȯ�� ���� ���� ��ư
    Button HelpBtn;                 //���� ��ư
    Button ExitBtn;                 //���� ���� ��ư

    GameManager CurActionPanel;     //���� Ȱ��ȭ �Ǿ��ִ� �г�.
    public GameObject player;

    private void Start()
    {
        /*
        RestartBtn.onClick.AddListener(OnClickRestartBtn);
        SettingBtn.onClick.AddListener(OnClickSettingBtn);
        HelpBtn.onClick.AddListener(OnClickHelpBtn);
        ExitBtn.onClick.AddListener(OnClickExitBtn);

        */
        //�ʱ⿡�� Ȱ��ȭ�� ���� �г��� ����.
        CurActionPanel = null;
    }

    public void OnClickSettingBtn()
    {
        //����â �߰�
        Debug.Log("���� ��ư Ŭ��");
    }
    public void OnClickRestartBtn()
    {
        //���� �������� ���� �ٽ� ����
        //�������� ���۽�, ī�� �г���
        Debug.Log("����� ��ư Ŭ��");

        //������� �ٽ� �ҷ�����
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void OnClickHelpBtn()
    {
        //���� ����
        Debug.Log("���� ��ư Ŭ��");
    }
    public void OnClickExitBtn()
    {
        //�κ�� �̵��ϱ�.
        Debug.Log("���� ��ư Ŭ��");

        //����� GameManager���� ����Ǿ��ִ� �����͸� ������ ���� DB�� �����Ѵ�.
        //����� GameManager �� ���� ������ ������ ���� SavePlayerData() ȣ��
        PlayerState playerState = player.GetComponent<PlayerController>().getCurPlayerState();
        GameManager.Instance.playerData.UpdatePlayerData(playerState);
        GameManager.Instance.SavePlayerData();

        //�÷��̾� ������ ������ �κ�� �̵�.
        SceneManager.LoadScene("Lobby");
    }
}
