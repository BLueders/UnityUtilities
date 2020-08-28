using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendVolume : MonoBehaviour
{

    public MeshCollider outerCollider;
    public MeshCollider innerCollider;

    [SerializeField] float innerSize;

    public Mesh cachedMesh;
    public Mesh modifiedMesh = new Mesh();

    [ContextMenu("Resize inner size")]
    void UpdateMeshColliders()
    {
        if (outerCollider == null)
        {
            outerCollider = GetComponent<MeshCollider>();
            cachedMesh = CopyMesh(outerCollider.sharedMesh);
        }
        if (innerCollider == null)
        {
            innerCollider = gameObject.AddComponent<MeshCollider>();
            innerCollider.convex = true;
            innerCollider.isTrigger = true;
        }

        innerCollider.sharedMesh = modifiedMesh;
    }

    [ContextMenu("Resize")]
    void ResizeMesh()
    {
        modifiedMesh = CopyMesh(cachedMesh);

        Vector3[] vertices = modifiedMesh.vertices;
        for (int i = 0; i < modifiedMesh.vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            vertex *= innerSize;

            vertices[i] = vertex;
        }

        modifiedMesh.vertices = vertices;

        modifiedMesh.RecalculateNormals();
    }

    Mesh CopyMesh(Mesh mesh)
    {
        return new Mesh()
        {
            vertices = mesh.vertices,
            normals = mesh.normals,
            uv = mesh.uv,
            triangles = mesh.triangles,
            tangents = mesh.tangents
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireMesh(modifiedMesh,transform.position);
    }
}
