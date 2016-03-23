using UnityEngine;
using System.Collections;

public class MyDropContainer : DropContainer {

    public override void OnDrop(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Vector3 randomColor = Random.onUnitSphere;
        GetComponent<Renderer>().material.color = new Color(randomColor.x, randomColor.y, randomColor.z, 1);
        Debug.Log(dropData.Draggable.name + " was dropped in me");
    }

    public override void OnStartHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        Debug.Log(dropData.Draggable.name + " started hovering me");
    }

    public override void OnStopHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        transform.localScale = new Vector3(3f, 3f, 3f);
        Debug.Log(dropData.Draggable.name + " stopped hovering me");
    }

    public override void OnHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f) + Random.onUnitSphere * 0.1f;
    }
}
