using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class PerspMultiFollowCamera : MonoBehaviour
{

	public Transform[] objectsToFollow;
	public float minDistance = 4;
	public float maxDistance = 200;
	public float relativePaddingY = 0.2f;
	public float relativePaddingX = 0.2f;
	public float movementSmoothing = 8;

	Camera cam;
	float distance;

	void Start ()
	{
		cam = GetComponent<Camera> ();
		if (objectsToFollow.Length == 0) {
			throw new UnityException ("PerspMultiFollowCamera needs to have at least one object to follow");
		}
	}

	void Update ()
	{
		
		Vector3[] camSpacePos = new Vector3[objectsToFollow.Length];
		float minCamSpaceX = float.MaxValue;
		float maxCamSpaceX = float.MinValue;
		float minCamSpaceY = float.MaxValue;
		float maxCamSpaceY = float.MinValue;

		// precalculate aTan of horizontal and vertical FOV to save some time
		float aTanFOVvertical = 1.0f / Mathf.Tan ((cam.fieldOfView / 2.0f) * Mathf.Deg2Rad);

		float vFOVrad = cam.fieldOfView * Mathf.Deg2Rad;
		float cameraHeightAt1 = Mathf.Tan (vFOVrad * .5f);
		float FOVhorizontal = Mathf.Atan (cameraHeightAt1 * cam.aspect) * 2f;
		float aTanFOVhorizontal = 1.0f / Mathf.Tan (FOVhorizontal / 2.0f);

		float minZ = float.MaxValue;
		for (int i = 0; i < objectsToFollow.Length; i++) {
			// Do camera positioning caltulations in camera space, so we can use x and y locally
			camSpacePos [i] = transform.worldToLocalMatrix.MultiplyPoint (objectsToFollow [i].position);

			if (camSpacePos [i].x > maxCamSpaceX) {
				maxCamSpaceX = camSpacePos [i].x;
			}
			if (camSpacePos [i].x < minCamSpaceX) {
				minCamSpaceX = camSpacePos [i].x;
			}
			if (camSpacePos [i].y > maxCamSpaceY) {
				maxCamSpaceY = camSpacePos [i].y;
			}
			if (camSpacePos [i].y < minCamSpaceY) {
				minCamSpaceY = camSpacePos [i].y;
			}
		}
		// new coordinates for the camera from the center of objects
		float centerX = (minCamSpaceX + maxCamSpaceX) / 2.0f;
		float centerY = (minCamSpaceY + maxCamSpaceY) / 2.0f;
		// find the furthest point back to have all objects visible (including relative padding), using the new center of the camera
		for (int i = 0; i < objectsToFollow.Length; i++) {

			// furthest point back depending on the horizontal FOV (x position)
			float xAdjustedPos = Mathf.Abs ((camSpacePos [i].x - centerX) * (1 + relativePaddingX));
			float xDependedCamZ = camSpacePos [i].z - (xAdjustedPos * aTanFOVhorizontal);

			// furthest point back depending on the vertical FOV (y position)
			float yAdjustedPos = Mathf.Abs ((camSpacePos [i].y - centerY) * (1 + relativePaddingY));
			float yDependedCamZ = camSpacePos [i].z - (yAdjustedPos * aTanFOVvertical);

			float newZ = Mathf.Min (xDependedCamZ, yDependedCamZ);
			// clamp to minimum and maximum distance of closet object
			newZ = Mathf.Clamp (newZ - camSpacePos [i].z, -maxDistance, -minDistance) + camSpacePos [i].z;
			if (minZ > newZ) {
				minZ = newZ;
			}
		}
		Vector3 newCamLocalPos = new Vector3 (centerX, centerY, minZ);
		Vector3 newCamWorldPos = transform.localToWorldMatrix.MultiplyPoint (newCamLocalPos);

		// lerp camera to new center
		transform.position = Vector3.Lerp (transform.position, newCamWorldPos, Time.deltaTime * movementSmoothing);
	}
}
