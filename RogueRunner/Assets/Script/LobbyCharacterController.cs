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
    public Dictionary<string, CharacterInform> CharacterInformDic;

    string selectedCharacter;

    void Start()
    {
        CharacterInformDic = GameManager.Instance.CharacterDic;
        CreateCharacterBtns();
    }
    
    void CreateCharacterBtns()
    {
        int columns = 2;              
        float spacingX = 10f;         
        float spacingY = 10f;         

        Vector2 startPosition = new Vector2(-70f, -80f);

        int totalRows = Mathf.CeilToInt(CharacterInformDic.Count / (float)columns);
        Vector2 btnSize = new Vector2(120f, 120f);
        int i = 0;
        foreach(var info in CharacterInformDic) { 
            Button btn = Instantiate(CharacterBtnPrefab, CharacterListContent.transform.GetComponent<RectTransform>()).GetComponent<Button>();
            Image btnImage = btn.transform.GetChild(0).GetComponent<Image>();            

            btnImage.sprite = info.Value.HeadIcon;

            int row = i / columns;      
            int column = i % columns;    

            RectTransform btnRect = btn.GetComponent<RectTransform>();
            btnRect.sizeDelta = btnSize; 

            btnRect.anchoredPosition = new Vector2(
                startPosition.x + column * (btnSize.x + spacingX),
                startPosition.y - row * (btnSize.y + spacingY)
            );

            int idx = i;
            btn.onClick.AddListener(() => OnClickCharBtn(info.Value.CharacterObj));
            i++;
        }
    }

    public void OnClickCharBtn(GameObject character)
    {
        foreach (Transform child in CharacterDisplay)
        {
            Destroy(child.gameObject);
        }

        GameObject newCharacter = Instantiate(character, CharacterDisplay.transform);

        selectedCharacter = character.name;

        Transform characterTrans = newCharacter.GetComponent<Transform>();
        if (characterTrans == null)
        {
            Debug.LogError("Ä³¸¯ÅÍ Transform null..");
        }

        characterTrans.localPosition = new Vector3(0f,0f,0f);
        characterTrans.localScale = Vector3.one;
        characterTrans.rotation = new Quaternion(0, 200, 0, 0);
    }

    public string getCharacterName()
    {
        return selectedCharacter;
    }
}