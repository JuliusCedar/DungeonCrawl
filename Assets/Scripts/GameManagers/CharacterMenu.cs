using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMenu : MonoBehaviour
{

    EquipmentManager equipInstance;
    public PlayerStats playerStats;

    public GameObject characterScreen;

    public Button headButton;
    public Button chestButton;
    public Button weaponButton;
    public Button shieldButton;
    public Button legsButton;
    public Button feetButton;

    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI attackText;

    bool menuOpen;

    #region Singleton

    public static CharacterMenu instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Character Menu found!");
            return;
        }
        instance = this;
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        characterScreen.SetActive(false);
        menuOpen = false;
    }

    public void ToggleMenu()
    {
        if (menuOpen)
        {
            ClearEquipmentSlots();
            equipInstance.OnEquipmentChanged -= UpdateCharacterMenu;
            equipInstance = null;
            characterScreen.SetActive(false);
            menuOpen = false;
        }
        else
        {
            equipInstance = PlayerController.instance.equipManager;
            equipInstance.OnEquipmentChanged += UpdateCharacterMenu;
            UpdateCharacterMenu();
            characterScreen.SetActive(true);
            menuOpen = true;
        }
    }

    private void UpdateCharacterMenu()
    {
        ClearEquipmentSlots();
        SetEquipmentSlots();
        SetStatWindow();
    }

    private void SetEquipmentSlots()
    {
        Equipment[] tempEquip = equipInstance.currentEquipment;

        for (int i=0;i<tempEquip.Length;i++)
        {
            if (tempEquip[i] != null) {
                switch ((int)tempEquip[i].equipSlot)
                {
                    case 0:
                        headButton.image.sprite = tempEquip[i].itemImage;
                        headButton.onClick.AddListener(() => equipInstance.Unequip(0));
                        headButton.gameObject.SetActive(true);
                        break;
                    case 1:
                        chestButton.image.sprite = tempEquip[i].itemImage;
                        chestButton.onClick.AddListener(() => equipInstance.Unequip(1));
                        chestButton.gameObject.SetActive(true);
                        break;
                    case 2:
                        legsButton.image.sprite = tempEquip[i].itemImage;
                        legsButton.onClick.AddListener(() => equipInstance.Unequip(2));
                        legsButton.gameObject.SetActive(true);
                        break;
                    case 3:
                        weaponButton.image.sprite = tempEquip[i].itemImage;
                        weaponButton.onClick.AddListener(() => equipInstance.Unequip(3));
                        weaponButton.gameObject.SetActive(true);
                        break;
                    case 4:
                        shieldButton.image.sprite = tempEquip[i].itemImage;
                        shieldButton.onClick.AddListener(() => equipInstance.Unequip(4));
                        shieldButton.gameObject.SetActive(true);
                        break;
                    case 5:
                        feetButton.image.sprite = tempEquip[i].itemImage;
                        feetButton.onClick.AddListener(() => equipInstance.Unequip(5));
                        feetButton.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    private void ClearEquipmentSlots()
    {
        headButton.image.sprite = null;
        chestButton.image.sprite = null;
        legsButton.image.sprite = null;
        weaponButton.image.sprite = null;
        shieldButton.image.sprite = null;
        feetButton.image.sprite = null;

        headButton.gameObject.SetActive(false);
        chestButton.gameObject.SetActive(false);
        legsButton.gameObject.SetActive(false);
        weaponButton.gameObject.SetActive(false);
        shieldButton.gameObject.SetActive(false);
        feetButton.gameObject.SetActive(false);
    }

    private void SetStatWindow()
    {
        defenseText.text = playerStats.defense.GetValue().ToString();
        attackText.text = playerStats.physicalAttack.GetValue().ToString();
    }

}
