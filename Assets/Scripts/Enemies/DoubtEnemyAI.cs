using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DoubtEnemyAI : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private Player player;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform sprite;
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int willReward = 2;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private float pathUpdateRate;

    private int health;
    private Path path;
    private int currentWaypoint;
    private Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        health = maxHealth;
        currentWaypoint = 0;
        //reachedEndOfPath = false;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, pathUpdateRate);
    }

    private void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            anim.SetBool("IsWalking", true);

            float FacingRightPDot = Vector3.Dot(rb.velocity, Vector3.right);
            if (FacingRightPDot > 0)
            {
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if (FacingRightPDot < 0)
            {
                sprite.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
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

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
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

    public void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            player.IncreaseWillPower(willReward);
            Destroy(gameObject);
        }
    }
}
