using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics instance;

    //Player
    [Header("Player")]
    public int health;
    public float defence;

    // Attacking
    [Header("Attack Stats")]
    public float meleeDamage;
    public float magicDamage;
    public float criticalStrike;
    public float attackSpeed;

    [Header("Projectile Data")]
    public float projectileSpeed;
    public float projectileLifetime;

    //Movement
    [Header("Movement")]
    public float moveSpeed;

    //Tracking
    public float currentHealth;
    public GameObject weaponInventorySlot;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        currentHealth = health;
    }

    public void SetHealth(int inHealth)
    {
        currentHealth = inHealth;
    }

    private void Update()
    {
        if(weaponInventorySlot.transform.childCount > 0)
        {
            SetPlayerStats(weaponInventorySlot.GetComponentInChildren<InventoryItem>().item);
        }
        else if(meleeDamage !=0)
        {
            ResetPlayerStats();
        }
    }

    public void SetPlayerStats(Item item)
    {
        meleeDamage = item.meleeDamage;
        magicDamage = item.magicDamage;
        criticalStrike = item.criticalStrike;
        attackSpeed = item.attackSpeed;
        moveSpeed = item.movementSpeed;
    }

    public void ResetPlayerStats()
    {
        meleeDamage = 0;
        magicDamage = 0;
        criticalStrike = 0;
        attackSpeed = 0;
        moveSpeed = 0;
    }

}
