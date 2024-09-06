using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableAbility : MonoBehaviour
{
    public Ability ability; // The item to add to the inventory
    public bool rotateItem = true;
    public GameObject pickUpFX;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private int abilityIndex;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        abilityIndex = ability.usableAbility.GetComponent<AbilityBaseUsable>().abilityIndex;

        if (!spriteRenderer.sprite)
        {
            spriteRenderer.sprite = ability.image;
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
            bool result = InventoryManager.instance.AddAbility(ability, abilityIndex);

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
