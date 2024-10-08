using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APIManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/PlayerData"; // .NET 서버 URL

    //.net 서버에 최종 점수 보내기
    private IEnumerator SendPlayerDataToAPI(PlayerDataRequest pdr)
    {
        // 메시지를 JSON 형식으로 포맷 (JsonUtility 대신 Newtonsoft.Json 사용)
        string jsonMessage = JsonConvert.SerializeObject(pdr);

        // UnityWebRequest를 사용하여 POST 요청 보내기
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonMessage);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기 및 응답 대기
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
        //pid를 기반으로 데이터 가져오기.
        string api = apiUrl + $"/{p_id}";
        UnityWebRequest request = new UnityWebRequest(api, "GET");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            //데이터를 가져오는데 실패 했을 때...
            if (request.responseCode == 404)
            {
                Debug.Log($"해당 P_id에 대한 데이터가 없어 새로운 데이터를 생성합니다...");

                //이후 새로운 GameManager에서 새로운 데이터들 초기화하기.
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
        //플레이어 데이터와 Game의 진행 상태를 Request에 저장.
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
            Skills = p_data.skills ?? new Dictionary<string, int>(); // null일 경우 초기화
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
