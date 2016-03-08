using UnityEngine;
using System.Collections;

public abstract class DragAndDropable : Dragable
{

    [SerializeField]
    private float _collisionRadius = 5;

    private bool _initialized = false;
    private bool _hovering = false;
    private DropContainer _hoveredDropContainer = null;

    protected abstract bool Use2DPhysics { get; }

    protected override void Awake()
    {
        base.Awake();

        if (_initialized) // prevent double subsription to events
            return;
        BeginDragEvent += HandleBeginDrag;
        DragEvent += HandleDrag;
        EndDragEvent += HandleEndDrag;
        _hoveredDropContainer = null;

        _initialized = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void HandleBeginDrag(DragInfo data)
    {

    }

    private void HandleDrag(DragInfo data)
    {
        _hovering = false;
        DropContainer currentHover = GetHoveredContainer3D();
        if (Use2DPhysics)
        {
            currentHover = GetHoveredContainer2D();
        }
        else // 3D Physics
        {
            currentHover = GetHoveredContainer3D();
        }
        SendHoverMessages(currentHover);

    }

    private void HandleEndDrag(DragInfo data)
    {
        if (_hoveredDropContainer != null)
        {
            _hoveredDropContainer.OnDrop(this);
            OnDrop(_hoveredDropContainer);
            OnEndHovering(_hoveredDropContainer);
        }
    }

    protected abstract void OnDrop(DropContainer container);
    protected abstract void OnStartHovering(DropContainer container);
    protected abstract void OnHovering(DropContainer container);
    protected abstract void OnEndHovering(DropContainer container);

    private DropContainer GetHoveredContainer2D()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _collisionRadius);
        DropContainer nowHoveredContainer = null;
        foreach (Collider2D hit in hits)
        {
            DropContainer container;
            float closestDist = float.PositiveInfinity;
            if ((container = hit.gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector2.Distance(container.transform.position, transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }

    private DropContainer GetHoveredContainer3D()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _collisionRadius);
        DropContainer nowHoveredContainer = null;
        foreach (Collider hit in hits)
        {
            DropContainer container;
            float closestDist = float.PositiveInfinity;
            if ((container = hit.gameObject.GetComponent<DropContainer>()) != null)
            {
                float containerDist = Vector3.Distance(container.transform.position, transform.position);
                if (containerDist < closestDist)
                {
                    nowHoveredContainer = container;
                }
            }
        }
        return nowHoveredContainer;
    }

    private void SendHoverMessages(DropContainer currentHover)
    {
        // if both are null nothing happes, if one if not null, we ether enter a new one, leave the current one or both
        if (currentHover != _hoveredDropContainer)
        {
            if (currentHover != null) // we leave the current hover
            {
                currentHover.OnEnterHover(this);
            }
            if (_hoveredDropContainer != null) // we enter a new hover
            {
                _hoveredDropContainer.OnExitHover(this);
                _hoveredDropContainer = currentHover;
            }
        }

        if (_hoveredDropContainer != null)
        {
            _hoveredDropContainer.OnHover(this);
            _hovering = true;
        }
    }

}
