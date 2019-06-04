using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NPCMove : MonoBehaviour
{
    private float speed;
    private NavMeshAgent agent;

    private void Awake()
    {
        speed = 5f;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Move(Vector3 direction)
    {
        agent.speed = speed;
        agent.SetDestination(direction);
    }
    public void Move(Vector3 direction, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(direction);
    }
}
