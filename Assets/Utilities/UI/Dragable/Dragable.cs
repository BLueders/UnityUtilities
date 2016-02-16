using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public abstract class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float PATH_NODE_DISTANCE = 10;

    public bool IsDragged { get; private set; }
    public Vector3 DragStartPos { get; private set; }
    public Vector3 DragEndPos { get; private set; }
    public Vector3 DragTotalDistance { get; private set; }
    public Vector3 DragFrameDistance { get; private set; }
    public float DragFrameForce { get; private set; }
    public float DragTotalForce { get; private set; }
    public float DragTime { get; private set; }
    public float DragStartTime { get; private set; }
    public float DragEndTime { get; private set; }
    public LinkedList<Vector3> DragPath { get; private set; }
    public float DragPathDistance { get; private set; }

    private float _lastNodeTime;

    void Start()
    {
        DragPath = new LinkedList<Vector3>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        IsDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragged = false;
    }

    private void ResetDrag() {
        DragPath.Clear();
        DragTotalDistance = Vector3.zero;
        DragStartPos = transform.position;
        DragEndPos = transform.position;
    }
}
