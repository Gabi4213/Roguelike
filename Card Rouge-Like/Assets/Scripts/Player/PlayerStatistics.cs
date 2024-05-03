using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics instance;

    //Player
    [Header("Player")]
    public int health;
    public float defence;

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

    //Movement
    [Header("Movement")]
    public float moveSpeed;
    public float stability;

    //Tracking
    public float currentHealth;
    private float defaultMoveSpeed;
    public GameObject[] specialInventorySlots;


    // Boolean to track if the player is currently taking damage
    private bool takingDamage = false;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        defaultMoveSpeed = moveSpeed;
        currentHealth = health;
    }

    public void SetHealth(float inHealth)
    {
        currentHealth += inHealth;
        currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0.
        PlayerUIManager.instance.UpdateHealthUI();
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
    }
}
