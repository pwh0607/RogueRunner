using System.Collections.Generic;
using UnityEngine;

public class MapObjectPoolingSC : MonoBehaviour
{
    public GameObject mapPrefab;            //메쉬 결합이 된 오브젝트
    public GameObject mapParent;
    public Transform spawnPos;
    public int initSize = 4;

    private List<GameObject> pool;
    private int MaxSize = 6;

    private void Start()
    {
        InitPooling();
    }

    public void InitPooling()
    {

        pool = new List<GameObject>();
        for (int i = 0; i < MaxSize; i++)
        {
            GameObject instance = Instantiate(mapPrefab);
            instance.transform.SetParent(mapParent.transform);
            instance.SetActive(false);
            pool.Add(instance);
        }

        for (int i = 0; i < 5; i++)
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

        GameObject newIns = Instantiate(mapPrefab);
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