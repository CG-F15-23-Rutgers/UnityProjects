using UnityEngine;
using System.Collections;

public class DemoScene : MonoBehaviour {
	
	public GUISkin skin;
	public GameObject siloTemplate = null;
	
	string[] displayText = {"Interactive Tutorial... Initiated. Please select a unit.",
							"Build an outpost. Drag and select some units, then click the outpost option, and place it somewhere visible.",
							"Construct a house. This will allow you to create more units once it is built.",
							"Create more workers by clicking the worker button. Make 5 more workers now.",
							"Now create a storehouse for the units to place harvested resources.",
							"",
							""};
	int state = 0;
	public UnitSelection select;
	
	void OnGUI () {
		GUI.skin = skin;
		GUI.Box(new Rect(Screen.width-300, 0, 300, 100), displayText[state]);
	}
	void FixedUpdate () {
		if(state == 0){
			if(select.curSelectedLength > 0){
				state = 1;
			}
		}
		else if(state == 1){
			if(GameObject.Find("Outpost") != null){
				state = 2;
			}
		}
		else if(state == 2){
			if(GameObject.Find("House") != null){
				state = 3;
			}
		}
		else if(state == 3){
			GameObject[] gameTag;
			gameTag = GameObject.FindGameObjectsWithTag("Unit");
			int amount = 0;
			for(int x = 0; x < gameTag.Length; x++){
				if(gameTag[x].name == "Worker Unit"){
					amount++;
				}
			}
			if(amount >= 10){
				state = 4;
			}
		}
		else if(state == 4){
			
		}
	}
}
