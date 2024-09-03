using Demo_Project;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class BaseSwordController : MonoBehaviour
{
    [Header("Animation Info")]
    public float pulseSpeed = 1.0f; // Adjust the pulsating speed
    public float pulseAmount = 0.2f; // Adjust the pulsating amount

    [Header("FX Info")]
    private SpriteRenderer fxColor;
    public Animator weaponAttackFxAnimation;

    [Header("Gameplay Info")]
    public Transform spawnPoint; // Make sure to assign this in the Inspector
    public GameObject projectile; // Make sure to assign this in the Inspector

    private Vector3 initialScale;
    private bool canAttack = true; // Flag to check if the player can attack

    private GameObject player;
    private Animator playerAnimator;
    public Item item;

    // Reference to the PivotRotation script
    private PivotRotation pivotRotation;

    private void Start()
    {
        initialScale = transform.localScale;

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponentInChildren<Animator>();

        // Find the PivotRotation script (assumes it's on the same GameObject as this script)
        pivotRotation = GetComponent<PivotRotation>();

        if (weaponAttackFxAnimation)
        {
            fxColor = weaponAttackFxAnimation.gameObject.GetComponent<SpriteRenderer>();
            fxColor.color = item.itemColor;
        }
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

        // Camera shake
        CameraFollow.instance.ShakeCamera();

        if (weaponAttackFxAnimation)
        {
            weaponAttackFxAnimation.SetTrigger("Play");
        }

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
        // Get the current direction from the PivotRotation script
        Vector2 direction = pivotRotation.GetDirection();

        // Set the animator parameters based on the direction
        playerAnimator.SetFloat("LastHorizontal", direction.x);
        playerAnimator.SetFloat("LastVertical", direction.y);

        playerAnimator.SetTrigger("Attack");
        PlayerStates.instance.SetState(PlayerState.Attack);
        canAttack = false;
        yield return new WaitForSeconds(PlayerStatistics.instance.attackSpeed);
        PlayerStates.instance.SetState(PlayerState.Idle);
        canAttack = true;
    }
}
