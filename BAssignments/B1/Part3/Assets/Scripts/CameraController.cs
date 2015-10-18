using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		if (Input.GetKeyDown(KeyCode.W))
		{
			position.y = position.y + 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			position.y = position.y - 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			position.x = position.x + 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			position.x = position.x - 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			position.z = position.z - 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			position.z = position.z + 10;
			transform.position = new Vector3(position.x,position.y,position.z);
		}
	}
}
