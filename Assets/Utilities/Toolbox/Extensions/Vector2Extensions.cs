
using System;
using UnityEngine;

namespace Toolbox
{
    public static class Vector2Extensions
    {
        public static Vector2 Scale(this Vector2 original, float length)
        {
            original.Normalize();
            return new Vector2(original.x * length, original.y * length);
        }

        public static Vector3 ToVector3(this Vector2 original)
        {
            return new Vector3(original.x, original.y, 0);
        }

        public static Vector3 ToVector3(this Vector2 original, float z)
        {
            return new Vector3(original.x, original.y, z);
        }

        public static Vector4 ToVector4(this Vector2 original)
        {
            return new Vector4(original.x, original.y, 0, 0);
        }

        public static Vector4 ToVector4(this Vector2 original, float z, float w)
        {
            return new Vector4(original.x, original.y, z, w);
        }
    }
}

