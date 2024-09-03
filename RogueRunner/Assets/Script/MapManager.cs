using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    private bool isInversion;
    public GameObject mapParent;
    public GameObject mapPrefab;
    public GameObject mapSpawnPos;

    // 인스턴스에 접근할 수 있는 정적 속성
    public static MapManager Instance
    {
        get
        {
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
        }else if (_instance != this)
        {
            Destroy(gameObject);            // 기존 인스턴스가 있으면 중복 생성된 인스턴스 파괴
        }

        isInversion = false;
    }

    public void SpawnRoad()
    {
        GameObject obj = Instantiate(mapPrefab);
        obj.transform.parent = mapParent.transform;
        obj.transform.position = mapSpawnPos.transform.position;
        if (isInversion) {
            obj.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        isInversion = !isInversion;
    }
}