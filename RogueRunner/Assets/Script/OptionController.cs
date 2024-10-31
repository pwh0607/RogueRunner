using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    Button RestartBtn;              
    Button SettingBtn;              
    Button HelpBtn;                
    Button ExitBtn;                

    GameManager CurActionPanel;     
    public GameObject player;

    private void Start()
    {
        CurActionPanel = null;
    }

    public void OnClickSettingBtn()
    {
        Debug.Log("���� ��ư Ŭ��");
    }
    public void OnClickRestartBtn()
    {
        Debug.Log("����� ��ư Ŭ��");

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnClickHelpBtn()
    {
        Debug.Log("���� ��ư Ŭ��");
    }

    public void OnClickExitBtn()
    {
        Debug.Log("���� ��ư Ŭ��");

        PlayerState playerState = player.GetComponent<PlayerController>().getCurPlayerState();
        GameManager.Instance.playerData.SetPlayerData(playerState.HP, playerState.score, playerState.speed, playerState.skills);
        GameManager.Instance.SavePlayerData();
        SceneManager.LoadScene("Lobby");
    }
}
