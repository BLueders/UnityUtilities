using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class AStarAgent2D : MovementAgent2D {

    public Movement movement;
    LinkedList<GridNode2D> path = new LinkedList<GridNode2D>();
    Rigidbody2D myRigidbody;
    public GameObject DebugTarget;

    // if target is unreachable, this returns false
    public override bool SetTarget(Vector3 target) {
        if (this.target != target) {
            this.target = target;
            // Do not use A star if super close
            if (Vector2.Distance(target, transform.position) < AStarGrid2D.CellSize * 2) {
                path.Clear();
                return true;
            }
            return recalculatePath();
        }
        return true;
    }
    
    bool recalculatePath() {
        LinkedList<GridNode2D> newPath = AStarGrid2D.GetPath(transform.position, target);
        if (newPath != null) {
            path = newPath;
            // first node is in 95% obsolete
            path.RemoveFirst();
            return true;
        } else {
            Debug.Log("AStarAgent2D: Can not reach target, path not available");
            path.Clear();
            return false;
        }
    }
    
    void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
    
        if (DebugTarget != null) {
            SetTarget(DebugTarget.transform.position);
        }
    
        Vector2 velocity = myRigidbody.velocity;
        
        if (canMove && target != Vector3.zero) {
            if (path.Count != 0) {
                velocity = MoveByPath(velocity);
            } else {
                velocity = MoveByTarget(velocity);
            }
        } else {
            velocity = Vector2.zero;
            Speed = 0;
        }
        myRigidbody.velocity = velocity;
        if (Speed > 0) {
            UpdateFacing(velocity);
        }
    }
    
    Vector2 MoveByPath(Vector2 velocity) {
        Vector2 currentPosition = transform.position;
        GridNode2D currentGridNode = path.First.Value;
        Vector2 currentTarget = new Vector2(currentGridNode.X, currentGridNode.Y);
        if ((currentTarget - currentPosition).sqrMagnitude < targetEpsilon) {
            path.RemoveFirst();
        }
        float maxSpeed = movement.walkSpeed;
        if (movement.running) {
            maxSpeed = movement.runSpeed;
        }
        Speed += movement.acceleration * Time.deltaTime;
        Speed = Mathf.Clamp(Speed, 0, maxSpeed);
        velocity = (currentTarget - currentPosition).normalized * Speed;
        return velocity;
    }
    
    Vector2 MoveByTarget(Vector2 velocity) {
        Vector2 currentPosition = transform.position;
        Vector2 currentTarget = target;
        float maxSpeed = movement.walkSpeed;
        if (movement.running) {
            maxSpeed = movement.runSpeed;
        }
        Speed += movement.acceleration * Time.deltaTime;
        Speed = Mathf.Clamp(Speed, 0, maxSpeed);
        float sqrDist = (currentTarget - currentPosition).sqrMagnitude;
        //reached target
        if (sqrDist < targetEpsilon) {
            Speed = 0;
            transform.position = target;
            target = Vector3.zero;
        }
        velocity = (currentTarget - currentPosition).normalized * Speed;
        return velocity;
    }
    
    void OnDrawGizmos() {
        if (!canMove) {
            return;
        }
        Vector3 start = transform.position;
        Vector3 end = transform.position;
        foreach (GridNode2D node in path) {
            end.x = node.X;
            end.y = node.Y;
            Gizmos.DrawLine(start, end);
            start.x = node.X;
            start.y = node.Y;
        }
    }
    
    [System.Serializable]
    public class Movement {
        public float runSpeed;
        public float walkSpeed;
        public float acceleration;
        public float rotationSpeed;
        internal bool running;
    }
}

