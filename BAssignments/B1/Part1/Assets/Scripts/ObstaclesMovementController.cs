using UnityEngine;
using System.Collections;

public class ObstaclesMovementController : MonoBehaviour {
    Rigidbody obstacle;
    public float obstacleSpeed;

	// Use this for initialization
	void Start () {
        //obstacle = null;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Container" && Input.GetMouseButtonDown(0))
            {
                obstacle = (hit.transform.gameObject).GetComponent<Rigidbody>();
            }
        }
        if (obstacle != null)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 obstacleMovement = new Vector3(moveHorizontal, 0, moveVertical);
            obstacle.AddForce(obstacleMovement * obstacleSpeed);
        }
    }
}
