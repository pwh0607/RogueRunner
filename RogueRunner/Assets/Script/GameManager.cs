using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //싱글톤으로 세팅
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
        }
    }

    //나중에 싱글톤으로 세팅하고 start 함수에서 stage=1로 초기화
    int stage = 0;

    //1은 일직선으로 다가오는 장애물
    //public GameObject[] obstaclePrefab1;
    public GameObject obstaclePrefab1;

    //2는 유도 장애물.
    //public GameObject[] obstaclePrefab2;
    public GameObject obstaclePrefab2;
    public GameObject obstacleSpawnPos;
    private Transform ost;

    //장애물 소환 간격
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
        //x좌표의 랜덤값 색출
        int ranX = Random.Range(-25, 25);
        GameObject obs = Instantiate(obstaclePrefab1);
        obs.transform.position = new Vector3(ost.position.x + ranX, ost.position.y, ost.position.z);
    }
    private void SpawnObstacle2()
    {
        //x좌표의 랜덤값 색출
        int ranX = Random.Range(-25, 25);
        GameObject obs = Instantiate(obstaclePrefab2);
        obs.transform.position = new Vector3(ost.position.x + ranX, ost.position.y, ost.position.z);
    }

    //씬전환시..
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        stage++; // 씬이 로드될 때마다 stage를 1씩 증가
        Debug.Log($"Stage: {stage}"); // 디버그 용으로 현재 스테이지 출력
    }
}
