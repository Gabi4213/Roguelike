using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHammerController : MonoBehaviour
{
    [Header("Animation Info")]
    public float pulseSpeed = 1.0f; // Adjust the pulsating speed
    public float pulseAmount = 0.2f; // Adjust the pulsating amount

    [Header("Gameplay Info")]
    private Transform spawnPoint; // Make sure to assign this in the Inspector
    public GameObject projectile; // Make sure to assign this in the Inspector

    private Vector3 initialScale;
    private bool canAttack = true; // Flag to check if the player can attack

    public Item item;

    private void Start()
    {
        initialScale = transform.localScale;
        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (canAttack && InputManager.attack && !InputManager.inventoryOpen)
        {
            StartCoroutine(AttackCooldown());
            InstantiateProjectile();
            StartCoroutine(ApplyPulsatingEffect());
        }
    }

    private void InstantiateProjectile()
    {
        // Instantiate the projectile at the spawn point
        GameObject projectileObj = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        projectileObj.GetComponent<SetableProjectile>().projectileLifetime = item.projectileLifetime;

        projectileObj.GetComponentInChildren<IDamage>().SetItem(item);
    }

    private IEnumerator ApplyPulsatingEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < pulseSpeed)
        {
            float pulsation = Mathf.Sin((elapsedTime / pulseSpeed) * Mathf.PI) * pulseAmount;
            transform.localScale = initialScale + new Vector3(pulsation, pulsation, pulsation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the scale to its initial value
        transform.localScale = initialScale;
    }

    private IEnumerator AttackCooldown()
    {
        PlayerStates.instance.SetState(PlayerState.Attack);
        canAttack = false;
        yield return new WaitForSeconds(PlayerStatistics.instance.attackSpeed);
        PlayerStates.instance.SetState(PlayerState.Idle);
        canAttack = true;
    }
}
