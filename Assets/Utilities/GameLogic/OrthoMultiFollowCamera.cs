using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class OrthoMultiFollowCamera : MonoBehaviour {

	public Transform[] objectsToFollow;
    public float minSize = 1;
    public float maxSize = 20;
    public float relativePaddingY = 0.1f;
    public float relativePaddingX = 0.1f;
    public float zoomSmoothing = 4;
    public float movementSmoothing = 2;

    Camera cam;
    float paddingY;
    float paddingX;

    void Start(){
        cam = GetComponent<Camera>();
        if(objectsToFollow.Length == 0){
            throw new UnityException("OrthoMultiFollowCamera needs to have at least one object to follow");
        }
    }

    void Update(){
        paddingY = Screen.height * relativePaddingY;
        paddingX = Screen.width * relativePaddingX;

        Vector3[] screenPos = new Vector3[objectsToFollow.Length];
        float minScreenX = float.MaxValue;
        float maxScreenX = float.MinValue;
        float minScreenY = float.MaxValue;
        float maxScreenY = float.MinValue;

        float mediumObjectDistance = 0;
        for(int i = 0; i <objectsToFollow.Length; i++){
            screenPos[i] = cam.WorldToScreenPoint(objectsToFollow[i].position);
            if(screenPos[i].x > maxScreenX) {
                maxScreenX = screenPos[i].x;
            }
            if(screenPos[i].x < minScreenX) {
                minScreenX = screenPos[i].x;
            }
            if(screenPos[i].y > maxScreenY) {
                maxScreenY = screenPos[i].y;
            }
            if(screenPos[i].y < minScreenY) {
                minScreenY = screenPos[i].y;
            }
            mediumObjectDistance += (transform.position - objectsToFollow[i].position).magnitude;
        }
        //follow mid of targets, keeping rotation
        mediumObjectDistance /= objectsToFollow.Length;

        // get mean center of objects on screen, use to calculate world center for camera to look at
        Vector3 objectScreenCenter = new Vector3((minScreenX + maxScreenX)/2, (minScreenY + maxScreenY)/2, mediumObjectDistance);
        Vector3 objectWorldCenter = cam.ScreenToWorldPoint(objectScreenCenter);

        // lerp camera to new center
        Vector3 newCamPosition = Vector3.Project(transform.position - objectWorldCenter, transform.forward) + objectWorldCenter;
        transform.position = Vector3.Lerp(transform.position, newCamPosition, Time.deltaTime * movementSmoothing);

        // TODO hard bound the player positions to the screen edges offset, so they dont get out of sight
        // TODO sync zoom and movement better

        //ZoomY
        float yMinDelta = paddingY - minScreenY;
        float yMaxDelta = maxScreenY - Screen.height + paddingY;
        float yPixelDelta = Mathf.Max(yMinDelta, yMaxDelta);
        float ySizeDelta = (cam.orthographicSize / Screen.height) * yPixelDelta;
        float newYDependingSize = cam.orthographicSize + ySizeDelta;

        //ZoomX
        float xMinDelta = paddingX - minScreenX;
        float xMaxDelta = maxScreenX - Screen.width + paddingX;
        float xPixelDelta = Mathf.Max(xMinDelta, xMaxDelta);
        float xSizeDelta = (cam.orthographicSize / Screen.height) * xPixelDelta;
        float newXDependingSize = cam.orthographicSize + xSizeDelta * (Screen.width/Screen.height);

        float newCamSize = Mathf.Max(newYDependingSize, newXDependingSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newCamSize, Time.deltaTime * zoomSmoothing);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
    }
}
