using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform target;

    public NavMeshAgent agent;

    private void Start()
    {
        agent.updateUpAxis = false;

        agent.SetDestination(target.position);
    }
}
