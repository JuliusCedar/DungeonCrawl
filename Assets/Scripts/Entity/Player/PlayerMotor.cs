using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : EntityMotor
{
    Animator playerAnimator;

    float horizontal = 0;
    float vertical = 0;
    float wPressCounter = 0;
    float aPressCounter = 0;
    float sPressCounter = 0;
    float dPressCounter = 0;

    public bool canMove = true;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    public override void Move()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            wPressCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            aPressCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            sPressCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dPressCounter = 0;
        }

        wPressCounter += Time.deltaTime;
        aPressCounter += Time.deltaTime;
        sPressCounter += Time.deltaTime;
        dPressCounter += Time.deltaTime;

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        if (canMove && !stats.stunned)
        {
            body.AddForce(new Vector2(horizontal, vertical) * speed * 10);
            if (!(horizontal == 0 && vertical == 0))
            {
                stats.direction = GetDirectionFromAngle(Mathf.Atan2(vertical, horizontal));
            }
        }
        else
        {
            if (!(body.velocity.x == 0 && body.velocity.y == 0)) 
            {
                stats.direction = GetDirectionFromAngle(Mathf.Atan2(body.velocity.y, body.velocity.x));   
            }
        }

        Animate();
    }

    private Direction GetDirectionFromAngle(float input)
    {
        Direction output = Direction.Left;

        switch (input)
        {
            case float temp when (temp > Mathf.PI / 4 && temp < 3 * Mathf.PI / 4):
                output = Direction.Up;
                break;
            case float temp when (temp > -Mathf.PI / 4 && temp < Mathf.PI / 4):
                output = Direction.Right;
                break;
            case float temp when (temp > -3 * Mathf.PI / 4 && temp < -Mathf.PI / 4):
                output = Direction.Down;
                break;
            case float temp when (temp > 3 * Mathf.PI / 4 || temp < -3 * Mathf.PI / 4):
                output = Direction.Left;
                break;
            case Mathf.PI/4:
                output = wPressCounter < dPressCounter ? Direction.Up : Direction.Right;
                break;
            case -Mathf.PI / 4:
                output = sPressCounter < dPressCounter ? Direction.Down : Direction.Right;
                break;
            case 3*Mathf.PI / 4:
                output = wPressCounter < aPressCounter ? Direction.Up : Direction.Left;
                break;
            case -3*Mathf.PI / 4:
                output = sPressCounter < aPressCounter ? Direction.Down : Direction.Left;
                break;
        }

        return output;
    }

    protected override void Animate()
    {

        playerAnimator.SetBool("Stunned", stats.stunned);
        playerAnimator.SetFloat("VelocityMagnitude", body.velocity.magnitude);

        switch (stats.direction)
        {
            case Direction.Up:
                playerAnimator.SetBool("Right", false);
                playerAnimator.SetBool("Left", false);
                playerAnimator.SetBool("Up", true);
                playerAnimator.SetBool("Down", false);
                break;
            case Direction.Down:
                playerAnimator.SetBool("Right", false);
                playerAnimator.SetBool("Left", false);
                playerAnimator.SetBool("Up", false);
                playerAnimator.SetBool("Down", true);
                break;
            case Direction.Left:
                playerAnimator.SetBool("Right", false);
                playerAnimator.SetBool("Left", true);
                playerAnimator.SetBool("Up", false);
                playerAnimator.SetBool("Down", false);
                break;
            case Direction.Right:
                playerAnimator.SetBool("Right", true);
                playerAnimator.SetBool("Left", false);
                playerAnimator.SetBool("Up", false);
                playerAnimator.SetBool("Down", false);
                break;
        }
    }
}