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
    private string apiUrl = "http://localhost:5001/ScoreRank"; // .NET 서버 URL

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
        request.downloadHandler = new DownloadHandlerBuffer();          //GET은 가져오기용 로직이므로 다운로드만 사용!

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("랭크 리스트 가져오기 성공");
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
        //만약 스크롤 content에 자식이 있다면 그 자식을 삭제한다.
        if(ScrollContent.transform.childCount > 0)
        {
            Destroy(ScrollContent.transform.GetChild(0).gameObject);
        }

        // 빈 객체 생성. RankerPart의 부모 객체가 된다.
        GameObject rankerParent = new GameObject("RankerParent", typeof(RectTransform));
        RectTransform parentRectTransform = rankerParent.GetComponent<RectTransform>();

        // Parent RectTransform 설정
        parentRectTransform.SetParent(ScrollContent.transform);

        // 상단을 기준으로 anchor와 pivot 설정
        parentRectTransform.anchorMin = new Vector2(0.5f, 1f);      // 상단 중앙에 anchor 설정
        parentRectTransform.anchorMax = new Vector2(0.5f, 1f);      // 상단 중앙에 anchor 설정
        parentRectTransform.pivot = new Vector2(0.5f, 1f);          // pivot을 상단 중앙으로 설정

        // 위치 설정 (상단에서 내려오게 설정)
        parentRectTransform.anchoredPosition = new Vector2(50f, 90f); 

        int idx = 0;
        int padding = 50;

        //정렬의 시작부분
        Vector2 startPosition = new Vector2(0f, -1 * padding);

        foreach (var rank in ranks)
        {
            GameObject RankerPart = Instantiate(RankerPartPrefab);

            idx++;
            RankerPart.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = idx.ToString();
            RankerPart.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = rank.Nickname;
            RankerPart.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rank.Score.ToString();

            // RankerPart의 RectTransform 설정
            RectTransform rankerPartRect = RankerPart.GetComponent<RectTransform>();

            rankerPartRect.SetParent(rankerParent.transform);

            // 각 RankerPart의 위치 설정
            rankerPartRect.anchoredPosition = startPosition - new Vector2(0f, idx * padding);
        }
    }

    public void OnClickUpdateBtn()
    {
        //랭킹 리스트 갱신
        StartCoroutine(GetRankList());
    }
}
