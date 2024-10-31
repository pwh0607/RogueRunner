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
        Debug.Log("세팅 버튼 클릭");
    }
    public void OnClickRestartBtn()
    {
        Debug.Log("재시작 버튼 클릭");

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnClickHelpBtn()
    {
        Debug.Log("도움말 버튼 클릭");
    }

    public void OnClickExitBtn()
    {
        Debug.Log("퇴장 버튼 클릭");

        PlayerState playerState = player.GetComponent<PlayerController>().getCurPlayerState();
        GameManager.Instance.playerData.SetPlayerData(playerState.HP, playerState.score, playerState.speed, playerState.skills);
        GameManager.Instance.SavePlayerData();
        SceneManager.LoadScene("Lobby");
    }
}
