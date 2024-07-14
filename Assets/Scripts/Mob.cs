using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : Entity
{
    NavMeshAgent agent;
    [SerializeField] private GameObject kuda;
    public float speed;
    private void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        agent.speed = speed;
        agent.acceleration = speed;
        agent.angularSpeed = speed;
    }

    private void Update()
    {
        base.Update();
        agent.SetDestination(kuda.transform.position);
    }
}
