using UnityEngine;
using System.Collections;

public class Demonstration : MonoBehaviour {

    public ValueInterpolater.InterpType type = ValueInterpolater.InterpType.ACCELERATE;

    public float duration = 2f;
    public float delay = 0f;
    public float coefficient = 2f;

    private float start = 0;
    private float end = 5;

	// Use this for initialization
	void Start () {
        Debug.Log("Click the ball! Change the interpolation function and values in the inspector.");
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == this.gameObject) {
                    start = transform.position.y;
                    float newEnd = start;

                    ValueInterpolater.LerpValueInterpolated(type, coefficient, start, end, duration, delay, (float value) => {
                        Vector3 pos = transform.position;
                        pos.y = value;
                        transform.position = pos;
                    });
                
                    end = newEnd;
                }
            }
        }

	}
}
