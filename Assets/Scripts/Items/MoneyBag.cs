using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag
{

    public int platinum;
    public int gold;
    public int silver;
    public int copper;

    public MoneyBag(int p, int g, int s, int c)
    {
        platinum = p;
        gold = g;
        silver = s;
        copper = c;
    }

    public MoneyBag()
    {
        platinum = 0;
        gold = 0;
        silver = 0;
        copper = 0;
    }

    public bool Add(MoneyBag input)
    {
        int tempCopper = this.copper + input.copper;
        int tempSilver = this.silver + input.silver;
        int tempGold = this.gold + input.gold;
        int tempPlat = this.platinum = input.platinum;

        while (tempCopper > 100)
        {
            tempSilver += 1;
            tempCopper -= 100;
        }

        while (tempSilver > 100)
        {
            tempGold += 1;
            tempSilver -= 100;
        }

        while (tempGold > 100)
        {
            tempPlat += 1;
            tempGold -= 100;
        }

        if (tempPlat > 99999)
        {
            return false;
        }
        else
        {
            this.platinum = tempPlat;
            this.gold = tempGold;
            this.silver = tempSilver;
            this.copper = tempCopper;
            return true;
        }
    }

    public void Remove(MoneyBag input)
    {
        //To be added
    }

    public void Print()
    {
        Debug.Log("Plat: " + platinum + ",\nGold: " + gold + ",\nSilver: " + silver + ",\nCopper: " + copper);
    }

    public Color GetCoinColor()
    {
        if (platinum > 0)
        {
            Debug.Log("Dropping Platinum");
            return new Color(162,200,201);
        }
        else if (gold > 0)
        {
            Debug.Log("Dropping Gold");
            return new Color(255,220,0);
        }
        else if (silver > 0)
        {
            Debug.Log("Dropping Silver");
            return new Color(200,200,200);
        }
        else if (copper > 0)
        {
            Debug.Log("Dropping Copper");
            return new Color(224,113,0);
        }
        else
        {
            Debug.Log("Dropping Generic");
            return new Color(255,255,255);
        }
    }

    public bool IsEmpty()
    {
        bool empty = true;
        if (platinum > 0)
        {
            empty = false;
        }
        else if (gold > 0)
        {
            empty = false;
        }
        else if (silver > 0)
        {
            empty = false;
        }
        else if (copper > 0)
        {
            empty = false;
        }
        return empty;
    }
}
