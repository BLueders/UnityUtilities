
using System;
using UnityEngine;
namespace Toolbox
{
    public static class Vector3Extensions
    {
        public static Vector3 Scale(this Vector3 original, float length)
        {
            original.Normalize();
            return new Vector3(original.x * length, original.y * length, original.z * length);
        }

        public static Vector2 ToVector2(this Vector3 original)
        {
            return new Vector2(original.x, original.y);
        }

        public static Vector4 ToVector4(this Vector3 original)
        {
            return new Vector4(original.x, original.y, original.z, 0);
        }

        public static Vector4 ToVector4(this Vector3 original, float w)
        {
            return new Vector4(original.x, original.y, original.z, w);
        }
    }
}

