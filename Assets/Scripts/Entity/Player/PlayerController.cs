using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    #region Singleton

    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Player Controller found!");
            return;
        }
        instance = this;
    }

    #endregion

    public Inventory inventory;
    InventoryManager invManager;

    public EquipmentManager equipManager;
    CharacterMenu charMenu;

    PlayerMotor motion;
    Animator playerAnimator;
    PlayerStats playerStats;
    PlayerCombat playerCombat;

    MapController map;

    float breakTimer = 0;
    public float breakInterval;

    void Start()
    {
        equipManager = GetComponent<EquipmentManager>();
        inventory = GetComponent<Inventory>();
        motion = GetComponent<PlayerMotor>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        map = MapController.instance;
        invManager = InventoryManager.instance;
        charMenu = CharacterMenu.instance;
    }

    void Update()
    {
        motion.Move();

        breakTimer = Mathf.Clamp(breakTimer -= Time.deltaTime, 0, breakInterval);

        if (Input.GetMouseButton(0) && breakTimer == 0)
        {
            map.BreakTile(map.worldTiles.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            breakTimer = breakInterval;
        }

        if (Input.GetMouseButtonDown(1))
        {
            PlaceItem(map.worldTiles.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            invManager.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            charMenu.ToggleMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ItemPickup pickupItem = collision.gameObject.GetComponent<ItemPickup>();
        if (pickupItem != null)
        {
            int prevCount = pickupItem.item.count;
            int leftover = inventory.AddItem(pickupItem.item);
            if (leftover < prevCount)
            {
                pickupItem.playPickup();
                if (leftover == 0)
                {
                    Destroy(pickupItem.gameObject);
                }
            }
        }
    }

    private void PlaceItem(Vector3Int spot)
    {
        if (invManager.currentDetailItem != null && map.PlaceItem(invManager.currentDetailItem, spot))
        {
            inventory.RemoveItem(invManager.currentDetailItem, 1);
        }
    }
}
