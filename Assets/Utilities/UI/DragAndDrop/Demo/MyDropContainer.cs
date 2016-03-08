using UnityEngine;
using System.Collections;
using System;

public class MyDropContainer : DropContainer {

    public override void OnDrop(DragAndDropable dragAndDropable)
    {
        Debug.Log(dragAndDropable.name + " was droppen in me");
    }

    public override void OnEnterHover(DragAndDropable dragAndDropable)
    {
        Debug.Log(dragAndDropable.name + " started hovering me");
    }

    public override void OnExitHover(DragAndDropable dragAndDropable)
    {
        Debug.Log(dragAndDropable.name + " stopped hovering me");
    }

    public override void OnHover(DragAndDropable dragAndDropable)
    {

    }
}
