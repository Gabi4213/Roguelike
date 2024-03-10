using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public Item item;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if(!spriteRenderer.sprite)
        {
            spriteRenderer.sprite = item.image;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only if the thign colliding is a player
        if (other.tag == "Player") 
        {
            bool result = InventoryManager.instance.AddItem(item);

            if (result)
            {
                Destroy(gameObject);
            }
        }
    }

}
