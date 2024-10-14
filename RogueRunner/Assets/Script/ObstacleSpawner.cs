using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    //1�� ���������� �ٰ����� ��ֹ�
    //public GameObject[] obstaclePrefab1;
    public GameObject obstaclePrefab1;

    //2�� ���� ��ֹ�.
    //public GameObject[] obstaclePrefab2;
    public GameObject obstaclePrefab2;

    public GameObject obstacleSpawnPos1;
    public GameObject obstacleSpawnPos2;

    private Transform ost1;
    private Transform ost2;

    //��ֹ� ��ȯ ����
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

        //���������� �ö󰥶����� ���̵� ���!
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
                //������ ���ۻ���������... Ÿ�̸� ��ų�� ������� ���� ���...
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
        //x��ǥ�� ������ ����A
        int ranX = Random.Range(-20, 20);
        int ranZ = Random.Range(-10, 10);
        GameObject obs = Instantiate(obstaclePrefab1);
        obs.transform.position = new Vector3(ost1.position.x + ranX, ost1.position.y, ost1.position.z + ranZ);
    }

    private void SpawnObstacle2()
    {
        //x��ǥ�� ������ ����
        int ranX = Random.Range(-25, 25);
        int ranZ = Random.Range(-10, 10);
        GameObject obs = Instantiate(obstaclePrefab2);
        obs.transform.position = new Vector3(ost2.position.x + ranX, ost2.position.y, ost2.position.z + ranZ);
    }
}
