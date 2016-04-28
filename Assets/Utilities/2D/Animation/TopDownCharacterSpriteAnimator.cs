using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MovementAgent2D))]
public class TopDownCharacterSpriteAnimator : MonoBehaviour {

	[SerializeField] Sprite[] walkCycleDown;
	[SerializeField] Sprite[] walkCycleUp;
	[SerializeField] Sprite[] walkCycleLeft;
	[SerializeField] Sprite[] walkCycleRight;

	[SerializeField] Sprite[] idleDown;
	[SerializeField] Sprite[] idleUp;
	[SerializeField] Sprite[] idleLeft;
	[SerializeField] Sprite[] idleRight;
	[SerializeField] float animationSpeed = 5;

	[SerializeField] bool useMovementAgentSpeed;

    MovementAgent2D movementAgent;

	private SpriteRenderer spriteRenderer;

    private bool isWalking;
	private float timer;

    void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer == null) {
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}
		if (spriteRenderer == null) {
			Debug.LogError ("No SpriteRenderer found on animated object: " + gameObject.name + " or its children");
		}
		movementAgent = GetComponent<MovementAgent2D> ();
    }

    void Update() {

		float deltaTimeAdjust = Time.deltaTime;
		if (useMovementAgentSpeed) {
			deltaTimeAdjust *= movementAgent.Speed;
		}
		timer += deltaTimeAdjust * animationSpeed;
		timer %= 1;
		isWalking = movementAgent.Speed > .1f;

        if (isWalking) {
			switch (movementAgent.Facing) {
			case MovementAgent2D.LookDir.DOWN:
				spriteRenderer.sprite = walkCycleDown [(int)(timer * walkCycleDown.Length)];
				break;	
			case MovementAgent2D.LookDir.UP:
				spriteRenderer.sprite = walkCycleUp [(int)(timer * walkCycleUp.Length)];
				break;	
			case MovementAgent2D.LookDir.LEFT:
				spriteRenderer.sprite = walkCycleLeft [(int)(timer * walkCycleLeft.Length)];
				break;
			case MovementAgent2D.LookDir.RIGHT:
				spriteRenderer.sprite = walkCycleRight [(int)(timer * walkCycleRight.Length)];
				break;
			}
        } else {
			switch (movementAgent.Facing) {
			case MovementAgent2D.LookDir.DOWN:
				spriteRenderer.sprite = idleDown [(int)(timer * idleDown.Length)];
				break;	
			case MovementAgent2D.LookDir.UP:
				spriteRenderer.sprite = idleUp [(int)(timer * idleUp.Length)];
				break;	
			case MovementAgent2D.LookDir.LEFT:
				spriteRenderer.sprite = idleLeft [(int)(timer * idleLeft.Length)];
				break;
			case MovementAgent2D.LookDir.RIGHT:
				spriteRenderer.sprite = idleRight [(int)(timer * idleRight.Length)];
				break;
			}
        }
    }
}
