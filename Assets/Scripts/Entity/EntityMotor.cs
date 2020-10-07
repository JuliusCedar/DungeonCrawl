using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMotor : MonoBehaviour
{
    protected EntityStats stats;
    protected Rigidbody2D body;

    public float speed;

    public abstract void Move();
    protected abstract void Animate();

    public IEnumerator Knockback(Vector2 angle)
    {
        Direction temp = stats.direction;
        body.AddForce(angle.normalized * 2000);
        stats.stunned = true;
        yield return new WaitForSeconds(0.4f);
        stats.direction = temp;
        stats.stunned = false;
    }
}
