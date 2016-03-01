using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float PATH_NODE_DISTANCE = 10;
    private const float DRAG_FORCE_FACTOR = 200;

    public bool IsDragged { get; private set; }
    public Vector3 DragStartPos { get; private set; }
    public Vector3 DragEndPos { get; private set; }
    public Vector3 DragPotentialForce { get { return (DragStartPos - DragEndPos) * DRAG_FORCE_FACTOR; } }
    public Vector3 DragVelocity { get; private set; }
    public float DragTime { get { return DragEndTime - DragStartTime; } }
    public float DragStartTime { get; private set; }
    public float DragEndTime { get; private set; }
    public LinkedList<Vector3> DragPath { get; private set; }
    public float DragDistance { get; private set; }

    public LinkedList<Vector2> DragScreenPath { get; private set; }
    public Vector2 DragStartScreenPos { get; private set; }
    public Vector2 DragEndScreenPos { get; private set; }
    public Vector2 DragTotalScreenDistance { get; private set; }
    public Vector2 DragFrameScreenDistance { get; private set; }

    private float _lastOnDragCallTime;
    private Vector3 _lastFramePos;
    private float _dragDistanceToScreen;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Initialize(eventData);
        //Debug.Log("Begin Drag");
        IsDragged = true;
        DragStartPos = transform.position;
        DragEndPos = transform.position;
        DragStartTime = Time.time;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        DragScreenPath.AddLast(screenPos);
        DragScreenPath.AddLast(screenPos);

        DragPath.AddLast(transform.position);
        DragPath.AddLast(transform.position);
    }

    private void Initialize(PointerEventData eventData)
    {
        if(DragPath == null)
            DragPath = new LinkedList<Vector3>();
        if (DragScreenPath == null)
            DragScreenPath = new LinkedList<Vector2>();
        _dragDistanceToScreen = eventData.pressEventCamera.WorldToScreenPoint(gameObject.transform.position).z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag");
        IsDragged = true;
        SetDraggedPosition(eventData);

        DragEndPos = transform.position;
        Vector3 dragMoveDelta = _lastFramePos - transform.position;
        float dragDeltaTime = (Time.time - _lastOnDragCallTime);
        DragDistance += dragMoveDelta.magnitude;
        DragVelocity = Vector3.Lerp(dragMoveDelta * (1 / dragDeltaTime), DragVelocity, 0.5f);
        _lastFramePos = transform.position;
        _lastOnDragCallTime = Time.time;

        Vector2 lastScreenPathPos = DragScreenPath.Last.Value;
        Vector2 screenPos = eventData.pressEventCamera.WorldToScreenPoint(transform.position);
        DragScreenPath.Last.Value = screenPos;
        DragPath.Last.Value = transform.position;
        if (Vector2.Distance(lastScreenPathPos, screenPos) > PATH_NODE_DISTANCE) {
            DragScreenPath.AddLast(screenPos);
            DragPath.AddLast(transform.position);
        }

#if UNITY_EDITOR
        IEnumerator<Vector3> it = DragPath.GetEnumerator();
        it.MoveNext();
        Vector3 first = it.Current;
        Vector3 last;
        while (it.MoveNext()) {
            last = it.Current;
            Debug.DrawLine(first, last, Color.blue);
            first = last;
        }
#endif
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Drag");
        IsDragged = false;
        DragEndPos = transform.position;
        DragEndTime = Time.time;
        ResetDrag();
    }

    private void ResetDrag() {
        DragPath.Clear();
        DragScreenPath.Clear();
        DragStartPos = transform.position;
        DragEndPos = transform.position;
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 newPos = new Vector3(data.position.x, data.position.y, _dragDistanceToScreen);
        transform.position = data.pressEventCamera.ScreenToWorldPoint(newPos);
    }
}
