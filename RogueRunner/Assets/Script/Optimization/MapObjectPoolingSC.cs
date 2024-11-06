using System.Collections.Generic;
using UnityEngine;

public class MapObjectPoolingSC : MonoBehaviour
{
    public GameObject roadPrefab;            //메쉬 결합이 된 오브젝트
    public GameObject roadParent;
    public Transform spawnPos;
    public int initSize = 5;

    private List<GameObject> pool;
    private int MaxSize = 10;

    private void Start()
    {
        pool = new List<GameObject>();
        InitPooling();
    }

    public void InitPooling()
    {
        for (int i = 0; i < MaxSize; i++)
        {
            GameObject instance = Instantiate(roadPrefab);
            instance.transform.SetParent(roadParent.transform);
            instance.SetActive(false);
            pool.Add(instance);
        }

        for (int i = 0; i < initSize; i++)
        {
            Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z - (i * 223));
            SpawnObject(newPos);
        }
    }

    public GameObject GetPoolObj()
    {
        foreach (GameObject instance in pool)
        {
            if (!instance.activeInHierarchy)
            {
                return instance;
            }
        }

        GameObject newIns = Instantiate(roadPrefab);
        pool.Add(newIns);
        newIns.SetActive(false);
        return newIns;
    }

    public void SpawnObject(Vector3 newPos)
    {
        GameObject instance = GetPoolObj();
        instance.transform.position = newPos;
        instance.SetActive(true);   
    }
}