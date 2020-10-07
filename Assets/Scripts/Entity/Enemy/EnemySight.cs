using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float sightRadius;
    CircleCollider2D trigger;
    public EnemyStats stats;

    private void Start()
    {
        trigger = GetComponent<CircleCollider2D>();
        trigger.radius = sightRadius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerInSight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerInSight(false);
        }
    }

    public void SetPlayerInSight(bool b)
    {
        stats.playerInSight = b;
    }
}
