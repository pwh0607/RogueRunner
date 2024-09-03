using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    Button RestartBtn;              //해당 스테이지 재시작 버튼
    Button SettingBtn;              //환경 설정 세팅 버튼
    Button HelpBtn;                 //도움말 버튼
    Button ExitBtn;                 //게임 퇴장 버튼

    GameManager CurActionPanel;     //현재 활성화 되어있는 패널.
    public GameObject player;

    private void Start()
    {
        /*
        RestartBtn.onClick.AddListener(OnClickRestartBtn);
        SettingBtn.onClick.AddListener(OnClickSettingBtn);
        HelpBtn.onClick.AddListener(OnClickHelpBtn);
        ExitBtn.onClick.AddListener(OnClickExitBtn);

        */
        //초기에는 활성화된 세부 패널이 없음.
        CurActionPanel = null;
    }

    public void OnClickSettingBtn()
    {
        //세팅창 추가
        Debug.Log("세팅 버튼 클릭");
    }
    public void OnClickRestartBtn()
    {
        //현재 스테이지 부터 다시 시작
        //스테이지 시작시, 카드 패널은
        Debug.Log("재시작 버튼 클릭");

        //현재씬을 다시 불러오기
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void OnClickHelpBtn()
    {
        //도움말 띄우기
        Debug.Log("도움말 버튼 클릭");
    }
    public void OnClickExitBtn()
    {
        //로비로 이동하기.
        Debug.Log("퇴장 버튼 클릭");

        //퇴장시 GameManager에서 저장되어있는 데이터를 서버에 보내 DB를 갱신한다.
        //퇴장시 GameManager 내 스탯 값들을 갱신한 다음 SavePlayerData() 호출
        PlayerState playerState = player.GetComponent<PlayerController>().getCurPlayerState();
        GameManager.Instance.playerData.UpdatePlayerData(playerState);
        GameManager.Instance.SavePlayerData();

        //플레이어 데이터 저장후 로비로 이동.
        SceneManager.LoadScene("Lobby");
    }
}
