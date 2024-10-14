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
    //응답만 받기 때문에 set은 private
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
    private string apiUrl = "http://localhost:5001/ScoreRank"; 

    List<ScoreRankResponse> rankList;

    public GameObject RankerPartPrefab;
        
    public GameObject ScrollContent;

    void Start()
    {
        StartCoroutine(GETRankList());
    }

    IEnumerator GETRankList()
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();         

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            RankListResponse rankListResponse = JsonConvert.DeserializeObject<RankListResponse>(request.downloadHandler.text);

            if (rankListResponse == null)
            {
                yield break;
            }
            if (rankListResponse.Ranks == null || rankListResponse.Ranks.Count == 0)
            {
                yield break;
            }

            rankList = rankListResponse.Ranks;
            MakeRankerInform(rankList);
        }
    }

    public void MakeRankerInform(List<ScoreRankResponse> ranks)
    {
        if(ScrollContent.transform.childCount > 0)
        {
            Destroy(ScrollContent.transform.GetChild(0).gameObject);
        }
        GameObject rankerParent = new GameObject("RankerParent", typeof(RectTransform));
        RectTransform parentRectTransform = rankerParent.GetComponent<RectTransform>();

        parentRectTransform.SetParent(ScrollContent.transform);
        parentRectTransform.anchorMin = new Vector2(0.5f, 1f);      
        parentRectTransform.anchorMax = new Vector2(0.5f, 1f);      
        parentRectTransform.pivot = new Vector2(0.5f, 1f);          

        parentRectTransform.anchoredPosition = new Vector2(50f, 90f); 

        int idx = 0;
        int padding = 50;

        Vector2 startPosition = new Vector2(0f, -1 * padding);

        foreach (var rank in ranks)
        {
            GameObject RankerPart = Instantiate(RankerPartPrefab);

            idx++;
            RankerPart.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = idx.ToString();
            RankerPart.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = rank.Nickname;
            RankerPart.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rank.Score.ToString();

            RectTransform rankerPartRect = RankerPart.GetComponent<RectTransform>();

            rankerPartRect.SetParent(rankerParent.transform);

            rankerPartRect.anchoredPosition = startPosition - new Vector2(0f, idx * padding);
        }
    }

    public void OnClickUpdateBtn()
    {
        StartCoroutine(GETRankList());
    }
}
