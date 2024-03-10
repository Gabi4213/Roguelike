using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void Start()
    {
        // Set initial velocity based on the spawn rotation
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //set the velocity
        rb.velocity = transform.right * PlayerStatistics.instance.projectileSpeed;

        // Destroy the projectile after the set lifetime
        Destroy(gameObject, PlayerStatistics.instance.projectileLifetime);
    }
}
