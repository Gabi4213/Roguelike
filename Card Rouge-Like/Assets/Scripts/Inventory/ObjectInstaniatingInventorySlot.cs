using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInstaniatingInventorySlot : MonoBehaviour
{
    public GameObject instantiatedObject;

    void Update()
    {
        if(transform.childCount != 0 && instantiatedObject == null)
        {
            InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

            if (inventoryItem == null)
            {
                InventoryAbility inventoryAbility = GetComponentInChildren<InventoryAbility>();

                Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                instantiatedObject = Instantiate(inventoryAbility.ability.usableAbility, player);
                instantiatedObject.transform.position = Vector3.zero;
                instantiatedObject.name = inventoryAbility.ability.instantiatedName;
            }
            else
            {
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                instantiatedObject = Instantiate(inventoryItem.item.usablePrefab, player);
                instantiatedObject.transform.position = Vector3.zero;
            }
        }
        else if (transform.childCount == 0 && instantiatedObject != null)
        { 
            Destroy(instantiatedObject);
        }
    }
}
