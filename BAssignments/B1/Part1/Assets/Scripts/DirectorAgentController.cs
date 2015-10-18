using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectorAgentController : MonoBehaviour
{
    List<NavMeshAgent> agentsList;

    // Use this for initialization
    void Start()
    {
        agentsList = new List<NavMeshAgent>();
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

}
