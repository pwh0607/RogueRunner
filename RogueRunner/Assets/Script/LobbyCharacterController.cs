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

    string selectedCharacter;               //���� ���õǾ��ִ� ĳ���͸�.

    // Start is called before the first frame update
    void Start()
    {
        CharacterInformDic = GameManager.Instance.CharacterDic;
        CreateCharacterBtns();
    }
    
    void CreateCharacterBtns()
    {
        //��ư ����Ʈ �ɼ�
        int columns = 2;               // ���� ��
        float spacingX = 10f;         // �� ������ ����
        float spacingY = 10f;         // �� ������ ����

        //���� ����.
        Vector2 startPosition = new Vector2(-70f, -80f);

        // �ʿ��� �� ���� �� ���
        int totalRows = Mathf.CeilToInt(CharacterInformDic.Count / (float)columns);
        Vector2 btnSize = new Vector2(120f, 120f);
        int i = 0;
        foreach(var info in CharacterInformDic) { 
            // ���ο� ��ư ����
            Button btn = Instantiate(CharacterBtnPrefab, CharacterListContent.transform.GetComponent<RectTransform>()).GetComponent<Button>();
            Image btnImage = btn.transform.GetChild(0).GetComponent<Image>();            //��ư�� �ڽð�ü ���� ����

            //��ư �̸� ����(���� ��������)
            //���� ���� ��, ĳ���� head ���������� ����.
            btnImage.sprite = info.Value.HeadIcon;

            int row = i / columns;       // �� ��° ������ ���
            int column = i % columns;    // �� ��° ������ ���

            // ��ư�� ��ġ ����
            RectTransform btnRect = btn.GetComponent<RectTransform>();
            btnRect.sizeDelta = btnSize; // ��ư�� ũ�� ����

            btnRect.anchoredPosition = new Vector2(
                startPosition.x + column * (btnSize.x + spacingX),
                startPosition.y - row * (btnSize.y + spacingY)
            );

            //���ٽ� ���� �߻�.
            int idx = i;
            btn.onClick.AddListener(() => OnClickCharBtn(info.Value.CharacterObj));
            
            i++;
        }
    }

    public void OnClickCharBtn(GameObject character)
    {
        Debug.Log("���� ĳ���� : " + character.name);
        foreach (Transform child in CharacterDisplay)
        {
            Destroy(child.gameObject);
        }

        //TEST
        // ĳ���͸� ȭ�鿡 ǥ��
        GameObject newCharacter = Instantiate(character, CharacterDisplay.transform);

        //������ ĳ���� ����
        selectedCharacter = character.name;

        // ĳ������ ��ġ�� ũ�� ����
        Transform characterTrans = newCharacter.GetComponent<Transform>();
        if (characterTrans == null)
        {
            // ĳ���Ͱ� RectTransform�� ������ ���� �ʴٸ� Transform�� ���
            Debug.LogError("ĳ���� Transform null..");
        }

        // ĳ������ ��ġ�� ũ�� ����
        characterTrans.localPosition = new Vector3(0f,0f,0f);     // UI ������ �߾ӿ� ��ġ�ϵ��� ����
        characterTrans.localScale = Vector3.one;
        characterTrans.rotation = new Quaternion(0, 200, 0, 0);
    }

    public string getCharacterName()
    {
        return selectedCharacter;
    }
}