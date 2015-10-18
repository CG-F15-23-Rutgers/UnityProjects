using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[System.NonSerialized]
	
	public float animSpeed = 1.5f;              // animation speed
	public bool curve;                          // curves to fix collider during jump
	
	private Animator refPlayerAnim;                         // reference player animator
	private AnimatorStateInfo refAnimState;         // reference animator state
	private CapsuleCollider refCollider;                    // reference collider
	
	static int checkMovement = Animator.StringToHash("Base Layer.Movement");
	static int checkJump = Animator.StringToHash("Base Layer.Jump");
	
	void Start()
	{
		// references
		refPlayerAnim = GetComponent<Animator>();
		refCollider = GetComponent<CapsuleCollider>();
		if (refPlayerAnim.layerCount == 2)
			refPlayerAnim.SetLayerWeight(1, 1);
	}
	
	void FixedUpdate()
	{
		float horiz = Input.GetAxis("Horizontal");              // horizontal input axis
		float vert = Input.GetAxis("Vertical");             // vertical input axis
		refPlayerAnim.SetFloat("Speed", vert);
		refPlayerAnim.SetFloat("Direction", horiz);
		refPlayerAnim.speed = animSpeed;                                // animation speed variable
		refAnimState = refPlayerAnim.GetCurrentAnimatorStateInfo(0); // set our currentState variable to the current state of the Base Layer (0) of animation
		
		// if shift, run
		if (refAnimState.fullPathHash == checkMovement)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				refPlayerAnim.SetFloat("Run", 1);
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			refPlayerAnim.SetFloat("Run", 0);
		}
		
		// if moving, allow jump
		if (refAnimState.fullPathHash == checkMovement)
		{
			if (Input.GetButtonDown("Jump"))
			{
				refPlayerAnim.SetBool("Jump", true);
			}
		}
		else if (refAnimState.fullPathHash == checkJump)
		{
			if (!refPlayerAnim.IsInTransition(0))
			{
				if (curve)
					// fix collider
					refCollider.height = refPlayerAnim.GetFloat("ColliderHeight");
				
				// no longer jumping
				refPlayerAnim.SetBool("Jump", false);
			}
			
			// raycast from center of player
			Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
			RaycastHit results = new RaycastHit();
			if (Physics.Raycast(ray, out results))
			{
				// when distance greater than 1.75
				if (results.distance > 1.75f)
				{
					// match target
					refPlayerAnim.MatchTarget(results.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
				}
			}
		}
		
	}
}