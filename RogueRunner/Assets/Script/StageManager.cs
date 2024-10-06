using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("StageManager");
                    instance = obj.AddComponent<StageManager>();
                }
            }
            return instance;
        }
    }

    float SceneTime;
    public bool isStopSkilled { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SceneTime = 0;
        isStopSkilled = false;
        Debug.Log("Stage Manager ���� => " + "SceneTime : " + SceneTime);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        //������ �������� ������ ��츸 ����(isPaused == false)
        if (GameManager.Instance.getGameCode() == "Start")
        {
            SceneTime += Time.deltaTime;
        }

        //��Ȯ�ϰ� 100��.
        if (Mathf.Approximately(SceneTime, 100f))
        {
            // �������� �Ϸ�.
            GameManager.Instance.StageClear();
        }
    }

    public GameObject getSpawnPos()
    {
        GameObject SpawnPos = transform.GetChild(0).gameObject;
        return SpawnPos;
    }
}
