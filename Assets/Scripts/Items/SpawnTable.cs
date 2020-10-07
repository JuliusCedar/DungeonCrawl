using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewSpawnTable", menuName = "SpawnTable")]
public class SpawnTable : ScriptableObject
{
    [System.Serializable]
    public struct ItemEntry
    {
        public Item item;
        public float spawnChance;
        public int minCount;
        public int maxCount;
    }

    public List<ItemEntry> itemSpawns;

    System.Random rand = new System.Random();

    public List<Item> GenerateItems()
    {
        List<Item> output = new List<Item>();
        for(int i = 0; i < itemSpawns.Count; i++)
        {
            if(rand.Next(0, 100) < itemSpawns[i].spawnChance)
            {
                Item tempItem = Instantiate(itemSpawns[i].item);
                tempItem.SetCount(rand.Next(itemSpawns[i].minCount, itemSpawns[i].maxCount));
                output.Add(tempItem);
            }
        }
        return output;
    }
}
