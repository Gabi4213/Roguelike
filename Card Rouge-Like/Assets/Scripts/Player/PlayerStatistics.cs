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
    [Header("Attack Data")]
    public float damage;
    public float attackSpeed;
    public float critChance;

    [Header("Projectile Data")]
    public float projectileSpeed;
    public float projectileLifetime;

    //Movement
    [Header("Movement")]
    public float moveSpeed;

    //Tracking
    public float currentHealth;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        currentHealth = health;
    }
}
