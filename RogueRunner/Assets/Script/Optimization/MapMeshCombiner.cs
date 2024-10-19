using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMeshCombiner : MonoBehaviour
{
    void Start()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        // ���յ� �޽��� ���� �� GameObject ����
        GameObject combinedObject = new GameObject("Road");

        //�޽� �Ĺ��� ��, object�� �̵� ��ũ��Ʈ �����.
        combinedObject.AddComponent<GroundController>();
        combinedObject.transform.position = Vector3.zero;

        // �ڽ� ������Ʈ���� ��� MeshFilter ������Ʈ ��������
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(meshFilters.Length + "�޽� �� : " + meshFilters[0].name);
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.CombineMeshes(combine);

        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;

        //������ ������Ʈ�� MapObjectPool�� �߰��ϱ� ��Ʈ
        pushPool(combinedObject);
    }

    void pushPool(GameObject combinedObject)
    {
        MapObjectPoolingSC mapPool= transform.parent.GetComponent<MapObjectPoolingSC>();

        mapPool.AddMeshObj(combinedObject);
    }
}
