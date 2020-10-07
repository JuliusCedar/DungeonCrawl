using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Inventory : MonoBehaviour
{
    public MoneyBag money;

    public List<Item> items;

    public int maxItems;

    public event System.Action OnInventoryChanged;
    public event System.Action OnMoneyChanged;

    private void Start()
    {
        money = new MoneyBag(0,0,0,0);
    }

    public int AddItem(Item item)
    {
        int leftToAdd = item.GetCount();

        for (int i=0;i< items.Count;i++)
        {
            if (items[i].itemName == item.itemName)
            {
                while (items[i].HasSpace() && leftToAdd > 0)
                {
                    items[i].Add();
                    item.Remove();
                    leftToAdd--;
                }
            }
        }
        if (leftToAdd > 0 && items.Count < maxItems)
        {
            items.Add(item);
            leftToAdd = 0;
        }
        if (OnInventoryChanged != null)
        {
            OnInventoryChanged();
        }
        return leftToAdd;
    }

    public Item RemoveItem(Item item, int num)
    {
        Item output = null;
        for (int i=0;i<items.Count;i++)
        {
            if (items[i].Equals(item))
            {
                output = Instantiate(item);
                output.SetCount(0);
                for (int j=0;j< num;j++)
                {
                    output.Add();
                    items[i].Remove();
                    if (!(items[i].GetCount() > 0))
                    {
                        items.Remove(item);
                        break;
                    }
                    if (!output.HasSpace())
                    {
                        break;
                    }
                }
            }
        }
        if (OnInventoryChanged != null)
        {
            OnInventoryChanged();
        }

        if (output == null || output.GetCount() <= 0)
        {
            return null;
        }
        return output;
    }

    public void DropItem(Item item)
    {
        float angle = Random.Range(0, 360);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Equals(item))
            {
                MapController.instance.SpawnItem(RemoveItem(items[i], items[i].GetCount()+1), new Vector3(Mathf.Cos(angle) + transform.position.x, Mathf.Sin(angle) + transform.position.y, 0), transform.rotation);
            }
        }
    }

    public void UseItem(Item item, GameObject source)
    {
        if (items.Contains(item))
        {
            item.Use(source);
        }
        
    }

    public int GetNumItems()
    {
        return items.Count;
    }

    public bool AddMoney(MoneyBag input)
    {
        bool success = money.Add(input);
        if (success && OnMoneyChanged != null)
        {
            OnMoneyChanged();
        }
        return success;
    }
}
