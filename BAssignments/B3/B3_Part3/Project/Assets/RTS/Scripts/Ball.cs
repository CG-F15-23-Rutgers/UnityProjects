using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Ball : MonoBehaviour {
	Rigidbody rb;

    void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void OnInteractionStart(Transform t)
	{
		rb.isKinematic = true;
		rb.useGravity = false;
	}

    void Update () {
    }

    public RunStatus throwingBall(Val<Vector3> direction)
    {
        transform.parent = null;
        rb.velocity = direction.Value * 2f;
        rb.isKinematic = false;
        rb.useGravity = true;
        return RunStatus.Success;
    }


    public Node nodeThrow(Val<Vector3> direction)
	{
		return new LeafInvoke(() => throwingBall(direction));
	}
}
