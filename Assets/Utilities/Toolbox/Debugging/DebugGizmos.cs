using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allows the injection of Gizmo draws from outside the Unity OnDrawGizmos() function. Gizmos are used to give visual debug and design aid when working in a Unity scene
/// </summary>
public class DebugGizmos : Singleton<DebugGizmos> {

    List<DebugGizmo> gizmos = new List<DebugGizmo>();

    void OnDrawGizmos()
    {
        Color gizmoColor = Gizmos.color;
        foreach (DebugGizmo gizmo  in gizmos) {
            gizmo.Draw();
        }
        gizmos.Clear();
        Gizmos.color = gizmoColor;
    }


    public static void DrawFrustum(Vector3 center, float fov, float maxRange, float minRange, float aspect, Color color)
    {
        Instance.gizmos.Add(new FrustumGizmo(center, fov, maxRange, minRange, aspect, color));
    }

    public static void DrawMesh(Mesh mesh, int subMeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
    {
        Instance.gizmos.Add(new MeshGizmo(mesh, subMeshIndex, position, rotation, scale, color));
    }

    public static void DrawMesh(Mesh mesh, Vector3 position, Color color)
    {
        Instance.gizmos.Add(new MeshGizmo(mesh, 0, position, Quaternion.identity, new Vector3(1,1,1), color));
    }

    public static void DrawWireMesh(Mesh mesh, int subMeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
    {
        Instance.gizmos.Add(new WireMeshGizmo(mesh, subMeshIndex, position, rotation, scale, color));
    }

    public static void DrawWireMesh(Mesh mesh, Vector3 position, Color color)
    {
        Instance.gizmos.Add(new WireMeshGizmo(mesh, 0, position, Quaternion.identity, new Vector3(1, 1, 1), color));
    }

    public static void DrawCube(Vector3 position, Vector3 size, Color color)
    {
        Instance.gizmos.Add(new CubeGizmo(position, size, color));
    }

    public static void DrawWireCube(Vector3 position, Vector3 size, Color color)
    {
        Instance.gizmos.Add(new WireCubeGizmo(position, size, color));
    }

    public static void DrawSphere(Vector3 position, float radius, Color color) {
        Instance.gizmos.Add(new SphereGizmo(position, radius, color));
    }

    public static void DrawWireSphere(Vector3 position, float radius, Color color)
    {
        Instance.gizmos.Add(new WireSphereGizmo(position, radius, color));
    }

    private abstract class DebugGizmo {
        protected Color _color;
        internal abstract void Draw();

        internal DebugGizmo(Color color) {
            _color = color;
        }
    }

    private class MeshGizmo : DebugGizmo
    {
        private Vector3 _position;
        private Mesh _mesh;
        private int _subMeshIndex;
        private Quaternion _rotation;
        private Vector3 _scale;

        internal MeshGizmo(Mesh mesh, int subMeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, Color color) : base(color)
        {
            _mesh = mesh;
            _subMeshIndex = subMeshIndex;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawMesh(_mesh, _subMeshIndex, _position, _rotation, _scale);
        }
    }

    private class WireMeshGizmo : DebugGizmo
    {
        private Vector3 _position;
        private Mesh _mesh;
        private int _subMeshIndex;
        private Quaternion _rotation;
        private Vector3 _scale;

        internal WireMeshGizmo(Mesh mesh, int subMeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, Color color) : base(color)
        {
            _mesh = mesh;
            _subMeshIndex = subMeshIndex;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawWireMesh(_mesh, _subMeshIndex, _position, _rotation, _scale);
        }
    }

    private class CubeGizmo : DebugGizmo
    {
        private Vector3 _position;
        private Vector3 _size;

        internal CubeGizmo(Vector3 position, Vector3 size, Color color) : base(color)
        {
            _position = position;
            _size = size;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(_position, _size);
        }
    }


    private class WireCubeGizmo : DebugGizmo
    {
        private Vector3 _position;
        private Vector3 _size;

        internal WireCubeGizmo(Vector3 position, Vector3 size, Color color) : base(color)
        {
            _position = position;
            _size = size;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawWireCube(_position, _size);
        }
    }

    private class SphereGizmo : DebugGizmo
    {
        private Vector3 _position;
        private float _radius;

        internal SphereGizmo(Vector3 position, float radius, Color color) : base(color) {
            _position = position;
            _radius = radius;
        }

        internal override void Draw() {
            Gizmos.color = _color;
            Gizmos.DrawSphere(_position, _radius);
        }
    }

    private class WireSphereGizmo : DebugGizmo
    {
        private Vector3 _position;
        private float _radius;

        internal WireSphereGizmo(Vector3 position, float radius, Color color) : base(color)
        {
            _position = position;
            _radius = radius;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(_position, _radius);
        }
    }

    private class FrustumGizmo : DebugGizmo
    {
        private Vector3 _center;
        private float _fov;
        private float _maxRange;
        private float _minRange;
        private float _aspect;

        internal FrustumGizmo(Vector3 center, float fov, float maxRange, float minRange, float aspect, Color color) : base(color)
        {
            _center = center;
            _fov = fov;
            _maxRange = maxRange;
            _minRange = minRange;
            _aspect = aspect;
            _color = color;
        }

        internal override void Draw()
        {
            Gizmos.color = _color;
            Gizmos.DrawFrustum(_center, _fov, _maxRange, _minRange, _aspect);
        }
    }
}
