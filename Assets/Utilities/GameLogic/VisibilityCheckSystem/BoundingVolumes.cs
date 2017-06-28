using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundingVolumes
{
	public enum BoundingVolume{
		AABB,
		OBB
	}

	public struct AABB
	{
		public Vector3 center;

		public Vector3 halfWidths;

		public AABB (Vector3 center, Vector3 halfWidths)
		{
			this.center = center;
			this.halfWidths = halfWidths;
		}

		public AABB (float right, float left, float top, float bottom, float front, float back)
		{
			this.center = new Vector3 ((right + left) / 2.0f, (top + bottom) / 2.0f, (front + back) / 2.0f);
			this.halfWidths = new Vector3 (right - center.x, top - center.y, front - center.z);
		}

		public float rightBound { get { return center.x + halfWidths.x; } }

		public float leftBound { get { return center.x - halfWidths.x; } }

		public float topBound { get { return center.y + halfWidths.y; } }

		public float bottomBound { get { return center.y - halfWidths.y; } }

		public float frontBound { get { return center.z + halfWidths.z; } }

		public float backBound { get { return center.z + halfWidths.z; } }

		public Vector3 leftBottomBack { get { return new Vector3 (leftBound, bottomBound, backBound); } }

		public Vector3 leftBottomFront { get { return new Vector3 (leftBound, bottomBound, frontBound); } }

		public Vector3 leftTopBack  { get { return new Vector3 (leftBound, topBound, backBound); } }

		public Vector3 leftTopFront  { get { return new Vector3 (leftBound, topBound, frontBound); } }

		public Vector3 rightBottomBack  { get { return new Vector3 (rightBound, bottomBound, backBound); } }

		public Vector3 rightBottomFront  { get { return new Vector3 (rightBound, bottomBound, frontBound); } }

		public Vector3 rightTopBack  { get { return new Vector3 (rightBound, topBound, backBound); } }

		public Vector3 rightTopFront  { get { return new Vector3 (rightBound, topBound, frontBound); } }

		public Vector3[] points { get { return new Vector3[] {
					leftBottomBack,
					leftBottomFront,
					leftTopBack,
					leftTopFront,
					rightBottomBack,
					rightBottomFront,
					rightTopBack,
					rightTopFront
				};}}

		public void DrawGizmo (Color color)
		{
			Color col = Gizmos.color;
			Gizmos.color = color;

			Gizmos.DrawLine (leftBottomBack, leftBottomFront);
			Gizmos.DrawLine (rightBottomBack, rightBottomFront);
			Gizmos.DrawLine (leftBottomBack, rightBottomBack);
			Gizmos.DrawLine (leftBottomFront, rightBottomFront);

			Gizmos.DrawLine (leftTopBack, leftTopFront);
			Gizmos.DrawLine (rightTopBack, rightTopFront);
			Gizmos.DrawLine (leftTopBack, rightTopBack);
			Gizmos.DrawLine (leftTopFront, rightTopFront);

			Gizmos.DrawLine (leftBottomBack, leftTopBack);
			Gizmos.DrawLine (rightBottomBack, rightTopBack);
			Gizmos.DrawLine (leftBottomFront, leftTopFront);
			Gizmos.DrawLine (rightBottomFront, rightTopFront);
			Gizmos.color = col;
		}
	}

	public struct OBB
	{
		public Vector3 center;
		public Vector3 scale;
		public Quaternion rotation;

		public static OBB FromTransform(Transform transform){
			OBB obb;
			obb.center = transform.position;
			obb.scale = transform.lossyScale;
			obb.rotation = transform.rotation;
			return obb;
		}

		public OBB (Vector3 center, Vector3 scale, Quaternion rotation)
		{
			this.center = center;
			this.scale = scale;
			this.rotation = rotation;
		}

		public OBB (Vector3 center, Vector3 right, Vector3 up, Vector3 front)
		{
			right = Vector3.Cross (front.normalized, up.normalized) * right.magnitude;
			this.center = center;
			this.scale = new Vector3 (right.magnitude * 2.0f, up.magnitude * 2.0f, front.magnitude * 2.0f);
			this.rotation = Quaternion.LookRotation (front.normalized, up.normalized);
		}

		public Vector3[] points { get { return new Vector3[] {
				leftBottomBack,
				leftBottomFront,
				leftTopBack,
				leftTopFront,
				rightBottomBack,
				rightBottomFront,
				rightTopBack,
				rightTopFront
			};}}

		public Vector3 leftBottomBack { get { return center - rotation * new Vector3 (-scale.x * 0.5f, -scale.y * 0.5f, -scale.z * 0.5f); } }

		public Vector3 leftBottomFront { get { return center - rotation * new Vector3 (-scale.x * 0.5f, -scale.y * 0.5f, scale.z * 0.5f); } }

		public Vector3 leftTopBack  { get { return center - rotation * new Vector3 (-scale.x * 0.5f, scale.y * 0.5f, -scale.z * 0.5f); } }

		public Vector3 leftTopFront  { get { return center - rotation * new Vector3 (-scale.x * 0.5f, scale.y * 0.5f, scale.z * 0.5f); } }

		public Vector3 rightBottomBack  { get { return center - rotation * new Vector3 (scale.x * 0.5f, -scale.y * 0.5f, -scale.z * 0.5f); } }

		public Vector3 rightBottomFront  { get { return center - rotation * new Vector3 (scale.x * 0.5f, -scale.y * 0.5f, scale.z * 0.5f); } }

		public Vector3 rightTopBack  { get { return center - rotation * new Vector3 (scale.x * 0.5f, scale.y * 0.5f, -scale.z * 0.5f); } }

		public Vector3 rightTopFront  { get { return center - rotation * new Vector3 (scale.x * 0.5f, scale.y * 0.5f, scale.z * 0.5f); } }

		public Vector3 right { get { return rotation * Vector3.right; } }

		public Vector3 left { get { return rotation * Vector3.left; } }

		public Vector3 up { get { return rotation * Vector3.up; } }

		public Vector3 down { get { return rotation * Vector3.down; } }

		public Vector3 forward { get { return rotation * Vector3.forward; } }

		public Vector3 back { get { return rotation * Vector3.back; } }

		public float rightBound { get { return Mathf.Max (leftBottomBack.x, leftBottomFront.x, leftTopBack.x, leftTopFront.x, rightBottomBack.x, rightBottomFront.x, rightTopBack.x, rightTopFront.x); } }

		public float leftBound { get { return Mathf.Min (leftBottomBack.x, leftBottomFront.x, leftTopBack.x, leftTopFront.x, rightBottomBack.x, rightBottomFront.x, rightTopBack.x, rightTopFront.x); } }

		public float upBound { get { return Mathf.Max (leftBottomBack.y, leftBottomFront.y, leftTopBack.y, leftTopFront.y, rightBottomBack.y, rightBottomFront.y, rightTopBack.y, rightTopFront.y); } }

		public float bottomBound { get { return Mathf.Min (leftBottomBack.y, leftBottomFront.y, leftTopBack.y, leftTopFront.y, rightBottomBack.y, rightBottomFront.y, rightTopBack.y, rightTopFront.y); } }

		public float frontBound { get { return Mathf.Max (leftBottomBack.z, leftBottomFront.z, leftTopBack.z, leftTopFront.z, rightBottomBack.z, rightBottomFront.z, rightTopBack.z, rightTopFront.z); } }

		public float backBound { get { return Mathf.Min (leftBottomBack.z, leftBottomFront.z, leftTopBack.z, leftTopFront.z, rightBottomBack.z, rightBottomFront.z, rightTopBack.z, rightTopFront.z); } }

		public void DrawGizmo (Color color)
		{
			Color col = Gizmos.color;
			Gizmos.color = color;

			Gizmos.DrawLine (leftBottomBack, leftBottomFront);
			Gizmos.DrawLine (rightBottomBack, rightBottomFront);
			Gizmos.DrawLine (leftBottomBack, rightBottomBack);
			Gizmos.DrawLine (leftBottomFront, rightBottomFront);

			Gizmos.DrawLine (leftTopBack, leftTopFront);
			Gizmos.DrawLine (rightTopBack, rightTopFront);
			Gizmos.DrawLine (leftTopBack, rightTopBack);
			Gizmos.DrawLine (leftTopFront, rightTopFront);

			Gizmos.DrawLine (leftBottomBack, leftTopBack);
			Gizmos.DrawLine (rightBottomBack, rightTopBack);
			Gizmos.DrawLine (leftBottomFront, leftTopFront);
			Gizmos.DrawLine (rightBottomFront, rightTopFront);
			Gizmos.color = col;
		}
	}

	public struct TransformOBB
	{
		public Transform transform;

		public TransformOBB(Transform transform){
			this.transform = transform;
		}

		public Vector3 leftBottomBack { get { return transform.position - transform.rotation * new Vector3 (-transform.lossyScale.x * 0.5f, -transform.lossyScale.y * 0.5f, -transform.lossyScale.z * 0.5f); } }

		public Vector3 leftBottomFront { get { return transform.position - transform.rotation * new Vector3 (-transform.lossyScale.x * 0.5f, -transform.lossyScale.y * 0.5f, transform.lossyScale.z * 0.5f); } }

		public Vector3 leftTopBack  { get { return transform.position - transform.rotation * new Vector3 (-transform.lossyScale.x * 0.5f, transform.lossyScale.y * 0.5f, -transform.lossyScale.z * 0.5f); } }

		public Vector3 leftTopFront  { get { return transform.position - transform.rotation * new Vector3 (-transform.lossyScale.x * 0.5f, transform.lossyScale.y * 0.5f, transform.lossyScale.z * 0.5f); } }

		public Vector3 rightBottomBack  { get { return transform.position - transform.rotation * new Vector3 (transform.lossyScale.x * 0.5f, -transform.lossyScale.y * 0.5f, -transform.lossyScale.z * 0.5f); } }

		public Vector3 rightBottomFront  { get { return transform.position - transform.rotation * new Vector3 (transform.lossyScale.x * 0.5f, -transform.lossyScale.y * 0.5f, transform.lossyScale.z * 0.5f); } }

		public Vector3 rightTopBack  { get { return transform.position - transform.rotation * new Vector3 (transform.lossyScale.x * 0.5f, transform.lossyScale.y * 0.5f, -transform.lossyScale.z * 0.5f); } }

		public Vector3 rightTopFront  { get { return transform.position - transform.rotation * new Vector3 (transform.lossyScale.x * 0.5f, transform.lossyScale.y * 0.5f, transform.lossyScale.z * 0.5f); } }

		public Vector3 right { get { return transform.rotation * Vector3.right; } }

		public Vector3 left { get { return transform.rotation * Vector3.left; } }

		public Vector3 up { get { return transform.rotation * Vector3.up; } }

		public Vector3 down { get { return transform.rotation * Vector3.down; } }

		public Vector3 forward { get { return transform.rotation * Vector3.forward; } }

		public Vector3 back { get { return transform.rotation * Vector3.back; } }

		public float rightBound { get { return Mathf.Max (leftBottomBack.x, leftBottomFront.x, leftTopBack.x, leftTopFront.x, rightBottomBack.x, rightBottomFront.x, rightTopBack.x, rightTopFront.x); } }

		public float leftBound { get { return Mathf.Min (leftBottomBack.x, leftBottomFront.x, leftTopBack.x, leftTopFront.x, rightBottomBack.x, rightBottomFront.x, rightTopBack.x, rightTopFront.x); } }

		public float upBound { get { return Mathf.Max (leftBottomBack.y, leftBottomFront.y, leftTopBack.y, leftTopFront.y, rightBottomBack.y, rightBottomFront.y, rightTopBack.y, rightTopFront.y); } }

		public float bottomBound { get { return Mathf.Min (leftBottomBack.y, leftBottomFront.y, leftTopBack.y, leftTopFront.y, rightBottomBack.y, rightBottomFront.y, rightTopBack.y, rightTopFront.y); } }

		public float frontBound { get { return Mathf.Max (leftBottomBack.z, leftBottomFront.z, leftTopBack.z, leftTopFront.z, rightBottomBack.z, rightBottomFront.z, rightTopBack.z, rightTopFront.z); } }

		public float backBound { get { return Mathf.Min (leftBottomBack.z, leftBottomFront.z, leftTopBack.z, leftTopFront.z, rightBottomBack.z, rightBottomFront.z, rightTopBack.z, rightTopFront.z); } }

		public Vector3[] points { get { return new Vector3[] {
					leftBottomBack,
					leftBottomFront,
					leftTopBack,
					leftTopFront,
					rightBottomBack,
					rightBottomFront,
					rightTopBack,
					rightTopFront
				};}}

		public void DrawGizmo (Color color)
		{
			Color col = Gizmos.color;
			Gizmos.color = color;

			Gizmos.DrawLine (leftBottomBack, leftBottomFront);
			Gizmos.DrawLine (rightBottomBack, rightBottomFront);
			Gizmos.DrawLine (leftBottomBack, rightBottomBack);
			Gizmos.DrawLine (leftBottomFront, rightBottomFront);

			Gizmos.DrawLine (leftTopBack, leftTopFront);
			Gizmos.DrawLine (rightTopBack, rightTopFront);
			Gizmos.DrawLine (leftTopBack, rightTopBack);
			Gizmos.DrawLine (leftTopFront, rightTopFront);

			Gizmos.DrawLine (leftBottomBack, leftTopBack);
			Gizmos.DrawLine (rightBottomBack, rightTopBack);
			Gizmos.DrawLine (leftBottomFront, leftTopFront);
			Gizmos.DrawLine (rightBottomFront, rightTopFront);
			Gizmos.color = col;
		}
	}
}
	
