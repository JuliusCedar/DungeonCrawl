using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    ItemDrop drop;
    Rigidbody2D body;

    public bool active;
    public bool playerInSight;

    private void Start()
    {
        drop = GetComponent<ItemDrop>();
        body = GetComponent<Rigidbody2D>();
        playerInSight = false;
        active = false;
    }

    public override void Die()
    {
        drop.DropItem();
        //PlayerController.instance.inventory.money.Add(new MoneyBag(0, 0, 0, 15));
        MapController.instance.SpawnMoney(new MoneyBag(10,1,2,10), transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public override void OnDamageTaken(GameObject source)
    {
        Vector2 direction = (transform.position - source.transform.position);
        body.AddForce(direction.normalized * 2000);
    }
}
