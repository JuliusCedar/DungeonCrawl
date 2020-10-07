using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Equipment[] currentEquipment;

    public delegate void OnEquip(Equipment newItem, Equipment oldItem);
    public OnEquip onEquip;

    public event System.Action OnEquipmentChanged;

    Inventory inventory;

    private void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        inventory = GetComponent<Inventory>();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.AddItem(oldItem);
        }

        if (onEquip != null)
        {
            onEquip.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;

        if (OnEquipmentChanged != null)
        {
            OnEquipmentChanged();
        }
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.AddItem(oldItem);

            if (onEquip != null)
            {
                onEquip.Invoke(null, oldItem);
            }

            currentEquipment[slotIndex] = null;

            if (OnEquipmentChanged != null)
            {
                OnEquipmentChanged();
            }
        }
    }
}
