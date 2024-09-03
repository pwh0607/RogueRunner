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
    //api��
    private string apiUrl = "http://localhost:5001/Register/register";
    public Button signUpBtn;

    //�Է�â
    public GameObject IDInput;       
    public GameObject PWInput;
    public GameObject NicknameInput;

    //��� â
    public GameObject WarnPan;

    private void Start()
    {
        signUpBtn.onClick.AddListener(OnClickSignUpBtn);
        WarnPan.SetActive(false);
    }

    //����Ƽ Coroutin�� �޼���
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
            //ȸ�� ���� â �ݱ�.
            this.gameObject.SetActive(false);
            showWarnPan("ȸ�� ���� ����!");
        }
        else
        {
            if (request.responseCode == 409)
            {
                // ������ �ߺ� Ȯ�ο�
                var responseText = request.downloadHandler.text;
                // ���� JSON���� �޽��� ����
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
        StartCoroutine(RegisterUser(id, pw, nickname));                                 //Debug.Log($"�Է� ������ id : {id}, pw : {pw}, nickname : {nickname},");
    }

    void showWarnPan(string text)
    {
        WarnPan.SetActive(true);
        WarnPan.GetComponent<WarnPanController>().setWarnText(text);
    }
}
