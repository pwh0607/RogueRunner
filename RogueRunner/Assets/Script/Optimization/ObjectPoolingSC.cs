using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPoolingSC : MonoBehaviour
{
    public GameObject prefab;
    public int MaxSize = 10;
    public Transform spawnPos; 
    
    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject>();
        for(int i = 0; i < MaxSize; i++)
        {
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(transform);
            instance.SetActive(false);
            pool.Add(instance);
        }
    }

    public GameObject GetPoolObj()
    {
        foreach(GameObject instance in pool) {
            if (!instance.activeInHierarchy) 
            {
                return instance;
            }
        }

        GameObject newIns = Instantiate(prefab);
        newIns.SetActive(false);
        pool.Add(newIns);
        return newIns;
    }

    public void SpawnObject()
    {
        GameObject instance = GetPoolObj();
        int ranX = Random.Range(-25, 25);
        int ranZ = Random.Range(-10, 10);
        Vector3 newPos = new Vector3(spawnPos.position.x + ranX, spawnPos.position.y, spawnPos.position.z + ranZ);

        instance.transform.position = newPos;  
        instance.SetActive(true);            
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}