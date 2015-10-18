using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class DirectorAgentController : MonoBehaviour
{
    List<NavMeshAgent> agentsList;

	[System.NonSerialized]
	
	public float animSpeed = 1f;              // animation speed
	public bool curve;                          // curves to fix collider during jump
	
	private Animator refPlayerAnim;                         // reference player animator
	private AnimatorStateInfo refAnimState;         // reference animator state
	private CapsuleCollider refCollider;                    // reference collider

	static int checkMovement = Animator.StringToHash("Base Layer.Movement");
	static int checkJump = Animator.StringToHash("Base Layer.Jump");

	public GameObject agentObject;
	public GameObject speedSetting;

	private bool runFlag;
	
    // Use this for initialization
    void Start()
    {
		runFlag = false;
		refPlayerAnim = agentObject.GetComponent<Animator>();
		refCollider = agentObject.GetComponent<CapsuleCollider>();
		if (refPlayerAnim.layerCount == 2) {
			refPlayerAnim.SetLayerWeight (1, 1);
		}

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
				if (Input.GetKey(KeyCode.T)){

					runFlag = true;
				}
				else{
					runFlag = false;
				}
            }
        }
    }

	void FixedUpdate()
	{
		NavMeshAgent agentMesh = agentObject.GetComponent<NavMeshAgent>();

		if (agentMesh.hasPath && agentMesh.remainingDistance >= speedSetting.GetComponent<Slider>().value * 0.5) {
			refPlayerAnim.SetFloat("Speed", 1);
			//speedSetting.GetComponent<Slider>().value + 0.5f;
			//agentMesh.velocity = refPlayerAnim.deltaPosition / Time.deltaTime;
			if (runFlag){
				refPlayerAnim.SetFloat("Run", 1);
			}
			else{
				refPlayerAnim.SetFloat("Run", 0);
			}

			refPlayerAnim.speed = speedSetting.GetComponent<Slider>().value;
			//transform.rotation = Quaternion.LookRotation(agentMesh.desiredVelocity);

			if (agentMesh.isOnOffMeshLink){
				refPlayerAnim.SetBool("Jump", true);
			}else{
				refPlayerAnim.SetBool("Jump", false);
			}

			refAnimState = refPlayerAnim.GetCurrentAnimatorStateInfo(0);
			//refPlayerAnim.SetFloat("Run", 0);
		} else {
			refPlayerAnim.SetFloat("Speed", 0);
			refPlayerAnim.speed = animSpeed;
			refAnimState = refPlayerAnim.GetCurrentAnimatorStateInfo(0);
			//refPlayerAnim.SetFloat("Run", 1);
		}
	}

}
