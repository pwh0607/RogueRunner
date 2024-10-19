using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapObjectPoolingSC : MonoBehaviour
{
    public GameObject mapPrefab;            //�޽� ������ �� ������Ʈ
    public int MaxSize = 7;
    public Transform spawnPos;

    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject> ();
        //���۽� 7���� map�� �����ϱ�
        //�׷��� map�� Start �Լ��� ���� Combined�� �޽ð� pool�� ����ȴ�.
        for(int i = 0; i < MaxSize; i++)
        {
            //������ �ڵ����� �޽��� �����ǰ� ������ ������Ʈ�� �����Ϸ�.
            GameObject instance = Instantiate(mapPrefab);
        }
    }

    public void AddMeshObj(GameObject MeshObject)
    {
        pool.Add(MeshObject);
    }

    public void CheckSize()
    {
        Debug.Log("Count üũ : " + pool.Count());
    }
}