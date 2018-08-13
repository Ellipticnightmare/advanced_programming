using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Include Artificial Intelligence part of API
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    // Property
    public int Health
    {
        get
        {
            return health;
        }
    }

    public NavMeshAgent agent;
    public Transform target;
    public Transform waypointParent;
    public float distanceToWaypoint = 1f;
    public float detectionRadius = 5f;

    private int health = 100;
    private int currentIndex = 1;
    private Transform[] waypoints;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);    
    }

    void Start()
    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // Update the AI's target position
            agent.SetDestination(target.position);
        }
        else
        {
            if(currentIndex >= waypoints.Length)
            {
                currentIndex = 1;
            }

            Transform point = waypoints[currentIndex];

            float distance = Vector3.Distance(transform.position, point.position);
            if(distance <= distanceToWaypoint)
            {
                currentIndex++;
            }

            agent.SetDestination(point.position);
        }
    }

    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player)
            {
                target = player.transform;
                return;
            }
        }

        target = null;
    }

    public void DealDamage(int damageDealt)
    {
        health -= damageDealt;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
