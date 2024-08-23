using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : Entity
{
    NavMeshAgent agent;
    [SerializeField] private Transform finish;
    public float speed; 

    public Vector3 Direction
    {
        get
        {
            return !agent ? Vector3.zero : agent.desiredVelocity.normalized;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Direction * 10);
    }
    new private void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        finish = GameObject.FindGameObjectWithTag("Finish").transform;
        if (agent)
        {
            agent.speed = 0.0001f;
            agent.acceleration = speed;
            agent.angularSpeed = speed;
        }
    }

    private void Update()
    {
        base.Update();
        if(agent.isActiveAndEnabled)
            agent.SetDestination(finish.position);
    }
    private void FixedUpdate()
    {
        agent.Move(agent.desiredVelocity.normalized * speed * Time.deltaTime);
    }
}
