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

        if (hasVelocity)
        {
            //set the velocity
            rb.velocity = transform.right * PlayerStatistics.instance.projectileSpeed;
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
        if (other.gameObject.tag == "Player") return;

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
