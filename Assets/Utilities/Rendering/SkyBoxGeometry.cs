using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the object along with the given camera (or current, main). Thus the object appears to be part of the skybox.
/// </summary>
public class SkyBoxGeometry : MonoBehaviour {


    public enum CameraMode
    {
        Main, Active, Custom
    }

    [SerializeField]
    Camera _camera;
	[SerializeField]
    Vector3 offsetToCamera = new Vector3();
    [SerializeField]
    bool _keepUpwardsFacing = false;
    [SerializeField]
    bool _executeInEditor = true;
    [SerializeField]
    CameraMode _cameraMode = CameraMode.Main;

    void Start()
	{
        switch (_cameraMode)
        {
            case CameraMode.Main:
                _camera = Camera.main;
                break;
            case CameraMode.Active:
                _camera = Camera.current;
                break;
            case CameraMode.Custom:
                break;
        }
    }
	
	void Update() 
	{
        switch (_cameraMode)
        {
            case CameraMode.Main:
                _camera = Camera.main;
                break;
            case CameraMode.Active:
                _camera = Camera.current;
                break;
            case CameraMode.Custom:
                break;
        }
        if (_camera == null)
            return;
        OrientObjectToCamera();
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos () {
		if(!_executeInEditor) {
			return;
		}
        switch (_cameraMode)
        {
            case CameraMode.Main:
                _camera = Camera.main;
                break;
            case CameraMode.Active:
                _camera = Camera.current;
                break;
            case CameraMode.Custom:
                break;
        }
        if (_camera == null)
            return;
        OrientObjectToCamera();
		
	}
	#endif
	
	void OrientObjectToCamera() {
		transform.position = _camera.transform.position + offsetToCamera;
	}
}
