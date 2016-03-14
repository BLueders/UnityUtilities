﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float PATH_NODE_DISTANCE = 10;
    private const float DRAG_FORCE_FACTOR = 200;
    private const int PHYSICS_CHECK_MEMORY_ALLOC = 10;
    private static readonly Dictionary<DroppablePhysics, GetHoveredContainer> GET_HOVERED_CONTAINER = null;

    private static Collider2D[] hits2D = new Collider2D[PHYSICS_CHECK_MEMORY_ALLOC];
    private static Collider[] hits3D = new Collider[PHYSICS_CHECK_MEMORY_ALLOC];
    private static RaycastHit[] RaycastHits = new RaycastHit[PHYSICS_CHECK_MEMORY_ALLOC];

    static Draggable(){
        GET_HOVERED_CONTAINER = new Dictionary<DroppablePhysics, GetHoveredContainer>();
        GET_HOVERED_CONTAINER.Add(DroppablePhysics.Overlap2D, GetHoveredContainerOverlap2D);
        GET_HOVERED_CONTAINER.Add(DroppablePhysics.Overlap3D, GetHoveredContainerOverlap3D);
        GET_HOVERED_CONTAINER.Add(DroppablePhysics.Point2D, GetHoveredContainerPosition2D);
        GET_HOVERED_CONTAINER.Add(DroppablePhysics.ScreenRay3D, GetHoveredContainerScreenRay3D);
    }

    public bool IsDragged { get; private set; }

    private DragInfo _dragInfo;
    private DropInfo _dropInfo;

    #region Draggable fields
    private float _lastOnDragCallTime;
    private Vector3 _lastFramePos;
    private float _dragDistanceToScreen;
    #endregion

    #region Draggable abstract functions
    protected abstract void HandleBeginDrag(DragInfo dragData);
    protected abstract void HandleDrag(DragInfo dragData);
    protected abstract void HandleEndDrag(DragInfo dragData);
    #endregion

    #region Droppable fields
    [SerializeField]
    private bool _isDroppable = true;
    [SerializeField]
    private float _collisionRadius = 5;
    [SerializeField]
    private LayerMask _dropContainerLayers = 1; // default layer
    [SerializeField]
    DroppablePhysics _overlapPhysics = DroppablePhysics.ScreenRay3D;

    private delegate DropContainer GetHoveredContainer(PointerEventData data, Draggable draggable);

    private bool _hovering = false;
    private DropContainer _currentHoveredContainer = null;

    #endregion

    #region Droppable abstract functions
    protected abstract void OnDrop(DragInfo dragData, DropInfo dropData);
    protected abstract void OnStartHovering(DragInfo dragData, DropInfo dropData);
    protected abstract void OnHovering(DragInfo dragData, DropInfo dropData);
    protected abstract void OnStopHovering(DragInfo dragData, DropInfo dropData);
    #endregion

    private void InitializeDrag(PointerEventData eventData)
    {
        _dragInfo = new DragInfo();
        _dropInfo = new DropInfo();
        _dropInfo.Draggable = this;
        _dragDistanceToScreen = eventData.pressEventCamera.WorldToScreenPoint(gameObject.transform.position).z;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InitializeDrag(eventData);
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
        HandleBeginDrag(_dragInfo);
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
        HandleDrag(_dragInfo);
        if(_isDroppable){
            UpdateDroppableHover(eventData);
#if UNITY_EDITOR
            Debug.DrawLine(transform.position + new Vector3(_collisionRadius, 0, 0), transform.position - new Vector3(_collisionRadius, 0, 0), Color.blue);
            Debug.DrawLine(transform.position + new Vector3(0, _collisionRadius, 0), transform.position - new Vector3(0, _collisionRadius, 0), Color.blue); 
            Debug.DrawLine(transform.position + new Vector3(0, 0, _collisionRadius), transform.position - new Vector3(0, 0, _collisionRadius), Color.blue); 
#endif
        }

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

    public void OnEndDrag(PointerEventData data)
    {
        //Debug.Log("End Drag");
        IsDragged = false;
        _dragInfo.DragEndPos = transform.position;
        _dragInfo.DragEndTime = Time.time;
        HandleEndDrag(_dragInfo);
        if(_isDroppable && _currentHoveredContainer != null){
            DropIntoContainer(_currentHoveredContainer);
            _currentHoveredContainer = null;
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 newPos = new Vector3(data.position.x, data.position.y, _dragDistanceToScreen);
        transform.position = data.pressEventCamera.ScreenToWorldPoint(newPos);
    }

    #region Droppable functions
    private void UpdateDroppableHover(PointerEventData data)
    {
        DropContainer oldHover = _currentHoveredContainer;
        _currentHoveredContainer = GET_HOVERED_CONTAINER[_overlapPhysics](data, this);
        if(_currentHoveredContainer != null){
            _hovering = true;
            _dropInfo.Container = _currentHoveredContainer;
        } else {
            _hovering = false;
        }
        SendHoverMessages(oldHover);
    }

    private void DropIntoContainer(DropContainer container)
    {
        OnStopHovering(_dragInfo, _dropInfo);
        container.OnStopHover(_dragInfo, _dropInfo);
        OnDrop(_dragInfo, _dropInfo);
        container.OnDrop(_dragInfo, _dropInfo);
        _hovering = false;
    }

    private void SendHoverMessages(DropContainer oldHover)
    {
        // if both are null nothing happes, if one if not null, we ether enter a new one, leave the current one or both
        if (_currentHoveredContainer != oldHover)
        {
            if (_currentHoveredContainer != null) // we enter a new hover
            {
                OnStartHovering(_dragInfo, _dropInfo);
                _currentHoveredContainer.OnStartHover(_dragInfo, _dropInfo);
            }
            else if (oldHover != null) // we leave the current hover
            {
                OnStopHovering(_dragInfo, _dropInfo);
                oldHover.OnStopHover(_dragInfo, _dropInfo);
            }
        }

        if (_currentHoveredContainer != null)
        {
            OnHovering(_dragInfo, _dropInfo);
            _currentHoveredContainer.OnHover(_dragInfo, _dropInfo);
            _hovering = true;
        }
    }

    private static DropContainer GetHoveredContainerPosition2D(PointerEventData data, Draggable draggable){
        int numHits = Physics2D.OverlapPointNonAlloc(draggable.transform.position, hits2D, draggable._dropContainerLayers);
        DropContainer nowHoveredContainer = null;
        float closestDist;
        DropContainer container;
        for(int i = 0; i < numHits; i++)
        {
            closestDist = float.PositiveInfinity;
            if ((container = hits2D[i].gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector2.Distance(container.transform.position, draggable.transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }

    private static DropContainer GetHoveredContainerScreenRay3D(PointerEventData data, Draggable draggable){
        Ray ray = data.pressEventCamera.ScreenPointToRay(draggable.transform.position);
        int numHits = Physics.RaycastNonAlloc(ray, RaycastHits, draggable._collisionRadius, draggable._dropContainerLayers);
        DropContainer nowHoveredContainer = null;
        float closestDist;
        DropContainer container;
        for(int i= 0; i < numHits; i++)
        {
            closestDist = float.PositiveInfinity;
            if ((container = RaycastHits[i].collider.gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector3.Distance(container.transform.position, draggable.transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }

    private static DropContainer GetHoveredContainerOverlap2D(PointerEventData data, Draggable draggable)
    {
        int numHits = Physics2D.OverlapCircleNonAlloc(draggable.transform.position, draggable._collisionRadius, hits2D, draggable._dropContainerLayers);
        DropContainer nowHoveredContainer = null;
        float closestDist;
        DropContainer container;
        for(int i = 0; i < numHits; i++)
        {
            closestDist = float.PositiveInfinity;
            if ((container = hits2D[i].gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector2.Distance(container.transform.position, draggable.transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }

    private static DropContainer GetHoveredContainerOverlap3D(PointerEventData data, Draggable draggable)
    {
        int numHits = Physics.OverlapSphereNonAlloc(draggable.transform.position, draggable._collisionRadius, hits3D, draggable._dropContainerLayers);
        DropContainer nowHoveredContainer = null;
        float closestDist;
        DropContainer container;
        for(int i = 0; i < numHits; i++)
        {
            closestDist = float.PositiveInfinity;
            if ((container = hits3D[i].gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector3.Distance(container.transform.position, draggable.transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }
    #endregion

    protected enum DroppablePhysics{
        Overlap2D, Overlap3D, Point2D, ScreenRay3D
    }

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

    public class DropInfo{

        internal DropInfo(){
        }

        public DropContainer Container { get; internal set; }
        public Draggable Draggable { get; internal set; }
    }
}