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
        // �α��� �� ȸ������ ������ GameObject ���� ����
        LoginTool = transform.GetChild(0).gameObject;
        SignUpTool = transform.GetChild(1).gameObject;

        ActiveTool = LoginTool;
        SignUpTool.SetActive(false);

        // ��ư �̺�Ʈ ������ �߰�
        LoginToolBtn.onClick.AddListener(OnClickLoginToolBtn);
        SignUpToolBtn.onClick.AddListener(OnClickSignUpToolBtn);

        // �⺻ �� ���õ� ���� ���� (���� ���� ���)
        defaultColor = new Color(0, 0, 0); // ���
        selectedColor = new Color(0, 0, 1); // �Ķ���

        // TextMeshProUGUI ������Ʈ ���� ����
        LoginBtnText = LoginToolBtn.GetComponentInChildren<TextMeshProUGUI>();
        SignUpBtnText = SignUpToolBtn.GetComponentInChildren<TextMeshProUGUI>();

        // �ʱ� �ؽ�Ʈ ���� ����
        LoginBtnText.color = selectedColor; // �α��� ��ư�� �⺻ Ȱ�� �����̹Ƿ� ���õ� ����
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickLoginToolBtn()
    {
        // �α��� ���� Ȱ��ȭ �� ȸ������ ���� ��Ȱ��ȭ
        LoginTool.SetActive(true);
        SignUpTool.SetActive(false);

        // ��ư �ؽ�Ʈ ���� ����
        LoginBtnText.color = selectedColor;
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickSignUpToolBtn()
    {
        // �α��� ���� ��Ȱ��ȭ �� ȸ������ ���� Ȱ��ȭ
        LoginTool.SetActive(false);
        SignUpTool.SetActive(true);

        // ��ư �ؽ�Ʈ ���� ����
        LoginBtnText.color = defaultColor;
        SignUpBtnText.color = selectedColor;
    }
}