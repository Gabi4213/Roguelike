using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject soulFragment;
    public int minSouls, maxSouls;
    public float dropRadius = 1.0f;

    // Array for possible item drops
    public GameObject[] itemDrops;

    // Array for the corresponding drop chances (percentages)
    [Range(0, 100)]
    public float[] itemDropChances;

    // Drop Soul Fragments (this part is unchanged)
    public void DropSoulFragments()
    {
        int numberOfItems = Random.Range(minSouls, maxSouls + 1);

        for (int i = 0; i < numberOfItems; i++)
        {
            // Determine a random position around the enemy
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;

            // Instantiate the soul fragment at the random position
            Instantiate(soulFragment, randomPosition, Quaternion.identity);
        }
    }

    // New method: Drop only one item based on its drop chances, or none if no chance passes
    public void DropItems()
    {
        // Check if itemDrops and itemDropChances arrays are properly set up
        if (itemDrops.Length != itemDropChances.Length)
        {
            Debug.LogError("Item Drops and Item Drop Chances arrays must be the same length!");
            return;
        }

        // Loop through all possible item drops
        for (int i = 0; i < itemDrops.Length; i++)
        {
            float dropChance = itemDropChances[i];
            float randomValue = Random.Range(0f, 100f); // Generate a random number between 0 and 100

            // If the random value is less than or equal to the drop chance, drop the item
            if (randomValue <= dropChance)
            {
                // Determine a random position around the enemy
                Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;

                // Instantiate the item at the random position
                Instantiate(itemDrops[i], randomPosition, Quaternion.identity);

                // Break after the first successful drop (only one item can drop)
                return;
            }
        }

        // If no item is dropped after checking all items, nothing happens
    }
}
