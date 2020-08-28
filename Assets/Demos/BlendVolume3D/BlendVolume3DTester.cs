using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class BlendVolume3DTester : MonoBehaviour
{
    public BlendVolume3D blendV;
    void Update()
    {
        float scale = 1 - blendV.GetBlendValue(transform.position);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
