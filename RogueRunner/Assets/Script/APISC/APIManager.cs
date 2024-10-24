using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class APIManager : MonoBehaviour
{
    private string defaultAPI = "http://localhost:5001";

    private IEnumerator POSTFinalScoreData(FinalScoreRequest fsr)
    {
        string url = defaultAPI + "/ScoreRank";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        string jsonMessage = JsonConvert.SerializeObject(fsr);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonMessage);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
    }
    
    private IEnumerator POSTPlayerData(PlayerDataRequest pdr)
    {
        string url = defaultAPI + "/PlayerData";

        string jsonMessage = JsonConvert.SerializeObject(pdr);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonMessage);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
    }

    private IEnumerator GETPlayerData(string p_id)
    {
        string url = defaultAPI + "/PlayerData" + $"/{p_id}";

        UnityWebRequest request = new UnityWebRequest(url, "GET");

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 404)
            {
                GameManager.Instance.initState();
            }
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            PlayerDataResponse playerData = JsonConvert.DeserializeObject<PlayerDataResponse>(jsonResponse);
            var skills = JsonConvert.DeserializeObject<Dictionary<string, int>>(playerData.Skills);

            GameManager.Instance.initState();
            GameManager.Instance.playerData.SetPlayerData(playerData.HP, playerData.Score, playerData.Speed, skills);
            GameManager.Instance.gameState.MapGameState(playerData.Stage, playerData.SceneName);
        }
    }

    private IEnumerator DeletePlayerData(string p_id)
    {
        string url = defaultAPI + "/PlayerData" + $"/{p_id}";
        UnityWebRequest request = new UnityWebRequest(url, "DELETE");

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
    }

    public void SendFinalScoreData(PlayerData p_data, GameState g_state)
    {
        FinalScoreRequest fsr = new FinalScoreRequest(p_data.curScore);

        StartCoroutine(POSTFinalScoreData(fsr));
        StartCoroutine(DeletePlayerData(p_data.p_id));
    }

    public void SendPlayerData(PlayerData p_data, GameState g_state)
    {
        PlayerDataRequest request = new PlayerDataRequest(p_data, g_state);

        StartCoroutine(POSTPlayerData(request));
    }

    public void GetTemp()
    {
        string playerId = UserSession.Instance.P_Id;

        StartCoroutine(GETPlayerData(playerId));
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
            Skills = p_data.skills ?? new Dictionary<string, int>();
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

    [System.Serializable]
    public class FinalScoreRequest
    {
        public string P_id { get; set; }
        public string Nickname { get; set; }
        public float Score { get; set; }

        public FinalScoreRequest(float score)
        {
            P_id = UserSession.Instance.P_Id;
            Nickname = UserSession.Instance.Nickname;
            Score = score;
        }  
    }
}
