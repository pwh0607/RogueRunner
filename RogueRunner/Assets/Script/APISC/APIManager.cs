using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APIManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/PlayerData"; // .NET ���� URL

    //.net ������ ���� ���� ������
    private IEnumerator SendPlayerDataToAPI(PlayerDataRequest pdr)
    {
        // �޽����� JSON �������� ���� (JsonUtility ��� Newtonsoft.Json ���)
        string jsonMessage = JsonConvert.SerializeObject(pdr);

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

    private IEnumerator GetPlayerDataFromAPI(string p_id)
    {
        //pid�� ������� ������ ��������.
        string api = apiUrl + $"/{p_id}";
        UnityWebRequest request = new UnityWebRequest(api, "GET");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            //�����͸� �������µ� ���� ���� ��...
            if (request.responseCode == 404)
            {
                Debug.Log($"�ش� P_id�� ���� �����Ͱ� ���� ���ο� �����͸� �����մϴ�...");

                //���� ���ο� GameManager���� ���ο� �����͵� �ʱ�ȭ�ϱ�.
                GameManager.Instance.initState();
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
            }
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            PlayerDataResponse playerData = JsonConvert.DeserializeObject<PlayerDataResponse>(jsonResponse);
            var skills = JsonConvert.DeserializeObject<Dictionary<string, int>>(playerData.Skills);

            GameManager.Instance.initState();
            GameManager.Instance.playerData.MapPlayerData(playerData.HP, playerData.Score, playerData.Speed, skills);
            GameManager.Instance.gameState.MapGameState(playerData.Stage, playerData.SceneName);
        }
    }

    public void SendPlayerData(PlayerData p_data, GameState g_state)
    {
        //�÷��̾� �����Ϳ� Game�� ���� ���¸� Request�� ����.
        PlayerDataRequest request = new PlayerDataRequest(p_data, g_state);
        StartCoroutine(SendPlayerDataToAPI(request));
    }

    public void GetPlayerData()
    {
        string playerId = UserSession.Instance.P_Id;
        StartCoroutine(GetPlayerDataFromAPI(playerId));
    }

    [System.Serializable]
    public class PlayerDataRequest
    {
        public string P_Id;
        public int Stage;
        public string SceneName;
        public string PlayerCharacter;
        public int HP;
        public float Score;
        public float Speed;
        public Dictionary<string, int> Skills;

        public PlayerDataRequest(PlayerData p_data, GameState g_state)
        {
            P_Id = p_data.p_id;
            Stage = g_state.stage;
            SceneName = g_state.nowScene;

            HP = p_data.curHP;
            Score = p_data.curScore;
            Speed = p_data.curSpeed;
            PlayerCharacter = p_data.curCharacter;
            Skills = p_data.skills ?? new Dictionary<string, int>(); // null�� ��� �ʱ�ȭ
        }
    }
    [System.Serializable]
    public class PlayerDataResponse
    {
        public int Stage { get; set; }
        public string SceneName { get; set; }
        public string PlayerCharacter { get; set; }
        public int HP { get; set; }
        public float Score { get; set; }
        public float Speed { get; set; }
        public string Skills { get; set; }
    }
}
