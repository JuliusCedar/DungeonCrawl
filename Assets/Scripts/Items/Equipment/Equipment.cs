using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Items/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    public int armorValue;
    public int physicalAttackValue;

    public override void Use(GameObject source)
    {
        base.Use(source);

        Inventory inv = source.GetComponent<Inventory>();
        EquipmentManager equip = source.GetComponent<EquipmentManager>();

        equip.Equip((Equipment)inv.RemoveItem(this, 1));
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet}
