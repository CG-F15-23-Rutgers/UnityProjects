using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class DirectorAgentController : MonoBehaviour
{
    List<NavMeshAgent> agentsList;

	[System.NonSerialized]
	
	public float animSpeed = 1.5f;              // animation speed
	public bool curve;                          // curves to fix collider during jump
	
	private Animator refPlayerAnim;                         // reference player animator
	private AnimatorStateInfo refAnimState;         // reference animator state
	private CapsuleCollider refCollider;                    // reference collider

	static int checkMovement = Animator.StringToHash("Base Layer.Movement");
	static int checkJump = Animator.StringToHash("Base Layer.Jump");

	public GameObject agentObject;

    // Use this for initialization
    void Start()
    {
        agentsList = new List<NavMeshAgent>();
		// references
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Agent" && Input.GetMouseButtonDown(0))
            {
                NavMeshAgent tempAgent = (hit.transform.gameObject).GetComponent<NavMeshAgent>();
                if (!agentsList.Contains(tempAgent))
                {
                    agentsList.Add(tempAgent);
                } 
            }
            if (hit.transform.tag == "Ground" && Input.GetMouseButtonDown(1))
            {
                foreach (NavMeshAgent agent in agentsList)
                {
                    agent.destination = hit.point;
                }
                agentsList.Clear();
            }
        }
    }

	void FixedUpdate()
	{
		refPlayerAnim = agentObject.GetComponent<Animator>();
		refCollider = agentObject.GetComponent<CapsuleCollider>();
		if (refPlayerAnim.layerCount == 2) {
			refPlayerAnim.SetLayerWeight (1, 1);
		}
		//float horiz = Input.GetAxis("Horizontal");              // horizontal input axis
		//float vert = Input.GetAxis("Vertical");             // vertical input axis
		//refPlayerAnim.SetFloat("Speed", vert);
		//refPlayerAnim.SetFloat("Direction", horiz);
		refPlayerAnim.speed = animSpeed;                                // animation speed variable
		refAnimState = refPlayerAnim.GetCurrentAnimatorStateInfo(0); // set our currentState variable to the current state of the Base Layer (0) of animation

		NavMeshAgent agentMesh = agentObject.GetComponent<NavMeshAgent>();
		Debug.Log (agentMesh.hasPath);


		if (agentMesh.hasPath) {
			refPlayerAnim.SetFloat("Run", 0);
		} else {
			refPlayerAnim.SetFloat("Run", 1);
		}

	}

}
