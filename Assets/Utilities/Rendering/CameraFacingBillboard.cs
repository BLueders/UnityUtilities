using UnityEngine;
using System.Collections;

/// <summary>
/// A gameobject with this component will always rotate towards the main camera, or the specified (custom) one.
/// </summary>
public class CameraFacingBillboard : MonoBehaviour
{
    public enum CameraMode {
        Main, Active, Custom
    }

	[SerializeField]
    Camera _camera;
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
	
	public void Update() 
	{
        switch (_cameraMode) {
            case CameraMode.Main:
                _camera = Camera.main;
                break;
            case CameraMode.Active:
                _camera = Camera.current;
                break;
            case CameraMode.Custom:
                break;
        }
		if(_camera == null) 
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
		if(_camera.orthographic){
			if(_keepUpwardsFacing) {
				transform.LookAt(transform.position - _camera.transform.forward, Vector3.up);
			} else {
				transform.LookAt(transform.position - _camera.transform.forward, transform.up);
			}
		}
		else {
			if(_keepUpwardsFacing) {
				transform.LookAt(_camera.transform.position, Vector3.up);
			} else {
				transform.LookAt(_camera.transform.position, transform.up);
			}
		}
	}
}
