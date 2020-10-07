using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public Sprite itemImage;
    public Obstacle itemObstacle;
    public string examineText;
    public string itemName;

    public int count = 0;
    public int maxStack;

    public bool Add()
    {
        if (count < maxStack)
        {
            count++;
            return true;
        }
        return false;
    }

    public bool Remove()
    {
        if (count > 0)
        {
            count--;
            return true;
        }
        return false;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int c)
    {
        count = c;
    }

    public bool HasSpace()
    {
        return !(count >= maxStack);
    }

    public virtual void Use(GameObject source)
    {
        Debug.Log(source.name + " using " + itemName);
    }

    public void RemoveFromInventory()
    {
        PlayerController.instance.inventory.RemoveItem(this, count);
    }
}
