
using System;
using UnityEngine;

public static class Vector3Extensions
{
	public static Vector3 WithLength (this Vector3 original, float length)
	{
		original.Normalize();
		return new Vector3(original.x * length, original.y * length, original.z * length);
	}
}


