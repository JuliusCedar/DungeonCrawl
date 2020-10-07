using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject itemPrefab;
    public SpawnTable dropTable;

    MapController mapCon;

    private void Start()
    {
        mapCon = MapController.instance;
    }

    public void DropItem()
    {
        dropTable.GenerateItems().ForEach(delegate (Item item)
        {
            mapCon.SpawnItem(item, transform.position, transform.rotation);
        });
    }
}
