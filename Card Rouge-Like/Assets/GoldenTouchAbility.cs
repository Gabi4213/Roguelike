using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class GoldenTouchAbility : MonoBehaviour
{
    public float initialRange = 1f;
    public float maxRange = 5f;
    public float growSpeed = 2f; // Speed at which the range grows per second
    public AbilityBaseUsable abilityBase;
    public Sprite goldenSprite;
    public GameObject useAbilityParticlePrefab;
    public GameObject freezeParticlePrefab;  // Particle system prefab for freezing
    public GameObject unfreezeParticlePrefab; // Particle system prefab for unfreezing
    public GameObject rangeVisual; // Prefab for the visual representation of the range

    public float freezeSpeedMultiplier = 1.0f; // Multiplier for speeding up freezing

    private bool abilityInProgress = false; // Track if the ability is already in progress
    private HashSet<GameObject> frozenEnemies = new HashSet<GameObject>(); // Set to store frozen enemies
    private float currentRange; // Current range value during expansion

    void Start()
    {
        // Instantiate the range visual GameObject
        rangeVisual.SetActive(false); // Hide the visual initially
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position to visualize the current range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }

    void Update()
    {
        if (abilityBase.inUse && !abilityInProgress)
        {
            StartCoroutine(UseAbility());
        }
    }

    IEnumerator UseAbility()
    {
        abilityInProgress = true;
        currentRange = initialRange;
        rangeVisual.SetActive(true); // Show the visual

        //visual fx
        Instantiate(useAbilityParticlePrefab, transform.position, Quaternion.identity);

        //camera shake
        CameraFollow.instance.ShakeCamera();

        // Begin range expansion loop
        while (currentRange < maxRange)
        {
            currentRange += growSpeed * Time.deltaTime;
            rangeVisual.transform.localScale = Vector3.one * currentRange * 1.1f; // Adjust the scale based on currentRange
            FindAndFreezeEnemiesInRange();
            yield return null;
        }

        // Ensure range reaches max value
        currentRange = maxRange;
        rangeVisual.transform.localScale = Vector3.one * maxRange * 1.1f; // Set the scale to max range
        FindAndFreezeEnemiesInRange();

        if(currentRange == maxRange)
        {
            rangeVisual.SetActive(false); // Hide the visual
        }

        // Wait for the ability duration
        yield return new WaitForSeconds(abilityBase.ability.duration);

        //camera shake
        CameraFollow.instance.ShakeCamera();

        // Unfreeze enemies
        UnfreezeEnemies();

        // Clean up and reset
        abilityBase.inUse = false;
        abilityInProgress = false; // Reset the ability progress tracker
    }

    void FindAndFreezeEnemiesInRange()
    {
        // Find all game objects with the tag "Enemy"
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            // Calculate the distance between the object and the enemy
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Check if the enemy is within current range and not already frozen
            if (distance <= currentRange && !frozenEnemies.Contains(enemy))
            {
                //camera shake
                CameraFollow.instance.ShakeCamera();

                FreezeEnemy(enemy);
                frozenEnemies.Add(enemy);
            }
        }
    }

    void FreezeEnemy(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().SetState(EnemyState.Frozen);
        enemy.GetComponentInChildren<SpriteRenderer>().sprite = goldenSprite;
        Instantiate(freezeParticlePrefab, enemy.transform.position, Quaternion.identity);
    }

    void UnfreezeEnemies()
    {
        foreach (GameObject enemy in frozenEnemies)
        {
            if (enemy != null) // Check if the enemy still exists
            {
                UnfreezeEnemy(enemy);
            }
        }
        frozenEnemies.Clear();
    }

    void UnfreezeEnemy(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().SetState(EnemyState.Follow);
        enemy.GetComponentInChildren<SpriteRenderer>().sprite = enemy.GetComponent<Enemy>().defaultSprite;
        Instantiate(unfreezeParticlePrefab, enemy.transform.position, Quaternion.identity);
    }
}
