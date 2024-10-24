using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    MapObjectPoolingSC MapObjectPoolingSC;

    public Transform mapSpawnPos;

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

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);            
        }
    }

    private void Start()
    {
        MapObjectPoolingSC = GetComponent<MapObjectPoolingSC>();
    }

    public void SpawnRoad()
    {
        MapObjectPoolingSC.SpawnObject(mapSpawnPos.position);
    }
}