using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private ObjectPoolingSC objectPool;

    public float defaultTime;

    private float timeSlice;
    private float time;

    private float stageMultiplier;

    int stage;

    void Start()
    {
        objectPool = GetComponent<ObjectPoolingSC>();
        stage = GameManager.Instance.gameState.stage;
        stageMultiplier = 1f / (1 + (stage * 0.07f));
        timeSlice = defaultTime * stageMultiplier;
        time = 0;
    }

    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start")
        {
            if (!StageManager.Instance.isStopSkilled)
            {
                time += Time.deltaTime;
                if (time >= timeSlice)
                {
                    time = 0;
                    objectPool.SpawnObject();
                }
            }
        }
    }
}
