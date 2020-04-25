using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patroller : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 waypoint;
    private Vector3 startPoint;
    private bool headingBack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        startPoint = this.transform.position;
        headingBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.pathPending)
        {
            return;
        }
        if(this.transform.position.x < waypoint.x && this.transform.position.z > waypoint.z)
        {
            headingBack = true;
        } else if(this.transform.position.x < startPoint.x && this.transform.position.z < startPoint.z)
        {
            headingBack = false;
        }

        if(headingBack)
        {
            agent.destination = startPoint;
        } else
        {
            agent.destination = waypoint;
        }
    }
}
