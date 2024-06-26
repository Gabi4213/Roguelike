using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDamage : MonoBehaviour
{
    private Item item;
    private bool damageDealt;

    public void SetItem(Item inItem)
    {
        item = inItem;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IHealth>() && item && other.GetComponent<Enemy>())
        {
            if (!other.GetComponent<Enemy>().enemyHit)
            {
                other.gameObject.GetComponent<IHealth>().SetHealth(-item.damage);
                other.gameObject.GetComponent<Enemy>().SetKnockbackForce(item.knockbackForce);
            }
        }
    }
}
