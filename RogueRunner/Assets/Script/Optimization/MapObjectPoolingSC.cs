using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapObjectPoolingSC : MonoBehaviour
{
    public GameObject mapPrefab;            //메쉬 결합이 된 오브젝트
    public int MaxSize = 7;
    public Transform spawnPos;

    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject> ();
        //시작시 7개의 map을 생성하기
        //그러면 map의 Start 함수를 통해 Combined된 메시가 pool에 저장된다.
        for(int i = 0; i < MaxSize; i++)
        {
            //생성시 자동으로 메쉬가 생성되고 기존의 오브젝트는 삭제완료.
            GameObject instance = Instantiate(mapPrefab);
        }
    }

    public void AddMeshObj(GameObject MeshObject)
    {
        pool.Add(MeshObject);
    }

    public void CheckSize()
    {
        Debug.Log("Count 체크 : " + pool.Count());
    }
}