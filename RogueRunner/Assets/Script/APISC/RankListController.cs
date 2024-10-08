using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

[System.Serializable]
public class ScoreRankResponse
{
    //���丸 �ޱ� ������ set�� private
    public string P_Id { get; set; }
    public string Nickname { get; set; }   
    public int Score {  get; set; }
}

[System.Serializable]
public class RankListResponse
{
    public List<ScoreRankResponse> Ranks;
}

public class RankListController : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/ScoreRank"; // .NET ���� URL

    List<ScoreRankResponse> rankList;

    public GameObject RankerPartPrefab;
        
    public GameObject ScrollContent;

    void Start()
    {
        StartCoroutine(GetRankList());
    }

    IEnumerator GetRankList()
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();          //GET�� ��������� �����̹Ƿ� �ٿ�ε常 ���!

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("��ũ ����Ʈ �������� ����");
            Debug.Log(request.downloadHandler.text);

            // Deserialize JSON response
            RankListResponse rankListResponse = JsonConvert.DeserializeObject<RankListResponse>(request.downloadHandler.text);

            if (rankListResponse == null)
            {
                Debug.LogError("Deserialized response is null.");
                yield break;
            }

            if (rankListResponse.Ranks == null || rankListResponse.Ranks.Count == 0)
            {
                Debug.LogError("Rank list is empty or null.");
                yield break;
            }

            rankList = rankListResponse.Ranks;

            MakeRankerInform(rankList);
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void MakeRankerInform(List<ScoreRankResponse> ranks)
    {
        //���� ��ũ�� content�� �ڽ��� �ִٸ� �� �ڽ��� �����Ѵ�.
        if(ScrollContent.transform.childCount > 0)
        {
            Destroy(ScrollContent.transform.GetChild(0).gameObject);
        }

        // �� ��ü ����. RankerPart�� �θ� ��ü�� �ȴ�.
        GameObject rankerParent = new GameObject("RankerParent", typeof(RectTransform));
        RectTransform parentRectTransform = rankerParent.GetComponent<RectTransform>();

        // Parent RectTransform ����
        parentRectTransform.SetParent(ScrollContent.transform);

        // ����� �������� anchor�� pivot ����
        parentRectTransform.anchorMin = new Vector2(0.5f, 1f);      // ��� �߾ӿ� anchor ����
        parentRectTransform.anchorMax = new Vector2(0.5f, 1f);      // ��� �߾ӿ� anchor ����
        parentRectTransform.pivot = new Vector2(0.5f, 1f);          // pivot�� ��� �߾����� ����

        // ��ġ ���� (��ܿ��� �������� ����)
        parentRectTransform.anchoredPosition = new Vector2(50f, 90f); 

        int idx = 0;
        int padding = 50;

        //������ ���ۺκ�
        Vector2 startPosition = new Vector2(0f, -1 * padding);

        foreach (var rank in ranks)
        {
            GameObject RankerPart = Instantiate(RankerPartPrefab);

            idx++;
            RankerPart.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = idx.ToString();
            RankerPart.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = rank.Nickname;
            RankerPart.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rank.Score.ToString();

            // RankerPart�� RectTransform ����
            RectTransform rankerPartRect = RankerPart.GetComponent<RectTransform>();

            rankerPartRect.SetParent(rankerParent.transform);

            // �� RankerPart�� ��ġ ����
            rankerPartRect.anchoredPosition = startPosition - new Vector2(0f, idx * padding);
        }
    }

    public void OnClickUpdateBtn()
    {
        //��ŷ ����Ʈ ����
        StartCoroutine(GetRankList());
    }
}
