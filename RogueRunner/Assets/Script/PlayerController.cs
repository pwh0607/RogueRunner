using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Runtime.CompilerServices;

public class PlayerState {
    public int HP { get; set; }
    public float speed { get; set; }
    public Dictionary<string, int> skills { get; set; }

    public PlayerState(int hp, float speed)
    {
        this.HP = hp;
        this.speed = speed;
        skills = new Dictionary<string, int>();
    }
}

public class PlayerController : MonoBehaviour
{
    //player ������ġ
    public GameObject SpawnPos;
    private PlayerState state;

    //UI ���� ������Ʈ ����
    public GameObject HPCnt;
    public GameObject CardPanel;
    public GameObject[] cards;
    public Button[] skillBtn;


    //�ڷ�ƾ�� �Լ�
    IEnumerator SlowEffect()
    {
        float originalSpeed = state.speed;
        state.speed *= 0.2f;                // �ӵ��� 0.6��� ����
        yield return new WaitForSeconds(3f);
        state.speed = originalSpeed;        // ���� �ӵ��� ����
    }

    IEnumerator MakeCard()
    {
        int i = -350;
        GameObject cardsParent = new GameObject("Cards");
        cardsParent.transform.SetParent(CardPanel.transform, false);
        //card�� �������̴�.!!!
        foreach (GameObject card in cards)
        {
            GameObject newCard = Instantiate(card);
            newCard.GetComponent<RectTransform>().SetParent(cardsParent.transform, false);    // �θ� ���� �� ���� ��ġ ����
            newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(i, 500);     // ���� ��ġ ����
            i += 350;

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = SpawnPos.transform.position;


        //Scene ���۽� GameManager���� Card ������ ��������.
        cards = GameManager.Instance.getCardPrefab();
        showCard();

        //�� ���۽� �÷��̾� data�� GameManager���� �����´�.
        initState();
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���� ���°� true�� ��츸...
        if (GameManager.Instance.getStart())
        {
            Move();
            CheckHP();
            CheckSkill();
        }
    }
    
    //GameManager�κ��� ������ ��������.
    void initState()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();
        state = new PlayerState(data.curHP, data.curSpeed);
        state.skills = data.skills;
    }

    void CheckHP()
    {
        HPCnt.GetComponent<TextMeshProUGUI>().text = state.HP.ToString();
        if (state.HP <= 0)
        {
            //���� ����.
            Debug.Log("HP ��� ����");
            GameManager.Instance.GameOver();
        }
    }
    void CheckSkill()
    {
        //skill ��ųʸ� ��... cnt = 0 �̸� ��Ȱ��ȭ
        if (state.skills["SHILED"] <= 0)
        {
            skillBtn[0].interactable = false;
        }
        else
        {
            skillBtn[0].interactable = true;
        }

        if (state.skills["BOMB"] <= 0)
        {
            skillBtn[1].interactable = false;
        }
        else
        {
            skillBtn[1].interactable = true;
        }

        if (state.skills["TIMER"] <= 0)
        {
            skillBtn[2].interactable = false;
        }
        else
        {
            skillBtn[2].interactable = true;
        }
    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        // �̵� ���� ���� ���
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        // �̵�
        transform.position += movement * state.speed * Time.deltaTime;
    }
    
    public void MinusHP()
    {
        state.HP--;
        Debug.Log("��ֹ��� �´�Ҵ�. ���� HP" + state.HP);

        /*
         2�� �����ð� �߰��ϱ�
         */
    }

    public void showCard()
    {
        StartCoroutine(MakeCard());
    }

    public void showBuf()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹�߻�");
        //��ֹ��� �ε��� ���.

    }   
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Ʈ���� �߻�");
        if (collision.gameObject.tag == "OBSTACLE")
        {
            MinusHP();
        }
        if (collision.gameObject.tag == "SLOW")
        {
            Debug.Log("SLOW");
            StartCoroutine(SlowEffect());
        }
    }
    public void ApplyCard(string cardName)
    {
        switch (cardName)
        {
            case "HPUP":
                Debug.Log("HP UP!");
                state.HP++;
                break;

            case "BOMB":
                Debug.Log("��ź ��ų �߰�");
                state.skills["BOMB"]++;
                break;

            case "SHILED":
                Debug.Log("�ǵ� ��ų �߰�");
                state.skills["SHILED"]++;
                break;

            case "TIMER":
                Debug.Log("Ÿ�̸� ��ų �߰�");
                state.skills["TIMER"]++;
                break;

            case "SPEEDUP":
                Debug.Log("���ǵ� UP!");
                state.speed *= 1.3f;
                break;
        }
    }


    //���� ���� �׽�Ʈ�� �ڵ�(�׽�Ʈ �Ϸ��� ���� ����
    public void onClickStartBtn()
    {
        GameManager.Instance.setStart();
    }
}