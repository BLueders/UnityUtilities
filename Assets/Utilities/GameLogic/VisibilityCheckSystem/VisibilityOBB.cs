using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundingVolumes;
using System;

public class VisibilityOBB : MonoBehaviour
{

	public GameObject boundingObject;
	public OBB obb = new OBB ();
	public bool isVisible = false;

	public virtual void OnBecomeVisible ()
	{
	
	}

	public virtual void OnStopVisible ()
	{
	
	}

	void OnDrawGizmos ()
	{
		obb = OBB.FromTransform (transform);
		obb.DrawGizmo (Color.cyan);
	}
}
