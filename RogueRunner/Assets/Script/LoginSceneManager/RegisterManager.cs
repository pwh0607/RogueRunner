using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

[System.Serializable]
public class RegisterRequest
{
    public string Id { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }
}

public class RegisterManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/Register";
    public Button signUpBtn;

    public GameObject IDInput;       
    public GameObject PWInput;
    public GameObject NicknameInput;

    public GameObject WarnPan;

    private void Start()
    {
        signUpBtn.onClick.AddListener(OnClickSignUpBtn);
        WarnPan.SetActive(false);
    }

    IEnumerator RegisterUser(string id, string password, string nickname)
    {
        RegisterRequest userData = new RegisterRequest
        {
            Id = id.Trim(),
            Password = password.Trim(),
            Nickname = nickname.Trim()
        };

        string jsonData = JsonConvert.SerializeObject(userData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            this.gameObject.SetActive(false);
            showWarnPan("회원 가입 성공!");
        }
        else
        {
            if (request.responseCode == 409)
            {
                var responseText = request.downloadHandler.text;
                var responseJson = JsonConvert.DeserializeObject<MessageResponse>(responseText);
                showWarnPan(responseJson.message);
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
            }
        }
    }

    void OnClickSignUpBtn()
    {
        string id = IDInput.GetComponent<TextMeshProUGUI>().text;                       
        string pw = PWInput.GetComponent<TextMeshProUGUI>().text;                       
        string nickname = NicknameInput.GetComponent<TextMeshProUGUI>().text;           
        StartCoroutine(RegisterUser(id, pw, nickname));                                 
    }

    void showWarnPan(string text)
    {
        WarnPan.SetActive(true);
        WarnPan.GetComponent<WarnPanController>().setWarnText(text);
    }
}
