using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public InventorySlot[] inventorySlots;
    public AbilitySlot[] abilitySlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryAbilityPrefab;
    public GameObject mainInventoryGroup;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (InputManager.inventoryOpen)
        {
            mainInventoryGroup.SetActive(true);
        }
        else
        {
            mainInventoryGroup.SetActive(false);
        }
    }

    public bool AddItem(Item item)
    {
        // check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null 
                && itemInSlot.item == item 
                && itemInSlot.count < itemInSlot.item.maxStackAmount 
                && itemInSlot.item.stackable)
            {

                foreach(ItemType type in slot.allowedItemTypes)
                {
                    if(type == item.type)
                    {
                        itemInSlot.count++;
                        itemInSlot.RefreshCount();
                        return true;
                    }
                }
            }
        }

        // find an empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                foreach (ItemType type in slot.allowedItemTypes)
                {
                    if (type == item.type)
                    {
                        SpawnNewItem(item, slot);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool AddAbility(Ability ability)
    {     
        // find an empty slot
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            AbilitySlot slot = abilitySlots[i];
            InventoryAbility itemInSlot = slot.GetComponentInChildren<InventoryAbility>();

            if (itemInSlot == null)
            {
                SpawnNewAbility(ability, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();

        inventoryItem.InitialiseItem(item);
    }

    void SpawnNewAbility(Ability ability, AbilitySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryAbilityPrefab, slot.transform);
        InventoryAbility inventoryAbility = newItemGo.GetComponent<InventoryAbility>();

        inventoryAbility.InitialiseAbility(ability);
    }

    public InventorySlot FindSlotByItemType(ItemType itemType)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.type == itemType)
            {
                return slot;
            }
        }
        return null;
    }
}
