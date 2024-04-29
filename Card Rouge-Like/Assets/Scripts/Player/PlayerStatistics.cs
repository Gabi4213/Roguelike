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


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        defaultMoveSpeed = moveSpeed;
        currentHealth = health;
    }

    public void SetHealth(int inHealth)
    {
        currentHealth = inHealth;
    }

    private void Update()
    {
        float tempDamage = 0;
        float tempCriticalStrike = 0;
        float tempAttackSpeed = 0;
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
        else if (tempCriticalStrike != criticalStrike)
        {
            criticalStrike = tempCriticalStrike;
        }
        else if (tempAttackSpeed != attackSpeed)
        {
            attackSpeed = tempAttackSpeed;
        }
        else if (tempStability != stability)
        {
            stability = tempStability;
        }
        else if (tempDefence != defence)
        {
            defence = tempDefence;
        }
        else if (tempProjectileLifetime != projectileLifetime)
        {
            projectileLifetime = tempProjectileLifetime;
        }
        else if (tempProjectileSpeed != projectileSpeed)
        {
            projectileSpeed = tempProjectileSpeed;
        }
        else if (tempMoveSpeed != moveSpeed)
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
