using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginSignUpToolController : MonoBehaviour
{
    GameObject LoginTool;
    GameObject SignUpTool;
    GameObject ActiveTool;

    public Button LoginToolBtn;
    public Button SignUpToolBtn;

    TextMeshProUGUI LoginBtnText;
    TextMeshProUGUI SignUpBtnText;

    private Color defaultColor;
    public Color selectedColor;

    void Start()
    {
        // 로그인 및 회원가입 도구의 GameObject 참조 설정
        LoginTool = transform.GetChild(0).gameObject;
        SignUpTool = transform.GetChild(1).gameObject;

        ActiveTool = LoginTool;
        SignUpTool.SetActive(false);

        // 버튼 이벤트 리스너 추가
        LoginToolBtn.onClick.AddListener(OnClickLoginToolBtn);
        SignUpToolBtn.onClick.AddListener(OnClickSignUpToolBtn);

        // 기본 및 선택된 색상 설정 (정상 범위 사용)
        defaultColor = new Color(0, 0, 0); // 흰색
        selectedColor = new Color(0, 0, 1); // 파란색

        // TextMeshProUGUI 컴포넌트 참조 설정
        LoginBtnText = LoginToolBtn.GetComponentInChildren<TextMeshProUGUI>();
        SignUpBtnText = SignUpToolBtn.GetComponentInChildren<TextMeshProUGUI>();

        // 초기 텍스트 색상 설정
        LoginBtnText.color = selectedColor; // 로그인 버튼이 기본 활성 상태이므로 선택된 색상
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickLoginToolBtn()
    {
        // 로그인 도구 활성화 및 회원가입 도구 비활성화
        LoginTool.SetActive(true);
        SignUpTool.SetActive(false);

        // 버튼 텍스트 색상 변경
        LoginBtnText.color = selectedColor;
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickSignUpToolBtn()
    {
        // 로그인 도구 비활성화 및 회원가입 도구 활성화
        LoginTool.SetActive(false);
        SignUpTool.SetActive(true);

        // 버튼 텍스트 색상 변경
        LoginBtnText.color = defaultColor;
        SignUpBtnText.color = selectedColor;
    }
}