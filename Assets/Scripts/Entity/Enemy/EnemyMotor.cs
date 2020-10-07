using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : EntityMotor
{
    Vector2[] path;

    int targetIndex;
    public float repathWaitTime;
    float repathCounter;

    public float wanderRadius;
    public float wanderMinTime;
    public float wanderMaxTime;
    float wanderTime;
    float wanderCounter;
    Coroutine pathRoutine = null;

    PlayerController player;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        repathCounter = 0;
        player = PlayerController.instance;
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Move()
    {
        if (((EnemyStats)stats).active)
        {
            if (((EnemyStats)stats).playerInSight)
            {
                FollowPlayer();
            }
            else
            {
                Wander();
            }
        }
        Animate();
    }

    protected override void Animate()
    {
        animator.SetFloat("Velocity", rb.velocity.magnitude);
        if (rb.velocity.x > 0.05)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.velocity.x < -0.05)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccess)
    {
        try{
            if (pathSuccess && newPath.Length > 0)
            {
                path = newPath;
                if (pathRoutine != null) {
                    StopCoroutine(pathRoutine);
                }
                pathRoutine = StartCoroutine(FollowPath());
            }
            else
            {
                if (pathRoutine != null)
                {
                    StopCoroutine(pathRoutine);
                }
            } 
        }
        catch
        {
            Debug.LogError("Enemy path returned after enemy was destroyed");
        }
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];
        targetIndex = 0;

        while (true)
        {
            Vector2 tempLoc = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(tempLoc ,currentWaypoint) < 0.1)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            body.AddForce((new Vector2(currentWaypoint.x, currentWaypoint.y) - new Vector2(transform.position.x, transform.position.y)).normalized * speed * 5);

            yield return null;
        }
    }

    public void FollowPlayer()
    {
        repathCounter += Time.deltaTime;
        Vector2 entityLoc = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerLoc = new Vector2(player.transform.position.x, player.transform.position.y);
        if (Vector2.Distance(entityLoc, playerLoc) < 1)
        {
            if (pathRoutine != null)
            {
                StopCoroutine(pathRoutine);
            }
            body.AddForce((new Vector2(playerLoc.x, playerLoc.y) - new Vector2(transform.position.x, transform.position.y)).normalized * speed * 5);
        }
        else if (repathCounter >= repathWaitTime && NodeGrid.instance.GetNodeFromPosition(entityLoc) != NodeGrid.instance.GetNodeFromPosition(playerLoc))
        {
            repathCounter = 0;
            PathRequestManager.RequestPath(new PathRequest(entityLoc, playerLoc, OnPathFound));
        }
    }

    public void Wander()
    {
        wanderCounter += Time.deltaTime;

        if (wanderCounter >= wanderTime)
        {
            float xDisplacement;
            float yDisplacement;

            Vector2 entityLoc;
            Vector2 targetLoc;

            entityLoc = new Vector2(transform.position.x, transform.position.y);
            wanderTime = Random.Range(wanderMinTime, wanderMaxTime);
            wanderCounter = 0;

            do
            {
                xDisplacement = Random.Range(-wanderRadius, wanderRadius);
                yDisplacement = Random.Range(-wanderRadius, wanderRadius);

                targetLoc = new Vector2(entityLoc.x + xDisplacement, entityLoc.y + yDisplacement);

            } while (!MapController.instance.grid.GetNodeFromPosition(targetLoc).traversable || MapController.instance.grid.GetNodeFromPosition(targetLoc) == MapController.instance.grid.GetNodeFromPosition(new Vector2(transform.position.x, transform.position.y)));

            PathRequestManager.RequestPath(new PathRequest(entityLoc, targetLoc, OnPathFound));
        }
    }

    private void OnDestroy()
    {
        if (pathRoutine != null)
        {
            StopCoroutine(pathRoutine);
        }
    }
}
