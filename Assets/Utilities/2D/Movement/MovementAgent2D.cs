using UnityEngine;
using System.Collections;

public class MovementAgent2D : MovementAgent {

    public enum LookDir {
        DOWN = 0,
        LEFT = 1,
        UP = 2,
        RIGHT = 3
    }

    public LookDir Facing{ get; protected set; }

    public override void LookAt(Vector3 target) {
        
        UpdateFacing(target - transform.position);
    }

    protected virtual void UpdateFacing(Vector3 lookDir) {
        if (Mathf.Abs(lookDir.y) > Mathf.Abs(lookDir.x)) {
            if (lookDir.y < 0) {
                Facing = MovementAgent2D.LookDir.DOWN;
                return;
            }
            Facing = MovementAgent2D.LookDir.UP;
            return;
        } else if (lookDir.x < 0) {
            Facing = MovementAgent2D.LookDir.LEFT;
            return;
        }
        Facing = MovementAgent2D.LookDir.RIGHT;
    }
}
