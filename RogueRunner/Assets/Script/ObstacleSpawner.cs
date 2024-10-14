using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    //1은 일직선으로 다가오는 장애물
    //public GameObject[] obstaclePrefab1;
    public GameObject obstaclePrefab1;

    //2는 유도 장애물.
    //public GameObject[] obstaclePrefab2;
    public GameObject obstaclePrefab2;

    public GameObject obstacleSpawnPos1;
    public GameObject obstacleSpawnPos2;

    private Transform ost1;
    private Transform ost2;

    //장애물 소환 간격
    private float defaultTime1;
    private float defaultTime2;

    private float timeSlice1;
    private float time1;

    private float timeSlice2;
    private float time2;

    private float stageMultiplier1;         // = 1f / (1 + (currentStage * 0.1f));  
    private float stageMultiplier2;
    int stage;

    void Start()
    {
        stage = GameManager.Instance.gameState.stage;

        ost1 = obstacleSpawnPos1.transform;
        ost2 = obstacleSpawnPos2.transform;

        defaultTime1 = 3f;
        defaultTime2 = 6f;

        stageMultiplier1 = 1f / (1 + (stage * 0.1f));
        stageMultiplier2 = 1f / (1 + (stage * 0.07f));

        //스테이지가 올라갈때마다 난이도 상승!
        timeSlice1 = defaultTime1 * stageMultiplier1;
        timeSlice2 = defaultTime2 * stageMultiplier2;
        time1 = 0;
        time2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start")
        {
            if (!StageManager.Instance.isStopSkilled)
            {
                //게임이 시작상태이지만... 타이머 스킬을 사용하지 않은 경우...
                time1 += Time.deltaTime;
                time2 += Time.deltaTime;
                if (time1 >= timeSlice1)
                {
                    time1 = 0;
                    SpawnObstacle1();
                }

                if (time2 >= timeSlice2)
                {
                    time2 = 0;
                    SpawnObstacle2();
                }
            }       
        }
    }

    private void SpawnObstacle1()
    {
        //x좌표의 랜덤값 색출A
        int ranX = Random.Range(-20, 20);
        int ranZ = Random.Range(-10, 10);
        GameObject obs = Instantiate(obstaclePrefab1);
        obs.transform.position = new Vector3(ost1.position.x + ranX, ost1.position.y, ost1.position.z + ranZ);
    }

    private void SpawnObstacle2()
    {
        //x좌표의 랜덤값 색출
        int ranX = Random.Range(-25, 25);
        int ranZ = Random.Range(-10, 10);
        GameObject obs = Instantiate(obstaclePrefab2);
        obs.transform.position = new Vector3(ost2.position.x + ranX, ost2.position.y, ost2.position.z + ranZ);
    }
}
