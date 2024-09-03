using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APITest : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/message"; // .NET ���� URL

    void Start()
    {
        string message = "Hello from Unity!";
        StartCoroutine(SendMessage(message));
    }

    private IEnumerator SendMessage(string message)
    {
        // �޽����� JSON �������� ����
        string jsonMessage = JsonUtility.ToJson(new MessageRequest { Message = message });

        // UnityWebRequest�� ����Ͽ� POST ��û ������
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonMessage);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // ��û ������ �� ���� ���
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    [System.Serializable]
    public class MessageRequest
    {
        public string Message;
    }
}
