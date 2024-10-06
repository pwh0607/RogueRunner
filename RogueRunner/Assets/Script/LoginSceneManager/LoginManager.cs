using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Microsoft.CSharp;

[System.Serializable]
public class LoginRequest
{
    public string Id;
    public string Password;
}

[System.Serializable]
public class LoginResponse
{
    public string P_Id { get; set; }
    public string User_Id { get; set; }
    public string NickName { get; set; }
}
public class MessageResponse
{
    public string message { get; set; }
}

public class LoginManager : MonoBehaviour
{
    //api용
    private string apiUrl = "http://localhost:5001/Login/login";

    public Button loginBtn;

    //회원가입창
    public GameObject signUpWindow;

    //입력창
    public GameObject IDInput;
    public GameObject PWInput;

    public GameObject WarnPan;

    void Start()
    {
        //초기에는 비활성화.
        signUpWindow.SetActive(false);
        loginBtn.onClick.AddListener(OnClickLoginBtn);
        WarnPan.SetActive(false);
    }

    IEnumerator LoginUser(string id, string pw)
    {
        LoginRequest userData = new LoginRequest
        {
            Id = id.Trim(),
            Password = pw.Trim(),
        };

        //api 구조 수동 세팅
        /*
            1. class data를 json 직렬화수행, JsonConvert.SerializeObject(jsonData);
            2. request 객체 세팅 -> UnityWebRequest.***(...);
            3. byte 세팅 UTP8
            4. upload, download 핸들러 세팅
            ...
         */
        string jsonData = JsonConvert.SerializeObject(userData);    
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log($"Sending request to {apiUrl} with data: {jsonData}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //회원 가입 창 닫기.
            this.gameObject.SetActive(false);

            //User Session
            string responseText = request.downloadHandler.text;
            LoginResponse responseObject = JsonConvert.DeserializeObject<LoginResponse>(responseText);
            string pId = responseObject.P_Id;
            Debug.Log($"P_Id: {pId}");

            UserSession.Instance.setData(pId, responseObject.User_Id, responseObject.NickName);

            //성공 후 씬 전환
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.LogWarning("Raw response: " + request.downloadHandler.text);

            // 응답 메시지 추출.
            var responseText = request.downloadHandler.text;
            var responseJson = JsonConvert.DeserializeObject<MessageResponse>(responseText);

            if (responseJson != null)
            {
                Debug.LogWarning(responseJson);
                WarnPan.SetActive(true);
                WarnPan.GetComponent<WarnPanController>().setWarnText(responseJson.message);
            }
        }
    }
    void OnClickSignUpBtn()
    {
        //회원 가입 창이 띄워져 있지 않은 상태에서만 실행한다.
        if (!signUpWindow.activeSelf)
        {
            signUpWindow.SetActive(true);
        }
    }
    void OnClickLoginBtn()
    {
        //서버에 두 데이터 보내서 받은 응답에 따라...
        string id = "pid123";       //IDInput.GetComponent<TextMeshProUGUI>().text;           
        string pw = "pw12345";      //PWInput.GetComponent<TextMeshProUGUI>().text;           
        StartCoroutine(LoginUser(id, pw));
    }
}