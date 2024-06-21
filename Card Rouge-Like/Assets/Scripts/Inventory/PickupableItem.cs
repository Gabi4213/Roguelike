using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public Item item; // The item to add to the inventory
    public bool rotateItem  = true;
    public GameObject pickUpFX;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!spriteRenderer.sprite)
        {
            spriteRenderer.sprite = item.image;
        }

        if (rotateItem)
        {
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only if the thing colliding is a player
        if (other.tag == "Player")
        {
            bool result = InventoryManager.instance.AddItem(item);

            if (result)
            {
                if (pickUpFX)
                {
                    Instantiate(pickUpFX, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}
