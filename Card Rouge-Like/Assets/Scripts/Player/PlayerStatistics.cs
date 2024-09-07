using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics instance;

    //game
    [Header("Game")]
    public int soulFragments;
    public int Karma;

    //Player
    [Header("Player")]
    public int health;
    public float defence;
    public float manna;
    public float hitDuration;

    // Attacking
    [Header("Attack Stats")]
    public float damage;
    public float criticalStrike;
    public float attackSpeed;
    public float knockbackForce;

    [Header("Projectile Data")]
    public float projectileSpeed;
    public float projectileLifetime;
    public float staffProjectileLifetime;

    [Header("Magic")]
    public float mannaCost;
    public float mannaRegenSpeed;
    public float mannaRegenAmount;

    //Movement
    [Header("Movement")]
    public float moveSpeed;
    public float stability;
    private PlayerMovement playerMovement;

    //Tracking
    public float currentHealth;
    public float currentManna;
    private float defaultMoveSpeed;
    public GameObject[] specialInventorySlots;

    private bool currentRegening;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        currentRegening = false;
        defaultMoveSpeed = moveSpeed;
        currentHealth = health;
        currentManna = manna;
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void SetHealth(float inHealth)
    {
        currentHealth += inHealth;
        currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0.
        PlayerUIManager.instance.UpdateHealthUI();
        playerMovement.Hit();
    }

    public void SetManna(float inManna)
    {
        currentManna += inManna;
        currentManna = Mathf.Max(currentManna, 0f); // Ensure health doesn't go below 0.
        PlayerUIManager.instance.UpdateMannaUI();
    }

    private void Update()
    {
        float tempDamage = 0;
        float tempCriticalStrike = 0;
        float tempAttackSpeed = 0;
        float tempKnockbackForce = 0;
        float tempMoveSpeed = 0;
        float tempStability = 0;
        float tempDefence = 0;
        float tempProjectileSpeed = 0;
        float tempProjectileLifetime = 0;
        float tempMannaCost = 0;

        foreach(GameObject slot in specialInventorySlots)
        {
            if(slot.transform.childCount > 0)
            {
                InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

                tempDamage += inventoryItem.item.damage;
                tempCriticalStrike += inventoryItem.item.criticalStrike;
                tempAttackSpeed += inventoryItem.item.attackSpeed;
                tempKnockbackForce += inventoryItem.item.knockbackForce;
                tempMoveSpeed += inventoryItem.item.movementSpeed;
                tempStability += inventoryItem.item.stability;
                tempDefence += inventoryItem.item.defence;
                tempProjectileSpeed += inventoryItem.item.projectileSpeed;
                tempProjectileLifetime += inventoryItem.item.projectileLifetime;
                tempMannaCost += inventoryItem.item.mannaCost;
            }
        }

        if(tempDamage != damage)
        {
            damage = tempDamage;
        }
        if (tempCriticalStrike != criticalStrike)
        {
            criticalStrike = tempCriticalStrike;
        }
        if (tempAttackSpeed != attackSpeed)
        {
            attackSpeed = tempAttackSpeed;
        }
        if (tempKnockbackForce != attackSpeed)
        {
            knockbackForce = tempKnockbackForce;
        }
        if (tempStability != stability)
        {
            stability = tempStability;
        }
        if (tempDefence != defence)
        {
            defence = tempDefence;
        }
        if (tempProjectileLifetime != projectileLifetime)
        {
            projectileLifetime = tempProjectileLifetime;
        }
        if (tempProjectileSpeed != projectileSpeed)
        {
            projectileSpeed = tempProjectileSpeed;
        }
        if (tempMoveSpeed != moveSpeed)
        {
            if(tempMoveSpeed > 0)
            {
                moveSpeed = tempMoveSpeed;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
            }
        }
        if (tempMannaCost != mannaCost)
        {
            mannaCost = tempMannaCost;
        }

        RegenManna();
    }

    public bool CanRegenManna()
    {
        if(currentManna >= manna || currentRegening || PlayerStates.instance.GetState() == PlayerState.Attack)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RegenManna()
    {
        if (CanRegenManna())
        {
            StartCoroutine(RegenMannaInterval());
        }
    }

    IEnumerator RegenMannaInterval()
    {
        currentRegening = true;
        yield return new WaitForSeconds(mannaRegenSpeed);
        SetManna(+mannaRegenAmount);
        currentRegening = false;
    }


    public void SetSoulFragments(int inSouls)
    {
        soulFragments += inSouls;
        PlayerUIManager.instance.UpdateSoulsUI();
    }
}
