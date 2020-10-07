using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    EnemyStats stats;
    EnemyMotor motion;

    private void Start()
    {
        motion = GetComponent<EnemyMotor>();
        stats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        motion.Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats tempPlayer = collision.gameObject.GetComponent<PlayerStats>();
            tempPlayer.TakeDamage(stats.physicalAttack.GetValue(), gameObject);
        }
    }

}
