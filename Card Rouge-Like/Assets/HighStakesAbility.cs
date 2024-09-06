using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighStakesAbility : MonoBehaviour
{
    [Header("References")]
    public AbilityBaseUsable abilityBase; // Reference to the base ability
    public Transform spawnPoint; // Projectile spawn point
    public GameObject[] projectilePrefabs; // Array of projectile prefabs

    [Header("Sprites")]
    public Sprite[] cardSprites; // Array of card sprites
    public SpriteRenderer cardSpriteRenderer; // Renderer for displaying cards

    [Header("Ability Data")]
    public float cardShuffleTime; // Time to shuffle/randomize cards
    public float cardDisplayTime; // Time to display each card before shuffling
    public float cooldownAfterThrow; // Cooldown period after throwing a card
    public float projectileLifetime; // Lifetime of the spawned projectile

    [Header("VFX")]
    public GameObject cardChosenParticlePrefab;

    // Private fields
    private int selectedCardIndex; // Currently selected card index
    private bool isAbilityActive = false; // Tracks if the ability is in progress
    private bool isShufflingCards = false; // Tracks if the card shuffling process has started
    private bool isCardSelected = false; // Tracks if a card has been selected

    private void Update()
    {
        // Check if ability is ready to use
        if (abilityBase.currentCooldown <= 0.0f && cardSpriteRenderer.sprite)
        {
            EndAbility();
        }

        // Start card shuffling when ability is used
        if (abilityBase.inUse && !isAbilityActive)
        {
            StartCoroutine(RandomizeCard());
        }

        // Start iterating through card visuals if ability is in use and shuffling hasn't started
        if (abilityBase.inUse && !isShufflingCards)
        {
            StartCoroutine(IterateCardVisual());
        }

        // Throw the selected card when conditions are met
        if (isCardSelected && InputManager.attack && !InputManager.inventoryOpen && abilityBase.currentCooldown > 0.0f)
        {
            ThrowCard();
            isCardSelected = false;
        }
    }

    // Instantiate a projectile at the spawn point
    private void InstantiateProjectile(GameObject projectilePrefab)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        projectile.GetComponent<CardProjectile>().parentAbility = this;
    }

    // Handle the card throw
    private void ThrowCard()
    {
        InstantiateProjectile(projectilePrefabs[selectedCardIndex]);
        EndAbility();
    }

    // End the ability and reset relevant variables
    public void EndAbility()
    {
        CameraFollow.instance.ShakeCamera();
        cardSpriteRenderer.sprite = null;
        abilityBase.currentCooldown = cooldownAfterThrow;
    }

    // Iterate through card visuals, cycling through available sprites
    IEnumerator IterateCardVisual()
    {
        int index = 0;
        isShufflingCards = true;

        while (abilityBase.inUse)
        {
            cardSpriteRenderer.sprite = cardSprites[index];
            yield return new WaitForSeconds(cardDisplayTime); // Wait before displaying the next card
            index = (index + 1) % cardSprites.Length; // Cycle back to the first card when reaching the end
        }
        Instantiate(cardChosenParticlePrefab, transform.position, Quaternion.identity);
        isShufflingCards = false;
    }

    // Randomize and select a card after a brief shuffle period
    IEnumerator RandomizeCard()
    {
        isAbilityActive = true;
        yield return new WaitForSeconds(cardShuffleTime);

        // Randomly pick a card
        int randomIndex = Random.Range(0, cardSprites.Length);
        selectedCardIndex = randomIndex;

        cardSpriteRenderer.sprite = cardSprites[randomIndex]; // Set the chosen sprite

        isCardSelected = true;
        isShufflingCards = false;
        abilityBase.inUse = false;
        isAbilityActive = false;
    }
}
