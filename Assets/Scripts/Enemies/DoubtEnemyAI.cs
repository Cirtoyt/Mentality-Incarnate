using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DoubtEnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float nextWaypointDistance;
    public float pathUpdateRate;

    Path path;
    int currentWaypoint;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        currentWaypoint = 0;
        //reachedEndOfPath = false;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, pathUpdateRate);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.fixedDeltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if ( distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
