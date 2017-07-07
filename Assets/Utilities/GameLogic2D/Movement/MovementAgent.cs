using UnityEngine;
using System.Collections;

public class MovementAgent : MonoBehaviour {

    protected Vector3 target;
    protected bool canMove = true;
    protected float targetEpsilon = 0.1f;
    public float Speed{get; protected set;}
    public bool CanMove{get{return canMove;}}
    
    // if target is unreachable, this returns false
    public virtual bool SetTarget(Vector3 target) {
        this.target = target;    
        return true;
    }

    public virtual void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    public virtual void LookAt(Vector3 target) {
        
    }
    
    public virtual bool HasTarget {
        get{
            if(target != Vector3.zero) {
                return (target - transform.position).sqrMagnitude > targetEpsilon;
            } else {
                return false;
            }
        }
    }
}

