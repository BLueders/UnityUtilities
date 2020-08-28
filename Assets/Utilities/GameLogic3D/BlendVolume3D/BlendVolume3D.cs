using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class BlendVolume3D : MonoBehaviour
{
    MeshCollider innerC;
    MeshCollider outerC;
    Mesh outerMesh;
    Vector3 outerMeshCenter;

    [SerializeField]
    private float fadeDistance = 1;

    void Awake()
    {
        innerC = GetComponent<MeshCollider>();
        GameObject outerObject = new GameObject();
        outerObject.transform.SetParent(transform, false);
        outerC = outerObject.AddComponent<MeshCollider>();
        if(outerMesh == null)
        {
            CreateOuterMesh();
        }
        outerC.sharedMesh = outerMesh;
        outerC.convex = true;
        innerC.convex = true;
        innerC.isTrigger = true;
        outerC.isTrigger = true;
    }

    public float GetBlendValue(Vector3 point)
    {
        if (!outerC.bounds.Contains(point)){
            return 0f;
        }

        Vector3 innerHit = innerC.ClosestPoint(point);
        if(innerHit == point)
        {
            return 1f;
        }
        Vector3 rayDir = (innerHit - point).normalized;
        RaycastHit hit;
        outerC.Raycast(new Ray(point - rayDir * 1000000, rayDir), out hit, 1100000); // no idea how to get outer hit without raycast, lol just use 1000000 why not
        Vector3 outerHit = hit.point;

        float outerDist = (outerHit - innerHit).magnitude;
        float pointInnerDist = (point - outerHit).magnitude;
        return Mathf.Clamp01(pointInnerDist / outerDist);
    }

    private void CreateOuterMesh()
    {
        if(outerMesh == null)
        {
            outerMesh = new Mesh();
            outerMesh.name = innerC.sharedMesh.name + "_Clone";
        }

        Vector3[] outerVertices = new Vector3[innerC.sharedMesh.vertices.Length];
        Vector3[] outerNormals = new Vector3[innerC.sharedMesh.normals.Length];
        int[] outerTriangles = new int[innerC.sharedMesh.triangles.Length];

        Mesh innerMesh = innerC.sharedMesh;
        // calculate center
        outerMeshCenter = Vector3.zero;
        for(int i = 0; i < innerMesh.vertices.Length; i++)
        {
            outerMeshCenter += innerMesh.vertices[i];
        }
        outerMeshCenter /= (float)innerMesh.vertices.Length;
        // create scaled vertices
        for (int i = 0; i < innerMesh.vertices.Length; i++)
        {
            Vector3 newPos = (innerMesh.vertices[i] - outerMeshCenter) * fadeDistance;
            outerVertices[i] = newPos + outerMeshCenter;
        }
        // create normals
        for (int i = 0; i < innerMesh.normals.Length; i++)
        {
            outerNormals[i] = innerMesh.normals[i];
        }
        // create triangles
        for (int i = 0; i < innerMesh.triangles.Length; i++)
        {
            outerTriangles[i] = innerMesh.triangles[i];
        }
        outerMesh.vertices = outerVertices;
        outerMesh.normals = outerNormals;
        outerMesh.triangles = outerTriangles;
        outerMesh.RecalculateBounds();
        outerMesh.RecalculateNormals();
        outerMesh.RecalculateTangents();
    }


    public void AssignFadeDistance(float newFadeDistance)
    {
        fadeDistance = newFadeDistance;
        Mesh innerMesh = innerC.sharedMesh;
        Vector3[] outerVertices = new Vector3[innerC.sharedMesh.vertices.Length];
        for (int i = 0; i < outerMesh.vertices.Length; i++)
        {
            Vector3 newPos = (innerMesh.vertices[i] - outerMeshCenter) * fadeDistance;
            outerVertices[i] = newPos;
        }
        outerMesh.vertices = outerVertices;
        outerMesh.RecalculateBounds();
        outerMesh.RecalculateNormals();
        outerMesh.RecalculateTangents();
    }


    private void OnDrawGizmosSelected()
    {
        if(innerC == null){
            innerC = GetComponent<MeshCollider>();
        }
        Color col = Gizmos.color;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireMesh(innerC.sharedMesh, transform.position, transform.rotation, transform.lossyScale * fadeDistance);
        Gizmos.color = col;
    }
}
