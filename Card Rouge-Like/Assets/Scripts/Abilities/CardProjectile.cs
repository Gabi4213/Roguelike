using Demo_Project;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProjectile : MonoBehaviour
{
    public HighStakesAbility parentAbility;
    public float projectileLifetime;
    public float projectileSpeed;
    public bool destroyOnImpact;

    public bool hasVelocity = true;
    public int cardIndex; //what card is it 0= ace, 1 = joker, 2 = 2 of hearts

    private void Start()
    {
        // Set initial velocity based on the spawn rotation
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        if (hasVelocity && playerRb != null)
        {
            // Set the velocity to be the sum of the projectile's speed and the player's velocity
            Vector2 initialVelocity = new Vector2(transform.right.x, transform.right.y) * projectileSpeed;
            rb.velocity = initialVelocity + playerRb.velocity;
        }

        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(projectileLifetime);

        if (parentAbility.projectileDestroyParticles[cardIndex])
        {
            Instantiate(parentAbility.projectileDestroyParticles[cardIndex], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy") return;

        if (destroyOnImpact)
        {
            if (parentAbility.projectileDestroyParticles[cardIndex])
            {
                Instantiate(parentAbility.projectileDestroyParticles[cardIndex], transform.position, Quaternion.identity);
            }

            switch (cardIndex)
            {
                case 0:
                    CardOne();
                    break;
                case 1:
                    CardTwo();
                    break;
                case 2:
                    CardThree();
                    break;
            }

            // Destroy the projectile after the set lifetime
            Destroy(gameObject);
        }
    }

    //Ace
    void CardOne()
    {

    }

    //Joker
    void CardTwo()
    {

    }

    //2 of Hearts
    void CardThree()
    {
        PlayerStatistics.instance.SetHealth(parentAbility.healthIncreaseAmount);
    }
}
