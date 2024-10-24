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
        LoginTool = transform.GetChild(0).gameObject;
        SignUpTool = transform.GetChild(1).gameObject;

        ActiveTool = LoginTool;
        SignUpTool.SetActive(false);

        LoginToolBtn.onClick.AddListener(OnClickLoginToolBtn);
        SignUpToolBtn.onClick.AddListener(OnClickSignUpToolBtn);

        defaultColor = new Color(0, 0, 0); 
        selectedColor = new Color(0, 0, 1); 

        LoginBtnText = LoginToolBtn.GetComponentInChildren<TextMeshProUGUI>();
        SignUpBtnText = SignUpToolBtn.GetComponentInChildren<TextMeshProUGUI>();

        LoginBtnText.color = selectedColor; 
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickLoginToolBtn()
    {
        LoginTool.SetActive(true);
        SignUpTool.SetActive(false);

        LoginBtnText.color = selectedColor;
        SignUpBtnText.color = defaultColor;
    }

    public void OnClickSignUpToolBtn()
    {
        LoginTool.SetActive(false);
        SignUpTool.SetActive(true);

        LoginBtnText.color = defaultColor;
        SignUpBtnText.color = selectedColor;
    }
}