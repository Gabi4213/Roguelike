using UnityEngine;
using System.Collections;

public class ArrowProjectile : MonoBehaviour
{
    public float projectileLifetime;
    public GameObject droppedGameObject, destroyedDroppedGameObject;
    public GameObject destroyEffect;

    private void Start()
    {
        // Set initial velocity based on the spawn rotation
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //set the velocity
        rb.velocity = transform.right * PlayerStatistics.instance.projectileSpeed;

        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(projectileLifetime);

        if (droppedGameObject)
        {
            Instantiate(destroyedDroppedGameObject, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90f));
        }

        if (destroyEffect)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Destroy the projectile after the set lifetime
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "Ammo")
        {
            if (droppedGameObject)
            {
                GameObject arrow = Instantiate(droppedGameObject, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90f));
                arrow.transform.parent = other.transform;
            }
            if (destroyEffect)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }
            // Destroy the projectile after the set lifetime
            Destroy(gameObject);
        }
    }
}
