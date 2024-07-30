using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public DetectionZone biteDetectionzone;
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public List<Transform> waypoints;
    Damageable damageable;
    Transform nextWaypoint;

    int waypointNum = 0;

    Animator animator;
    Rigidbody2D rb;
    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            if (animator != null)
            {
                animator.SetBool(AnimationStrings.hasTarget, value); // Ensure AnimationStrings is defined correctly
            }
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        if (waypoints.Count > 0) // Ensure there are waypoints defined
        {
            nextWaypoint = waypoints[waypointNum];
        }
    }

    public bool CanMove
    {
        get
        {
            if (animator != null)
            {
                return animator.GetBool(AnimationStrings.canMove); // Ensure AnimationStrings is defined correctly
            }
            return false;
        }
    }

    void Update()
    {
        if (biteDetectionzone != null) // Add null check
        {
            HasTarget = biteDetectionzone.detectedColliders.Count > 0;
        }
        else
        {
            Debug.LogWarning("biteDetectionzone is not assigned.");
        }
    }

    private void FixedUpdate()
    {
        if (damageable != null && damageable.IsAlive) // Add null check
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        if (nextWaypoint == null) return; // Check if nextWaypoint is defined

        // fly to the next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized; // Correctly calculate the direction

        // Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        // see if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            //Switch to the next waypoint 
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                // loop back to the original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;

        if (rb.velocity.x > 0)
        {
            // Moving right
            if (localScale.x < 0)
            {
                // Flip to the right
                transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
        }
        else if (rb.velocity.x < 0)
        {
            // Moving left
            if (localScale.x > 0)
            {
                // Flip to the left
                transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
        }
    }
}
