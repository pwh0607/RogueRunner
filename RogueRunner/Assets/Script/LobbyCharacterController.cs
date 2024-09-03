using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LobbyCharacterController : MonoBehaviour
{
    public GameObject CharacterListContent;
    public GameObject CharacterBtnPrefab;
    public Transform CharacterDisplay;

    string selectedCharacter;

    GameObject[] CharacterList;             //GameManager로 부터 가져온 캐릭터 리스트

    // Start is called before the first frame update
    void Start()
    {
        CharacterList = GameManager.Instance.getCharacterList();
        CreateCharacterBtns();
    }

    void CreateCharacterBtns()
    {
        //버튼 리스트 옵션
        int columns = 2;               // 열의 수
        float spacingX = 10f;         // 열 사이의 간격
        float spacingY = 10f;         // 행 사이의 간격

        //시작 지점.
        Vector2 startPosition = new Vector2(-70f, -80f);

        // 필요한 총 행의 수 계산
        int totalRows = Mathf.CeilToInt(CharacterList.Length / (float)columns);
        Vector2 btnSize = new Vector2(120f, 120f);

        for (int i = 0; i < CharacterList.Length; i++)
        {
            // 새로운 버튼 생성
            Button btn = Instantiate(CharacterBtnPrefab, CharacterListContent.transform.GetComponent<RectTransform>()).GetComponent<Button>();

            //버튼 text 수정
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CharacterList[i].name;

            int row = i / columns;       // 몇 번째 행인지 계산
            int column = i % columns;    // 몇 번째 열인지 계산

            // 버튼의 위치 설정
            RectTransform btnRect = btn.GetComponent<RectTransform>();
            btnRect.sizeDelta = btnSize; // 버튼의 크기 설정

            btnRect.anchoredPosition = new Vector2(
                startPosition.x + column * (btnSize.x + spacingX),
                startPosition.y - row * (btnSize.y + spacingY)
            );

            //람다식 오류 발생.
            int idx = i;
            btn.onClick.AddListener(() => OnClickCharBtn(CharacterList[idx]));
        }
    }

    public void OnClickCharBtn(GameObject character)
    {
        Debug.Log("선택 캐릭터 : " + character.name);
        foreach (Transform child in CharacterDisplay)
        {
            Destroy(child.gameObject);
        }

        //TEST
        // 캐릭터를 화면에 표시
        GameObject newCharacter = Instantiate(character, CharacterDisplay.transform);

        //선택한 캐릭터 설정
        selectedCharacter = character.name;

        // 캐릭터의 위치와 크기 조정
        Transform characterTrans = newCharacter.GetComponent<Transform>();
        if (characterTrans == null)
        {
            // 캐릭터가 RectTransform을 가지고 있지 않다면 Transform을 사용
            Debug.LogError("캐릭터 Transform null..");
        }

        // 캐릭터의 위치와 크기 조정
        characterTrans.localPosition = new Vector3(0f,0f,0f);     // UI 내에서 중앙에 위치하도록 조정
        characterTrans.localScale = Vector3.one;
        characterTrans.rotation = new Quaternion(0, 200, 0, 0);
    }

    public string getCharacterName()
    {
        return selectedCharacter;
    }
}