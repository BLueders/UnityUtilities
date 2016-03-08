using UnityEngine;
using System.Collections;
using System;

public abstract class DropContainer : MonoBehaviour
{
    public abstract void OnEnterHover(DragAndDropable dragAndDropable);
    public abstract void OnHover(DragAndDropable dragAndDropable);
    public abstract void OnExitHover(DragAndDropable dragAndDropable);

    public abstract void OnDrop(DragAndDropable dragAndDropable);

}
