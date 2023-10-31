using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExampleEnemy : ExampleEntity
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent agent; // Reference to the NavMeshAgent

    [SerializeField] HealthMeter health;

    void Awake()
    {
        health = GetComponent<HealthMeter>();
        health.OnHPDepleted += DestroyEntity;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // Set the agent's destination to the player's position
        }
    }



}
