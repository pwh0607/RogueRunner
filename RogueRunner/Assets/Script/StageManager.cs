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

    void Start()
    {
        SceneTime = 0;
        isStopSkilled = false;
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
        if (GameManager.Instance.getGameCode() == "Start")
        {
            SceneTime += Time.deltaTime;
        }

        if (Mathf.Approximately(SceneTime, 100f))
        {
            GameManager.Instance.StageClear();
        }
    }

    public GameObject getSpawnPos()
    {
        GameObject SpawnPos = transform.GetChild(0).gameObject;
        return SpawnPos;
    }
}
