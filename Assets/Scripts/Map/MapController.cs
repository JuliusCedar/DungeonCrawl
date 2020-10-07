using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{

    #region Singleton

    public static MapController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Map Controller found!");
            return;
        }
        instance = this;
    }

    #endregion

    public Tilemap worldTiles;
    public GridInformation worldInfo;

    public GameObject itemPrefab;
    public GameObject moneyPrefab;

    public NodeGrid grid;

    public int width;
    public int height;

    private void Start()
    {
        
    }

    public void BreakTile(Vector3Int tileLoc)
    {
        GameObject tempObject = worldTiles.GetInstantiatedObject(tileLoc);
        if (tempObject != null)
        {
            int dur = worldInfo.GetPositionProperty(tileLoc, "durability", -1);
            dur = dur - 1;
            if (dur > 0)
            {
                worldInfo.SetPositionProperty(tileLoc, "durability", dur);
            }
            else
            {
                if (tempObject.GetComponent<ItemDrop>() != null)
                {
                    tempObject.GetComponent<ItemDrop>().DropItem();
                }

                worldTiles.SetTile(tileLoc, null);
                grid.UpdateGrid();
            }
        }
    }

    public void SpawnItem(Item item, Vector3 loc, Quaternion rot)
    {
        GameObject newItem = Instantiate(itemPrefab, loc, rot);
        newItem.GetComponent<ItemPickup>().item = item;
        newItem.GetComponent<SpriteRenderer>().sprite = item.itemImage;
        newItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
        newItem.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0,5));
    }

    public void SpawnMoney(MoneyBag money, Vector3 loc, Quaternion rot)
    {

        GameObject newMoney;

        for (int i=0;i<4;i++)
        {

            newMoney = Instantiate(moneyPrefab, loc, rot);

            switch (i)
            {
                case 0:
                    newMoney.GetComponent<MoneyPickup>().amount = new MoneyBag(money.platinum,0,0,0);
                    break;
                case 1:
                    newMoney.GetComponent<MoneyPickup>().amount = new MoneyBag(0, money.gold, 0, 0);
                    break;
                case 2:
                    newMoney.GetComponent<MoneyPickup>().amount = new MoneyBag(0, 0, money.silver, 0);
                    break;
                case 3:
                    newMoney.GetComponent<MoneyPickup>().amount = new MoneyBag(0, 0, 0, money.copper);
                    break;
            }

            if (!newMoney.GetComponent<MoneyPickup>().amount.IsEmpty())
            {
                Color coinColor = newMoney.GetComponent<MoneyPickup>().amount.GetCoinColor();
                newMoney.GetComponent<SpriteRenderer>().color = coinColor;
                newMoney.GetComponent<ParticleSystem>().startColor = coinColor;
                newMoney.GetComponent<Light2D>().color = coinColor;
                newMoney.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
                newMoney.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0, 5));
            }
            else
            {
                Destroy(newMoney);
            }
        }




/*        GameObject newMoney = Instantiate(moneyPrefab, loc, rot);
        newMoney.GetComponent<MoneyPickup>().amount = new MoneyBag();
        newMoney.GetComponent<SpriteRenderer>().color = money.GetCoinColor();
        newMoney.GetComponent<ParticleSystem>().startColor = money.GetCoinColor();
        newMoney.GetComponent<Light2D>().color = money.GetCoinColor();
        newMoney.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
        newMoney.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0, 5)); */
    }

    public bool PlaceItem(Item item, Vector3Int loc)
    {
        if (worldTiles.GetTile(loc) == null && item.itemObstacle.tile != null)
        {
            worldTiles.SetTile(loc, item.itemObstacle.tile);
            worldInfo.SetPositionProperty(loc, "durability", item.itemObstacle.durability);
            grid.UpdateGrid();
            return true;
        }
        return false;
    }
}
