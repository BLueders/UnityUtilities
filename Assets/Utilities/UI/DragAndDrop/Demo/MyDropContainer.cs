using UnityEngine;
using System.Collections;
using System;

public class MyDropContainer : DropContainer {

    public override void OnDrop(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log(dropData.Draggable.name + " was droppen in me");
    }

    public override void OnStartHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log(dropData.Draggable.name + " started hovering me");
    }

    public override void OnStopHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {
        Debug.Log(dropData.Draggable.name + " stopped hovering me");
    }

    public override void OnHover(Draggable.DragInfo dragData, Draggable.DropInfo dropData)
    {

    }
}
