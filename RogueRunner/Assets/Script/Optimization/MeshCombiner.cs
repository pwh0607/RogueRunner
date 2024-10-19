using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeshCombiner : MonoBehaviour
{
    void Start()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        // ���յ� �޽��� ���� �� GameObject ����
        GameObject combinedObject = new GameObject("CombinedObject");
        combinedObject.transform.parent = transform;                    //�ڽ� ��ü�� �α�.
        
        //�޽� �Ĺ��� ��, object�� �̵� ��ũ��Ʈ �����.
        combinedObject.AddComponent<TestObjController>();
        combinedObject.transform.position = Vector3.zero;

        // �ڽ� ������Ʈ���� ��� MeshFilter ������Ʈ ��������
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(meshFilters.Length + "�޽� �� : "+ meshFilters[0].name);
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        // �޽��� �ϳ��� ����
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.CombineMeshes(combine);

        // MeshRenderer �߰� �� ���� �Ҵ�
        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;
    }
}
