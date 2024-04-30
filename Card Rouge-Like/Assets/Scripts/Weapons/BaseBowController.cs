using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBowController : MonoBehaviour
{
    [Header("Animation Info")]
    public float pulseSpeed = 1.0f; // Adjust the pulsating speed
    public float pulseAmount = 0.2f; // Adjust the pulsating amount

    [Header("Gameplay Info")]
    public Transform spawnPoint; // Make sure to assign this in the Inspector
    public GameObject projectile; // Make sure to assign this in the Inspector

    private Vector3 initialScale;
    private bool canAttack = true; // Flag to check if the player can attack

    private InventoryManager invManager;

    [Header("Sprite Info")]
    public SpriteRenderer bowSprite;
    public Sprite drawnBowSprite, neutralBowSprite;
    public float animationTime;

    public Item item;

    private void Start()
    {
        initialScale = transform.localScale;
        invManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    private void Update()
    {
        //search through the inv manager and see if we have ammo


        if (canAttack && InputManager.attack && !InputManager.inventoryOpen)
        {
            InventorySlot slot = invManager.FindSlotByItemType(ItemType.Ammo);

            if(slot!= null)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();

                if (item.count > 1)
                {
                    item.count--;
                    item.RefreshCount();

                    StartCoroutine(AttackCooldown());
                    StartCoroutine(AnimateBowSprite());
                    InstantiateProjectile();
                    StartCoroutine(ApplyPulsatingEffect());
                }
                else
                {
                    Destroy(item.gameObject);

                    StartCoroutine(AttackCooldown());
                    StartCoroutine(AnimateBowSprite());
                    InstantiateProjectile();
                    StartCoroutine(ApplyPulsatingEffect());
                }
            }
        }      
    }

    private void InstantiateProjectile()
    {
        // Instantiate the projectile at the spawn point
        GameObject projectileObj = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        projectileObj.GetComponent<ArrowProjectile>().projectileLifetime = item.projectileLifetime;
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
        canAttack = false;
        yield return new WaitForSeconds(PlayerStatistics.instance.attackSpeed);
        canAttack = true;
    }

    private IEnumerator AnimateBowSprite()
    {
        bowSprite.sprite = drawnBowSprite;
        yield return new WaitForSeconds(animationTime);
        bowSprite.sprite = neutralBowSprite;
    }
}
