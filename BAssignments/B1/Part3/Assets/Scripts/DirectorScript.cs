using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectorScript : MonoBehaviour {
    NavMeshAgent[] agentsList;

	// Use this for initialization
	void Start () {
        agentsList = GetComponents<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            foreach(NavMeshAgent agent in agentsList)
            {
                if(agent.CompareTag("mazeagents"))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        agent.destination = hit.point;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (NavMeshAgent agent in agentsList)
            {
                if (agent.CompareTag("stairagents"))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        agent.destination = hit.point;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            foreach (NavMeshAgent agent in agentsList)
            {
                if (agent.CompareTag("jummpagents"))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        agent.destination = hit.point;
                    }
                }
            }
        }
    }
}
