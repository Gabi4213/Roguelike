using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableInventorySlot : MonoBehaviour
{
    private InventorySlot inventorySlot;
    public int consumableSlot;

    void Start()
    {
        inventorySlot = GetComponentInChildren<InventorySlot>();
    }

    void Update()
    {
        if (inventorySlot.transform.childCount == 0) return;

        if(consumableSlot == 1)
        {

            if (InputManager.consumeSlotOne)
            {
                InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

                if (inventoryItem.count > 1)
                {
                    inventoryItem.count--;
                    inventoryItem.RefreshCount();
                }
                else
                {
                    Destroy(inventoryItem.gameObject);
                }
            }
        }
        else
        {

            if (InputManager.consumeSlotTwo)
            {
                InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

                if (inventoryItem.count > 1)
                {
                    inventoryItem.count--;
                    inventoryItem.RefreshCount();
                }
                else
                {
                    Destroy(inventoryItem.gameObject);
                }
            }
        }
    }
}
