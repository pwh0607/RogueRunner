using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public GameObject parentObj;

    void CombineMeshes()
    {
        GameObject combinedObject = new GameObject("CombinedObject");
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        combinedObject.transform.position = Vector3.zero;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();

        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.CombineMeshes(combine);

        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;

        combinedObject.transform.SetParent(parentObj.transform);
    }

    void Start()
    {
        CombineMeshes();
    }
}