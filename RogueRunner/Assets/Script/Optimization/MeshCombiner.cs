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
        // 결합된 메쉬를 담을 빈 GameObject 생성
        GameObject combinedObject = new GameObject("CombinedObject");
        combinedObject.transform.parent = transform;                    //자식 객체로 두기.
        
        //메쉬 컴바인 후, object에 이동 스크립트 만들기.
        combinedObject.AddComponent<TestObjController>();
        combinedObject.transform.position = Vector3.zero;

        // 자식 오브젝트에서 모든 MeshFilter 컴포넌트 가져오기
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(meshFilters.Length + "메쉬 명 : "+ meshFilters[0].name);
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        // 메쉬를 하나로 결합
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.CombineMeshes(combine);

        // MeshRenderer 추가 및 재질 할당
        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;
    }
}
