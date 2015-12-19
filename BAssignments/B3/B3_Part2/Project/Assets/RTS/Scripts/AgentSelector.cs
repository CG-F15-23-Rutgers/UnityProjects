using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentSelector : MonoBehaviour
{
    public GameObject selectedTarget;
    public GameObject selectedPlayer;

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < 100; i++)
                {
                    UnityEngine.Debug.Log("Pressed left key");
                }
            }
            if (hit.transform.tag == "selectable" && Input.GetMouseButtonDown(0))
            {
                selectedTarget = hit.transform.gameObject;
                for (int i = 0; i < 100; i++) {
                    UnityEngine.Debug.Log("Agent Selected is");
                }
            }
            /*if (hit.transform.tag == "Ground" && Input.GetMouseButtonDown(1))
            {
                foreach (NavMeshAgent agent in agentsList)
                {
                    agent.destination = hit.point;
                }
                agentsList.Clear();
            }*/
            if (hit.transform.tag == "selectable" && Input.GetMouseButtonDown(1))
            {
                selectedPlayer = hit.transform.gameObject;
                for (int i = 0; i < 100; i++)
                {
                    UnityEngine.Debug.Log("Player selected is" + selectedPlayer.gameObject.name);
                }
            }
        }
    }

}
