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

    // �ν��Ͻ��� ������ �� �ִ� ���� �Ӽ�
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

    // �ν��Ͻ��� �������� Ȯ���ϱ� ���� ���
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }else if (_instance != this)
        {
            Destroy(gameObject);            // ���� �ν��Ͻ��� ������ �ߺ� ������ �ν��Ͻ� �ı�
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