using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�̱������� ����
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� �ν��Ͻ��� �ı�
        }
    }

    //���߿� �̱������� �����ϰ� start �Լ����� stage=1�� �ʱ�ȭ
    int stage = 0;

    //1�� ���������� �ٰ����� ��ֹ�
    //public GameObject[] obstaclePrefab1;
    public GameObject obstaclePrefab1;

    //2�� ���� ��ֹ�.
    //public GameObject[] obstaclePrefab2;
    public GameObject obstaclePrefab2;
    public GameObject obstacleSpawnPos;
    private Transform ost;

    //��ֹ� ��ȯ ����
    private float timeSlice;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        ost = obstacleSpawnPos.transform;
        timeSlice = 5;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= timeSlice)
        {
            time = 0;
            //SpawnObstacle1();
            SpawnObstacle2();
        }
    }
    public int getMapIdx()
    {
        return 0;
    }
    private void SpawnObstacle1()
    {
        //x��ǥ�� ������ ����
        int ranX = Random.Range(-25, 25);
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

    //����ȯ��..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        stage++; // ���� �ε�� ������ stage�� 1�� ����
        Debug.Log($"Stage: {stage}"); // ����� ������ ���� �������� ���
    }
}
