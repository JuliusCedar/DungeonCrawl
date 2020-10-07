using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class MoneyPickup : MonoBehaviour
{
    public MoneyBag amount;
    public float pickupRadius;

    //Transform player = PlayerController.instance.gameObject.transform;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.inventory.AddMoney(amount);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //if (player != null && Vector3.Distance(this.transform.position, player.position) < pickupRadius)
        //{
        //    rb.MovePosition(player.position);
        //}
    }

}
