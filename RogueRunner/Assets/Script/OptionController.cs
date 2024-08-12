using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
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
    }
}
