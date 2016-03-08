using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public abstract class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float PATH_NODE_DISTANCE = 10;
    private const float DRAG_FORCE_FACTOR = 200;

    public delegate void DragHandler(DragInfo data);
    public event DragHandler BeginDragEvent;
    public event DragHandler DragEvent;
    public event DragHandler EndDragEvent;

    public bool IsDragged { get; private set; }

    public class DragInfo
    {
        internal DragInfo()
        {
            DragPath = new LinkedList<Vector3>();
            DragScreenPath = new LinkedList<Vector2>();
        }

        public Vector3 DragStartPos { get; internal set; }
        public Vector3 DragEndPos { get; internal set; }
        public Vector3 DragPotentialForce { get { return (DragStartPos - DragEndPos) * DRAG_FORCE_FACTOR; } }
        public Vector3 DragVelocity { get; internal set; }

        public float DragTime { get { return DragEndTime - DragStartTime; } }
        public float DragStartTime { get; internal set; }
        public float DragEndTime { get; internal set; }
        public LinkedList<Vector3> DragPath { get; internal set; }
        public float DragDistance { get; internal set; }

        public LinkedList<Vector2> DragScreenPath { get; internal set; }
        public Vector2 DragStartScreenPos { get; internal set; }
        public Vector2 DragEndScreenPos { get; internal set; }
        public Vector2 DragTotalScreenDistance { get; internal set; }
        public Vector2 DragFrameScreenDistance { get; internal set; }

        internal void Reset()
        {
        }
    }

    private DragInfo _dragInfo;
    private float _lastOnDragCallTime;
    private Vector3 _lastFramePos;
    private float _dragDistanceToScreen;

    private void Initialize(PointerEventData eventData)
    {
        _dragInfo = new DragInfo();
        _dragDistanceToScreen = eventData.pressEventCamera.WorldToScreenPoint(gameObject.transform.position).z;
    }

    protected virtual void Awake()
    {

    }

    protected virtual void OnDestroy()
    {
        BeginDragEvent = null;
        DragEvent = null;
        EndDragEvent = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Initialize(eventData);
        //Debug.Log("Begin Drag");
        IsDragged = true;
        _dragInfo.DragStartPos = transform.position;
        _dragInfo.DragEndPos = transform.position;
        _dragInfo.DragStartTime = Time.time;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        _dragInfo.DragScreenPath.AddLast(screenPos);
        _dragInfo.DragScreenPath.AddLast(screenPos);

        _dragInfo.DragPath.AddLast(transform.position);
        _dragInfo.DragPath.AddLast(transform.position);
        if (BeginDragEvent != null) { BeginDragEvent(_dragInfo); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag");
        IsDragged = true;
        SetDraggedPosition(eventData);

        _dragInfo.DragEndPos = transform.position;
        Vector3 dragMoveDelta = _lastFramePos - transform.position;
        float dragDeltaTime = (Time.time - _lastOnDragCallTime);
        _dragInfo.DragDistance += dragMoveDelta.magnitude;
        _dragInfo.DragVelocity = Vector3.Lerp(dragMoveDelta * (1 / dragDeltaTime), _dragInfo.DragVelocity, 0.5f);
        _lastFramePos = transform.position;
        _lastOnDragCallTime = Time.time;

        Vector2 lastScreenPathPos = _dragInfo.DragScreenPath.Last.Value;
        Vector2 screenPos = eventData.pressEventCamera.WorldToScreenPoint(transform.position);
        _dragInfo.DragScreenPath.Last.Value = screenPos;
        _dragInfo.DragPath.Last.Value = transform.position;
        if (Vector2.Distance(lastScreenPathPos, screenPos) > PATH_NODE_DISTANCE)
        {
            _dragInfo.DragScreenPath.AddLast(screenPos);
            _dragInfo.DragPath.AddLast(transform.position);
        }
        if (DragEvent != null) { DragEvent(_dragInfo); }

#if UNITY_EDITOR
        IEnumerator<Vector3> it = _dragInfo.DragPath.GetEnumerator();
        it.MoveNext();
        Vector3 first = it.Current;
        Vector3 last;
        while (it.MoveNext())
        {
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
        _dragInfo.DragEndPos = transform.position;
        _dragInfo.DragEndTime = Time.time;
        if (EndDragEvent != null) { EndDragEvent(_dragInfo); }

    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 newPos = new Vector3(data.position.x, data.position.y, _dragDistanceToScreen);
        transform.position = data.pressEventCamera.ScreenToWorldPoint(newPos);
    }
}
