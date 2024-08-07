using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static MapManager _instance;

    public GameObject mapParent;
    public GameObject mapPrefab;
    public GameObject mapSpawnPos;

    // �ν��Ͻ��� ������ �� �ִ� ���� �Ӽ�
    public static MapManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ã�ų� ����
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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else if (_instance != this)
        {
            Destroy(gameObject);            // ���� �ν��Ͻ��� ������ �ߺ� ������ �ν��Ͻ� �ı�
        }
    }

    // MapManager�� ����� ������ ��
    public void SomeFunction()
    {
        Debug.Log("MapManager�� ����� ȣ��Ǿ����ϴ�.");
    }

    public void SpawnRoad()
    {
        GameObject obj = Instantiate(mapPrefab);
        obj.transform.parent = mapParent.transform;
        obj.transform.position = mapSpawnPos.transform.position;
    }
}
