using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldenTouchAbility : MonoBehaviour
{
    public float range = 5f;
    public AbilityBaseUsable abilityBase;
    public Sprite goldenSprite;
    public GameObject freezeParticlePrefab;  // Particle system prefab for freezing
    public GameObject unfreezeParticlePrefab; // Particle system prefab for unfreezing

    GameObject[] enemies;
    private bool abilityInProgress = false; // Track if the ability is already in progress

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position to visualize the range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
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
        FindEnemiesInRange();
        yield return new WaitForSeconds(0.1f);

        //camera shake
        CameraFollow.instance.ShakeCamera();

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().SetState(EnemyState.Frozen);
            enemy.GetComponentInChildren<SpriteRenderer>().sprite = goldenSprite;
            Instantiate(freezeParticlePrefab, enemy.transform.position, Quaternion.identity); // Instantiate freeze particle effect once
        }

        yield return new WaitForSeconds(abilityBase.ability.duration);

        FindEnemiesInRange();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().SetState(EnemyState.Follow);
            enemy.GetComponentInChildren<SpriteRenderer>().sprite = enemy.GetComponent<Enemy>().defaultSprite;
            Instantiate(unfreezeParticlePrefab, enemy.transform.position, Quaternion.identity); // Instantiate unfreeze particle effect once
        }

        //camera shake
        CameraFollow.instance.ShakeCamera();

        abilityBase.inUse = false;
        abilityInProgress = false; // Reset the ability progress tracker
    }

    void FindEnemiesInRange()
    {
        // Find all game objects with the tag "Enemy"
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            // Calculate the distance between the object and the enemy
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Check if the enemy is within range
            if (distance <= range)
            {
                Debug.Log("Enemy in range: " + enemy.name);
                enemiesInRange.Add(enemy);
            }
        }

        enemies = enemiesInRange.ToArray();
    }
}
