using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class VisibilityCheckSystem : MonoBehaviour
{

	public VisibilityOBB[] obbObjects;
	public LayerMask occludingLayers;

	Camera cam;

	void Awake ()
	{
		cam = GetComponent<Camera> ();
	}

	void Update ()
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes (cam);
		foreach (VisibilityOBB vOBB in obbObjects) {
			bool wasVisible = vOBB.isVisible;
			if (isVisible (vOBB)) {
				if (!vOBB.isVisible) {
					vOBB.OnBecomeVisible ();
				}
				vOBB.isVisible = true;
			}
			else if (wasVisible) {
				vOBB.OnStopVisible ();
				vOBB.isVisible = false;
			}
		}
	}

	bool isVisible (VisibilityOBB vOBB)
	{
		foreach (Vector3 point in vOBB.obb.points) {
			Vector3 pointOnScreen = cam.WorldToScreenPoint (point);

			//Is in front
			if (pointOnScreen.z < 0) {
				continue;
			}

			//Is in FOV
			if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
			    (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height)) {
				continue;
			}

			RaycastHit hit;
			Vector3 heading = point - transform.position;
			Vector3 direction = heading.normalized;// / heading.magnitude;

			if (Physics.Linecast (transform.position, point, out hit, occludingLayers)) {
				if (hit.transform != vOBB.boundingObject) {
					Debug.DrawLine (cam.transform.position, point, Color.red);
					continue;
				}
			}
			return true;
		}
		return false;
	}
}
