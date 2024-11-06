using UnityEngine;

public class MapMeshCombiner : MonoBehaviour
{
    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        transform.position = Vector3.zero;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        MeshFilter combinedMeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.CombineMeshes(combine);

        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;
    }

    void Awake()
    {
        CombineMeshes();
    }
}