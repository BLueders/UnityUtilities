using UnityEngine;
using System.Collections;
using System;

public class MyDragAndDropable : Draggable {

    protected override void HandleBeginDrag(DragInfo data){}
    protected override void HandleDrag(DragInfo data){}
    protected override void HandleEndDrag(DragInfo data){}

    protected override void OnDrop(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log("I was dropped into " + dropData.Container.name);
    }

    // Update is called once per frame
    void Update () {
	
	}

    protected override void OnStartHovering(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log("I started hovering " + dropData.Container.name);

    }

    protected override void OnHovering(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {

    }

    protected override void OnStopHovering(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log("I stopped hovering " + dropData.Container.name);

    }



}
