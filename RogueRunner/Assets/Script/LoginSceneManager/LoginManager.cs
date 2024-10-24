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
    private string apiUrl = "http://localhost:5001/Login";

    public Button loginBtn;

    public GameObject signUpWindow;

    public GameObject IDInput;
    public GameObject PWInput;

    public GameObject WarnPan;

    void Start()
    {
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

        string jsonData = JsonConvert.SerializeObject(userData);    
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            this.gameObject.SetActive(false);

            string responseText = request.downloadHandler.text;
            LoginResponse responseObject = JsonConvert.DeserializeObject<LoginResponse>(responseText);
            string pId = responseObject.P_Id;

            UserSession.Instance.setData(pId, responseObject.User_Id, responseObject.NickName);

            SceneManager.LoadScene("Lobby");
        }
        else
        {
            var responseText = request.downloadHandler.text;
            var responseJson = JsonConvert.DeserializeObject<MessageResponse>(responseText);

            if (responseJson != null)
            {
                WarnPan.SetActive(true);
                WarnPan.GetComponent<WarnPanController>().setWarnText(responseJson.message);
            }
        }
    }
    void OnClickSignUpBtn()
    {
        if (!signUpWindow.activeSelf)
        {
            signUpWindow.SetActive(true);
        }
    }
    void OnClickLoginBtn()
    {
        string id = IDInput.GetComponent<TextMeshProUGUI>().text;           
        string pw = PWInput.GetComponent<TextMeshProUGUI>().text;           
        StartCoroutine(LoginUser(id, pw));
    }
}