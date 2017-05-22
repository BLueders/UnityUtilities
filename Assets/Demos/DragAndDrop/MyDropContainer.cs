using UnityEngine;
using System.Collections;

public class MyDropContainer : DropContainer {

	private Vector3 originalScale;

	void Start(){
		originalScale = transform.localScale;
	}

    public override void OnDrop(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Vector3 randomColor = Random.onUnitSphere;
        GetComponent<Renderer>().material.color = new Color(randomColor.x, randomColor.y, randomColor.z, 1);
        Debug.Log(dropData.Draggable.name + " was dropped in me");
    }

    public override void OnStartHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
		transform.localScale = originalScale * 1.2f;
        Debug.Log(dropData.Draggable.name + " started hovering me");
    }

    public override void OnStopHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
		transform.localScale = originalScale;
        Debug.Log(dropData.Draggable.name + " stopped hovering me");
    }

    public override void OnHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
		transform.localScale = originalScale * (1.2f + Random.Range (-0.1f, 0.1f));
    }
}
