using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject detailImage;
    public GameObject detailText;
    public GameObject itemNameText;
    public GameObject itemNumText;

    public GameObject useButton;
    public GameObject dropButton;
    public GameObject cancelButton;

    public TextMeshProUGUI inventorySpace;

    public GameObject invWindow;
    public GameObject invContent;

    public GameObject detailWindow;

    public GameObject itemButtonPrefab;

    Inventory invInstance;

    MoneyBag moneyInstance;
    public TextMeshProUGUI platText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI silverText;
    public TextMeshProUGUI copperText;

    bool invOpen;

    public Item currentDetailItem;

    #region Singleton

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Inventory Manager found!");
            return;
        }
        instance = this;
    }

    #endregion


    private void Start()
    {
        invWindow.SetActive(false);
        invOpen = false;
        currentDetailItem = null;
        ClearDetailMenu();
    }

    public void ToggleInventory()
    {
        if (!invOpen)
        {
            invInstance = PlayerController.instance.inventory;
            moneyInstance = PlayerController.instance.inventory.money;
            invInstance.OnInventoryChanged += UpdateInventoryDisplay;
            invInstance.OnMoneyChanged += UpdateMoneyBagDisplay;
            UpdateInventoryDisplay();
            UpdateMoneyBagDisplay();
            invWindow.SetActive(true);
            invOpen = true;
        }
        else
        {
            invInstance.OnInventoryChanged -= UpdateInventoryDisplay;
            invInstance.OnMoneyChanged -= UpdateMoneyBagDisplay;
            ClearInvButtons();
            ClearDetailMenu();
            invInstance = null;
            moneyInstance = null;
            invWindow.SetActive(false);
            invOpen = false;
        }
    }

    public void SetInvButtons()
    {
        for (int i=0;i<invInstance.items.Count;i++)
        {
            GameObject tempButton = GameObject.Instantiate(itemButtonPrefab);
            tempButton.transform.SetParent(invContent.transform);
            int tempnum = i;
            tempButton.GetComponent<Button>().onClick.AddListener(delegate { SetDetailMenu(invInstance.items[tempnum]); });

            Text[] tempTexts = tempButton.GetComponentsInChildren<Text>();
            for (int j=0;j<tempTexts.Length;j++)
            {
                if (tempTexts[j].CompareTag("ItemName"))
                {
                    tempTexts[j].text = invInstance.items[i].itemName;
                }
                else if (tempTexts[j].CompareTag("ItemCount"))
                {
                    tempTexts[j].text = invInstance.items[i].GetCount().ToString();
                }
            }
            Image[] tempspots = tempButton.GetComponentsInChildren<Image>();
            for (int j=0;j<tempspots.Length;j++)
            {
                if (tempspots[j].CompareTag("ItemImage"))
                {
                    tempspots[j].sprite = invInstance.items[i].itemImage;
                }
            }
        }
    }
    private void ClearInvButtons()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in invContent.transform)
        {
            children.Add(child.gameObject);
        }

        children.ForEach(delegate (GameObject child)
        {
            Destroy(child);
        });
    }

    public void UpdateInventoryDisplay()
    {
        ClearInvButtons();
        SetInvButtons();
        SetDetailMenu(currentDetailItem);
        SetInventoryDetails();
    }

    public void UpdateMoneyBagDisplay()
    {
        platText.text = moneyInstance.platinum.ToString();
        goldText.text = moneyInstance.gold.ToString();
        silverText.text = moneyInstance.silver.ToString();
        copperText.text = moneyInstance.copper.ToString();
    }

    public void ClearDetailMenu()
    {
        currentDetailItem = null;

        detailImage.GetComponent<Image>().sprite = null;
        detailText.GetComponent<Text>().text = null;
        itemNameText.GetComponent<Text>().text = null;
        itemNumText.GetComponent<Text>().text = null;

        detailImage.SetActive(false);
        detailText.SetActive(false);
        itemNameText.SetActive(false);
        itemNumText.SetActive(false);
        dropButton.SetActive(false);
        useButton.SetActive(false);

        detailWindow.SetActive(false);
    }

    public void SetDetailMenu(Item item)
    {
        if (item != null)
        {
            if (item.GetCount() <= 0)
            {
                item = null;
                ClearDetailMenu();
            }
            else
            {
                currentDetailItem = item;

                detailImage.GetComponent<Image>().sprite = item.itemImage;
                detailText.GetComponent<Text>().text = item.examineText;
                itemNameText.GetComponent<Text>().text = item.itemName;
                itemNumText.GetComponent<Text>().text = item.GetCount().ToString() + "/" + item.maxStack.ToString();

                dropButton.GetComponent<Button>().onClick.RemoveAllListeners();
                dropButton.GetComponent<Button>().onClick.AddListener(() => invInstance.DropItem(currentDetailItem));

                useButton.GetComponent<Button>().onClick.RemoveAllListeners();
                useButton.GetComponent<Button>().onClick.AddListener(() => invInstance.UseItem(currentDetailItem, invInstance.gameObject));

                detailText.SetActive(true);
                detailImage.SetActive(true);
                itemNameText.SetActive(true);
                itemNumText.SetActive(true);
                dropButton.SetActive(true);
                useButton.SetActive(true);

                detailWindow.SetActive(true);
            }
        }
    }

    public void SetInventoryDetails()
    {
        inventorySpace.text = invInstance.GetNumItems() + "/" + invInstance.maxItems;
    }
}
