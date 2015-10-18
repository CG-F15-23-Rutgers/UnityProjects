using UnityEngine;
using System.Collections;

public class DevilController : MonoBehaviour {
	
	private Transform pos;
	private int direction;

	void Start(){
		Transform pos;
		pos = GetComponent<Transform>();
		float zPos = pos.position.z;
		if (Mathf.Abs(4 - zPos) < Mathf.Abs(-8 - zPos)) {
			direction = -1;
		} else {
			direction = 1;
		}
	}

	void Update(){
		Transform pos;
		pos = GetComponent<Transform>();
		float xPos = pos.position.x;
		float yPos = pos.position.y;
		float zPos = pos.position.z;
		if (zPos < -8){
			direction = 1;
		}else if (zPos >= 4){
			direction = -1;
		}
		zPos = zPos + (direction * Time.deltaTime);

		transform.position = new Vector3(xPos,yPos,zPos);
		transform.Rotate(new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}