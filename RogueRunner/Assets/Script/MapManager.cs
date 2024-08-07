using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 정적 변수
    private static MapManager _instance;

    public GameObject mapParent;
    public GameObject mapPrefab;
    public GameObject mapSpawnPos;

    // 인스턴스에 접근할 수 있는 정적 속성
    public static MapManager Instance
    {
        get
        {
            // 인스턴스가 없으면 찾거나 생성
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(MapManager).Name);
                    _instance = singleton.AddComponent<MapManager>();
                }
            }
            return _instance;
        }
    }

    // 인스턴스가 유일한지 확인하기 위한 방법
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else if (_instance != this)
        {
            Destroy(gameObject);            // 기존 인스턴스가 있으면 중복 생성된 인스턴스 파괴
        }
    }

    // MapManager의 기능을 구현할 곳
    public void SomeFunction()
    {
        Debug.Log("MapManager의 기능이 호출되었습니다.");
    }

    public void SpawnRoad()
    {
        GameObject obj = Instantiate(mapPrefab);
        obj.transform.parent = mapParent.transform;
        obj.transform.position = mapSpawnPos.transform.position;
    }
}
