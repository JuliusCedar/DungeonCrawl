using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PolygonCollider2D upAttack;
    public PolygonCollider2D downAttack;
    public PolygonCollider2D leftAttack;
    public PolygonCollider2D rightAttack;

    public PlayerStats playerStats;
    public PlayerMotor motion;
    public Animator playerAnimator;

    public AudioSource swordSwingSound;


    bool attacking = false;

    public void Attack()
    {
        if (!attacking && !playerStats.stunned)
        {
            StartCoroutine(SwingSword());
        }
    }

    public IEnumerator SwingSword()
    {
        playerAnimator.SetTrigger("Attacking");
        attacking = true;
        motion.canMove = false;

        PolygonCollider2D temp = null;
        switch (playerStats.direction)
        {
            case Direction.Up:
                temp = upAttack;
                break;
            case Direction.Down:
                temp = downAttack;
                break;
            case Direction.Left:
                temp = leftAttack;
                break;
            case Direction.Right:
                temp = rightAttack;
                break;
        }

        yield return new WaitForSeconds(0.1f);
        temp.enabled = true;
        swordSwingSound.Play();
        yield return new WaitForSeconds(0.1f);
        temp.enabled = false;
        yield return new WaitForSeconds(0.3f);
        attacking = false;
        motion.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStats enemy = collision.GetComponent<EnemyStats>();
        if (enemy)
        {
            enemy.TakeDamage(playerStats.physicalAttack.GetValue(), gameObject);
        }
    }
}
