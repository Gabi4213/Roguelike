using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetableProjectile : MonoBehaviour
{
    public float projectileLifetime;
    public GameObject destroyEffect;

    public bool destroyOnImpact;

    public bool hasVelocity = true;

    private void Start()
    {
        // Set initial velocity based on the spawn rotation
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        if (hasVelocity && playerRb != null)
        {
            // Set the velocity to be the sum of the projectile's speed and the player's velocity
            Vector2 initialVelocity = new Vector2(transform.right.x, transform.right.y) * PlayerStatistics.instance.projectileSpeed;
            rb.velocity = initialVelocity + playerRb.velocity;
        }

        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(projectileLifetime);

        if (destroyEffect)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Destroy the projectile after the set lifetime
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Projectile") return;

        if (destroyOnImpact)
        {
            if (destroyEffect)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }

            // Destroy the projectile after the set lifetime
            Destroy(gameObject);
        }
    }
}
