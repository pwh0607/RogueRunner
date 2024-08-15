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
    public GameObject obstacleSpawnPos;
    private Transform ost;

    //��ֹ� ��ȯ ����
    private float timeSlice1;
    private float time1;

    private float timeSlice2;
    private float time2;
    int stage;

    void Start()
    {
        ost = obstacleSpawnPos.transform;
        timeSlice1 = 3;
        time1 = 0;
        timeSlice2 = 6;
        time2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start" && !SceneManager.Instance.isStopSkilled)
        {
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
    private void SpawnObstacle1()
    {
        //x��ǥ�� ������ ����A
        int ranX = Random.Range(-20, 20);
        GameObject obs = Instantiate(obstaclePrefab1);
        obs.transform.position = new Vector3(ost.position.x + ranX, ost.position.y, ost.position.z);
    }
    private void SpawnObstacle2()
    {
        //x��ǥ�� ������ ����
        int ranX = Random.Range(-25, 25);
        GameObject obs = Instantiate(obstaclePrefab2);
        obs.transform.position = new Vector3(ost.position.x + ranX, ost.position.y, ost.position.z);
    }
}
