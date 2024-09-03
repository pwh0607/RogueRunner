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
    //api용
    private string apiUrl = "http://localhost:5001/Register/register";
    public Button signUpBtn;

    //입력창
    public GameObject IDInput;       
    public GameObject PWInput;
    public GameObject NicknameInput;

    //경고 창
    public GameObject WarnPan;

    private void Start()
    {
        signUpBtn.onClick.AddListener(OnClickSignUpBtn);
        WarnPan.SetActive(false);
    }

    //유니티 Coroutin용 메서드
    IEnumerator RegisterUser(string id, string password, string nickname)
    {
        RegisterRequest userData = new RegisterRequest
        {
            Id = id.Trim(),
            Password = password.Trim(),
            Nickname = nickname.Trim()
        };

        string jsonData = JsonConvert.SerializeObject(userData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //회원 가입 창 닫기.
            this.gameObject.SetActive(false);
            showWarnPan("회원 가입 성공!");
        }
        else
        {
            if (request.responseCode == 409)
            {
                // 데이터 중복 확인용
                var responseText = request.downloadHandler.text;
                // 응답 JSON에서 메시지 추출
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
        string id = IDInput.GetComponent<TextMeshProUGUI>().text;                       //"pid123";
        string pw = PWInput.GetComponent<TextMeshProUGUI>().text;                       //"pw12345";
        string nickname = NicknameInput.GetComponent<TextMeshProUGUI>().text;           //"nick123";
        StartCoroutine(RegisterUser(id, pw, nickname));                                 //Debug.Log($"입력 데이터 id : {id}, pw : {pw}, nickname : {nickname},");
    }

    void showWarnPan(string text)
    {
        WarnPan.SetActive(true);
        WarnPan.GetComponent<WarnPanController>().setWarnText(text);
    }
}
