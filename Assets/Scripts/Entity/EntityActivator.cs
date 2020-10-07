using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActivator : MonoBehaviour
{
    CircleCollider2D activationBorder;
    public float activationRadius;

    // Start is called before the first frame update
    void Start()
    {
        activationBorder = GetComponent<CircleCollider2D>();
        activationBorder.radius = activationRadius;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyActivation activator = collision.GetComponent<EnemyActivation>();
        if (activator != null)
        {
            activator.Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyActivation activator = collision.GetComponent<EnemyActivation>();
        if (activator != null)
        {
            activator.Deactivate();
        }
    }
}
