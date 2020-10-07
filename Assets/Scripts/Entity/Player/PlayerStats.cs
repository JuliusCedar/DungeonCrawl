using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    Inventory inventory;
    EquipmentManager equipManager;

    PlayerMotor motion;

    private void Start()
    {
        motion = GetComponent<PlayerMotor>();
        inventory = GetComponent<Inventory>();
        equipManager = GetComponent<EquipmentManager>();

        equipManager.onEquip += OnEquipmentChanged;
    }

    public override void Die()
    {
        while (inventory.items.Count > 0)
        {
            inventory.DropItem(inventory.items[0]);
        }
        currentHealth = maxHealth.GetValue();
    }

    public override void OnDamageTaken(GameObject source)
    {
        Vector2 direction = (transform.position - source.transform.position);
        if (kbRoutine != null)
        {
            StopCoroutine(kbRoutine);
        }
        kbRoutine = StartCoroutine(motion.Knockback(direction));
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            defense.AddModifier(newItem.armorValue);
            physicalAttack.AddModifier(newItem.physicalAttackValue);
        }

        if (oldItem != null)
        {
            defense.RemoveModifier(oldItem.armorValue);
            physicalAttack.RemoveModifier(oldItem.physicalAttackValue);
        } 
    }
}
